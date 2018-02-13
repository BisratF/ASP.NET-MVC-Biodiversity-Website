using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace zefitret.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.", AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string PassWord { get; set; }
        public string Privilege { get; set; }
        public bool RememberMe { get; set; }
    }
}