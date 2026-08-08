using System;

namespace HungNT
{
    /// <summary>
    /// Các hook vòng đời ứng dụng mà container không cấp (VContainer chỉ có Start / Tick / Dispose).
    /// Nhờ interface này, service không phải trở thành MonoBehaviour chỉ để nghe pause/quit.
    /// </summary>
    public interface IAppLifecycleService
    {
        /// <summary>
        /// <c>true</c> = app bị pause (mobile: về background).
        /// Trên mobile đây là hook lưu dữ liệu tin cậy nhất — quit thường không kịp gọi.
        /// </summary>
        event Action<bool> OnPaused;

        /// <summary><c>true</c> = app lấy lại focus.</summary>
        event Action<bool> OnFocused;

        /// <summary>
        /// App chuẩn bị thoát. Trên mobile KHÔNG được đảm bảo gọi khi OS kill process —
        /// đừng chỉ dựa vào sự kiện này để lưu dữ liệu.
        /// </summary>
        event Action OnQuitting;
    }
}
