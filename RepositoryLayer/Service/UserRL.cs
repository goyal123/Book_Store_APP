using CommonLayer.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public string LoginUser(Login login)
        {
            try
            {
                LoginEntity loginentity = new LoginEntity();
                var result= fundooContext.UserTable.Where(u => u.Email==login.Email && u.Password==login.Password).FirstOrDefault();
                if (result != null)
                    return GetJWTToken(result.Email, result.UserID);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*
        public string LoginUser(string email,string password)
        {
            var result = fundooContext.UserTable.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
            if (result != null)
                return GetJWTToken(email, result.UserID);
            else
                return null;
        }
        */

        private static string GetJWTToken(string email,long userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userID", userID.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
              
        }

        public string ForgetPassword(string email)
        {
            try
            {
                var emailcheck = fundooContext.UserTable.FirstOrDefault(e => e.Email == email);
                if (emailcheck != null)
                {
                    var token = GetJWTToken(emailcheck.Email, emailcheck.UserID);
                    MSMQModel mSMQ = new MSMQModel();
                    mSMQ.sendData2Queue(token);
                    return token.ToString();
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

    }
}
