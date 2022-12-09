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
    public class AuthorizationVM : BaseVM
    {
        public override event EventHandler<ChangeUCEventArgs> OnChangeUC;

        #region Команды
        private RelayCommand openRegistrationUC;
        public RelayCommand OpenRegistrationUC
        {
            get
            {
                return openRegistrationUC ?? (openRegistrationUC = new RelayCommand(obj =>
                {
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("Registration"));
                }));
            }
        }

        
        private RelayCommand authorize;
        public RelayCommand Authorize
        {
            get
            {
                return authorize ?? (authorize = new RelayCommand(obj =>
                {
                    using(KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                    {
                        var user = dBContext.Users.Where(u => u.TelephoneNumber == TelephoneNumber && u.Password == Password).FirstOrDefault();
                        if (user != null)
                            OnChangeUC?.Invoke(this, new ChangeUCEventArgs("MainMenu", user, "MainMenu", new MainMenuVM(user)));
                        else
                            MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }                  
                }));
            }
        }
        #endregion

        private string telephoneNumber;
        public string TelephoneNumber
        {
            get { return telephoneNumber; }
            set { telephoneNumber = value; OnPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

    }
}
