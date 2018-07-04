using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using thePlayList.Data;

namespace thePlayList.Controllers
{
    public class SongController : Controller
    {
        private MusicDbContext _context { get; set; }

        public SongController(MusicDbContext context)
        {
            _context = context;
        }
    }
}
