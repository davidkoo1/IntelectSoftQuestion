﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication2.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string Token { get; set; }
    }
}
