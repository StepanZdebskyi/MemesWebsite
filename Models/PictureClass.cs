using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UkrMemesWeb.Models
{
    public class PictureClass
    {
        static public int ObjCounter = 0;
        public int _PictureID;
        public string _PictureTitle;
        public string _PictureKeywordsString;
        public string _PictureDescription;
        public string _PicureLink;
        public string _PictureAddingDate;

        public string[] _PictureKeywords;

        public PictureClass()
        {
            ObjCounter++;
            _PictureID = ObjCounter;
        }
    }
}
