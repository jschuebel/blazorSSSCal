using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;


namespace SSSCalBlazor.Client.Models
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddMYServices(this IServiceCollection services, string baseadd)
        {
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseadd) });
            services.AddScoped<IPersonService, PersonService>();

            return services;
        }
    }
}
