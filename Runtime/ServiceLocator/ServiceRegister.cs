using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Composition root: đăng ký services vào <see cref="T:HungNT.ServiceLocator"/> khi scene khởi động.
    /// — Mono services: tự động tìm qua <c>GetComponentsInChildren</c> trên cùng GameObject.
    /// — Non-Mono services: thêm trực tiếp từ Inspector qua <c>[SerializeReference]</c>.
    /// <para>Mỗi service được đăng ký dưới <b>tất cả</b> interface kế thừa <see cref="IService"/>
    /// (không có interface trung gian thì dùng concrete type).</para>
    /// <para><see cref="_dontDestroyOnLoad"/> BẬT (mặc định — bootstrap): GameObject sống xuyên scene,
    /// chỉ cho phép một register persistent; vào lại scene bootstrap sẽ tự hủy bản duplicate.
    /// TẮT: services sống theo scene và tự Unregister khỏi locator khi GameObject bị destroy
    /// — dùng cho service chỉ có nghĩa trong một scene (scene-scoped).</para>
    /// <para>⚠️ <see cref="IService.Initialize"/> chạy ngay trong Awake của register (execution order -999),
    /// tức TRƯỚC cả <c>Awake()</c> của chính service — đừng dựa vào Awake để chuẩn bị state cho Initialize.</para>
    /// </summary>
    [DefaultExecutionOrder(-999)]
    public class ServiceRegister : MonoBehaviour
    {
        private static ServiceRegister _persistentInstance;

        [SerializeField]
        [Tooltip("Bật: sống xuyên scene (bootstrap, duy nhất một register persistent).\nTắt: services scene-scoped — tự unregister khi GameObject destroy.")]
        private bool _dontDestroyOnLoad = true;

        [ShowInInspector, ReadOnly]
        [Tooltip("Danh sách MonoBehaviour services đã được register (chỉ đọc, cập nhật lúc Play Mode).")]
        private List<IService> _monoServices = new();

        [SerializeReference]
        [Tooltip("Plain C# services (non-MonoBehaviour). Thêm từ Inspector bằng cách right-click → 'Add Reference'.")]
        private List<IService> _nonMonoServices = new();

        // (key type, impl) đã đăng ký bởi register này — để unregister đúng phần của mình khi destroy.
        private readonly List<(Type type, IService impl)> _registered = new();

        // ── Unity ────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (_dontDestroyOnLoad)
            {
                if (_persistentInstance != null && _persistentInstance != this)
                {
                    this.LogWarning($"Duplicate persistent ServiceRegister — destroy {gameObject.name.Color("red")}.");
                    Destroy(gameObject);
                    return;
                }

                _persistentInstance = this;

                if (transform.parent == null)
                    DontDestroyOnLoad(gameObject);
                else
                    this.LogWarning("DontDestroyOnLoad chỉ hoạt động với root GameObject — register này vẫn sẽ chết theo scene.");
            }

            Register();
        }

        private void OnDestroy()
        {
            if (_persistentInstance == this)
                _persistentInstance = null;

            // Rút các service của mình khỏi locator (nếu locator còn sống) — tránh stale reference.
            if (!ServiceLocator.HasInstance)
                return;

            var locator = ServiceLocator.Instance;
            if (locator == null)
                return;

            foreach (var (type, impl) in _registered)
                locator.Unregister(type, impl);
        }

        protected virtual void Register()
        {
            RegisterMonoServices();
            RegisterNonMonoServices();
            InitializeAllServices();
            LateInitializeAllServices();
        }

        // ── Private ──────────────────────────────────────────────────────────

        private void RegisterMonoServices()
        {
            // Tìm tất cả MonoBehaviour implement IService trên GameObject và children
            var found = GetComponentsInChildren<IService>(includeInactive: true);

            _monoServices.Clear();

            foreach (var service in found)
            {
                RegisterService(service);
                _monoServices.Add(service);
            }
        }

        private void RegisterNonMonoServices()
        {
            foreach (var service in _nonMonoServices)
            {
                if (service == null) continue;

                RegisterService(service);
            }
        }

        /// <summary>
        /// Đăng ký <paramref name="impl"/> dưới mọi interface kế thừa <see cref="IService"/>;
        /// không có interface trung gian nào thì dùng concrete type làm key.
        /// </summary>
        private void RegisterService(IService impl)
        {
            var concreteType = impl.GetType();
            var registeredAny = false;

            foreach (var iface in concreteType.GetInterfaces())
            {
                if (iface == typeof(IService) || !typeof(IService).IsAssignableFrom(iface))
                    continue;

                ServiceLocator.Instance.Register(iface, impl);
                _registered.Add((iface, impl));
                registeredAny = true;
            }

            if (!registeredAny)
            {
                ServiceLocator.Instance.Register(concreteType, impl);
                _registered.Add((concreteType, impl));
            }
        }

        private void InitializeAllServices()
        {
            foreach (var service in _monoServices)
                service.Initialize();

            foreach (var service in _nonMonoServices)
                service?.Initialize();
        }

        private void LateInitializeAllServices()
        {
            foreach (var service in _monoServices)
                service.LateInitialize();

            foreach (var service in _nonMonoServices)
                service?.LateInitialize();
        }
    }
}
