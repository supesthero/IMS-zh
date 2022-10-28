﻿using IMS.CoreBusiness.Validations;
using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        
        [Required]
        [StringLength(150)]
        public string InventoryName { get; set; } = string.Empty;
        
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }
        public string? ImageURL { get; set; }


        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>(); //Navigation property for EFCore
    }
}