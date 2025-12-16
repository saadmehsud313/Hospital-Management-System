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
using Hospital_Management_System.Repository;

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
            builder.Services.AddTransient<Patient>();
            builder.Services.AddTransient<PatientService>();
            builder.Services.AddSingleton<AddPatientView>();
            builder.Services.AddTransient<AppointmentView>();
            builder.Services.AddSingleton<AddPatientViewModel>();
            builder.Services.AddTransient<AppointmentViewModel>();
            builder.Services.AddTransient<Doctor>();
            builder.Services.AddTransient<DoctorService>();
            builder.Services.AddSingleton<DoctorViewModel>();
            builder.Services.AddTransient<DoctorPage>();
            builder.Services.AddTransient<NursePage>();
            builder.Services.AddTransient<NurseViewModel>();
            builder.Services.AddTransient<NurseService>();
            builder.Services.AddTransient<RoomAssignmentRepository>();
            builder.Services.AddTransient<RoomAssignmentService>();
            builder.Services.AddTransient<RoomAssignmentPage>();
            builder.Services.AddTransient<RoomAssignmentViewModel>();
            builder.Services.AddTransient<VisitManagementViewModel>();
            builder.Services.AddTransient<VisitPage>();
            builder.Services.AddTransient<VisitService>();
            builder.Services.AddTransient<PatientRepository>(
                sp=>
                {
                    var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                    return new PatientRepository(dbConfig);
                }
                );
            builder.Services.AddTransient<NurseRepository>(
                sp=>
                {
                    var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                    return new NurseRepository(dbConfig);
                }
                );
            builder.Services.AddTransient<DoctorRepository>(
                sp=>
                {
                    var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                    return new DoctorRepository(dbConfig);
                }
                );
            builder.Services.AddTransient<VisitRepository>(
                sp=>
                {
                    var dbConfig = sp.GetRequiredService<DatabaseConfig>();
                    return new VisitRepository(dbConfig);
                }
                );
            builder.Services.AddSingleton<DatabaseConfig>(new DatabaseConfig
                {
                ConnectionString = @"Data Source=127.0.0.1,1433;Database=HospitalDatabase;User ID=saadmehsud;password=mrcomputer313;Encrypt=false;TrustServerCertificate=true;Connect Timeout=30;"
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
            builder.Services.AddTransient<AppointmentRepository>(ap =>
            {
                var dbConfig = ap.GetRequiredService<DatabaseConfig>();
                return new AppointmentRepository(dbConfig);
            });
            builder.Services.AddTransient<UserAccount>();
            builder.Services.AddTransient<UserAccountService>();
            builder.Services.AddTransient<AppointmentService>();

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
