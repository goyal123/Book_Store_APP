using CommonLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
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
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
