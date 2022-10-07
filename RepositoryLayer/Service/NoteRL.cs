using CommonLayer.Model;
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

        public bool UpdateNoteUser(long userId, long noteId,Notes updateNote)
        {
            try
            {
                NoteEntity updateNoteobj = new NoteEntity();
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
                    result.Color = updateNoteobj.Color;
                    result.trash = updateNoteobj.trash;
                    fundooContext.NoteTable.Update(result);
                    int ans=fundooContext.SaveChanges();
                    if (ans > 0)
                        return true;
                    else
                        return false;

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
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
