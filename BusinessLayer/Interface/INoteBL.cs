using CommonLayer.Model;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public NoteEntity CreateNoteUser(long UserId, Notes createnote);
        //public string GetNoteUser(long UserId);
        public List<NoteEntity> GetNoteUser(long UserId);

        public bool DeleteNoteUser(long userId, long noteid);
    }
}
