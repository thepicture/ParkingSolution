using ParkingSolution.XamarinApp.Models.Serialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {

        private ObservableCollection<SerializedUser> employees;

        internal void OnAppearing()
        {
            IsRefreshing = true;
        }

        public ObservableCollection<SerializedUser> Employees
        {
            get => employees;
            set => SetProperty(ref employees, value);
        }

        private Command<SerializedUser> deleteEmployeeCommand;

        public Command<SerializedUser> DeleteEmployeeCommand
        {
            get
            {
                if (deleteEmployeeCommand == null)
                {
                    deleteEmployeeCommand = new Command<SerializedUser>(DeleteEmployeeAsync);
                }

                return deleteEmployeeCommand;
            }
        }

        private async void DeleteEmployeeAsync(SerializedUser user)
        {
            if (await UserDataStore.DeleteItemAsync(user.Id
                .ToString()))
            {
                IsRefreshing = true;
            }
        }

        private Command refreshCommand;

        public EmployeesViewModel()
        {
            Employees = new ObservableCollection<SerializedUser>();
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new Command(Refresh);
                }

                return refreshCommand;
            }
        }

        private void Refresh()
        {
            LoadEmployeesAsync();
        }

        private async void LoadEmployeesAsync()
        {
            Employees.Clear();
            IEnumerable<SerializedUser> employeesResponse =
                await UserDataStore.GetItemsAsync();
            foreach (SerializedUser employee in employeesResponse)
            {
                await Task.Delay(500);
                Employees.Add(employee);
            }
            IsRefreshing = false;
        }
    }
}