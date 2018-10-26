using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace RtrackerV3Wpf.ViewModels
{
    sealed class SettingsViewModel : Screen
    {
        private string _password;
        private string _password2;
        private string _remoteHost;
        private string _username;

        public string RemoteHost
        {
            get => _remoteHost; set
            {
                Set(ref _remoteHost, value);
                NotifyOfPropertyChange(nameof(CanSave));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                Set(ref _username, value);
                NotifyOfPropertyChange(nameof(CanSave));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                Set(ref _password, value);
                NotifyOfPropertyChange(nameof(CanSave));
            }
        }

        public string Password2
        {
            get => _password2;
            set
            {
                Set(ref _password2, value);
                NotifyOfPropertyChange(nameof(CanSave));
            }
        }

        public bool CanSave => !string.IsNullOrWhiteSpace(RemoteHost) &&
                               !string.IsNullOrWhiteSpace(Username) &&
                               !string.IsNullOrWhiteSpace(Password) &&
                               Password == Password2;

        public void Save()
        {
            Properties.Settings.Default.REMOTE_HOST = RemoteHost;
            Properties.Settings.Default.USERNAME = Username;
            Properties.Settings.Default.PASSWORD = Password;
            Properties.Settings.Default.Save();

            Process.Start(Process.GetCurrentProcess().ProcessName);
            Application.Current.Shutdown(0);
        }

        public void Quit()
        {
            Application.Current.Shutdown(0);
        }

        public SettingsViewModel()
        {
            DisplayName = "Settings";
        }

        protected override void OnViewReady(object view)
        {
            RemoteHost = Properties.Settings.Default.REMOTE_HOST;
            Username = Properties.Settings.Default.USERNAME;
            Password = Properties.Settings.Default.PASSWORD;
            Password2 = Properties.Settings.Default.PASSWORD;
        }
    }
}
