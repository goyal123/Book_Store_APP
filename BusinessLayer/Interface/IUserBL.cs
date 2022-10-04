using CommonLayer.Model;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserEntity UserRegistration(Registration registration);
        //public string LoginUser(string email, string password);

        public string LoginUser(Login login);

        public ResetEntity ResetPassUser(ResetPassword resetpass);

        public string ForgetPassword(string email);
    }
}
