using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILabeBL
    {
        public LabelEntity CreateLabel(long userId, long noteId, string LabelName);

        public bool DeleteLabel(long userId, long noteId, string LabelName);
    }
}
