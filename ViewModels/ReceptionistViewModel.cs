using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.Serialization;
using Hospital_Management_System.Views;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
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
            LoadStaffData(id);

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
        [ObservableProperty]
        bool profileViewStatus = true;
        [ObservableProperty]
        bool infoViewState = false;
        [RelayCommand]
        public void ToggleProfileView()
        {
            ProfileViewStatus = true;
            InfoViewState = false;
        }
        [RelayCommand]
        public void ToggleInfoView()
        {
            ProfileViewStatus = false;
            InfoViewState = true;
        }
        [RelayCommand]
        async Task OnLogoutClicked()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        public async Task LoadStaffData(int id)
        {

            Staff staff = await _staffService.GetStaff(id);

            if (staff != null)
            {
                StaffId = staff.StaffID.ToString();
                StaffFirstName = staff.FirstName;
                StaffLastName = staff.LastName;
                StaffRole = staff.Role;
                StaffName = staff.FirstName + " " + staff.LastName;
                StaffEmail = staff.Email;
                StaffPhoneNumber = staff.Phone;
                StaffDepartment = staff.DepartmentName;
                StaffHireDate = staff.HireDate;
                StaffStatus = staff.IsActive ? "Active" : "Inactive";
                UserId = $"{staff.StaffID}";
                UserName = $"{staff.FirstName} {staff.LastName}";
                UserPassword = $"{staff.Password}";
            }
            else
            {
                Application.Current.MainPage.DisplayAlertAsync("Staff Not Found","Staff With mentioned user id cannot be acces.","Ok");
            }
        }
    }
}
