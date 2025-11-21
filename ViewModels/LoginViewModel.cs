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
        private readonly UserAccount _userAccount;
        private readonly Staff _staff;
        public LoginViewModel(LoginServices LoginServices, UserAccount userAccount, Staff staff)
        {
            _LoginServices = LoginServices;
            _userAccount = userAccount;
            _staff = staff;
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
            string role;
            string email = EmailEntry;
            string password = PasswordEntry;
            Debug.WriteLine("PasswordEntry:" + PasswordEntry);
            Debug.WriteLine("Password:"+ password);
            bool loginStatus = await _LoginServices.LoginCheck(email, password);
            Debug.WriteLine(loginStatus);
            if (loginStatus)
            {
                UserAccount user = await _LoginServices.GetUserData(email);
                if (user.Role.Equals("Database Admin"))
                {
                    LoginLabel =string.Empty;
                    EmailEntry = string.Empty;
                    PasswordEntry = string.Empty;
                    _staff.StaffId=user.UserId;
                    Debug.WriteLine("Staff ID in login"+_staff.StaffId);
                    Application.Current.MainPage = new AppShell(user.Role);
                    
                }

            }
            
        }
    }
}
