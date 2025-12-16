using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Services;
using Hospital_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hospital_Management_System.ViewModels
{
    public partial class NurseViewModel : ObservableObject
    {
        private Staff staff;
        private readonly StaffService _staffService;
        private readonly NurseService _nurseService;
        private readonly RoomAssignmentService _roomAssignmentService;
        private int nurseID;

        // Staff Profile Properties
        [ObservableProperty]
        private int staffId;

        [ObservableProperty]
        private string staffFirstName = string.Empty;

        [ObservableProperty]
        private string staffLastName = string.Empty;

        [ObservableProperty]
        private string staffRole = string.Empty;

        [ObservableProperty]
        private string staffName = string.Empty;

        [ObservableProperty]
        private string staffPhoneNumber = string.Empty;

        [ObservableProperty]
        private string staffDepartment = string.Empty;

        [ObservableProperty]
        private DateTime staffHireDate;

        [ObservableProperty]
        private string staffStatus = string.Empty;

        [ObservableProperty]
        private string staffEmail = string.Empty;

        // View Visibility
        [ObservableProperty]
        private bool showProfileView = true;

        [ObservableProperty]
        private bool showRoomAssignmentsView = false;

        [ObservableProperty]
        private bool showInfoView = false;

        // Room Assignments Collections
        [ObservableProperty]
        private ObservableCollection<RoomAssignment> todayRoomAssignments = new();

        [ObservableProperty]
        private ObservableCollection<RoomAssignment> historyRoomAssignments = new();

        // Tab Management
        [ObservableProperty]
        private bool showTodayTab = true;

        [ObservableProperty]
        private bool showHistoryTab = false;

        [ObservableProperty]
        private string todayTabColor = "#4A90E2";

        [ObservableProperty]
        private string historyTabColor = "#F0F0F0";

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private DateTime? selectedHistoryDate = null;

        [ObservableProperty]
        private int todayCount = 0;

        [ObservableProperty]
        private int historyCount = 0;

        // Update Profile Fields
        [ObservableProperty]
        private string newUsername = string.Empty;

        [ObservableProperty]
        private string newPassword = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        public NurseViewModel(StaffService staffService, NurseService nurseService, RoomAssignmentService roomAssignmentService)
        {
            _staffService = staffService;
            _nurseService = nurseService;
            _roomAssignmentService = roomAssignmentService;

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await LoadStaffData();
                Debug.WriteLine($"NurseViewModel initialized for: {staffName}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialization error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load profile: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadStaffData()
        {
            try
            {
                if (ReceptionistViewModel.user == null)
                {
                    Debug.WriteLine("User is null in LoadStaffData");
                    return;
                }

                Debug.WriteLine($"Loading staff data for ID: {ReceptionistViewModel.user.DocOrStaffId}");

                staff = await _staffService.GetStaff(ReceptionistViewModel.user.DocOrStaffId);

                if (staff == null)
                {
                    Debug.WriteLine("Staff data returned null");
                    await Shell.Current.DisplayAlertAsync("Error", "Staff data not found", "OK");
                    return;
                }

                StaffId = staff.StaffID;
                StaffFirstName = staff.FirstName;
                StaffLastName = staff.LastName;
                StaffRole = staff.Role;
                StaffPhoneNumber = staff.Phone ?? "Not available";
                StaffDepartment = staff.DepartmentName ?? "Not assigned";
                StaffHireDate = staff.HireDate;
                StaffStatus = staff.IsActive ? "Active" : "Inactive";
                StaffEmail = staff.Email;
                StaffName = staff.FullName;

                nurseID = await _nurseService.GetNurseIDByStaffId(staff.StaffID);

                Debug.WriteLine($"Staff data loaded: {staffName} ({staffRole}), NurseID: {nurseID}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadStaffData error: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load staff data: {ex.Message}", "OK");
            }
        }

        // Navigation Commands
        [RelayCommand]
        private void ShowProfile()
        {
            ShowProfileView = true;
            ShowRoomAssignmentsView = false;
            ShowInfoView = false;
            Debug.WriteLine("Showing Profile View");
        }

        [RelayCommand]
        private void ShowRoomAssignments()
        {
            ShowProfileView = false;
            ShowRoomAssignmentsView = true;
            ShowInfoView = false;
            Debug.WriteLine("Showing Room Assignments View");

            _ = LoadRoomAssignmentsForCurrentTab();
        }

        [RelayCommand]
        private void ShowInfo()
        {
            ShowProfileView = false;
            ShowRoomAssignmentsView = false;
            ShowInfoView = true;
            Debug.WriteLine("Showing Update Credentials View");
        }

        // Tab Management Commands
        [RelayCommand]
        private async Task SelectTodayTab()
        {
            try
            {
                ShowTodayTab = true;
                ShowHistoryTab = false;

                TodayTabColor = "#4A90E2";
                HistoryTabColor = "#F0F0F0";

                await LoadTodayRoomAssignments();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SelectTodayTab error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task SelectHistoryTab()
        {
            try
            {
                ShowTodayTab = false;
                ShowHistoryTab = true;

                TodayTabColor = "#F0F0F0";
                HistoryTabColor = "#4A90E2";

                HistoryRoomAssignments.Clear();
                HistoryCount = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SelectHistoryTab error: {ex.Message}");
            }
        }

        private async Task LoadRoomAssignmentsForCurrentTab()
        {
            if (nurseID <= 0)
            {
                Debug.WriteLine("Invalid nurseID, cannot load room assignments");
                return;
            }

            IsLoading = true;

            try
            {
                if (ShowTodayTab)
                {
                    await LoadTodayRoomAssignments();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadRoomAssignmentsForCurrentTab error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load room assignments: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTodayRoomAssignments()
        {
            try
            {
                Debug.WriteLine($"Loading today's room assignments for NurseID: {nurseID}");

                var assignments = await _roomAssignmentService.GetTodayRoomAssignments(nurseID);

                TodayRoomAssignments.Clear();
                foreach (var assignment in assignments)
                {
                    TodayRoomAssignments.Add(assignment);
                    Debug.WriteLine($"Added room assignment: Room {assignment.RoomNumber} - {assignment.PatientName}");
                }

                TodayCount = TodayRoomAssignments.Count;

                Debug.WriteLine($"Loaded {TodayCount} today room assignments");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadTodayRoomAssignments error: {ex.Message}");
                throw;
            }
        }

        [RelayCommand]
        private async Task LoadHistoryByDate()
        {
            if (!SelectedHistoryDate.HasValue)
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please select a date", "OK");
                return;
            }

            if (nurseID <= 0)
            {
                Debug.WriteLine("Invalid nurseID, cannot load history");
                return;
            }

            IsLoading = true;

            try
            {
                Debug.WriteLine($"Loading room assignments for date: {SelectedHistoryDate.Value:yyyy-MM-dd}");

                var assignments = await _roomAssignmentService.GetRoomAssignmentsByDate(nurseID, SelectedHistoryDate.Value);

                HistoryRoomAssignments.Clear();
                foreach (var assignment in assignments)
                {
                    HistoryRoomAssignments.Add(assignment);
                }

                HistoryCount = HistoryRoomAssignments.Count;

                Debug.WriteLine($"Loaded {HistoryCount} history room assignments");

                if (HistoryCount == 0)
                {
                    await Shell.Current.DisplayAlertAsync("No Data", "No room assignments found for the selected date", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadHistoryByDate error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load history: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateProfile()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Please enter a new password", "OK");
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Passwords do not match", "OK");
                    return;
                }

                if (NewPassword.Length < 6)
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Password must be at least 6 characters", "OK");
                    return;
                }

                var passwordUpdated = await _staffService.UpdatePassword($"StaffId", NewPassword);

                if (passwordUpdated)
                {
                    await Shell.Current.DisplayAlertAsync("Success", "Password updated successfully!", "OK");
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Failed to update password", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateProfile error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to update profile: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            try
            {
                ReceptionistViewModel.user = null;
                ReceptionistViewModel.id = 0;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var loginViewModel = MauiProgram.Services.GetRequiredService<LoginViewModel>();
                    var loginPage = new LoginPage(loginViewModel);
                    Application.Current.MainPage = loginPage;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logout error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Logout failed: {ex.Message}", "OK");
            }
        }
    }
}