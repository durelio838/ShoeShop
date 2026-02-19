using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShoeShop
{
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            Setup();
            LoadData();
        }

        private void Setup()
        {
            User u = LoginWindow.CurrentUser;
            if (u != null && (u.RoleID == 1 || u.RoleID == 2))
            {
                pnlAdmin.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            using (var db = new ShoeShopContext())
            {
                var list = db.Orders
                    .Include("PickupPoint")
                    .Include("User")
                    .Include("Status")
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                dgOrders.ItemsSource = list;
                txtCount.Text = "Всего: " + list.Count;
            }
        }

        private void DgOrders_Changed(object sender, SelectionChangedEventArgs e)
        {
            User u = LoginWindow.CurrentUser;
            if (u != null && (u.RoleID == 1 || u.RoleID == 2))
            {
                bool sel = dgOrders.SelectedItem != null;
                btnEdit.Visibility = sel ? Visibility.Visible : Visibility.Collapsed;
                btnDel.Visibility = sel ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow w = new AddOrderWindow();
            if (w.ShowDialog() == true) LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Order o = dgOrders.SelectedItem as Order;
            if (o != null)
            {
                MessageBox.Show("Редактирование заказа №" + o.OrderNumber);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Order o = dgOrders.SelectedItem as Order;
            if (o != null)
            {
                if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new ShoeShopContext())
                    {
                        var items = db.OrderItems.Where(i => i.OrderID == o.OrderID);
                        db.OrderItems.RemoveRange(items);
                        var ord = db.Orders.Find(o.OrderID);
                        if (ord != null) { db.Orders.Remove(ord); db.SaveChanges(); }
                    }
                    LoadData();
                }
            }
        }
    }
}