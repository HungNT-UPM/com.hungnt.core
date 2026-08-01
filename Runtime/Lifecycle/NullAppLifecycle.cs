using System;

namespace HungNT
{
    /// <summary>
    /// <see cref="IAppLifecycle"/> không bao giờ phát event — dùng cho công cụ Editor, EditMode test,
    /// hoặc bất cứ nơi nào cần tạo service ngoài container mà không quan tâm hook vòng đời.
    /// </summary>
    public class NullAppLifecycle : IAppLifecycle
    {
        public event Action<bool> Paused
        {
            add { }
            remove { }
        }

        public event Action<bool> Focused
        {
            add { }
            remove { }
        }

        public event Action Quitting
        {
            add { }
            remove { }
        }
    }
}
