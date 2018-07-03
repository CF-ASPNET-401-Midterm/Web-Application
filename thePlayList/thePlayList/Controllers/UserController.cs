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
        public IActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// Once user input the username, this method will find any existing username saved in the database
        /// </summary>
        /// <param name="username"> user inputed username </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Home(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Name == username);

            if (user == null)
            {
                User newuser = new User();
                newuser.Name = username;
                await _context.Users.AddAsync(newuser);
                await _context.SaveChangesAsync();
                return RedirectToAction("NewUser", new { id = newuser.Id });
            }

            if(user.DatListEyeDee == 0)
            {
                return RedirectToAction("MyList", new { id = user.Id });
                //return RedirectToAction("NewUser", new { id = user.Id });
            }
            

            return RedirectToAction("MyList", new { id = user.Id });
        }

        /// <summary>
        /// Method where new user will select their playlist
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> NewUser(int id)
        { 
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                var plResponse = client.GetAsync("/api/playlist").Result;

                if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var jsonDataPl = await plResponse.Content.ReadAsStringAsync();

                    List<Playlist> rawAllPlaylists = JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);

                    var allPlaylists = from a in rawAllPlaylists
                                       select a;

                    ViewData["Playlists"] = rawAllPlaylists;
                    return View(user);
                }
            }
            return RedirectToAction("Home");
        }


        
        [HttpPost]
        public async Task<IActionResult> NewUser([Bind("Id", "Name", "DatListEyeDee, DatGenreEyeDee")] User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                var plResponse = client.GetAsync("/api/playlist").Result;

                if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var jsonDataPl = await plResponse.Content.ReadAsStringAsync();

                    List<Playlist> rawAllPlaylists = JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);

                    var allPlaylists = from a in rawAllPlaylists
                                       select a;

                    PlaylistViewModel mylistVM = new PlaylistViewModel();
                    user.DatListEyeDee = allPlaylists.FirstOrDefault(pl => pl.GenreID == user.DatGenreEyeDee).Id;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyList", new { id = user.Id });
                }
                return NotFound();
            }
        }
        

        /// <summary>
        /// Main page displaying the playlist
        /// </summary>
        /// <param name="user"> loaded user object if already exist </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyList(int id)
        {
            //if(user.PlaylistId == 0)
            //{
            //    return View();
            //}

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                //var plResponse = client.GetAsync("/api/playlist").Result;
                var songResponse = client.GetAsync($"/api/playlist/{user.DatListEyeDee}").Result;

                if (songResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    //var jsonDataPl = await plResponse.Content.ReadAsStringAsync();
                    var jsonDataSong = await songResponse.Content.ReadAsStringAsync();

                    //List<Playlist> rawAllPlaylists= JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);
                    SongRoot rawAllSongs = JsonConvert.DeserializeObject<SongRoot>(jsonDataSong);

                    //var allPlaylists = from a in rawAllPlaylists
                                       //select a;

                    var allSongs = from s in rawAllSongs.Songs
                                   select s;

                    PlaylistViewModel mylistVM = new PlaylistViewModel();
                    mylistVM.Songs = allSongs.ToList();
                    //mylistVM.Playlists = allPlaylists.Where( pl => pl.GenreID == user.DatGenreEyeDee).ToList();

                    return View(mylistVM);
                }
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChoosePlaylist(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return View(user);
        }
    
        //[HttpPost]
        //public async Task<IActionResult> ChoosePlaylist([Bind("Id", "Name", "DatListEyeDee")] User user)
        //{

        //}
    }
}
