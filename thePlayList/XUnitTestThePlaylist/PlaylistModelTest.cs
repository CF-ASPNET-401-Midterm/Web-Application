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
    public class PlaylistModelTest
    {
        [Fact]
        public void PlaylistObjectPropertyGetter()
        {
            Playlist pl = new Playlist();
            pl.Name = "playlist Name";
            pl.GenreID = 1;
            pl.UserID = 2;

            Song song = new Song
            {
                ID = 1,
                Name = "song title",
                Artist = "song artist",
                Album = "song album",
                Genre = "Cool Song"
            };

            List<Song> songList = new List<Song>();
            songList.Add(song);
            pl.Songs = songList;

            Assert.Equal("playlist Name", pl.Name);
            Assert.Equal(1, pl.GenreID);
            Assert.Equal(2, pl.UserID);
            Assert.Equal("song title", pl.Songs[0].Name);
        }

        [Fact]
        public void PlaylistObjectPropertySetter()
        {
            Playlist pl = new Playlist();
            pl.Name = "playlist Name";
            pl.GenreID = 1;
            pl.UserID = 2;

            Song song = new Song
            {
                ID = 1,
                Name = "song title",
                Artist = "song artist",
                Album = "song album",
                Genre = "Cool Song"
            };

            List<Song> songList = new List<Song>();
            songList.Add(song);
            pl.Songs = songList;

            pl.Name = "new playlist name";
            pl.GenreID = 100;
            pl.UserID = 50;
            pl.Songs[0].Name = "new song name";

            Assert.Equal("new playlist name", pl.Name);
            Assert.Equal(100, pl.GenreID);
            Assert.Equal(50, pl.UserID);
            Assert.Equal("new song name", pl.Songs[0].Name);
        }
    }
}
