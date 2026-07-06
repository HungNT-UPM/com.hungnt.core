using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Service Locator (singleton). Dùng để register và resolve các game service theo interface.
    /// </summary>
    [DefaultExecutionOrder(-999)]
    public class ServiceLocator : MonoSingleton<ServiceLocator>
    {
        [ShowInInspector, TableList]
        private readonly Dictionary<Type, IService> _services = new();

        private readonly Dictionary<Type, List<Action<IService>>> _pendingCallbacks = new();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _services.Clear();
            _pendingCallbacks.Clear();
        }

        // ── Register / Unregister ────────────────────────────────────────────

        /// <summary>
        /// Đăng ký implementation cho <typeparamref name="TService"/>.
        /// Nếu đã tồn tại sẽ ghi đè và log warning.
        /// </summary>
        public void Register<TService>(TService impl) where TService : IService
        {
            if (impl == null)
            {
                this.LogWarning($"Register<{typeof(TService).Name}> — impl null, skipped.");
                return;
            }

            var type = typeof(TService);

            // Chỉ cảnh báo khi ghi đè service còn sống — thay service đã destroy là flow bình thường.
            if (TryGetAlive(type, out _))
            {
                this.LogWarning($"Register<{type.Name}> — đã có implementation, ghi đè bằng {impl.GetType().Name}.");
            }

            _services[type] = impl;
            this.Log($"Register<{type.Name}> — {impl.GetType().Name.Color("cyan")}.");
            FlushPendingCallbacks(type, impl);
        }

        /// <summary>
        /// Đăng ký implementation theo runtime Type. Dùng nội bộ bởi ServiceRegister.
        /// Throws <see cref="ArgumentException"/> nếu <paramref name="serviceType"/> không phải interface của <paramref name="impl"/>.
        /// </summary>
        public void Register(Type serviceType, IService impl)
        {
            if (impl == null)
            {
                this.LogWarning($"Register({serviceType.Name}) — impl null, skipped.");
                return;
            }

            if (!serviceType.IsAssignableFrom(impl.GetType()))
            {
                this.LogWarning($"Register({serviceType.Name}) — {impl.GetType().Name} không implement {serviceType.Name}, skipped.");
                return;
            }

            if (TryGetAlive(serviceType, out _))
            {
                this.LogWarning($"Register({serviceType.Name}) — đã có implementation, ghi đè bằng {impl.GetType().Name}.");
            }

            _services[serviceType] = impl;
            this.Log($"Register: {serviceType.Name.Color("cyan")} — {impl.GetType().Name.Color("yellow")}.");
            FlushPendingCallbacks(serviceType, impl);
        }

        /// <summary>Hủy đăng ký <typeparamref name="TService"/> khỏi locator.</summary>
        public void Unregister<TService>() where TService : IService
        {
            var type = typeof(TService);

            if (!_services.Remove(type))
            {
                this.LogWarning($"Unregister<{type.Name}> — không tìm thấy service để xóa.");
            }
        }

        /// <summary>
        /// Hủy đăng ký theo runtime Type, chỉ khi mapping hiện tại đúng là <paramref name="impl"/>.
        /// Dùng bởi <see cref="ServiceRegister"/> khi bị destroy — không xóa nhầm implementation mới hơn.
        /// </summary>
        public void Unregister(Type serviceType, IService impl)
        {
            if (_services.TryGetValue(serviceType, out var current) && ReferenceEquals(current, impl))
            {
                _services.Remove(serviceType);
            }
        }

        // ── Get ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy implementation của <typeparamref name="TService"/>.
        /// Log warning và trả về default nếu chưa register (hoặc service Mono đã bị destroy).
        /// </summary>
        public TService Get<TService>() where TService : IService
        {
            var type = typeof(TService);

            if (TryGetAlive(type, out var service))
            {
                return (TService)service;
            }

            this.LogWarning($"Get<{type.Name}> — chưa register service này. Trả về default.");
            return default;
        }

        /// <summary>
        /// Lấy implementation qua callback. Nếu đã register → gọi ngay.
        /// Nếu chưa → queue lại, tự gọi khi service được register.
        /// </summary>
        public void Get<TService>(Action<TService> callback) where TService : IService
        {
            if (callback == null) return;

            var type = typeof(TService);

            if (TryGetAlive(type, out var service))
            {
                callback((TService)service);
                return;
            }

            if (!_pendingCallbacks.TryGetValue(type, out var list))
            {
                list = new List<Action<IService>>();
                _pendingCallbacks[type] = list;
            }

            list.Add(s => callback((TService)s));
        }

        /// <summary>
        /// Thử lấy implementation của <typeparamref name="TService"/>.
        /// Trả về true nếu tìm thấy (service Mono đã destroy được coi là không tồn tại).
        /// </summary>
        public bool TryGet<TService>(out TService service) where TService : IService
        {
            if (TryGetAlive(typeof(TService), out var found))
            {
                service = (TService)found;
                return true;
            }

            service = default;
            return false;
        }

        // ── Utilities ────────────────────────────────────────────────────────

        /// <summary>Kiểm tra xem <typeparamref name="TService"/> đã được register (và còn sống) chưa.</summary>
        public bool IsRegistered<TService>() where TService : IService
        {
            return TryGetAlive(typeof(TService), out _);
        }

        /// <summary>Xóa toàn bộ service đã register.</summary>
        public void ClearAll()
        {
            _services.Clear();
            this.Log("All services cleared.".Color("orange"));
        }

        /// <summary>Trả về snapshot danh sách service hiện tại để debug/Editor hiển thị.</summary>
        public IReadOnlyDictionary<Type, IService> GetDebugSnapshot()
        {
            return _services;
        }

        // ── Private ───────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy service còn sống. Service là MonoBehaviour đã destroy (scene unload) sẽ bị coi là
        /// không tồn tại và dọn khỏi registry — check <c>== null</c> qua interface không bắt được fake-null.
        /// </summary>
        private bool TryGetAlive(Type type, out IService service)
        {
            if (_services.TryGetValue(type, out service))
            {
                if (service is UnityEngine.Object unityObj && unityObj == null)
                {
                    _services.Remove(type);
                    service = null;
                    return false;
                }

                return true;
            }

            return false;
        }

        private void FlushPendingCallbacks(Type type, IService impl)
        {
            if (!_pendingCallbacks.TryGetValue(type, out var list)) return;

            _pendingCallbacks.Remove(type);

            foreach (var callback in list)
            {
                callback(impl);
            }
        }
    }
}
