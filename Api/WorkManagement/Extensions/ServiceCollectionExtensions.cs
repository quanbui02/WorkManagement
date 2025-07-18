using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Work.DataContext;
using Work.DataContext.Models;

namespace WorkManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection DependencyInjectionDatacontext(
            this IServiceCollection services,
            IConfiguration configuration,
            string connStr)
        {
            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(connStr));
            services.AddDbContext<WorkManagementContext>(options => options.UseSqlServer(connStr));
            return services;
        }
    }
}