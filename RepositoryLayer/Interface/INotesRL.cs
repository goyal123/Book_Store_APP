using CommonLayer.Model;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public NoteEntity CreatNoteUser(long UserId, Notes createnote);
        //public string GetNotUser(long UserId);
        public List<NoteEntity> GetNotUser(long UserId);
    }
}
