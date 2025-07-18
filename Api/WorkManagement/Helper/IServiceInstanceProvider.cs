namespace WorkManagement.Helper
{
    public interface IServiceInstanceProvider
    {
        //
        // Summary:
        //     Gets a human readable description of the implementation
        string Description { get; }

        //
        // Summary:
        //     Gets all known service Ids
        IList<string> Services { get; }

        //
        // Summary:
        //     Get all ServiceInstances associated with a particular serviceId
        //
        // Parameters:
        //   serviceId:
        //     the serviceId to lookup
        //
        // Returns:
        //     List of service instances
        IList<IServiceInstance> GetInstances(string serviceId);
    }
}
