using System;
using Xunit;
using thePlayList.Models;
using thePlayList.Data;
using thePlayList.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace XUnitTestThePlaylist
{
    public class PlaylistViewModelTest
    {
        [Fact]
        public void CheckingForApiSongClassGetter()
        {
            PlaylistViewModel plVM = new PlaylistViewModel();

            plVM.User = new User
            {
                Id = 1,
                Name = "username",
                GenreID = 27,
                PlaylistID = 1
            };


            Playlist pl = new Playlist()
            {
                Id = 1,
                Name = "playlist name",
                GenreID = 27,
                UserID = 1
            };

            List<Playlist> plList = new List<Playlist>();
            plList.Add(pl);
            plVM.Playlists = plList;

            ApiSong apiSong = new ApiSong()
            {
                ID = 1,
                Name = "Api song name",
                Artist = "API song artist",
                Album = "API Song Alubm"
            };

            List<ApiSong> apiList = new List<ApiSong>();
            apiList.Add(apiSong);
            plVM.ApiSongs = apiList;

            Song song = new Song()
            {
                ID = 1,
                Name = "Song name",
                Artist = "song artist",
                Album = "Song Album",
                Genre = "Blues",
                OurListId = 1
            };

            List<Song> songList = new List<Song>();
            songList.Add(song);
            plVM.Songs = songList;

            Assert.Equal("username", plVM.User.Name);
            Assert.Equal("playlist name", plVM.Playlists[0].Name);
            Assert.Equal("Api song name", plVM.ApiSongs[0].Name);
            Assert.Equal(song, plVM.Songs[0]);
        }

        [Fact]
        public void CheckingForApiSongClassSetter()
        {
            PlaylistViewModel plVM2 = new PlaylistViewModel();

            plVM2.User = new User
            {
                Id = 1,
                Name = "username",
                GenreID = 27,
                PlaylistID = 1
            };


            Playlist pl2 = new Playlist()
            {
                Id = 1,
                Name = "playlist name",
                GenreID = 27,
                UserID = 1
            };

            List<Playlist> plList2 = new List<Playlist>();
            plList2.Add(pl2);
            plVM2.Playlists = plList2;

            ApiSong apiSong2 = new ApiSong()
            {
                ID = 1,
                Name = "Api song name",
                Artist = "API song artist",
                Album = "API Song Alubm"
            };

            List<ApiSong> apiList2 = new List<ApiSong>();
            apiList2.Add(apiSong2);
            plVM2.ApiSongs = apiList2;

            Song song2 = new Song()
            {
                ID = 1,
                Name = "Song name",
                Artist = "song artist",
                Album = "Song Album",
                Genre = "Blues",
                OurListId = 1
            };

            List<Song> songList2 = new List<Song>();
            songList2.Add(song2);
            plVM2.Songs = songList2;

            plVM2.Playlists[0].Name = "New playlist name";
            plVM2.ApiSongs[0].Name = "New Apisong name";
            plVM2.Songs[0].Name = "New Song name";
            plVM2.User.Name = "New user name";

            Assert.Equal("New playlist name", plVM2.Playlists[0].Name);
            Assert.Equal("New Apisong name", plVM2.ApiSongs[0].Name);
            Assert.Equal("New Song name", plVM2.Songs[0].Name);
            Assert.Equal("New user name", plVM2.User.Name);
        }
    }
}

