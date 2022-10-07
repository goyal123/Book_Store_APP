using BusinessLayer.Interface;
using CommonLayer.Model;
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




    }
}
