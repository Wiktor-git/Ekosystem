using CommunityToolkit.Maui;
using MauiApp1;
using MauiApp1.Services;
using MauiApp1.ViewModel;
using Microsoft.Extensions.Logging;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<PlanszaSerwis>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<PlanszeViewModel>();
            builder.Services.AddTransient<PlanszaDetailsViewModel>();
            builder.Services.AddTransient<PlanszaView>();
            builder.Services.AddTransient<CameraPage>();
            builder.Services.AddTransient<CameraViewModel>();


            var app = builder.Build();


            ServiceHelper.Services = app.Services;



            return app;
        }
    }
}
