using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stanochki.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string surn { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string middlen { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string address { get; set; }

        [Required]
        public string office = null;// { get; set; }

        [Required]
        [Phone]
        [StringLength(11, ErrorMessage = "Example : 8 999 999 99 99", MinimumLength = 11)]
        [Display(Name = "Номер")]
        public string number { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Pass { get; set; }
    }
}