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
    public class PlaylistController : Controller
    {
        private MusicDbContext _context { get; set;}

        public PlaylistController(MusicDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Home(string searchString, User user)
        {
            using (var client = new HttpClient())
            {
                // add the appropriate properties on top of the client base address.
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                //the .Result is important for us to extract the result of the response from the call
                var plResponse = client.GetAsync("/api/playlist").Result;
                //var songResponse = client.GetAsync("/api/playlist/23").Result;
                if (plResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringListResult = await plResponse.Content.ReadAsStringAsync();
                    //var stringItemsResult = await songResponse.Content.ReadAsStringAsync();

                    List<Playlist> rawPlaylists = JsonConvert.DeserializeObject<List<Playlist>>(stringListResult);
                    //List<Song> rawSongs = JsonConvert.DeserializeObject<List<Song>>(stringItemsResult);

                    var allPlaylists = from l in rawPlaylists
                                    select l;

                    List<Song> rawSongs = new List<Song>();
                    foreach(var item in allPlaylists)
                    {
                        var songResponse = client.GetAsync($"/api/playlist/{item.Id}").Result;
                        var stringItemsResult = await songResponse.Content.ReadAsStringAsync();
                        Playlist tempList = JsonConvert.DeserializeObject<Playlist>(stringItemsResult);

                        //var varList = from x in tempList
                        //              select x;
                        //var tempSongs = from s in tempList.Songs
                        //               where s.PlaylistID == item.Id
                        //               select s;

                        //rawSongs.AddRange(tempSongs);
                        //item.Songs = rawSongs;
                    }
                    //var allSongs = from i in rawSongs
                    //                select i;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        allPlaylists = rawPlaylists.Where(s => s.Name.Contains(searchString));
                    }

                    var playlistVM = new PlaylistViewModel();
                    playlistVM.Playlists = allPlaylists.ToList();
                    playlistVM.Songs = rawSongs.ToList();

                    ViewData["user"] = user;
                    return View(playlistVM);
                }
                return View();
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

                    ViewData["user"] = user;
                    return View(mylistVM);
                }
                return NotFound();
            }
        }
    }
}
