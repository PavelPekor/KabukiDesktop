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
    public class MainMenuVM : BaseVM 
    {
        public override event EventHandler<ChangeUCEventArgs> OnChangeUC;

        private RelayCommand changeCategory;
        public RelayCommand ChangeCategory
        {
            get
            {
                return changeCategory ?? (changeCategory = new RelayCommand(obj =>
                {
                    var category = obj as string;
                    Products = category == "Все" ? allProducts : allProducts.Where(c => c.CategoryNavigation.Name == category).ToList();
                }));
            }
        }

        private RelayCommand addProductToCart;
        public RelayCommand AddProductToCart
        {
            get
            {
                return addProductToCart ?? (addProductToCart = new RelayCommand(obj =>
                {
                var product = obj as Product;
                using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
                {
                    var cart = dBContext.Carts.Where(c => c.UserId == User.ID && c.ProductId == product.Id).FirstOrDefault();

                    if (cart != null)
                    {
                        cart.Quantity++;
                        dBContext.Entry(cart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                        dBContext.Carts.Add(new Models.Cart() { ProductId = product.Id, UserId = User.ID, Quantity = 1 });

                    dBContext.SaveChanges();
                }
                    CartSum += product.Price;
                }));
            }
        }

        private RelayCommand openCart;
        public RelayCommand OpenCart
        {
            get
            {
                return openCart ?? (openCart = new RelayCommand(obj =>
                {
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("UserCart", User, "UserCart", new UserCartVM(user, cartSum)));
                }));
            }
        }

        private RelayCommand openOrders;
        public RelayCommand OpenOrders
            {
            get
            {
                return openOrders ?? (openOrders = new RelayCommand(obj =>
                {
                    OnChangeUC?.Invoke(this, new ChangeUCEventArgs("UserOrders", User, "UserOrders", new UserOrdersVM(user)));
                }));
            }
        }

        private readonly List<Product> allProducts;

        private User user;
        public User User
        {
            get => user;
            set { user = value; OnPropertyChanged(); }
        }

        private int? cartSum;
        public int? CartSum
        {
            get => cartSum;
            set { cartSum = value; OnPropertyChanged(); }
        }

        private List<Product> products;
        public List<Product> Products
        {
            get => products;
            set { products = value; OnPropertyChanged(); }
        }

        private List<string> categories = new List<string> { "Все" };
        public List<string> Categories
        {
            get => categories;
            set { categories = value; OnPropertyChanged(); }
        }

        public MainMenuVM(User user)
        {
            User = user;
            using (KabukiDesktopDBContext dBContext = new KabukiDesktopDBContext())
            {
                foreach (var category in dBContext.Categories)
                {
                    Categories.Add(category.Name);
                }
                Products = allProducts = dBContext.Products.ToList();
                CartSum = dBContext.Carts.Where(c => c.UserId == User.ID).ToArray().Sum(p => p.Product.Price * p.Quantity);
            }
        }

    }
}
