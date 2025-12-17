using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hospital_Management_System.Models;
using Hospital_Management_System.Repositories;
using Hospital_Management_System.Services;
using Hospital_Management_System.Views;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Management_System.ViewModels
{
    public partial class DoctorViewModel : ObservableObject
    {


        // Loading control variables
        private bool _isCurrentlyLoading = false;
        private bool _hasInitialLoadCompleted = false;
        private DateTime _lastLoadTime = DateTime.MinValue;
        private string _lastLoadedDoctorId = null;
        private Staff staff;
        private readonly StaffService _staffService;
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        [ObservableProperty]
        bool showAppointmentsView;
        [ObservableProperty]
        bool showProfileView;
        // Profile Properties
        [ObservableProperty]
        private string staffId;

        [ObservableProperty]
        private string staffFirstName;

        [ObservableProperty]
        private string staffLastName;

        [ObservableProperty]
        private string staffRole;

        [ObservableProperty]
        private string staffName;

        [ObservableProperty]
        private string staffPhoneNumber;

        [ObservableProperty]
        private string staffDepartment;
        [ObservableProperty]
        string statusColor;
        [ObservableProperty]
        private DateTime staffHireDate;

        [ObservableProperty]
        private string staffStatus;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string staffEmail;

        [ObservableProperty]
        private string userId;

        // Appointments collections
        [ObservableProperty]
        private ObservableCollection<Appointment> todayAppointments = new ObservableCollection<Appointment>();

        [ObservableProperty]
        private ObservableCollection<Appointment> pendingAppointments = new ObservableCollection<Appointment>();

        [ObservableProperty]
        private ObservableCollection<Appointment> historyAppointments = new ObservableCollection<Appointment>();


        [ObservableProperty]
        private bool showInfoView = false;

        // Appointment tab states
        [ObservableProperty]
        private bool showTodayAppointments = true;

        [ObservableProperty]
        private bool showPendingAppointments = false;

        [ObservableProperty]
        private bool showHistoryAppointments = false;

        // Tab colors
        [ObservableProperty]
        private string todayTabColor = "#EC9C13";

        [ObservableProperty]
        private string pendingTabColor = "AntiqueWhite";

        [ObservableProperty]
        private string historyTabColor = "AntiqueWhite";

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool showNoAppointmentsMessage = false;

        [ObservableProperty]
        private string selectedAppointmentType = "Today";

        [ObservableProperty]
        private int todayCount = 0;

        [ObservableProperty]
        private int pendingCount = 0;

        [ObservableProperty]
        private int historyCount = 0;

        [ObservableProperty]
        private int totalCount = 0;

        // Selected appointment for actions
        [ObservableProperty]
        private Appointment selectedAppointment;

        // Update profile fields
        [ObservableProperty]
        private string newUsername;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        public DoctorViewModel(StaffService staffservice,
            AppointmentService appointment,
            DoctorService doctorService)
        {

            _staffService = staffservice;
            _appointmentService = appointment;
            _doctorService = doctorService;
            ShowProfileView = true;
            ShowAppointmentsView = false;
            // Initialize with empty values to prevent null reference
            StaffId = string.Empty;
            StaffFirstName = string.Empty;
            StaffLastName = string.Empty;
            StaffRole = string.Empty;
            StaffName = string.Empty;
            StaffPhoneNumber = string.Empty;
            StaffDepartment = string.Empty;
            StaffHireDate = DateTime.Now;
            StaffStatus = string.Empty;
            UserName = string.Empty;
            StaffEmail = string.Empty;
            UserId = string.Empty;
            _=LoadStaffData();

        }
        [RelayCommand]
        public void ShowAppointments()
        {
            ShowProfileView = false;
            ShowAppointmentsView = true;
        }
        [RelayCommand]
        public void ShowProfile()
        {
            ShowAppointmentsView = false;
            ShowProfileView = true;
        }

        [RelayCommand]
        private void ShowInfo()
        {
            ShowProfileView = false;
            ShowAppointmentsView = false;
            ShowInfoView = true;
        }


        private async Task LoadStaffData()
        {
            staff = await _staffService.GetStaff(ReceptionistViewModel.user.DocOrStaffId);
            StaffId = staff.StaffID.ToString();
            StaffFirstName = staff.FirstName;
            StaffLastName = staff.LastName;
            StaffRole = staff.Role;
            StaffPhoneNumber = staff.Phone;
            StaffDepartment = staff.DepartmentName;
            StaffHireDate = staff.HireDate;
            StaffStatus = staff.IsActive ? "Active" : "Inactive";
            UserName = staff.FullName;
            StaffEmail = staff.Email;
            UserId = staff.StaffID.ToString();
        }

        [RelayCommand]
        private async Task SelectPendingTab()
        {
            try
            {
                ShowTodayAppointments = false;
                ShowPendingAppointments = true;
                ShowHistoryAppointments = false;

                TodayTabColor = "AntiqueWhite";
                PendingTabColor = "#EC9C13";
                HistoryTabColor = "AntiqueWhite";
                SelectedAppointmentType = "Pending";

                // Load appointments for this tab
                await LoadAppointmentsForCurrentTab();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ SelectPendingTab error: {ex.Message}");
            }
        }

        // Tab switching with data loading
        [RelayCommand]
        private async Task SelectTodayTab()
        {
            try
            {

                // 1. Update visibility flags
                ShowTodayAppointments = true;
                ShowPendingAppointments = false;
                ShowHistoryAppointments = false;

                // 2. Update Tab Colors (Using new design palette)
                TodayTabColor = "#FFB93D";      // Active: Brighter Orange-Yellow
                PendingTabColor = "#F0F0F0";    // Inactive: Clean Light Gray
                HistoryTabColor = "#F0F0F0";    // Inactive: Clean Light Gray

                SelectedAppointmentType = "Today";

                // 3. Load appointments for this tab
                await LoadAppointmentsForCurrentTab();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ SelectTodayTab error: {ex.Message}");
            }
        }


        // Load appointments for current tab 
        private async Task LoadAppointmentsForCurrentTab()
        {
            if (_isCurrentlyLoading)
            {
                return;
            }

            if (staff == null)
            {
                ShowNoAppointmentsMessage = true;
                return;
            }

            _isCurrentlyLoading = true;
            IsLoading = true;

            try
            {
                ShowNoAppointmentsMessage = false;

                // Load appointments based on current tab
                if (ShowTodayAppointments)
                {
                    await LoadTodayAppointments();
                }
                else if (ShowPendingAppointments)
                {
                    await LoadPendingAppointments();
                }
                else if (ShowHistoryAppointments)
                {
                    await LoadHistoryAppointments();
                }

                // Update load tracking
                _hasInitialLoadCompleted = true;
                _lastLoadTime = DateTime.Now;

            }
            catch (Exception ex)
            {
                ShowNoAppointmentsMessage = true;
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load appointments: {ex.Message}", "Okay");
            }
            finally
            {
                IsLoading = false;
                _isCurrentlyLoading = false;
            }
        }
        private async Task LoadTodayAppointments()
        {
            try
            {
                int docID = await _doctorService.GetDocIDByStaffId(staff.StaffID);
                var appointments = await _appointmentService.GetTodayAppointmentsByDocID(docID);
                TodayAppointments.Clear();
                foreach (var appointment in appointments)
                {
                    TodayAppointments.Add(appointment);
                }
                TodayCount = TodayAppointments.Count;
                ShowNoAppointmentsMessage = TodayCount == 0;


            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load Today appointments: {ex.Message}", "Okay");
            }
        }



        [RelayCommand]
        private async Task SelectHistoryTab()
        {
            try
            {
                if (ShowHistoryAppointments)
                    return;


                ShowTodayAppointments = false;
                ShowPendingAppointments = false;
                ShowHistoryAppointments = true;

                // Update Tab Colors (Using new design palette)
                TodayTabColor = "#F0F0F0";
                PendingTabColor = "#F0F0F0";
                HistoryTabColor = "#FFB93D";    // Active

                SelectedAppointmentType = "History";

                await LoadAppointmentsForCurrentTab();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ SelectHistoryTab error: {ex.Message}");
            }
        }


        //Loads the pending appointments for the logged in doctor
        private async Task LoadPendingAppointments()
        {
            try
            {
                int docID = await _doctorService.GetDocIDByStaffId(staff.StaffID);
                var appointments = await _appointmentService.GetPendingAppointmentsByDocID(docID);
                PendingAppointments.Clear();
                foreach (var appointment in appointments)
                {
                    PendingAppointments.Add(appointment);

                }
                PendingCount = PendingAppointments.Count;
                ShowNoAppointmentsMessage = PendingCount == 0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ LoadPendingAppointments error: {ex.Message}");
            }
        }

        // Load REAL data from database
        private async Task LoadRealDoctorData()
        {
            try
            {

                // Load fresh data from database
                var freshStaffData = await _staffService.GetStaff(ReceptionistViewModel.user.DocOrStaffId);
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
                    // The StaffUpdateService should handle the hashing
                    var passwordUpdated = await staffUpdateService.UpdatePassword(StaffId, NewPassword);

                    if (passwordUpdated)
                    {
                        updateMessage += "✓ Password updated successfully\n";
                        anyUpdate = true;
                        Debug.WriteLine("✅ Password updated in database");

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


        private async Task LoadHistoryAppointments()
        {
            try
            {
                int docID = await _doctorService.GetDocIDByStaffId(staff.StaffID);
                var appointments = await _appointmentService.GetHistoryAppointmentByDocID(docID);

                HistoryAppointments.Clear();
                foreach (var appointment in appointments)
                {
                    HistoryAppointments.Add(appointment);
                }
                HistoryCount = HistoryAppointments.Count;
                ShowNoAppointmentsMessage = HistoryCount == 0;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to load History appointments: {ex.Message}", "Okay");

            }
        }


        [RelayCommand]
        private async Task Logout()
        {
            try
            {
                // Reset all state
                ResetState();

                var loginViewModel = MauiProgram.Services.GetRequiredService<LoginViewModel>();
                var loginPage = new LoginPage(loginViewModel);
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Logout failed: {ex.Message}", "Okay");
            }
        }

        [RelayCommand]
        private async Task CreateVisit(Appointment appointment)
        {
            await Shell.Current.GoToAsync(nameof(VisitPage), true, new Dictionary<string, object>
            {
                {"SelectedAppointment",appointment },
                {"Doctor",staff}
            });
        }
        private void ResetState()
        {
            TodayAppointments.Clear();
            PendingAppointments.Clear();
            HistoryAppointments.Clear();

            TodayCount = 0;
            PendingCount = 0;
            HistoryCount = 0;
            TotalCount = 0;

            ShowNoAppointmentsMessage = false;
            IsLoading = false;

            _hasInitialLoadCompleted = false;
            _lastLoadTime = DateTime.MinValue;
            _lastLoadedDoctorId = null;
            _isCurrentlyLoading = false;
        }
        public async void OnAppearing()
        {
            await LoadAppointmentsForCurrentTab();
        }
    }
}
