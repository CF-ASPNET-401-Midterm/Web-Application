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
    public class UserModelTest
    {
        [Fact]
        public void UserModelGetterTest()
        {
            User user = new User()
            {
                Id = 1,
                Name = "username here",
                PlaylistID = 5,
                GenreID = 10
            };

            Assert.Equal(1, user.Id);
            Assert.Equal("username here", user.Name);
            Assert.Equal(5, user.PlaylistID);
            Assert.Equal(10, user.GenreID);
        }

        [Fact]
        public void UserModelSetterTest()
        {
            User user = new User()
            {
                Id = 1,
                Name = "username here",
                PlaylistID = 5,
                GenreID = 10
            };

            user.Id = 5;
            user.Name = "new username there";
            user.PlaylistID = 55;
            user.GenreID = 88;

            Assert.Equal(5, user.Id);
            Assert.Equal("new username there", user.Name);
            Assert.Equal(55, user.PlaylistID);
            Assert.Equal(88, user.GenreID);
        }
    }
}
