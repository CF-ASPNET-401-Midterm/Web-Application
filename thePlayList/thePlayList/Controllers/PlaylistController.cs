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
        private MusicDbContext _context { get; set; }

        public PlaylistController(MusicDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays list of songs on the main playlist page
        /// </summary>
        /// <param name="id"> selected user id </param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            // Redirect if user has no selected playlist
            if (user.DatGenreEyeDee == 0 && user.DatListEyeDee == 0)
            {
                return RedirectToAction("Create", new { id = user.Id });
            }

            // Condition for first time user displaying default playlist 
            if (user.DatGenreEyeDee != 0)
            {
                if (user.DatListEyeDee != 0)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                        var playlistResponse = client.GetAsync("/api/playlist").Result;
                        var songResponse = client.GetAsync($"/api/playlist/{user.DatListEyeDee}").Result;

                        if (playlistResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                        {
                            var jsonDatapl = await playlistResponse.Content.ReadAsStringAsync();
                            var jsonDataSong = await songResponse.Content.ReadAsStringAsync();

                            List<Playlist> rawAllList = JsonConvert.DeserializeObject<List<Playlist>>(jsonDatapl);
                            SongRoot rawAllSongs = JsonConvert.DeserializeObject<SongRoot>(jsonDataSong);

                            var allPlaylists = from a in rawAllList
                                               select a;


                            var allSongs = from s in rawAllSongs.Songs
                                           select s;

                            var myPlaylist = allPlaylists.FirstOrDefault(p => p.Id == user.DatListEyeDee);

                            myPlaylist.UserID = user.Id;
                            myPlaylist.Id = null;
                            allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();

                            PlaylistViewModel mylistVM = new PlaylistViewModel();
                            mylistVM.ApiSongs = allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();
                            mylistVM.Playlists = allPlaylists.Where(pl => pl.GenreID == user.DatGenreEyeDee).ToList();
                            mylistVM.User = user;

                            return View(mylistVM);
                        }
                    }
                }

                // Condition for coming back default playlist user
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                    var playlistResponse = client.GetAsync("/api/playlist").Result;
                    var songResponse = client.GetAsync($"/api/playlist/{user.DatListEyeDee}").Result;

                    if (playlistResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        var jsonDatapl = await playlistResponse.Content.ReadAsStringAsync();
                        var jsonDataSong = await songResponse.Content.ReadAsStringAsync();

                        List<Playlist> rawAllList = JsonConvert.DeserializeObject<List<Playlist>>(jsonDatapl);
                        List<ApiSong> rawAllSongs = JsonConvert.DeserializeObject<List<ApiSong>>(jsonDataSong);

                        var allPlaylists = from a in rawAllList
                                           select a;

                        var allSongs = from s in rawAllSongs
                                       select s;

                        var myPlaylist = allPlaylists.FirstOrDefault(p => p.UserID == user.Id);

                        myPlaylist.UserID = user.Id;
                        myPlaylist.Id = null;
                        allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();

                        PlaylistViewModel mylistVM = new PlaylistViewModel();
                        mylistVM.ApiSongs = allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();
                        mylistVM.Playlists = allPlaylists.Where(pl => pl.GenreID == user.DatGenreEyeDee).ToList();
                        mylistVM.User = user;

                        return View(mylistVM);
                    }
                }
            }

            // Condition for custom playlist user
            PlaylistViewModel plVM = new PlaylistViewModel();
            plVM.User = user;
            plVM.Playlists = _context.Playlists.Where(p => p.UserID == user.Id).ToList();
            plVM.Songs = _context.Songs.Where(p => p.OurListId == user.DatListEyeDee).ToList();

            return View(plVM);
        }

        
        /// <summary>
        /// Generating a custom playlist based on the picking the random songs
        /// </summary>
        /// <param name="id"> selected user ID </param>
        /// <returns></returns>
        public async Task<IActionResult> CreateCustomPlaylist(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");
                var songsResponse = client.GetAsync("/api/song").Result;
                if (songsResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var jsonDataSongs = await songsResponse.Content.ReadAsStringAsync();
                    List<ApiSong> rawAllSongs = JsonConvert.DeserializeObject<List<ApiSong>>(jsonDataSongs);


                    var allSongs = from a in rawAllSongs
                                   select a;

                    // Creating place holders of custom song.id
                    var sortedSongs = allSongs.ToList();
                    List<int> idRef = new List<int>();
                    List<Song> customList = new List<Song>();
                    Playlist playlist = new Playlist();

                    playlist.UserID = user.Id;

                    await _context.Playlists.AddAsync(playlist);
                    await _context.SaveChangesAsync();

                    user.DatListEyeDee = playlist.Id.Value;


                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < 24; i++)
                    {
                        // Picking random songs
                        Random rdm = new Random();
                        int idX = rdm.Next(0, sortedSongs.Count());
                        if (!idRef.Contains(idX))
                        {
                            ApiSong apiSong = sortedSongs.Find(s => s.ID == idX);

                            if (apiSong != null)
                            {
                                // Moving ApiSong objec to Song object to save onto db
                                idRef.Add(idX);
                                Song newsong = new Song();
                                newsong.Name = apiSong.Name;

                                newsong.ApiListId = user.DatListEyeDee;
                                newsong.OurListId = user.DatListEyeDee;

                                newsong.ReleaseDate = apiSong.ReleaseDate;
                                newsong.Album = apiSong.Album;
                                newsong.Artist = apiSong.Artist;
                                newsong.Genre = apiSong.Genre;
                                await _context.Songs.AddAsync(newsong);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                i--;
                            }
                        }
                        else
                        {
                            i--;
                        }
                    }
                    return RedirectToAction("Get", "Playlist", new { id = user.Id });
                }
                return NotFound();
            }
        }

        
        /// <summary>
        /// Dynamically generating genre button based on the API 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create(int id)
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

                    PlaylistViewModel plVM = new PlaylistViewModel();
                    plVM.Playlists = rawAllPlaylists;
                    plVM.User = user;

                    return View(plVM);
                }
            }
            return RedirectToAction("Home");
        }

        /// <summary>
        /// Displaying Edit page of playlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        /// <summary>
        /// Update the playlist 
        /// </summary>
        /// <param name="user"> Selected user object </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
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
                    user.DatListEyeDee = allPlaylists.FirstOrDefault(pl => pl.GenreID == user.DatGenreEyeDee).Id.Value;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Get", "Playlist", new { id = user.Id });
                }
                return NotFound();
            }
        }

        /// <summary>
        /// Playlist delete function 
        /// </summary>
        /// <param name="userId"> Selected user id </param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int userId)
        {
            var user = _context.Users.Find(userId);
            var playlist = _context.Playlists.Find(user.DatListEyeDee);
            user.DatGenreEyeDee = 0;
            user.DatListEyeDee = 0;

            // update user information after removing a playlist
            _context.Users.Update(user);
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = user.Id });
        }
    }
}
