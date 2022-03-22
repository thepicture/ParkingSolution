﻿using ParkingSolution.XamarinApp.Services;
using ParkingSolution.XamarinApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParkingSolution.XamarinApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            if (VersionTracking.IsFirstLaunchForCurrentBuild)
            {
                SecureStorage.RemoveAll();
            }
            if (IsLoggedIn())
            {
                SetShellStacksDependingOnRole();
            }
            else
            {
                LoadLoginAndRegisterShell();
            }
        }

        public void LoadLoginAndRegisterShell()
        {
            CommonTabBar.Items.Clear();
            CommonTabBar
                .Items.Add(new ShellContent
                {
                    Route = nameof(LoginPage),
                    Icon = "call_1",
                    Title = "Авторизация",
                    ContentTemplate = new DataTemplate(typeof(LoginPage))
                });
            CommonTabBar
              .Items.Add(new ShellContent
              {
                  Route = nameof(RegisterPage),
                  Icon = "address",
                  Title = "Регистрация",
                  ContentTemplate = new DataTemplate(typeof(RegisterPage))
              });
        }

        private bool IsLoggedIn()
        {
            return SecureStorage.GetAsync("Identity").Result != null;
        }

        public void SetShellStacksDependingOnRole()
        {
            CommonTabBar.Items.Clear();
            switch (AppIdentity.Role)
            {
                case "Администратор":
                    break;
                case "Сотрудник":
                    break;
                case "Клиент":
                    break;
                default:
                    break;
            }
        }
    }
}