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
        [ObservableProperty]
        string emailEntry;
        [ObservableProperty]
        string passwordEntry;
        [ObservableProperty]
        string loginLabel;
        [ObservableProperty]
        bool isBusy;
        public LoginViewModel(LoginServices LoginServices)
        {
            _LoginServices = LoginServices;
            IsBusy = false;
        }

        [RelayCommand]
        async Task OnLoginClicked()
        {
            //Application.Current.MainPage = new AppShell("Database Admin");
            if (ValidateInput())
            {
                string email = EmailEntry;
                string password = PasswordEntry;
                IsBusy = true;
                bool loginStatus = await _LoginServices.LoginCheck(email, password);
                if (loginStatus)
                {
                    UserAccount user = await _LoginServices.GetUserData(email);

                    LoginLabel = string.Empty;
                    EmailEntry = string.Empty;
                    PasswordEntry = string.Empty;
                    ReceptionistViewModel.id = user.DocOrStaffId;
                    ReceptionistViewModel.user = user;
                    Application.Current.MainPage = new AppShell(user.Role);
                    IsBusy = false;

                }
                else
                {
                    IsBusy = false;
                }
            }

        }
        public bool ValidateInput()
        {
            if (EmailEntry is null)
            {
                Application.Current.MainPage.DisplayAlertAsync("Error!","Please insert a valid email","Ok");

                return false;
            }
            if (PasswordEntry is null)
            {
                Application.Current.MainPage.DisplayAlertAsync("Error!", "Please enter password", "Ok");
                return false;
            }
               return true;
        }
    }
}
