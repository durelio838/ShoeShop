using System;
using System.Linq;
using System.Windows;

namespace ShoeShop
{
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new ShoeShopContext())
            {
                cmbUser.ItemsSource = db.Users.ToList();
                cmbPoint.ItemsSource = db.PickupPoints.ToList();
                cmbStatus.ItemsSource = db.OrderStatuses.ToList();
            }
            dpDelivery.SelectedDate = DateTime.Today.AddDays(7);
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbUser.SelectedItem == null || cmbPoint.SelectedItem == null ||
                cmbStatus.SelectedItem == null || dpDelivery.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                using (var db = new ShoeShopContext())
                {
                    int maxNum = db.Orders.Any() ? db.Orders.Max(o => o.OrderNumber) : 0;
                    string code = new Random().Next(100000, 999999).ToString();

                    Order ord = new Order();
                    ord.OrderNumber = maxNum + 1;
                    ord.OrderDate = DateTime.Today;
                    ord.DeliveryDate = dpDelivery.SelectedDate;
                    ord.UserID = ((User)cmbUser.SelectedItem).UserID;
                    ord.PickupPointID = ((PickupPoint)cmbPoint.SelectedItem).PointID;
                    ord.StatusID = ((OrderStatus)cmbStatus.SelectedItem).StatusID;
                    ord.PickupCode = code;
                    db.Orders.Add(ord);
                    db.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}