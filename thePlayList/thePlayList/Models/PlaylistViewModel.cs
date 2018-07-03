using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thePlayList.Models
{
    public class PlaylistViewModel : Controller
    {
        public List<Playlist> Playlists { get; set; }

        public List<Song> Songs { get; set; }
    }
}
