using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using thePlayList.Data;
using thePlayList.Models;

namespace thePlayList.Controllers
{
    public class UserController : Controller
    {

        private MusicDbContext _context { get; set; }

        public UserController(MusicDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Login page once enter web application
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Info(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                var rawPlaylist = _context.Playlists.Where(p => p.YouserEyeDee == user.Id).ToList();

                PlaylistViewModel plVM = new PlaylistViewModel();
                plVM.Playlists = rawPlaylist;
                plVM.User = user;
                return View(plVM);
            }
            else
            {
                return RedirectToAction("Get");
            }
        }

        /// <summary>
        /// Once user input the username, this method will find any existing username saved in the database
        /// </summary>
        /// <param name="username"> user inputed username </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Get(string username)
        {
            if (username == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users.FirstOrDefaultAsync(n => n.Name == username);

            if (user == null)
            {
                User newuser = new User();
                newuser.Name = username;
                await _context.Users.AddAsync(newuser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Playlist", new { id = newuser.Id });
            }

            if (user.DatListEyeDee == 0)
            {
                return RedirectToAction("Create", "Playlist", new { id = user.Id });
            }

            return RedirectToAction("Get", "Playlist", new { id = user.Id });
        }

        // Edit username
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return RedirectToAction("Get", "User");
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, string newusername)
        {
            var user = _context.Users.Find(id);

            // Return back to page if username already exist
            if (user == null)
            {
                return View(user);
            }
            user.Name = newusername;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Get", "Playlist", new { id = user.Id });
        }


        // Remove user account
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}