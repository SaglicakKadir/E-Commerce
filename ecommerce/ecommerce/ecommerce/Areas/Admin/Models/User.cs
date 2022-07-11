﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ecommerce.Areas.Admin.Models
{
    public class User
    {
        public short UserId { get; set; }
        [Column(TypeName = "char(100)")]
        [EmailAddress]
        [Required]
        public string UserEmail { get; set; }
        [Required, Column(TypeName = "char(64)")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public bool ViewUsers { get; set; }
        [Required]
        public bool CreateUser { get; set; }
        [Required]
        public bool DeleteUser { get; set; }
        [Required]
        public bool EditUser { get; set; }
        [Required]
        public bool ViewSellers { get; set; }
        [Required]
        public bool CreateSeller { get; set; }
        [Required]
        public bool DeleteSeller { get; set; }
        [Required]
        public bool EditSeller { get; set; }
        [Required]
        public bool ViewCategories { get; set; }
        [Required]
        public bool CreateCategory { get; set; }
        [Required]
        public bool DeleteCategory { get; set; }
        [Required]
        public bool EditCategory { get; set; }
        [Required]
        public bool DeleteProduct { get; set; }
        [Required]
        public bool EditProduct { get; set; }
    }
}
