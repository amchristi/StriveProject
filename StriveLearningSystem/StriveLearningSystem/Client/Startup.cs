using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using StriveLearningSystem.Client.Agents;
using StriveLearningSystem.Client.Identity;

namespace StriveLearningSystem.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, StriveAuthenticationStateProvider>();

            services.AddScoped<CoursesAgent>();
            services.AddScoped<IdentityAgent>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
