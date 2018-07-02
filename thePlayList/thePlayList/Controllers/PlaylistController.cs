using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using thePlayList.Data;

namespace thePlayList.Controllers
{
    public class PlaylistController : Controller
    {
        private MusicDbContext _context { get; set;}

        public PlaylistController(MusicDbContext context)
        {
            _context = context;
        }


    }
}
