using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class UserVM
    {
        public List<Song> Songs { get; set; }
        public List<Playlist> Playlists { get; set; }
        public User User { get; set; }
    }
}
