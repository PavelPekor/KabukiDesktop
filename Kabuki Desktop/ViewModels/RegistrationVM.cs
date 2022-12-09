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
    public class RegistrationVM : BaseVM
    {
        public override event EventHandler<ChangeUCEventArgs> OnChangeUC;

        #region Команды
        private RelayCommand returnToAuthorization;
        public RelayCommand ReturnToAuthorization
        {
            get
            {
                return returnToAuthorization ?? (returnToAuthorization = new RelayCommand(obj =>
                {
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("Authorization"));
                }));
            }
        }

        private RelayCommand register;
        public RelayCommand Register
        {
            get
            {
                return register ?? (register = new RelayCommand(obj =>
                {
                    using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                    {
                        var user = new User() {Name = this.Name, Password = this.Password, TelephoneNumber = this.TelephoneNumber };
                        if (dBContext.Users.Where(u => u.TelephoneNumber == user.TelephoneNumber).FirstOrDefault() != null)
                            MessageBox.Show("Пользователь с таким номером телефона уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            dBContext.Users.Add(user);
                            dBContext.SaveChanges();
                            OnChangeUC?.Invoke(this, new ChangeUCEventArgs("MainMenu", user, "MainMenu", new MainMenuVM(user)));
                        }
                    }             
                }));
            }
        }
        #endregion

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

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
