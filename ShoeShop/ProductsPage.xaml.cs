using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShoeShop
{
    public partial class ProductsPage : Page
    {
        private List<Product> allProducts;

        public ProductsPage()
        {
            InitializeComponent();
            Setup();
            LoadData();
        }

        private User CurUser
        {
            get { return LoginWindow.CurrentUser; }
        }

        private void Setup()
        {
            if (CurUser != null && (CurUser.RoleID == 1 || CurUser.RoleID == 2))
            {
                pnlFilter.Visibility = Visibility.Visible;
            }

            if (CurUser != null && CurUser.RoleID == 1)
            {
                pnlAdmin.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            using (var db = new ShoeShopContext())
            {
                allProducts = db.Products
                    .Include("Supplier")
                    .Include("Manufacturer")
                    .Include("Category")
                    .ToList();

                var cats = db.Categories.ToList();
                cmbCat.Items.Clear();
                cmbCat.Items.Add(new ComboBoxItem { Content = "All", IsSelected = true });
                foreach (var c in cats)
                {
                    cmbCat.Items.Add(new ComboBoxItem { Content = c.CategoryName, Tag = c.CategoryID });
                }
            }
            Filter();
        }

        private void Filter()
        {
            if (allProducts == null) return;

            IEnumerable<Product> res = allProducts;

            string s = txtSearch.Text.ToLower().Trim();
            if (!string.IsNullOrEmpty(s))
            {
                res = res.Where(p =>
                    p.ProductName.ToLower().Contains(s) ||
                    (p.Description != null && p.Description.ToLower().Contains(s)) ||
                    p.Article.ToLower().Contains(s) ||
                    (p.Manufacturer != null && p.Manufacturer.ManufacturerName.ToLower().Contains(s)));
            }

            ComboBoxItem cat = cmbCat.SelectedItem as ComboBoxItem;
            if (cat != null && cat.Tag != null)
            {
                int id = (int)cat.Tag;
                res = res.Where(p => p.CategoryID == id);
            }

            if (cmbSort.SelectedIndex == 1)
                res = res.OrderBy(p => p.DiscountedPrice);
            else if (cmbSort.SelectedIndex == 2)
                res = res.OrderByDescending(p => p.DiscountedPrice);

            var list = res.ToList();
            lvProducts.ItemsSource = list;
            txtCount.Text = "Shown: " + list.Count + " of " + allProducts.Count;
        }

        private void TxtSearch_Changed(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void CmbSort_Changed(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void CmbCat_Changed(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void LvProducts_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (CurUser != null && CurUser.RoleID == 1)
            {
                bool sel = lvProducts.SelectedItem != null;
                btnEdit.Visibility = sel ? Visibility.Visible : Visibility.Collapsed;
                btnDel.Visibility = sel ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductEditWindow w = new ProductEditWindow(null);
            if (w.ShowDialog() == true)
                LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Product p = lvProducts.SelectedItem as Product;
            if (p != null)
            {
                ProductEditWindow w = new ProductEditWindow(p);
                if (w.ShowDialog() == true)
                    LoadData();
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Product p = lvProducts.SelectedItem as Product;
            if (p != null)
            {
                if (MessageBox.Show("Delete?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new ShoeShopContext())
                    {
                        var pr = db.Products.Find(p.ProductID);
                        if (pr != null)
                        {
                            db.Products.Remove(pr);
                            db.SaveChanges();
                        }
                    }
                    LoadData();
                }
            }
        }
    }
}