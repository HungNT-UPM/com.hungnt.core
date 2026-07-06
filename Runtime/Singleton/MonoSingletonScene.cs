using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Singleton MonoBehaviour sống theo scene (KHÔNG DontDestroyOnLoad — bị destroy khi unload scene).
    /// <para>⚠️ Subclass KHÔNG được tự khai báo <c>Awake()</c>/<c>OnApplicationQuit()</c> — override <see cref="OnAwake"/> thay thế.</para>
    /// </summary>
    public class MonoSingletonScene<TMono> : MonoBehaviour where TMono : MonoBehaviour
    {
        private static TMono _instance;
        private static bool _isQuitting;

        /// <summary>Đã có instance sống (không tự tạo mới khi kiểm tra).</summary>
        public static bool HasInstance => _instance != null;

        /// <summary>
        /// Instance duy nhất trong scene hiện tại. Tự tạo GameObject nếu chưa có.
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
                    _instance = go.AddComponent<TMono>();
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
                _isQuitting = false;
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
