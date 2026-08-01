using VContainer;
using VContainer.Unity;

namespace HungNT
{
    /// <summary>
    /// Hạ tầng của <c>com.hungnt.core</c>. Gọi trong <c>Configure</c> của scope <b>gốc</b>:
    /// <code>
    /// protected override void Configure(IContainerBuilder builder)
    /// {
    ///     builder.InstallCore();
    ///     builder.InstallDataSave();
    ///     // ...
    /// }
    /// </code>
    /// </summary>
    public static class CoreInstaller
    {
        /// <summary>Đăng ký <see cref="IAppLifecycle"/> (tạo sẵn GameObject persistent cho bridge).</summary>
        public static IContainerBuilder InstallCore(this IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<AppLifecycleBridge>(Lifetime.Singleton, "[HungNT] AppLifecycle")
                .DontDestroyOnLoad()
                .As<IAppLifecycle>();

            return builder;
        }
    }
}
