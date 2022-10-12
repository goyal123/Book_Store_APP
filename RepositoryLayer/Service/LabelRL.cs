using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LabelRL:ILabelRL
    {
        private readonly FundooContext fundooContext;

        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext=fundooContext;
        }
        public LabelEntity CreateLabel(long userId, long noteId, string LabelName)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();
                var result = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    labelEntity.LabelName = LabelName;
                    labelEntity.NoteId = noteId;
                    labelEntity.UserID = userId;
                    fundooContext.LabelTable.Add(labelEntity);
                    fundooContext.SaveChanges();
                    return labelEntity;

                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool DeleteLabel(long userId, long noteId, string LabelName)
        {
            try
            {
                var result1 = fundooContext.NoteTable.Where(u => u.UserId == userId && u.NoteID == noteId).FirstOrDefault();
                var result2 = fundooContext.LabelTable.Where(u => u.LabelName == LabelName && u.UserID==userId && u.NoteId==noteId).First();
                if (result1 != null && result2 != null)
                {
                    fundooContext.LabelTable.Remove(result2);
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
    }
}
