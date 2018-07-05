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
        /// Once user input the username, this method will find any existing username saved in the database
        /// </summary>
        /// <param name="username"> user inputed username </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Name == username);

            if (user == null)
            {
                User newuser = new User();
                newuser.Name = username;
                await _context.Users.AddAsync(newuser);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Playlist", new { id = newuser.Id });
            }

            if(user.DatListEyeDee == 0)
            {
                return RedirectToAction("Edit", "Playlist", new { id = user.Id });
            }

            return RedirectToAction("Get", "Playlist", new { id = user.Id });
        }

        // Edit username
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);

            if(user == null)
            {
                return RedirectToAction("Get", "Playlist", new { id = id });
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, string newusername)
        {
            var user = _context.Users.Find(id);

            // Return back to page if username already exist
            if(user == null)
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

            return RedirectToAction("Get");
        }

        //[HttpGet]
        //public async Task<IActionResult> NewUser(int id)
        //{ 
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

        //        var plResponse = client.GetAsync("/api/playlist").Result;

        //        if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
        //        {
        //            var jsonDataPl = await plResponse.Content.ReadAsStringAsync();

        //            List<Playlist> rawAllPlaylists = JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);

        //            var allPlaylists = from a in rawAllPlaylists
        //                               select a;

        //            ViewData["Playlists"] = rawAllPlaylists;
        //            return View(user);
        //        }
        //    }
        //    return RedirectToAction("Home");
        //}

        //[HttpPost]
        //public async Task<IActionResult> NewUser(User user)
        //{

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

        //        var plResponse = client.GetAsync("/api/playlist").Result;

        //        if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
        //        {
        //            var jsonDataPl = await plResponse.Content.ReadAsStringAsync();

        //            List<Playlist> rawAllPlaylists = JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);

        //            var allPlaylists = from a in rawAllPlaylists
        //                               select a;

        //            PlaylistViewModel mylistVM = new PlaylistViewModel();
        //            user.DatListEyeDee = allPlaylists.FirstOrDefault(pl => pl.GenreID == user.DatGenreEyeDee).Id;

        //            _context.Users.Update(user);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction("Get", "Playlist", new { id = user.Id });
        //        }
        //        return NotFound();
        //    }
        //}
       
    }
}