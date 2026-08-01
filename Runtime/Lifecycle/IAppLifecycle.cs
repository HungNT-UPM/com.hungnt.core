using System;

namespace HungNT
{
    /// <summary>
    /// Các hook vòng đời ứng dụng mà DI container không có sẵn (VContainer chỉ cấp Start / Tick / Dispose).
    /// <para>Service thuần C# inject interface này thay vì phải tự trở thành MonoBehaviour chỉ để nghe
    /// <c>OnApplicationPause</c> / <c>OnApplicationQuit</c>.</para>
    /// </summary>
    public interface IAppLifecycle
    {
        /// <summary>
        /// <c>true</c> = app bị pause (mobile: về background).
        /// Trên mobile đây là hook lưu dữ liệu <b>tin cậy nhất</b> — quit thường không kịp gọi.
        /// </summary>
        event Action<bool> Paused;

        /// <summary><c>true</c> = app lấy lại focus.</summary>
        event Action<bool> Focused;

        /// <summary>
        /// App chuẩn bị thoát. Trên mobile KHÔNG được đảm bảo gọi khi OS kill process —
        /// đừng chỉ dựa vào sự kiện này để lưu dữ liệu, dùng kèm <see cref="Paused"/>.
        /// </summary>
        event Action Quitting;
    }
}
