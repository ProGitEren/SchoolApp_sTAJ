﻿using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class RegistererUpdateViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]

        public string Name { get; set; }

        [Required]

        public string Phone { get; set; }

        [Required]

        public string email { get; set; }
    }
}
