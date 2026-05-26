namespace HungNT
{
    /// <summary>
    /// Base contract for services registered with <see cref="T:HungNT.ServiceLocator"/>.
    /// <para><see cref="Initialize"/>: one-time setup for this service only.</para>
    /// <para><see cref="LateInitialize"/>: after all services have initialized — safe to resolve others.</para>
    /// </summary>
    public interface IService
    {
        void Initialize();
        void LateInitialize();
    }
}
