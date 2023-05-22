using BookStoreAPI.Controllers;
using BookStoreAPI.Dto.UserDto;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookStoreAppTest
{
    [TestClass]
    public class UnitTest1
    {
        private UserController sut;
        //public UnitTest1(IConfiguration configuration, IMemoryCache memoryCache)
        //{
        //    Console.WriteLine("sgsgsgsgsggsgsgg");
        //    var userManagerMock = new Mock<UserManager<AppUser>>(MockBehavior.Strict);
        //    var roleManagerMock = new Mock<RoleManager<IdentityRole>>(MockBehavior.Strict);
        //    sut = new UserController(configuration, userManagerMock.Object, roleManagerMock.Object, memoryCache);
        //}
        [TestMethod]
        public void TestMethod1()
        {
        }
      


        [TestMethod]
        public  void RegisterBlogger_shouldReturnTokenAndRefreshToken()
        {
            //arrange
            var registrationData = new UserRegistrationParamsDto
            {
                Email = "wyahmed01yw@gmail.com",
                Password = "12345678",
                UserName = "ahmed"
            };
            //act

            //Task<ActionResult> result =  sut.Register(registrationData);

            ////assert
            //Assert.Equals(result, sut.Ok());
        }
    }
}




