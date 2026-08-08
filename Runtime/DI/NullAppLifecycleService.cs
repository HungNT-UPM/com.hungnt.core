using System;

namespace HungNT
{
    /// <summary>
    /// Bản không bao giờ phát event — dùng cho công cụ Editor, EditMode test, hoặc bất cứ nơi nào
    /// cần tạo service ngoài container mà không quan tâm hook vòng đời.
    /// </summary>
    public class NullAppLifecycleService : IAppLifecycleService
    {
        public event Action<bool> OnPaused
        {
            add { }
            remove { }
        }

        public event Action<bool> OnFocused
        {
            add { }
            remove { }
        }

        public event Action OnQuitting
        {
            add { }
            remove { }
        }
    }
}
