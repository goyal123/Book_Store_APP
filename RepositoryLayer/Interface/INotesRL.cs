using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
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

        public bool DeleteNoteUser(long userId, long noteid);

        public NoteEntity UpdateNoteUser(long userId, long noteId, Notes updateNote);

        public NoteEntity UpdateNoteColor(long userId, long noteId, string color);

        public NoteEntity Ispinned(long userId, long noteId);

        public NoteEntity IsArchieve(long userId, long noteId);

        public NoteEntity IsTrashed(long userId, long noteId);

        public NoteEntity Image(long userId, long noteId, IFormFile file);
    }
}
