using Newtonsoft.Json;
using ParkingSolution.XamarinApp.Models.Serialized;
using ParkingSolution.XamarinApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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
            Employees = new ObservableCollection<SerializedUser>();
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
            if (!await FeedbackService.Ask($"Удалить сотрудника {user.PhoneNumber}?"))
            {
                return;
            }
            if (await UserDataStore.DeleteItemAsync(user.Id
                .ToString()))
            {
                await FeedbackService.Inform("Сотрудник удалён");
                IsRefreshing = true;
            }
            else
            {
                await FeedbackService.InformError("Сотрудник не удалён. " +
                    "Перезайдите на страницу");
            }
        }

        private Command refreshCommand;

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
            Task.Run(() =>
            {
                LoadEmployeesAsync();
            });
        }

        private async void LoadEmployeesAsync()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Employees.Clear();
            });

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                  new AuthenticationHeaderValue("Basic",
                                                AppIdentity.AuthorizationValue);
                client.BaseAddress = new Uri((App.Current as App).BaseUrl);
                try
                {
                    string response = await client
                        .GetAsync($"users/employees")
                        .Result
                        .Content
                        .ReadAsStringAsync();
                    IEnumerable<SerializedUser> employeesResponse =
                        JsonConvert.DeserializeObject
                        <IEnumerable<SerializedUser>>
                        (response);
                    foreach (SerializedUser employee
                        in employeesResponse)
                    {
                        await Task.Delay(500);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Employees.Add(employee);
                        });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    await FeedbackService.Inform(
                        "Сотрудники не подгружены. " +
                        "Перезайдите на страницу");
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                IsRefreshing = false;
            });
        }
    }
}