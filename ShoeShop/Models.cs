using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }

    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }

    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
    }

    [Table("Manufacturers")]
    public class Manufacturer
    {
        [Key]
        public int ManufacturerID { get; set; }
        public string ManufacturerName { get; set; }
    }

    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string Article { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int? SupplierID { get; set; }
        public int? ManufacturerID { get; set; }
        public int? CategoryID { get; set; }
        public int Discount { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }

        [ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey("ManufacturerID")]
        public virtual Manufacturer Manufacturer { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [NotMapped]
        public decimal DiscountedPrice
        {
            get { return Price * (1 - Discount / 100m); }
        }

        [NotMapped]
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                    return "/Images/icon.jpg";
                return "/Images/" + Photo;
            }
        }
    }

    [Table("OrderStatuses")]
    public class OrderStatus
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }

    [Table("PickupPoints")]
    public class PickupPoint
    {
        [Key]
        public int PointID { get; set; }
        public string Address { get; set; }
    }

    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int? PickupPointID { get; set; }
        public int? UserID { get; set; }
        public string PickupCode { get; set; }
        public int? StatusID { get; set; }

        [ForeignKey("PickupPointID")]
        public virtual PickupPoint PickupPoint { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("StatusID")]
        public virtual OrderStatus Status { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}