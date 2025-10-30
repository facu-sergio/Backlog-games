using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BacklogGames")]

namespace BacklogGames.Bussinnes.Layer
{
    internal static class Extensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            return services;
        }
    }
}
