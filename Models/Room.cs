using System;

namespace Hospital_Management_System.Models
{
    public class Room
    {
        private int _roomID;
        private string _roomNumber;
        private string _ward;
        private string _roomType;
        private int _capacity;
        private double _dailyRate;

        public int RoomID { get => _roomID; set => _roomID = value; }
        public string RoomNumber { get => _roomNumber; set => _roomNumber = value; }
        public string Ward { get => _ward; set => _ward = value; }
        public string RoomType { get => _roomType; set => _roomType = value; }
        public int Capacity { get => _capacity; set => _capacity = value; }
        public double DailyRate { get => _dailyRate; set => _dailyRate = value; }
    }
}