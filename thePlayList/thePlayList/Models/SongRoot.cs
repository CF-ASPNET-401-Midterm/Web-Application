using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class SongRoot
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GenreID { get; set; }
        public List<ApiSong> Songs { get; set; }
    }
}
