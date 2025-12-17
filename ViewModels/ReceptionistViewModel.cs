using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repository;
using Hospital_Management_System.Services;
using Hospital_Management_System.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Management_System.ViewModels
{
    /// <summary>
    /// ViewModel for Receptionist Dashboard
    /// Handles profile management, password updates, and room assignment functionality
    /// </summary>
    public partial class ReceptionistViewModel : ObservableObject
    {
        private readonly StaffService _staffService;
        private readonly RoomAssignmentRepository _roomAssignmentRepository;

        public static int id;
        public static UserAccount user;

        // ============================================
        // PROFILE PROPERTIES
        // ============================================
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
        private string staffEmail = string.Empty;

        [ObservableProperty]
        private string staffPhoneNumber = string.Empty;

        [ObservableProperty]
        private string staffDepartment = string.Empty;

        [ObservableProperty]
        private DateTime staffHireDate;

        [ObservableProperty]
        private string staffStatus = string.Empty;

        [ObservableProperty]
        private string userId = string.Empty;

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string userPassword = string.Empty;

        // ============================================
        // VIEW TOGGLE PROPERTIES
        // ============================================
        [ObservableProperty]
        private bool profileViewStatus = true;

        [ObservableProperty]
        private bool infoViewState = false;

        [ObservableProperty]
        private bool roomAssignmentViewState = false;

        // ============================================
        // ROOM ASSIGNMENT DATA COLLECTIONS
        // ============================================
        [ObservableProperty]
        private ObservableCollection<Nurse> nurses = new();

        [ObservableProperty]
        private ObservableCollection<Room> availableRooms = new();

        [ObservableProperty]
        private ObservableCollection<RoomAssignment> todayAssignments = new();

        // ============================================
        // ROOM ASSIGNMENT SELECTION PROPERTIES
        // ============================================
        [ObservableProperty]
        private Nurse selectedNurse = null;

        [ObservableProperty]
        private Room selectedRoom = null;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now;

        // ============================================
        // UI CONTROL PROPERTIES
        // ============================================
        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string assignMessage = string.Empty;

        [ObservableProperty]
        private bool assignMessageVisible = false;

        [ObservableProperty]
        private string messageColor = "#27AE60";

        [ObservableProperty]
        private int todayCount = 0;

        // ============================================
        // PASSWORD UPDATE PROPERTIES
        // ============================================
        [ObservableProperty]
        private string newPassword = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        // ============================================
        // CONSTRUCTOR
        // ============================================
        public ReceptionistViewModel(
            StaffService staffService,
            RoomAssignmentRepository roomAssignmentRepository)
        {
            _staffService = staffService;
            _roomAssignmentRepository = roomAssignmentRepository;

            _ = InitializeAsync();
        }

        // ============================================
        // INITIALIZATION
        // ============================================
        private async Task InitializeAsync()
        {
            try
            {
                if (user != null)
                {
                    await LoadStaffData(user.DocOrStaffId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Initialization error: {ex.Message}");
            }
        }

        // ============================================
        // VIEW TOGGLE COMMANDS
        // ============================================
        [RelayCommand]
        public void ToggleProfileView()
        {
            ProfileViewStatus = true;
            InfoViewState = false;
            RoomAssignmentViewState = false;
            Debug.WriteLine("✅ Showing Profile View");
        }

        [RelayCommand]
        public void ToggleInfoView()
        {
            ProfileViewStatus = false;
            InfoViewState = true;
            RoomAssignmentViewState = false;
            Debug.WriteLine("✅ Showing Info View");
        }

        /// <summary>
        /// Toggle room assignment view and load necessary data
        /// </summary>
        [RelayCommand]
        public async Task ToggleRoomAssignmentView()
        {
            try
            {
                ProfileViewStatus = false;
                InfoViewState = false;
                RoomAssignmentViewState = true;

                IsLoading = true;
                await LoadNursesAsync();
                await LoadAvailableRoomsAsync();
                await LoadTodayAssignmentsAsync();
                IsLoading = false;

                Debug.WriteLine("✅ Room Assignment View opened");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ToggleRoomAssignmentView error: {ex.Message}");
                IsLoading = false;
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        // ============================================
        // STAFF DATA LOADING
        // ============================================
        public async Task LoadStaffData(int staffId)
        {
            try
            {
                Debug.WriteLine($"🔄 Loading staff data for ID: {staffId}");

                Staff staff = await _staffService.GetStaff(staffId);

                if (staff != null)
                {
                    StaffId = staff.StaffID;
                    StaffFirstName = staff.FirstName;
                    StaffLastName = staff.LastName;
                    StaffRole = staff.Role;
                    StaffName = $"{staff.FirstName} {staff.LastName}";
                    StaffEmail = staff.Email;
                    StaffPhoneNumber = staff.Phone;
                    StaffDepartment = staff.DepartmentName;
                    StaffHireDate = staff.HireDate;
                    StaffStatus = staff.IsActive ? "Active" : "Inactive";
                    UserId = $"{staff.StaffID}";
                    UserName = $"{staff.FirstName} {staff.LastName}";
                    UserPassword = $"{staff.Password}";

                    Debug.WriteLine($"✅ Staff data loaded: {StaffName} ({StaffRole})");
                }
                else
                {
                    Debug.WriteLine($"❌ Staff not found for ID: {staffId}");
                    await Shell.Current.DisplayAlertAsync("Staff Not Found", "Staff with mentioned user id cannot be accessed.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadStaffData error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load staff data: {ex.Message}", "OK");
            }
        }

        // ============================================
        // ROOM ASSIGNMENT DATA LOADING METHODS
        // ============================================
        /// <summary>
        /// Load all active nurses
        /// </summary>
        private async Task LoadNursesAsync()
        {
            try
            {
                var nurseList = await _roomAssignmentRepository.GetAllNursesAsync();
                Nurses.Clear();

                foreach (var nurse in nurseList)
                {
                    Nurses.Add(nurse);
                }

                Debug.WriteLine($"✅ Loaded {Nurses.Count} nurses");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadNursesAsync error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load available rooms for selected date
        /// Only rooms not assigned to any nurse on that date
        /// </summary>
        private async Task LoadAvailableRoomsAsync()
        {
            try
            {
                var roomList = await _roomAssignmentRepository.GetAvailableRoomsAsync(SelectedDate);
                AvailableRooms.Clear();

                foreach (var room in roomList)
                {
                    AvailableRooms.Add(room);
                }

                Debug.WriteLine($"✅ Loaded {AvailableRooms.Count} available rooms for {SelectedDate:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadAvailableRoomsAsync error: {ex.Message}");
            }
        }

        /// <summary>
        /// Load today's room assignments
        /// </summary>
        private async Task LoadTodayAssignmentsAsync()
        {
            try
            {
                var assignments = await _roomAssignmentRepository.GetRoomAssignmentsByDateAsync(DateTime.Now.Date);
                TodayAssignments.Clear();

                foreach (var assignment in assignments)
                {
                    TodayAssignments.Add(assignment);
                }

                TodayCount = TodayAssignments.Count;
                Debug.WriteLine($"✅ Loaded {TodayCount} today's assignments");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadTodayAssignmentsAsync error: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AssignRoom()
        {
            try
            {
                // Validation
                if (SelectedNurse == null)
                {
                    ShowMessage("❌ Please select a nurse", false);
                    return;
                }

                if (SelectedRoom == null)
                {
                    ShowMessage("❌ Please select a room", false);
                    return;
                }

                if (SelectedDate.Date < DateTime.Now.Date)
                {
                    ShowMessage("❌ Cannot assign to past dates", false);
                    return;
                }

                IsLoading = true;

                // Call repository method that handles all business rules
                // Business Rules Enforced:
                // 1. No room assigned to multiple nurses in a day
                // 2. Nurse max 10 rooms per day
                // 3. Only creator can remove
                var result = await _roomAssignmentRepository.AssignRoomToNurseAsync(
                    SelectedNurse.NurseID,
                    SelectedRoom.RoomID,
                    SelectedDate,
                    StaffId
                );

                if (result.Success)
                {
                    ShowMessage(result.Message, true);
                    SelectedNurse = null;
                    SelectedRoom = null;

                    // Reload data
                    await LoadAvailableRoomsAsync();
                    await LoadTodayAssignmentsAsync();
                    await LoadNursesAsync();

                    // Auto-hide message after 2 seconds
                    await Task.Delay(2000);
                    AssignMessageVisible = false;
                }
                else
                {
                    ShowMessage(result.Message, false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AssignRoom error: {ex.Message}");
                ShowMessage($"❌ Error: {ex.Message}", false);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ============================================
        // REMOVE ASSIGNMENT COMMAND
        // ============================================
        /// <summary>
        /// Remove a room assignment
        /// Only the receptionist who created it can remove it
        /// </summary>
        [RelayCommand]
        private async Task RemoveAssignment(int assignmentId)
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlert(
                    "Confirm Removal",
                    "Are you sure you want to remove this room assignment?",
                    "Yes",
                    "No"
                );

                if (!confirm)
                    return;

                IsLoading = true;

                bool result = await _roomAssignmentRepository.RemoveAssignmentAsync(assignmentId, StaffId);

                if (result)
                {
                    ShowMessage("✅ Room assignment removed successfully", true);
                    await LoadTodayAssignmentsAsync();
                    await LoadAvailableRoomsAsync();
                    await LoadNursesAsync();

                    await Task.Delay(1500);
                    AssignMessageVisible = false;
                }
                else
                {
                    ShowMessage("❌ Failed to remove assignment. Only creator can remove it.", false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ RemoveAssignment error: {ex.Message}");
                ShowMessage($"❌ Error: {ex.Message}", false);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ============================================
        // CLEAR SELECTION COMMAND
        // ============================================
        [RelayCommand]
        private void ClearSelection()
        {
            SelectedNurse = null;
            SelectedRoom = null;
            SelectedDate = DateTime.Now;
            AssignMessageVisible = false;
            Debug.WriteLine("✅ Selection cleared");
        }

        // ============================================
        // PASSWORD UPDATE COMMAND
        // ============================================
        [RelayCommand]
        private async Task UpdatePassword()
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

                var passwordUpdated = await _staffService.UpdatePassword($"{StaffId}", NewPassword);

                if (passwordUpdated)
                {
                    await Shell.Current.DisplayAlertAsync("Success", "Password updated successfully!", "OK");
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
                    await LoadStaffData(StaffId);
                }
                else
                {
                    await Shell.Current.DisplayAlertAsync("Error", "Failed to update password", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ UpdatePassword error: {ex.Message}");
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to update password: {ex.Message}", "OK");
            }
        }

        // ============================================
        // LOGOUT COMMAND
        // ============================================
        [RelayCommand]
        async Task OnLogoutClicked()
        {
            try
            {
                user = null;
                id = 0;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var loginViewModel = MauiProgram.Services.GetRequiredService<LoginViewModel>();
                    var loginPage = new LoginPage(loginViewModel);
                    Application.Current.MainPage = loginPage;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Logout error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Logout failed: {ex.Message}", "OK");
            }
        }

        // ============================================
        // HELPER METHODS
        // ============================================
        /// <summary>
        /// Show status message with color coding
        /// Green (#27AE60) for success
        /// Red (#E74C3C) for errors
        /// </summary>
        private void ShowMessage(string message, bool isSuccess)
        {
            AssignMessage = message;
            MessageColor = isSuccess ? "#27AE60" : "#E74C3C";
            AssignMessageVisible = true;
            Debug.WriteLine($"Message: {message} (Success: {isSuccess})");
        }

        /// <summary>
        /// Handle date change - reload available rooms for new date
        /// </summary>
        partial void OnSelectedDateChanged(DateTime value)
        {
            Debug.WriteLine($"Date changed to: {value:yyyy-MM-dd}");
            if (RoomAssignmentViewState)
            {
                _ = LoadAvailableRoomsAsync();
            }
        }
    }
}