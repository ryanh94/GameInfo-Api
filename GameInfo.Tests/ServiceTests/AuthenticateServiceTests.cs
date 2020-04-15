using GameInfo.Core.Interfaces;
using GameInfo.Core.Models;
using GameInfo.Core.Models.Entities;
using GameInfo.Core.Models.Requests;
using GameInfo.Infrastructure.Repository.Interfaces;
using GameInfo.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameInfo.Tests.ServiceTests
{
    [TestClass]
    [TestCategory("AuthenticateService")]
    public class UserServiceTests : BaseServiceTests
    {
        [TestMethod]
        public async Task Login_Valid_Returns_True()
        {

            // Arrange
            var userList = new List<User>();
            userList.Add(new User { Id = 1 });
            var mockUser = userList.AsQueryable().BuildMock();

            _Repo.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(mockUser.Object);

            var request = new SignInRequest { Username = "afag", Password = "afawg" };
            var token = new AuthToken { token = "" };

            var tokenBuilder = new Mock<ITokenBuilderService>();
            tokenBuilder.Setup(x => x.GetToken(It.IsAny<int>())).Returns(token);
            var passwordService = new Mock<IPasswordService>();
            passwordService.Setup(x => x.ValidatePassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var authService = new AuthenticateService(_DBFactory.Object, tokenBuilder.Object, passwordService.Object);
            // Act
            var results = await authService.Authenticate(request);

            // Assert
            Assert.IsTrue(results.Success);
        }
        [TestMethod]
        public async Task Login_InValid_Returns_False()
        {
            // Arrange
            var userList = new List<User>();
            var mockUser = userList.AsQueryable().BuildMock();

            _Repo.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(mockUser.Object);

            var request = new SignInRequest { Username = "afag", Password = "afawg" };

            var tokenBuilder = new Mock<ITokenBuilderService>();
            var passwordService = new Mock<IPasswordService>();
            var authService = new AuthenticateService(_DBFactory.Object, tokenBuilder.Object, passwordService.Object);
            // Act
            var results = await authService.Authenticate(request);

            // Assert
            Assert.IsFalse(results.Success);
        }
    }
}