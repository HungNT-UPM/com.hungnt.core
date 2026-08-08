using VContainer;
using VContainer.Unity;

namespace HungNT
{
    /// <summary>Hạ tầng của <c>com.hungnt.core</c>. Gọi trong <c>Configure</c> của scope gốc.</summary>
    public static class CoreInstaller
    {
        /// <summary>
        /// Đăng ký <see cref="IAppLifecycleService"/>. GameObject của nó được tạo sẵn và giữ xuyên scene
        /// vì sự kiện pause/quit phải bắt được ở mọi thời điểm.
        /// </summary>
        public static IContainerBuilder InstallCore(this IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<AppLifecycleService>(Lifetime.Singleton, "[HungNT] AppLifecycle")
                .DontDestroyOnLoad()
                .As<IAppLifecycleService>();

            return builder;
        }
    }
}
