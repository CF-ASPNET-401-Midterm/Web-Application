using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public List<Song> Song { get; set; }
    }
}
