namespace WorkManagement.Helper
{
    public interface IServiceInstance
    {
        //
        // Summary:
        //     Gets the service id as register by the DiscoveryClient
        string ServiceId { get; }

        //
        // Summary:
        //     Gets the hostname of the registered ServiceInstance
        string Host { get; }

        //
        // Summary:
        //     Gets the port of the registered ServiceInstance
        int Port { get; }

        //
        // Summary:
        //     Gets a value indicating whether if the port of the registered ServiceInstance
        //     is https or not
        bool IsSecure { get; }

        //
        // Summary:
        //     Gets the service uri address
        Uri Uri { get; }

        //
        // Summary:
        //     Gets the key value pair metadata associated with the service instance
        IDictionary<string, string> Metadata { get; }
    }
}
