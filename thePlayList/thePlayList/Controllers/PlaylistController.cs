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

        /*
        public async Task<IActionResult> Home(string searchString)
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

                    foreach(var item in allPlaylists)
                    {
                        var songResponse = client.GetAsync($"/api/playlist/{item.Id}").Result;
                        var stringItemsResult = await songResponse.Content.ReadAsStringAsync();
                        List<Song> rawSongs = JsonConvert.DeserializeObject<List<Song>>(stringItemsResult);
                        item.Songs = rawSongs;
                    }
                    //var allSongs = from i in rawSongs
                    //                select i;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        allPlaylists = rawPlaylists.Where(s => s.Name.Contains(searchString));
                    }

                    //var playlistVM = new PlaylistViewModel();
                    //playlistVM.Playlists = allPlaylists.ToList();
                    //playlistVM.Songs = allSongs.ToList();
                    //return View(playlistVM);
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

        */

        [HttpGet]
        public async Task<IActionResult> Mylist(int id)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://musicparserapi.azurewebsites.net");

                var playlistResponse = client.GetAsync("/api/playlist").Result;
                //var songResponse = client.GetAsync($"/api/playlist/{user.DatListEyeDee}").Result;

                if (playlistResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var jsonDatapl = await playlistResponse.Content.ReadAsStringAsync();
                    // var jsonDataSong = await songResponse.Content.ReadAsStringAsync();

                    List<Playlist> rawAllList = JsonConvert.DeserializeObject<List<Playlist>>(jsonDatapl);
                    //SongRoot rawAllSongs = JsonConvert.DeserializeObject<SongRoot>(jsonDataSong);

                    var allPlaylists = from a in rawAllList
                                       select a;

                    // var allSongs = from s in rawAllSongs.Songs
                    // select s;

                    var myPlaylist = allPlaylists.FirstOrDefault(p => p.Id == user.DatListEyeDee);
                    myPlaylist.YouserEyeDee = user.Id;
                    myPlaylist.Id = null;
                    myPlaylist.Songs = _context.Songs.Where(s => s.PlaylistId == user.DatListEyeDee).ToList();

                    await _context.Playlists.AddAsync(myPlaylist);
                    await _context.SaveChangesAsync();

                    PlaylistViewModel mylistVM = new PlaylistViewModel();
                   
                    mylistVM.Songs = _context.Songs.Where(s => s.DatListEyeDee == user.DatListEyeDee).ToList();
                    mylistVM.Playlists = allPlaylists.Where(pl => pl.GenreID == user.DatGenreEyeDee).ToList();
                    mylistVM.User = user;

                    return View(mylistVM);
                }
                return NotFound();
            }
        }

        // replacing MyList method
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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
                    //List<Song> rawAllSongs = JsonConvert.DeserializeObject<List<Song>>(jsonDataSong);

                    var allPlaylists = from a in rawAllList
                                       select a;

                    //var allSongs = from s in rawAllSongs
                    //               select s;

                    var myPlaylist = allPlaylists.FirstOrDefault(p => p.Id == user.DatListEyeDee);
                    myPlaylist.YouserEyeDee = user.Id;
                    myPlaylist.Id = null;
                    //allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();


                    await _context.Playlists.AddAsync(myPlaylist);
                    await _context.SaveChangesAsync();

                    PlaylistViewModel mylistVM = new PlaylistViewModel();
                    //mylistVM.Songs = allSongs.Where(s => s.DatListEyeDee == user.DatListEyeDee).ToList();
                    mylistVM.Playlists = allPlaylists.Where(pl => pl.GenreID == user.DatGenreEyeDee).ToList();
                    mylistVM.User = user;

                    return RedirectToAction("Mylist", new { id = user.Id });
                }
                return NotFound();
            }
        }

        // Grabbed these code from Usercontroller- NewUser method
        // Picking a playlist 
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

                    //var allPlaylists = from a in rawAllPlaylists
                    //                   select a;

                    PlaylistViewModel plVM = new PlaylistViewModel();
                    plVM.Playlists = rawAllPlaylists;
                    plVM.User = user;

                    return View(plVM);
                }
            }
            return RedirectToAction("Home");
        }

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
                    Playlist playlist = allPlaylists.FirstOrDefault(pl => pl.GenreID == user.DatGenreEyeDee);
                    playlist.YouserEyeDee = user.Id;
                    playlist.GenreID = user.DatGenreEyeDee;


                    user.DatListEyeDee = allPlaylists.FirstOrDefault(pl => pl.GenreID == user.DatGenreEyeDee).Id.Value;

                    var songResponse = client.GetAsync($"/api/playlist/{playlist.Id}").Result;

                    if (!songResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    var jsonDataSong = await songResponse.Content.ReadAsStringAsync();
                    SongRoot rawAllSongs = JsonConvert.DeserializeObject<SongRoot>(jsonDataSong);

                    var allSongs = from s in rawAllSongs.Songs
                                   select s;

                    List<ApiSong> userSongs = allSongs.Where(s => s.PlaylistID == user.DatListEyeDee).ToList();

                    playlist.Id = null;
                    
                    await _context.Playlists.AddAsync(playlist);
                    await _context.SaveChangesAsync();

                    foreach (ApiSong song in userSongs)
                    {
                        Song newsong = new Song();
                        //newsong.ID = null;
                        newsong.Name = song.Name;
                        newsong.PlaylistId = playlist.Id;
                        newsong.DatListEyeDee = song.PlaylistID;
                        newsong.ReleaseDate = song.ReleaseDate;
                        newsong.Album = song.Album;
                        newsong.Artist = song.Artist;
                        newsong.Genre = song.Genre;

                        await _context.Songs.AddAsync(newsong);
                        await _context.SaveChangesAsync();
                    }

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Get", "Playlist", new { id = user.Id });
                }
                return NotFound();
            }
        }




        // Creating custom playlist
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
                var sortedSongs = allSongs.ToList();
                //var sortedSongs = allSongs.OrderBy(s => s.ID).ToList();
                List<int> idRef = new List<int>();
                List<Song> customList = new List<Song>();
                Playlist playlist = new Playlist();
                for (int i = 0; i < 74; i++)
                {
                    Random rdm = new Random();
                    int idX = rdm.Next(0, sortedSongs.Count());
                    if (!idRef.Contains(id))
                    {
                        idRef.Add(id);
                        ApiSong apiSong = sortedSongs.Find(s => s.ID == idX);
                        Song newsong = new Song();
                        newsong.Name = apiSong.Name;
                        newsong.PlaylistId = playlist.Id.Value;
                        newsong.DatListEyeDee = apiSong.PlaylistID;
                        newsong.ReleaseDate = apiSong.ReleaseDate;
                        newsong.Album = apiSong.Album;
                        newsong.Artist = apiSong.Artist;
                        newsong.Genre = apiSong.Genre;
                        customList.Add(newsong);
                        await _context.Songs.AddAsync(newsong);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        i--;
                    }
                }
                playlist.YouserEyeDee = user.Id;
                user.DatListEyeDee = playlist.Id.Value;
                await _context.Users.AddAsync(user);
                await _context.Playlists.AddAsync(playlist);
                await _context.SaveChangesAsync();
                return RedirectToAction("Get", "Playlist", new { id = user.Id });
            }
               return NotFound();
           }
       }






       public async Task<IActionResult> Delete(int userId)
       {
           var user = _context.Users.Find(userId);
           var playlist = _context.Playlists.Find(user.DatListEyeDee);
           user.DatGenreEyeDee = 0;
           user.DatListEyeDee = 0;

           _context.Users.Update(user);
           _context.Playlists.Remove(playlist);
           await _context.SaveChangesAsync();

           return RedirectToAction("Edit", new { id = user.Id });
       }
   }


   /*
   [HttpGet("{id}", Name = "GetPlaylist")]
   public async Task<IActionResult> GetPlaylistByID([FromRoute]int id)
   {
       var playlist = _context.Playlists.FirstOrDefault(l => l.ID == id);
       if (playlist == null)
       {
           return NotFound();
       }
       playlist.Songs = await _context.Songs.Where(i => i.PlaylistID == id).ToListAsync();

       return Ok(playlist);
   }


   [HttpPost("{genreID}")]
   public async Task<IActionResult> PostByGenre(int? genreID)
   {
       List<Song> ofSongs = new List<Song>();
       Playlist playlist = new Playlist();
       playlist.GenreID = (genreID != null) ? genreID : 0;
       playlist.Songs = playlist.CreatePlaylist(ofSongs, genreID).Result;
       playlist.Name = (genreID != null) ? playlist.Songs[0].Genre : "Unknown";

       await _context.Playlists.AddAsync(playlist);
       await _context.SaveChangesAsync();

       return CreatedAtRoute("GetPlaylist", new { id = playlist.ID }, playlist);

   }

   [HttpDelete]
   public async Task<IActionResult> Delete(int id)
   {
       var playlist = await _context.Playlists.FindAsync(id);

       if (playlist == null)
       {
           return NotFound();
       }

       List<Song> songs = await _context.Songs.Where(i => i.PlaylistID == id).ToListAsync();

       foreach (Song song in songs)
       {
           _context.Songs.Remove(song);
       }

       _context.Playlists.Remove(playlist);
       await _context.SaveChangesAsync();
       return NoContent();
   }
}
*/


    }
