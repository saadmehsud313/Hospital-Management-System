using Hospital_Management_System.Repositories;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;

namespace Hospital_Management_System.Services
{
    public class LoginServices
    {
        private readonly HttpClient _httpClient;
        private readonly LoginRepository _loginPageRepository;

        public LoginServices(LoginRepository loginPageRepository,IHttpClientFactory factory)
        {
            _loginPageRepository = loginPageRepository;
            _httpClient = factory.CreateClient("api");
        }


        public async Task<bool> LoginCheck(string userID,string password)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Login/{userID}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await Application.Current.MainPage.DisplayAlertAsync("User Not Found", "User Does Not Exist", "Ok");
                return false;
            }
            else if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                UserAccount? user = JsonConvert.DeserializeObject<UserAccount?>(responseData);
                if(user is null)
                {
                    await Application.Current.MainPage.DisplayAlertAsync("Error", "User data is null.", "Ok");
                    return false;
                }
                else if (userID.Equals($"{user.Email}") && password.Equals(user.Password))
                {
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlertAsync("Invalid Credentials", "Please Enter Correct Credentials", "Ok");
                    return false;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                await Application.Current.MainPage.DisplayAlertAsync("Server Error", "An Error Occured In The Server.", "Ok");
                return false;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlertAsync("Error", "An error occurred while processing your request.", "Ok");
                return false;
            }
        }
        public async Task<UserAccount> GetUserData(string userID)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Login/{userID}");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                UserAccount? user = JsonConvert.DeserializeObject<UserAccount?>(responseData);
                if (user is null)
                {
                    Debug.WriteLine("User data is null.");
                }
                return user;

            }
            else
            {
                Debug.WriteLine("Failed to retrieve user data.");
                return null;
            }
                    

        }
    }
}
