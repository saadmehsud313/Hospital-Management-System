using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Hospital_Management_System.Views;
using Hospital_Management_System.Services;
using Hospital_Management_System.Models;
namespace Hospital_Management_System.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
       
        public MainViewModel(LoginServices mainPageServices,  UserAccount userAccount)
        {
        }
        
    }
}
