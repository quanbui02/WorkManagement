namespace WorkManagement.Helper
{
    public interface IDiscoveryClient : IServiceInstanceProvider
    {
        //
        // Summary:
        //     ServiceInstance with information used to register the local service
        //
        // Returns:
        //     The IServiceInstance
        IServiceInstance GetLocalServiceInstance();

        Task ShutdownAsync();
    }
}
