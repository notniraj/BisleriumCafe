using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using BisleriumCafe.Data;
using MudBlazor;
using BisleriumCafe.Components.Dialog;


namespace BisleriumCafe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.VisibleStateDuration = 4000;
                config.SnackbarConfiguration.HideTransitionDuration = 200;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;
                config.SnackbarConfiguration.MaxDisplayedSnackbars = 6;
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomStart;
            });
            builder.Services.AddSingleton<CoffeeServices>();
            builder.Services.AddSingleton<OrderServices>();
            builder.Services.AddSingleton<AddinServices>();
            builder.Services.AddSingleton<MembershipService>();
            builder.Services.AddSingleton<ReportServices>();
            builder.Services.AddMudServices();
            return builder.Build();
        }
    }
}
