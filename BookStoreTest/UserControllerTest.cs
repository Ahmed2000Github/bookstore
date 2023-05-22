
using BookStoreAPI.Controllers;
using BookStoreAPI.Dto.UserDto;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using Xunit;

namespace BookStoreTest
{
 
        [TestClass]
        public class UserControllerTest
        {
        private UserController sut;
            public UserControllerTest(IConfiguration configuration, IMemoryCache memoryCache)
            {
             Console.WriteLine("sgsgsgsgsggsgsgg");
            var userManagerMock = new Mock<UserManager<AppUser>>(MockBehavior.Strict);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(MockBehavior.Strict);
            sut = new UserController(configuration, userManagerMock.Object, roleManagerMock.Object, memoryCache);
        }


        [TestMethod]
        public async void RegisterBlogger_shouldReturnTokenAndRefreshToken()
            {
            //arrange
            var registrationData = new UserRegistrationParamsDto
            {
                Email = "wyahmed01yw@gmail.com",
                Password = "12345678",
                UserName = "ahmed"
            };
            //act
            
            ActionResult result = await sut.Register(registrationData);

                //assert
                Assert.Equals(result, sut.Ok());
            }
        }
    
}