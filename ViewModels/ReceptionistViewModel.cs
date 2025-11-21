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
        public ReceptionistViewModel(StaffService staffService,Staff staff)
        {
            _staffService = staffService;
            _staff = staff;
            Debug.WriteLine(_staff.StaffId);
            loadStaffData(_staff.StaffId);

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
        string staffHireDate;
        [ObservableProperty]
        string staffStatus;

        [RelayCommand]
        async Task OnLogoutClicked()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        public void loadStaffData(int id)
        {
            var staff = _staffService.GetStaff(id).Result;
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
                StaffStatus = staff.IsActive ? "Active" : "Inactive";
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Staff Not Found","Staff With mentioned user id cannot be acces.","Ok");
            }
        }
    }
}
