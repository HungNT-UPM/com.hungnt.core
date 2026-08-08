using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// MonoBehaviour duy nhất của base code: chuyển message vòng đời của Unity thành event C#,
    /// nhờ đó mọi service khác được viết như plain C# class. Container tự tạo GameObject cho nó.
    /// </summary>
    public class AppLifecycleService : MonoBehaviour, IAppLifecycleService
    {
        public event Action<bool> OnPaused;

        public event Action<bool> OnFocused;

        public event Action OnQuitting;

        private void OnApplicationPause(bool pause) => OnPaused?.Invoke(pause);

        private void OnApplicationFocus(bool focus) => OnFocused?.Invoke(focus);

        private void OnApplicationQuit() => OnQuitting?.Invoke();
    }
}
