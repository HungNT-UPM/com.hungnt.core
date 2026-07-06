namespace HungNT
{
    /// <summary>
    /// Base contract for services registered with <see cref="T:HungNT.ServiceLocator"/>.
    /// <para><see cref="Initialize"/>: one-time setup for this service only.</para>
    /// <para><see cref="LateInitialize"/>: after all services have initialized — safe to resolve others.</para>
    /// <para>⚠️ Cả hai được <see cref="T:HungNT.ServiceRegister"/> gọi ngay trong Awake của nó (execution order -999),
    /// tức TRƯỚC <c>Awake()</c> của chính service — đừng dựa vào Awake để chuẩn bị state cho Initialize;
    /// tự khởi tạo lazy bên trong Initialize (kiểu <c>EnsureXxx()</c>) nếu cần.</para>
    /// </summary>
    public interface IService
    {
        void Initialize();
        void LateInitialize();
    }
}
