using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class PlaylistViewModel
    {
        public List<Playlist> Playlists { get; set; }

        public List<ApiSong> ApiSongs { get; set; }

        public List<Song> Songs { get; set; }

        public User User { get; set; }
    }
}
