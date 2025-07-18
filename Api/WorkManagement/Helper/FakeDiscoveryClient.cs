namespace WorkManagement.Helper
{
    public class FakeDiscoveryClient : IDiscoveryClient, IServiceInstanceProvider
    {
        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> Services
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IList<IServiceInstance> GetInstances(string serviceId)
        {
            throw new NotImplementedException();
        }

        public IServiceInstance GetLocalServiceInstance()
        {
            throw new NotImplementedException();
        }

        public Task ShutdownAsync()
        {
            throw new NotImplementedException();
        }
    }
}
