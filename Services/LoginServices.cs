using Hospital_Management_System.Repositories;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hospital_Management_System.Services
{
    public class LoginServices
    {
        private readonly LoginRepository _loginPageRepository;

        public LoginServices(LoginRepository loginPageRepository)
        {
            _loginPageRepository = loginPageRepository;
        }


        public async Task<bool> LoginCheck(string userID,string password)
        {
                UserAccount user =await _loginPageRepository.GetUserData(userID);
                if(user is null)
                {
                    await Application.Current.MainPage.DisplayAlertAsync("User Not Found", "User Does Not Exist", "Ok");
                    return false;
                }
                else if(userID.Equals($"{user.DocOrStaffId}") && password.Equals(user.Password))
                {
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlertAsync("Invalid Credentials","Please Enter Correct Credentials","Ok");
                    return false;
                }
        }
        public async Task<UserAccount> GetUserData(string userID)
        {
            UserAccount user = await _loginPageRepository.GetUserData(userID);
            return user;
        }
    }
}
