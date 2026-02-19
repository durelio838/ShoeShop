using System;
using System.Linq;
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
                Title = "Новый товар";
                pnlArt.Visibility = Visibility.Collapsed;
            }
            else
            {
                isNew = false;
                prod = p;
                Title = "Изменить товар";
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
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Заполните обязательные поля");
                return;
            }

            decimal price;
            int disc, stock;

            if (!decimal.TryParse(txtPrice.Text, out price) || price < 0)
            {
                MessageBox.Show("Неверная цена");
                return;
            }

            if (!int.TryParse(txtDisc.Text, out disc) || disc < 0 || disc > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100");
                return;
            }

            if (!int.TryParse(txtStock.Text, out stock) || stock < 0)
            {
                MessageBox.Show("Неверное количество на складе");
                return;
            }

            try
            {
                using (var db = new ShoeShopContext())
                {
                    if (isNew)
                    {
                        string art = db.Database.SqlQuery<string>(
                            "DECLARE @art NVARCHAR(8); " +
                            "DECLARE @found BIT = 0; " +
                            "WHILE @found = 0 BEGIN " +
                            "  SET @art = LEFT(REPLACE(CAST(NEWID() AS NVARCHAR(36)), '-', ''), 8); " +
                            "  IF NOT EXISTS (SELECT 1 FROM Products WHERE Article = @art) SET @found = 1; " +
                            "END; " +
                            "SELECT @art;").First();

                        Product np = new Product();
                        np.Article = art;
                        np.ProductName = txtName.Text.Trim();
                        np.Unit = "шт";
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