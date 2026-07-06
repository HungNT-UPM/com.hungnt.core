using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Singleton MonoBehaviour sống xuyên scene (DontDestroyOnLoad); tự tạo GameObject khi truy cập <see cref="Instance"/> lần đầu.
    /// <para>⚠️ Subclass KHÔNG được tự khai báo <c>Awake()</c>/<c>OnApplicationQuit()</c> — Unity chỉ gọi bản khai báo
    /// ở class dẫn xuất, logic singleton của base sẽ bị bỏ qua. Override <see cref="OnAwake"/> thay thế.</para>
    /// </summary>
    public class MonoSingleton<TMono> : MonoBehaviour where TMono : MonoBehaviour
    {
        private static TMono _instance;
        private static bool _isQuitting;

        /// <summary>Đã có instance sống (không tự tạo mới khi kiểm tra). Dùng trong OnDestroy/teardown để tránh tạo ghost object.</summary>
        public static bool HasInstance => _instance != null;

        /// <summary>
        /// Instance duy nhất. Tự tạo GameObject nếu chưa có.
        /// Trả về <c>null</c> khi app đang quit (không tạo object mới giữa lúc teardown).
        /// </summary>
        public static TMono Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                if (_isQuitting)
                    return null;

                _instance = FindFirstObjectByType<TMono>();

                // create new instance
                if (_instance == null)
                {
                    var go = new GameObject(typeof(TMono).Name);
                    _instance = go.AddComponent<TMono>(); // sau khi AddComponent, Awake sẽ được gọi ngay lập tức
                }

                return _instance;
            }
        }

        private void Awake()
        {
            // khi có Instance sẵn trên scene
            if (_instance == null)
                _instance = this as TMono;

            if (_instance == this)
            {
                _isQuitting = false; // reset khi play lại (Editor tắt domain reload vẫn đúng)
                DontDestroyOnLoad(gameObject);
                OnAwake();
                return;
            }

            this.LogWarning($"Destroy duplicate instance: {gameObject.name.Color("red")}");
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void OnAwake()
        {
            this.Log($"On awake of instance: {gameObject.name.Color("cyan")}");
        }
    }
}
