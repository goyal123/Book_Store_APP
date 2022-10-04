using CommonLayer.Model;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity UserRegistration(Registration registration);
        //public string LoginUser(string email, string password);
        public string LoginUser(Login login);

        public string ForgetPassword(string email);
    }
}
