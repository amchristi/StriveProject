using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;
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
            services.AddScoped<CalendarAgent>();
            services.AddScoped<UserAgent>();
            services.AddScoped<AssignmentAgent>();
            services.AddScoped<GradeAgent>();

            services.AddSingleton<CoursesState>();

            services.AddToaster(config =>
            {
                //example customizations
                config.PositionClass = Defaults.Classes.Position.TopRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = false;
            });
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
