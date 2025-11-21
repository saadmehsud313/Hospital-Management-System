using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.Serialization;
using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System.Diagnostics;
namespace Hospital_Management_System.ViewModels
{
    public partial class ReceptionistViewModel:ObservableObject
    {
        private readonly StaffService _staffService;
        private readonly Staff _staff;
        public static int id;
        public static UserAccount user;
        public ReceptionistViewModel(StaffService staffService,Staff staff)
        {
            _staffService = staffService;
            _staff = staff;
            loadStaffData(ReceptionistViewModel.id);

        }
        
        [ObservableProperty]
        string staffId;
        [ObservableProperty]
        string staffFirstName;
        [ObservableProperty]
        string staffLastName;
        [ObservableProperty]
        string staffRole;
        [ObservableProperty]
        string staffName;
        [ObservableProperty]
        string staffEmail;
        [ObservableProperty]
        string staffPhoneNumber;
        [ObservableProperty]
        string staffAddress;
        [ObservableProperty]
        string staffCode;
        [ObservableProperty]
        string staffDepartment;
        [ObservableProperty]
        DateTime staffHireDate;
        [ObservableProperty]
        string staffStatus;
        [ObservableProperty]
        string userId;
        [ObservableProperty]
        string userName;
        [ObservableProperty]
        string userPassword;

        [RelayCommand]
        async Task OnLogoutClicked()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        public void loadStaffData(int id)
        {
            var staff = _staffService.GetStaff(user.UserId).Result;
            if (staff != null)
            {
                StaffId = staff.StaffId.ToString();
                StaffFirstName = staff.FirstName;
                StaffLastName = staff.LastName;
                StaffRole = staff.Role;
                StaffName = staff.FirstName + " " + staff.LastName;
                StaffEmail = staff.Email;
                StaffPhoneNumber = staff.Phone;
                StaffCode = staff.StaffCode;
                StaffDepartment = $"{staff.DepartmentId}";
                StaffHireDate = staff.HireDate;
                StaffStatus = staff.IsActive ? "Active" : "Inactive";
                UserId = $"{user.UserId}";
                UserName = $"{staff.FirstName} {staff.LastName}";
                UserPassword = $"{user.Password}";
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Staff Not Found","Staff With mentioned user id cannot be acces.","Ok");
            }
        }
    }
}
