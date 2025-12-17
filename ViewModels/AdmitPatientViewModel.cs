using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repository;
using Hospital_Management_System.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;


namespace Hospital_Management_System.ViewModels
{
    public partial class AdmitPatientViewModel:ObservableObject
    {

        private readonly StaffService _staffService;
        private readonly RoomAssignmentRepository _roomAssignmentRepository;
        [ObservableProperty]
        bool toggleNewAssignment = true;
        [ObservableProperty]
        bool toggleManageAssignment = false;
        [ObservableProperty]
        bool toggleAssignmentHistory=false;
        [ObservableProperty]
        private ObservableCollection<Nurse> availableNurses = new();

        [ObservableProperty]
        private ObservableCollection<Room> availableRooms = new();

        [ObservableProperty]
        private ObservableCollection<RoomAssignment> todayAssignments = new();
        [ObservableProperty]
        private Nurse selectedNurse = null;

        [ObservableProperty]
        private Room selectedRoom = null;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Now;

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
        [ObservableProperty]
        string patientIDInput;
        [ObservableProperty]
        string roomNumberInput;

        [ObservableProperty]
        string selectedRoomStatus;
        [ObservableProperty]
        DateTime assignmentDate;
        [ObservableProperty]
        DateTime? dischargeDate;
        [ObservableProperty]
        string staffName;
        [ObservableProperty]
        string staffRole;
        [ObservableProperty]
        string staffDepartment;
        public AdmitPatientViewModel()
        {
            _staffService = MauiProgram.Services.GetRequiredService<StaffService>();
            _roomAssignmentRepository = MauiProgram.Services.GetRequiredService<RoomAssignmentRepository>();
            LoadStaffData();
            _ = LoadNursesAsync();
        }

        private void ClearSelection()
        {
            PatientIDInput = string.Empty;
            RoomNumberInput = string.Empty;
            SelectedNurse = null;
            SelectedRoom = null;
            SelectedRoomStatus = null;
            AssignmentDate = DateTime.Today;
            DischargeDate = null;
        }

        private bool CanAssignRoom()
        {
            return SelectedRoom != null && SelectedNurse != null && !string.IsNullOrWhiteSpace(PatientIDInput);
        }

        private void OnRoomTapped(object? parameter)
        {
            //if (parameter is RoomAssignment ra)
            //{
            //    SelectedRoomAssignment = ra;
            //}
        }

        private void OnRoomHistoryTapped(object? parameter)
        {
            //if (parameter is RoomAssignment ra)
            //{
            //    SelectedRoomAssignmentHistory = ra;
            //}
        }
        private async Task LoadNursesAsync()
        {
            try
            {
                var nurseList = await _roomAssignmentRepository.GetAllNursesAsync();
                AvailableNurses.Clear();

                foreach (var nurse in nurseList)
                {
                    AvailableNurses.Add(nurse);
                }

                Debug.WriteLine($"✅ Loaded {AvailableNurses.Count} nurses");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadNursesAsync error: {ex.Message}");
            }
        }

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

                }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadAvailableRoomsAsync error: {ex.Message}");
            }
        }
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
                    await Shell.Current.DisplayAlertAsync("Error", "❔Please select a nurse.", "Okay");
                    return;
                }

                if (SelectedRoom == null)
                {
                    await Shell.Current.DisplayAlertAsync("Error", "❔Please Select a room.", "Okay");
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
                    AssignmentDate,
                    2003
                );

                if (result.Success)
                {
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ AssignRoom error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        public void LoadStaffData()
        {
            StaffName = ReceptionistViewModel.user.Username;
            StaffRole = ReceptionistViewModel.user.Role;
            StaffDepartment = ReceptionistViewModel.user.DepartmentName;
        }
        [RelayCommand]
        private void ShowNewRoomAssignment()
        {
            ToggleNewAssignment = true;
            ToggleManageAssignment = false;
            ToggleAssignmentHistory = false;
        }
        [RelayCommand]
        private void ShowManageAssignment()
        {
            ToggleManageAssignment = true;
            ToggleNewAssignment = false;
            ToggleAssignmentHistory = false;
        }
        [RelayCommand]
        private void ShowAssignmentHistory()
        {
            ToggleNewAssignment = false;
            ToggleManageAssignment = false;
            ToggleAssignmentHistory = true;
        }
    }
}
