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
        [ObservableProperty]
        string newUsername;
        [ObservableProperty]
        string newPassword;
        [ObservableProperty]
        string confirmPassword;
        





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
                await Shell.Current.DisplayAlertAsync("Staff Not Found","Staff With mentioned user id cannot be acces.","Ok");
            }
        }
        [RelayCommand]
        private async Task UpdateProfile()
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(NewUsername) && string.IsNullOrWhiteSpace(NewPassword))
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Please provide a new username or password to update", "Okay");
                    return;
                }

                bool anyUpdate = false;
                string updateMessage = "";

                // Get the service ONCE at the beginning
                var staffUpdateService = MauiProgram.Services.GetRequiredService<StaffService>();

                // Update Username if provided
                if (!string.IsNullOrWhiteSpace(NewUsername))
                {
                    //var usernameUpdated = await staffUpdateService.UpdateUsername(StaffId, NewUsername);
                    updateMessage += "✓ Username update feature is currently disabled\n";
                    anyUpdate = true;

                    // Force refresh profile data
                    await LoadRealDoctorData();
                }

                // Update Password if provided - FIXED SECTION
                if (!string.IsNullOrWhiteSpace(NewPassword))
                {
                    if (NewPassword != ConfirmPassword)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", "Passwords do not match", "Okay");
                        return;
                    }

                    if (NewPassword.Length < 6)
                    {
                        await Shell.Current.DisplayAlertAsync("Error", "Password must be at least 6 characters", "Okay");
                        return;
                    }


                    // FIX: Pass the plain text password, NOT a hash
                    var passwordUpdated = await staffUpdateService.UpdatePassword(StaffId, NewPassword);

                    if (passwordUpdated)
                    {
                        updateMessage += "✓ Password updated successfully\n";
                        anyUpdate = true;
                        // Clear password fields after successful update
                        NewPassword = string.Empty;
                        ConfirmPassword = string.Empty;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlertAsync("Error", "Failed to update password in database", "Okay");
                        return;
                    }
                }

                if (anyUpdate)
                {
                    // Clear fields
                    NewUsername = string.Empty;
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;

                    // Show success message
                    await Shell.Current.DisplayAlertAsync("Success", $"Profile updated successfully!\n\n{updateMessage}", "Okay");

                    Debug.WriteLine("✅ Profile update complete");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to update profile: {ex.Message}", "Okay");
            }
        }

        // Load REAL data from database
        private async Task LoadRealDoctorData()
        {
            try
            {

                // Load fresh data from database
                var freshStaffData = await _staffService.GetStaff(_staff.StaffID);
                if (freshStaffData == null)
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Could not load your profile data. Please contact administrator.", "Okay");
                    return;
                }

                // Update all properties with REAL data
                StaffId = freshStaffData.StaffID.ToString();
                StaffFirstName = freshStaffData.FirstName;
                StaffLastName = freshStaffData.LastName;
                StaffRole = freshStaffData.Role;
                StaffName = $"{freshStaffData.FirstName} {freshStaffData.LastName}";
                StaffPhoneNumber = freshStaffData.Phone ?? "Not available";
                StaffDepartment = freshStaffData.DepartmentName ?? "Not assigned";
                StaffHireDate = freshStaffData.HireDate;
                StaffStatus = freshStaffData.IsActive ? "Active" : "Inactive";
                UserName = freshStaffData.FullName;
                UserId = freshStaffData.StaffID.ToString();
                StaffEmail = freshStaffData.Email;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load profile: {ex.Message}", "Okay");
            }
        }


    }
}
