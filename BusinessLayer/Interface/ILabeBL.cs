﻿using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ILabeBL
    {
        public LabelEntity CreateLabel(long userId, long noteId, string LabelName);

        public bool DeleteLabel(long userId, long noteId, string LabelName);
        public LabelEntity UpdateLabel(long userId, long noteId,long LabelId, string LabelName);
        public List<LabelEntity> GetLabel(long userId);
    }
}
