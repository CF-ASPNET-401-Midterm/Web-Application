using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using thePlayList.Data;

namespace thePlayList.Controllers
{
    public class UserController : Controller
    {
        private MusicDbContext _context { get; set; }

        public UserController(MusicDbContext context)
        {
            _context = context;
        }
    }
}
