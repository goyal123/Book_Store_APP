﻿using CommonLayer.Model;
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
                userEntity.Password= EncryptPassword(registration.Password);
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
        /*
        public NoteEntity CreatNoteUser(string email,Notes createnote)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                var result = fundooContext.UserTable.Where(u => u.Email == email).FirstOrDefault();
                noteEntity.Title = createnote.Title;
                noteEntity.Description = createnote.Description;
                noteEntity.Reminder = createnote.Reminder;
                noteEntity.image = createnote.image;
                noteEntity.pinned = createnote.pinned;
                noteEntity.archieve = createnote.archieve;
                noteEntity.Color = createnote.Color;
                noteEntity.Created_At = createnote.Created_At;
                noteEntity.Updated_At=createnote.Updated_At;
                noteEntity.trash = createnote.trash;
                noteEntity.UserId=result.UserID;
                fundooContext.NoteTable.Add(noteEntity);
                int ans = fundooContext.SaveChanges();
                if (ans > 0)
                    return noteEntity;
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }*/

        public string LoginUser(Login login)
        {
            try
            {
                LoginEntity loginentity = new LoginEntity();
                var data=fundooContext.UserTable.Where(u => u.Email==login.Email).SingleOrDefault();
                if (data != null)
                {
                    bool isValid = DecryptPassword(data.Password) == login.Password;
                    if (isValid)
                        return GetJWTToken(data.Email, data.UserID);
                    else
                        return null;

                }
                else
                    return null;
                /*
                var result= fundooContext.UserTable.Where(u => u.Email==login.Email && DecryptPassword(u.Password)==login.Password).FirstOrDefault();
                if (result != null)
                    return GetJWTToken(result.Email, result.UserID);
                else
                    return null;
                */
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ResetEntity ResetPassUser(string email,ResetPassword resetpass)
        {
            try
            {
                ResetEntity resetentity = new ResetEntity();
                resetentity.Password = resetpass.Password;
                resetentity.ConfirmPassword = resetpass.ConfirmPassword;
                if(resetentity.Password.Equals(resetentity.ConfirmPassword))
                {
                    var result = fundooContext.UserTable.Where(u => u.Email == email).FirstOrDefault();
                    if (result != null)
                    {
                        result.Password = resetpass.Password;
                        int ans = fundooContext.SaveChanges();
                        if (ans > 0)
                            return resetentity;
                        else
                            return null;
                        //fundooContext.UserTable.Update()
                    }
                    else
                        return null;
                }
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

        public static string EncryptPassword(string password)
        {
            try
            {
                if (password != null && password.Length > 0)
                {
                    byte[] storepassword = ASCIIEncoding.ASCII.GetBytes(password);
                    string encrpytpassword = Convert.ToBase64String(storepassword);
                    return encrpytpassword;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        public static string DecryptPassword(string password)
        {
            if (password != null && password.Length > 0)
            {
                byte[] encrpytpassword = Convert.FromBase64String(password);
                string decryptpassword = ASCIIEncoding.ASCII.GetString(encrpytpassword);
                return decryptpassword;
            }
            else
                return null;
        }




    }

    
}
