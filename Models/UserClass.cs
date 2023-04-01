using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UkrMemesWeb.Models
{
    public class UserClass
    {
        static public int ObjCounter = 0;
        public int _UsersID;
        public string _UsersRole { get; set; }
        public string _UsersName { get; set; }
        public string _UsersLogin { get; set; }
        public string _UsersPass { get; set; }
        public bool _IsAdmin { get; set; }

        public UserClass()
        {
            ObjCounter++;
            _UsersID = ObjCounter;
        } 
    }
}
