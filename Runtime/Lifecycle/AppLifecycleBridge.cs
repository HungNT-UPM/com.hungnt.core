using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// MonoBehaviour <b>duy nhất</b> của base code: chuyển message vòng đời của Unity thành event C#,
    /// nhờ đó mọi service khác được viết như plain C# class (test được, không cần nằm trong scene).
    /// <para>Container tự tạo GameObject cho nó — xem <see cref="CoreInstaller.InstallCore"/>.</para>
    /// </summary>
    public class AppLifecycleBridge : MonoBehaviour, IAppLifecycle
    {
        public event Action<bool> Paused;

        public event Action<bool> Focused;

        public event Action Quitting;

        private void OnApplicationPause(bool pause) => Paused?.Invoke(pause);

        private void OnApplicationFocus(bool focus) => Focused?.Invoke(focus);

        private void OnApplicationQuit() => Quitting?.Invoke();
    }
}
