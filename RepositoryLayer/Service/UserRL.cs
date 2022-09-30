using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RepositoryLayer.Service
{
    public class UserRL:IUserRL
    {
        private readonly FundooContext fundooContext;

        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public UserEntity UserRegistration(Registration registration)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = registration.FirstName;
                userEntity.LastName = registration.LastName;
                userEntity.Email = registration.Email;
                userEntity.Password= registration.Password;
                fundooContext.UserTable.Add(userEntity);
                int result=fundooContext.SaveChanges();
                if (result > 0)
                    return userEntity;
                else
                    return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
