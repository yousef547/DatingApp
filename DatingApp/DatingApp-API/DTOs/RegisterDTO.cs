﻿using System.ComponentModel.DataAnnotations;

namespace DatingApp_API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }

    }
}
