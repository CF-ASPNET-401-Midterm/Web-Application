using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class Playlist
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int GenreID { get; set; }
        public int YouserEyeDee { get; set; }
        public List<Song> Songs { get; set; }
    }
}
