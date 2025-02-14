﻿using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface ICollabBL
    {
        public CollabEntity AddCollab(long userId, long noteId, string receiver_email);
        public List<CollabEntity> GetCollab(long userId);
        public bool RemoveCollab(long noteId, long userId, string emailId);
    }
}
