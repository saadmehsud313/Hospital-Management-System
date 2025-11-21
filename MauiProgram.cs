using Hospital_Management_System.ViewModels;
using Microsoft.Extensions.Logging;
using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Services;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using WinRT.Interop;
using System.Runtime.InteropServices;
#endif



namespace Hospital_Management_System
{

    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder
          .UseMauiApp<App>()
          .ConfigureLifecycleEvents(events =>
          {
#if WINDOWS
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated((window) =>
                    {
                        var hwnd = WindowNative.GetWindowHandle(window);

                        // Maximize using Win32 API
                        ShowWindow(hwnd, 3);  // 3 = SW_MAXIMIZE
                    });
                });
#endif
          });


#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<LoginServices>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddTransient<Staff>();
            builder.Services.AddTransient<StaffService>();
            builder.Services.AddTransient<ReceptionistView>();
            builder.Services.AddTransient<ReceptionistViewModel>();
            builder.Services.AddTransient<AddPatientView>();
            builder.Services.AddSingleton<DatabaseConfig>(new DatabaseConfig
                {
                ConnectionString = @"Data Source=MR-ANDROID\SQLEXPRESS;Database=HospitalDatabase;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=30"
            });
            builder.Services.AddTransient<LoginRepository>(sp =>
            {
                var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                return new LoginRepository(dbConfig);
            });
            builder.Services.AddTransient<StaffRepository>(sp =>
            {
                var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                return new StaffRepository(dbConfig);
            });
            builder.Services.AddTransient<UserAccount>();
            builder.Services.AddTransient<UserAccountService>();


            var app = builder.Build();
            Services = app.Services;
            return builder.Build();
        }
#if WINDOWS
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(nint hWnd, int nCmdShow);
#endif
    }

}
