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

        /// <summary>
        /// Display detail on the user information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Info(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                var rawPlaylist = _context.Playlists.Where(p => p.UserID == user.Id).ToList();

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
            // condition if user name is taken
            if (username == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users.FirstOrDefaultAsync(n => n.Name == username);
            // condition if its a new user
            if (user == null)
            {
                User newuser = new User();
                newuser.Name = username;
                await _context.Users.AddAsync(newuser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Playlist", new { id = newuser.Id });
            }

            // condition if user is selected, but playlist not selected
            if (user.DatListEyeDee == 0)
            {
                return RedirectToAction("Create", "Playlist", new { id = user.Id });
            }

            return RedirectToAction("Get", "Playlist", new { id = user.Id });
        }

        /// <summary>
        /// Updating name of user 
        /// </summary>
        /// <param name="id"> selected user id </param>
        /// <returns></returns>
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


        /// <summary>
        /// Update a new username.name
        /// </summary>
        /// <param name="id"> selected user id </param>
        /// <param name="newusername"> user input with new username </param>
        /// <returns></returns>
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


        /// <summary>
        /// Remove user account
        /// </summary>
        /// <param name="id"> selected user id </param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}