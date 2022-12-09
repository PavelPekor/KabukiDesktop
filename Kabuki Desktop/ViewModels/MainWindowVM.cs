using Kabuki_Desktop.Commands;
using Kabuki_Desktop.EventArgsClasses;
using Kabuki_Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kabuki_Desktop.ViewModels
{
    public class MainWindowVM : BaseVM
    {
        #region Команды
        private RelayCommand changeApplicationState;
        public RelayCommand СhangeApplicationState
        {
            get
            {
                return changeApplicationState ?? (changeApplicationState = new RelayCommand(obj =>
                {
                    var action = obj as string;
                    switch (action)
                    {
                        case "close":
                            Application.Current.MainWindow.Close();
                            break;
                        case "minimize":
                            Application.Current.MainWindow.WindowState = WindowState.Minimized;
                            break;
                        case "maximize":
                            Application.Current.MainWindow.WindowState =
                            Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                            break;
                        default:
                            break;
                    }
                }));
            }
        }

        private RelayCommand logOut;
        public RelayCommand LogOut
        {
            get
            {
                return logOut ?? (logOut = new RelayCommand(obj =>
                {
                    CurrentContent = ViewModels["Authorization"];
                    var c = (AuthorizationVM)CurrentContent;
                    c.Password = c.TelephoneNumber = null;
                    User = null;
                }));
            }
        }
        #endregion

        public Dictionary<string, BaseVM> ViewModels;

        private User user;
        public User User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }

        private BaseVM currentContent;
        public BaseVM CurrentContent
        {
            get => currentContent;
            set { currentContent = value; OnPropertyChanged(); }
        }
        public MainWindowVM()
        {
            ViewModels = new Dictionary<string, BaseVM>()
            {
                {"Authorization", new AuthorizationVM()},
                {"Registration", new RegistrationVM()}
            };

            foreach(var viewModel in ViewModels.Values)
            {
                viewModel.OnChangeUC += ChangeUC;
            }

            CurrentContent = ViewModels["Authorization"];
        }

        private void ChangeUC(object sender, ChangeUCEventArgs e)
        {
            if (e.NewUCName != null && e.NewUCObject != null && !ViewModels.ContainsKey(e.NewUCName))
            {
                ViewModels.Add(e.NewUCName, e.NewUCObject);
                var vm = ViewModels[e.NewUCName];
                vm.OnChangeUC += ChangeUC;
            }
            if (e.NewUCName != null && e.NewUCObject != null && ViewModels.ContainsKey(e.NewUCName))
            {
                ViewModels[e.NewUCName] = e.NewUCObject;
                var vm = ViewModels[e.NewUCName];
                vm.OnChangeUC += ChangeUC;
            }
            CurrentContent = ViewModels[e.UserControlName];
            User ??= e.User;           
        }

    }
}
