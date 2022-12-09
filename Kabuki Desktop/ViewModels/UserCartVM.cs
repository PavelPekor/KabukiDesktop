using Kabuki_Desktop.Commands;
using Kabuki_Desktop.EventArgsClasses;
using Kabuki_Desktop.Models;
using Kabuki_Desktop.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kabuki_Desktop.ViewModels
{
    public class UserCartVM : BaseVM 
    {

        public override event EventHandler<ChangeUCEventArgs> OnChangeUC;

        private RelayCommand deleteProductFromCart;
        public RelayCommand DeleteProductFromCart
        {
            get
            {
                return deleteProductFromCart ?? (deleteProductFromCart = new RelayCommand(obj =>
                {
                    var id = (int)obj;
                    var cart = Carts.Where(c => c.Id == id).FirstOrDefault();
                    CartSum -= cart.Quantity * cart.Product.Price;
                    Carts.Remove(cart);
                    using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                    {
                        dBContext.Carts.Remove(cart);
                        dBContext.SaveChanges();
                    }
                }));
            }
        }

        private RelayCommand addQuantity;
        public RelayCommand AddQuantity
        {
            get
            {
                return addQuantity ?? (addQuantity = new RelayCommand(obj =>
                {
                    var id = (int)obj;
                    var cart = Carts.Where(c => c.Id == id).FirstOrDefault();
                    cart.Quantity++;
                    CartSum += cart.Product.Price;
                }));
            }
        }

        private RelayCommand subtractQuantity;
        public RelayCommand SubtractQuantity
        {
            get
            {
                return subtractQuantity ?? (subtractQuantity = new RelayCommand(obj =>
                {
                    var id = (int)obj;
                    var cart = Carts.Where(c => c.Id == id).FirstOrDefault();
                    if (cart.Quantity > 1)
                    {
                        cart.Quantity--;
                        CartSum -= cart.Product.Price;
                    }
                }));
            }
        }

        private RelayCommand backToMainMenu;
        public RelayCommand BackToMainMenu
        {
            get
            {
                return backToMainMenu ?? (backToMainMenu = new RelayCommand(obj =>
                {
                    using(KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                    {
                        foreach(var cart in Carts)
                        {
                            dBContext.Entry(cart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        dBContext.SaveChanges();
                    }
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("MainMenu", User, "MainMenu", new MainMenuVM(User)));
                }));
            }
        }

        private RelayCommand createOrder;
        public RelayCommand CreateOrder
        {
            get
            {
                return createOrder ?? (createOrder = new RelayCommand(obj =>
                {
                    using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                    {
                        var discountCard = dBContext.DiscountCards.Where(c => c.UserId == User.ID && c.NumberCard == NumberCard).FirstOrDefault();

                        if (NumberCard != "" && discountCard == null)
                        {
                            MessageBox.Show($"Скидочной карты с номером {NumberCard} не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        var isDiscountCard = NumberCard == "" ? false : true;
                        var order = new Order() { UserId = User.ID, Address = Address, OrderPrice = CartSum, IsDiscountCard = isDiscountCard, DateTimeOrder = DateTime.Now };
                        dBContext.Orders.Add(order);
                        dBContext.SaveChanges();

                        foreach (var cart in Carts)
                        {
                            dBContext.ProductsInOrders.Add(new ProductsInOrder() { OrderId = order.Id, ProductId = cart.ProductId, Quantity = cart.Quantity });
                            dBContext.Carts.Remove(cart);
                        }                     
                        dBContext.SaveChanges();

                        CartSum = 0; Carts = new ObservableCollection<Cart>();
                        MessageBox.Show($"Заказ удачно оформлен, Вы можете найти его в списке заказов", "Заказ оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnChangeUC?.Invoke(this, new ChangeUCEventArgs("MainMenu", User, "MainMenu", new MainMenuVM(User)));
                    }
                }, obj =>
                {
                    return CartSum > 0 && Address != "";
                })
                );
            }
        }

        private User user;
        public User User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }   

        private string? numberCard;
        public string? NumberCard
        {
            get { return numberCard; }
            set { numberCard = value; OnPropertyChanged(); }
        }

        private string? address;
        public string? Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Cart> carts;
        public ObservableCollection<Cart> Carts
        {
            get { return carts; }
            set { carts = value; OnPropertyChanged(); }
        }

        private int? cartSum;
        public int? CartSum
        {
            get => cartSum;
            set { cartSum = value; OnPropertyChanged(); }
        }

        public UserCartVM(User user, int? cartSum)
        {
            User = user; CartSum = cartSum; NumberCard = ""; Address = "";
            using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
            {
                var c = dBContext.Carts.Where(c => c.UserId == User.ID).ToList();
                Carts = new ObservableCollection<Cart>(c);
                foreach(var cart in Carts)
                {
                    cart.Product = dBContext.Products.Where(p => p.Id == cart.ProductId).FirstOrDefault();
                }
            }
        }
    }
}
