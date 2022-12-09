using Kabuki_Desktop.Commands;
using Kabuki_Desktop.EventArgsClasses;
using Kabuki_Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kabuki_Desktop.ViewModels
{
    public class UserOrdersVM : BaseVM
    {
        public override event EventHandler<ChangeUCEventArgs> OnChangeUC;

        private RelayCommand backToMainMenu;
        public RelayCommand BackToMainMenu
        {
            get
            {
                return backToMainMenu ?? (backToMainMenu = new RelayCommand(obj =>
                {
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("MainMenu", User, "MainMenu", new MainMenuVM(User)));
                }));
            }
        }

        private List<Order> orders;

        public List<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }

        private User user;
        public User User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }
        public UserOrdersVM(User user)
        {
            User = user;
            using(KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
            {
                Orders = dBContext.Orders.ToList();
                foreach(var order in Orders)
                {
                    order.ProductsInOrders = dBContext.ProductsInOrders.Where(o => o.OrderId == order.Id).ToList();
                    foreach(var product in order.ProductsInOrders)
                    {
                        product.Product = dBContext.Products.FirstOrDefault(p => p.Id == product.ProductId);
                    }
                }
            }
        }
    }
}
