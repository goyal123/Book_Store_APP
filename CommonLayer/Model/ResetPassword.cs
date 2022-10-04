using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Model
{
    public class ResetPassword
    {
        public string email { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }
    }
}
