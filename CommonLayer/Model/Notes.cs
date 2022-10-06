using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class Notes
    {
        //public long NoteID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string Color { get; set; }
        public string image { get; set; }
        public bool archieve { get; set; }
        public bool pinned { get; set; }
        public bool trash { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        
       
    }
}
