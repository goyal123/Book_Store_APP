using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class NoteRL:INotesRL
    {
        private readonly FundooContext fundooContext;

        public NoteRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public NoteEntity CreatNoteUser(long UserId, Notes createnote)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                var result = fundooContext.UserTable.Where(u => u.UserID == UserId).FirstOrDefault();
                noteEntity.Title = createnote.Title;
                noteEntity.Description = createnote.Description;
                noteEntity.Reminder = createnote.Reminder;
                noteEntity.image = createnote.image;
                noteEntity.pinned = createnote.pinned;
                noteEntity.archieve = createnote.archieve;
                noteEntity.Color = createnote.Color;
                noteEntity.Created_At = createnote.Created_At;
                noteEntity.Updated_At = createnote.Updated_At;
                noteEntity.trash = createnote.trash;
                noteEntity.UserId = result.UserID;
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

        }

        public List<NoteEntity> GetNotUser(long UserId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == UserId).ToList();
                //if(result.Count!=0)
                return result;
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteNoteUser(long userId,long noteid)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID==noteid).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.NoteTable.Remove(result);
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NoteEntity UpdateNoteUser(long userId, long noteId,Notes updateNote)
        {
            try
            {
                //NoteEntity updateNoteobj = new NoteEntity();
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    //UpdateNoteEntity updateNoteobj = new UpdateNoteEntity();
                    result.Title = updateNote.Title;
                    result.Description=updateNote.Description;
                    result.Reminder = updateNote.Reminder;
                    result.Updated_At = updateNote.Updated_At;
                    result.Created_At = updateNote.Created_At;
                    result.archieve = updateNote.archieve;
                    result.pinned = updateNote.pinned;
                    result.Color = updateNote.Color;
                    result.trash = updateNote.trash;
                    fundooContext.NoteTable.Update(result);
                    int ans=fundooContext.SaveChanges();
                    if (ans > 0)
                        return result;
                    else
                        return null;

                    /*
                    updateNoteobj.Title = updateNote.Title;
                    updateNoteobj.Description = updateNote.Description;
                    updateNoteobj.Reminder = updateNote.Reminder;
                    updateNoteobj.Color = updateNote.Color;
                    updateNoteobj.image = updateNote.image;
                    updateNoteobj.archieve = updateNote.archieve;
                    updateNote.pinned = updateNote.pinned;
                    updateNoteobj.trash = updateNote.trash;
                    updateNoteobj.Created_At = updateNote.Created_At;
                    updateNoteobj.Updated_At = updateNote.Updated_At;
                    updateNoteobj.NoteID = result.NoteID;
                    updateNoteobj.UserId = result.UserId;
                    fundooContext.NoteTable.Update(updateNoteobj);
                    int ans = fundooContext.SaveChanges();
                    
                    if (ans > 0)
                        return true;
                    else
                        return false;*/
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public NoteEntity UpdateNoteColor(long userId, long noteId,string color)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    result.Color = color;
                    fundooContext.NoteTable.Update(result);
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public NoteEntity Ispinned(long userId, long noteId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result.pinned == true)
                {
                    result.pinned = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.pinned = true;
                    fundooContext.SaveChanges();
                    return result;
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public NoteEntity IsArchieve(long userId,long noteId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result.archieve == true)
                {
                    result.archieve = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.archieve = true;
                    fundooContext.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public NoteEntity IsTrashed(long userId,long noteId)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result.trash == true)
                {
                    result.trash = false;
                    fundooContext.SaveChanges();
                    return result;
                }
                else
                {
                    result.trash = true;
                    fundooContext.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public NoteEntity Image(long userId, long noteId, IFormFile file)
        {
            try
            {
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if(result!=null)
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account { ApiKey = "818957792646971", ApiSecret = "3DwNVsLP2GiQcH8Dy_GSOggh3x0", Cloud = "dyainxvh0" };
                    Cloudinary _cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream())
                    };
                    var uploadresult = _cloudinary.Upload(uploadParams);
                    result.image=uploadresult.Url.ToString();
                    fundooContext.SaveChanges();
                    return result;

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


    }
}
