﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class ApiSong
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int PlaylistID { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
