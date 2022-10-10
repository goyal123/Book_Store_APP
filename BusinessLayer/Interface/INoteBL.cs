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

        //public bool UpdateNoteUser(long userId, long noteId);
        public NoteEntity UpdateNoteUser(long userId, long noteId, Notes updateNote);

        public NoteEntity UpdateNoteColor(long userId, long noteId, string color);

        public NoteEntity Ispinned(long userId, long noteId);
    }
}
