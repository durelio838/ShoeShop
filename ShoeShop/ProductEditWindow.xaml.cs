using System;
using System.Windows;

namespace ShoeShop
{
    public partial class ProductEditWindow : Window
    {
        private Product prod;
        private bool isNew;

        public ProductEditWindow(Product p)
        {
            InitializeComponent();

            if (p == null)
            {
                isNew = true;
                prod = new Product();
                Title = "New";
            }
            else
            {
                isNew = false;
                prod = p;
                Title = "Edit";
                txtArt.Text = prod.Article;
                txtName.Text = prod.ProductName;
                txtPrice.Text = prod.Price.ToString();
                txtDisc.Text = prod.Discount.ToString();
                txtStock.Text = prod.StockQuantity.ToString();
                txtDesc.Text = prod.Description;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArt.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Fill required fields");
                return;
            }

            decimal price;
            int disc, stock;

            if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
            {
                MessageBox.Show("Invalid price");
                return;
            }

            if (!int.TryParse(txtDisc.Text, out disc) || disc < 0 || disc > 100)
            {
                MessageBox.Show("Discount 0-100");
                return;
            }

            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MessageBox.Show("Invalid stock");
                return;
            }

            try
            {
                using (var db = new ShoeShopContext())
                {
                    if (isNew)
                    {
                        Product np = new Product();
                        np.Article = txtArt.Text.Trim();
                        np.ProductName = txtName.Text.Trim();
                        np.Unit = "pcs";
                        np.Price = price;
                        np.Discount = disc;
                        np.StockQuantity = stock;
                        np.Description = txtDesc.Text.Trim();
                        db.Products.Add(np);
                    }
                    else
                    {
                        var pr = db.Products.Find(prod.ProductID);
                        if (pr != null)
                        {
                            pr.Article = txtArt.Text.Trim();
                            pr.ProductName = txtName.Text.Trim();
                            pr.Price = price;
                            pr.Discount = disc;
                            pr.StockQuantity = stock;
                            pr.Description = txtDesc.Text.Trim();
                        }
                    }
                    db.SaveChanges();
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}