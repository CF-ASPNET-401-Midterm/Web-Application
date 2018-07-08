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
    public class SongRootModelTest
    {
        [Fact]
        public void SongRootObjectGetterTest()
        {
            SongRoot sr = new SongRoot()
            {
                ID = 1,
                Name = "name of song root",
                GenreID = 50
            };

            ApiSong api = new ApiSong()
            {
                ID = 5,
                Name = "name of api song",
                Artist = "artist of api song",
                Album = "album of api song",
                Genre = "slow song"
            };

            List<ApiSong> apiList = new List<ApiSong>();
            apiList.Add(api);
            sr.Songs = apiList;

            Assert.Equal(1, sr.ID);
            Assert.Equal("name of song root", sr.Name);
            Assert.Equal(50, sr.GenreID);
            Assert.Equal("name of api song", sr.Songs[0].Name);
        }

        [Fact]
        public void SongRootObjectSetterTest()
        {
            SongRoot sr1 = new SongRoot()
            {
                ID = 1,
                Name = "name of song root",
                GenreID = 50
            };

            ApiSong api1 = new ApiSong()
            {
                ID = 5,
                Name = "name of api song",
                Artist = "artist of api song",
                Album = "album of api song",
                Genre = "slow song"
            };

            List<ApiSong> apiList1 = new List<ApiSong>();
            apiList1.Add(api1);
            sr1.Songs = apiList1;

            sr1.ID = 20;
            sr1.Name = "new name of api song";
            sr1.GenreID = 80;
            sr1.Songs[0].Artist = "name of a new artist here";

            Assert.Equal(20, sr1.ID);
            Assert.Equal("new name of api song", sr1.Name);
            Assert.Equal(80, sr1.GenreID);
            Assert.Equal("name of a new artist here", sr1.Songs[0].Artist);
        }
    }
}
