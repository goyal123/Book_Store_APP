using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Identity;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL:IUserBL
    {
        private readonly IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserEntity UserRegistration(Registration registration)
        {
            try
            {
                return userRL.UserRegistration(registration);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string LoginUser(string email,string password)
        {
            try
            {
                return this.userRL.LoginUser(email, password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


    }
}
