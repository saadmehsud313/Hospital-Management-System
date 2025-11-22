using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Hospital_Management_System.Views;
using Hospital_Management_System.Services;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using System.Diagnostics;
namespace Hospital_Management_System.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginServices _LoginServices;
        public LoginViewModel(LoginServices LoginServices)
        {
            _LoginServices = LoginServices;
        }
        [ObservableProperty]
        string emailEntry;
        [ObservableProperty]
        string passwordEntry;
        [ObservableProperty]
        string loginLabel;


        [RelayCommand]
        async Task OnLoginClicked()
        {
            Application.Current.MainPage = new AppShell("Database Admin");
            //string email = EmailEntry;
            //string password = PasswordEntry;
            //bool loginStatus = await _LoginServices.LoginCheck(email, password);
            //if (loginStatus)
            //{
            //    var user = await _LoginServices.GetUserData(email);
            //    if (user.Role.Equals("Database Admin"))
            //    {
            //        LoginLabel = string.Empty;
            //        EmailEntry = string.Empty;
            //        PasswordEntry = string.Empty;
            //        ReceptionistViewModel.user = user;
            //        Application.Current.MainPage = new AppShell(user.Role);

            //    }

            //}

        }
    }
}
