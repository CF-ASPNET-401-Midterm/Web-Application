using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using thePlayList.Data;

namespace thePlayList.Controllers
{
    public class HomeController : Controller
    {
        private MusicDbContext _context { get; set; }

        public HomeController(MusicDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About(int? id)
        {
            if (id != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                return View(user);
            }

            return View();
        }

        public IActionResult AboutOut()
        {
            return View();
        }
    }
}
