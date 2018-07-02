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

            if ( user == null)
            {
                User newuser = new User();
                newuser.Name = username;
                await _context.Users.AddAsync(newuser);
                await _context.SaveChangesAsync();
                return View(newuser);
            }

            return View(user);
        }


        /// <summary>
        /// Main page displaying the playlist
        /// </summary>
        /// <param name="user"> loaded user object if already exist </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyList(User user)
        {
            if(user.PlaylistId == null)
            {
                return View();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                var plResponse = client.GetAsync("/api/playlist").Result;
                //var songResponse = client.GetAsync("/api/song").Result;

                if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var jsonDataPl = await plResponse.Content.ReadAsStringAsync();
                   // var jsonDataSong = await songResponse.Content.ReadAsStringAsync();

                    List<Playlist> rawAllPlaylists= JsonConvert.DeserializeObject<List<Playlist>>(jsonDataPl);
                   // List<Song> rawAllSongs = JsonConvert.DeserializeObject<List<Song>>(jsonDataSong);

                    var allPlaylists = from a in rawAllPlaylists
                                       select a;
                   // var allSongs = from s in rawAllSongs
                   //                select s;

                    PlaylistViewModel mylistVM = new PlaylistViewModel();
                    mylistVM.Playlists = allPlaylists.ToList();
                  //  mylistVM.Songs = allSongs.ToList();

                    return View(mylistVM);
                }
                return NotFound();
            }
        }
    
    }
}
