using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class NoteBL:INoteBL
    {
        private readonly INotesRL noteRL;
        public NoteBL(INotesRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public NoteEntity CreateNoteUser(long UserId, Notes createnote)
        {
            try
            {
                return noteRL.CreatNoteUser(UserId, createnote);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<NoteEntity> GetNoteUser(long UserId)
        {
            try
            {
                return noteRL.GetNotUser(UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public bool DeleteNoteUser(long userId,long noteId)
        {
            try
            {
                return noteRL.DeleteNoteUser(userId, noteId);
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
                return noteRL.UpdateNoteUser(userId, noteId,updateNote);
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
                return noteRL.UpdateNoteColor(userId, noteId,color);
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
                return noteRL.Ispinned(userId, noteId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }







    }
}
