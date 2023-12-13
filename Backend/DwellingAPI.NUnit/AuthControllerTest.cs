using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.UOW;
using DwellingAPI.NUnit.Application;
using DwellingAPI.NUnit.ResponseWrapper;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Shared.Models;
using DwellingAPI.StartupSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit
{
    public class AuthControllerTest
    {
        [Test]
        public async Task SignUpAsync_ReturnSuccess()
        {
            WebApplicationFactory<Program> webApp = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var uow = services.FirstOrDefault(x => x.ServiceType == typeof(IDBRepository));
                    var applicationStartup = services.FirstOrDefault(x => x.ServiceType == typeof(IApplicationStartup));
                    services.Remove(uow);
                    services.Remove(applicationStartup);
                    var uowMock = new Mock<IDBRepository>();
                    var appStartMock = new Mock<IApplicationStartup>();

                    var returnAccount = new Account
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "testaccount@gmail.com",
                        PhoneNumber = "111-222-33-44",
                        UserName = "User"
                    };

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AccountRepo.InsertAsync(It.IsAny<Account>())).ReturnsAsync(returnAccount);
                    uowMock.Setup(x => x.AccountRepo.GetRoleAsync(It.IsAny<string>())).ReturnsAsync("");
                    uowMock.Setup(x => x.AccountRepo.SetPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(returnAccount);

                    services.AddTransient(_ => uowMock.Object);
                    services.AddTransient(_ => appStartMock.Object);
                });

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Photos");

                builder.UseContentRoot(Directory.GetCurrentDirectory());
            });

            HttpClient client = webApp.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new SignUpModel()
            {
                FullName = Guid.NewGuid().ToString(),
                Username = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                MobilePhone = Guid.NewGuid().ToString(),
                KeepAuthorized = true,
                Password = "password123",
                PasswordConfirm = "password123",
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Auth/SignUp", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AuthorizedUser>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task LogInAsync_ReturnSuccess()
        {
            WebApplicationFactory<Program> webApp = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var uow = services.FirstOrDefault(x => x.ServiceType == typeof(IDBRepository));
                    var applicationStartup = services.FirstOrDefault(x => x.ServiceType == typeof(IApplicationStartup));
                    services.Remove(uow);
                    services.Remove(applicationStartup);
                    var uowMock = new Mock<IDBRepository>();
                    var appStartMock = new Mock<IApplicationStartup>();

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AccountRepo.LogInAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Account
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "testaccount@gmail.com",
                        PhoneNumber = "111-222-33-44",
                        UserName = "User"
                    });
                    uowMock.Setup(x => x.AccountRepo.GetRoleAsync(It.IsAny<string>())).ReturnsAsync("Test");

                    services.AddTransient(_ => uowMock.Object);
                    services.AddTransient(_ => appStartMock.Object);
                });

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Photos");

                builder.UseContentRoot(Directory.GetCurrentDirectory());
            });

            HttpClient client = webApp.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(new LogInModel()
            {
                UserIdentifier = Guid.NewGuid().ToString(),
                KeepAuthorized = true,
                Password = "password123",

            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Auth/LogIn", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AuthorizedUser>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task LogOutAsync_ReturnSuccess()
        {
            WebApplicationFactory<Program> webApp = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var uow = services.FirstOrDefault(x => x.ServiceType == typeof(IDBRepository));
                    var applicationStartup = services.FirstOrDefault(x => x.ServiceType == typeof(IApplicationStartup));

                    services.Remove(uow);
                    services.Remove(applicationStartup);

                    var uowMock = new Mock<IDBRepository>();
                    var appStartMock = new Mock<IApplicationStartup>();

                    uowMock.Setup(x => x.AccountRepo.LogOutAsync()).ReturnsAsync(new Account
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "testaccount@gmail.com",
                        PhoneNumber = "111-222-33-44",
                        UserName = "User"
                    });

                    services.AddTransient(_ => uowMock.Object);
                    services.AddTransient(_ => appStartMock.Object);
                });

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Photos");

                builder.UseContentRoot(Directory.GetCurrentDirectory());
            });

            HttpClient client = webApp.CreateClient();

            AccountModel testAccountModel = new AccountModel()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "testaccount@gmail.com",
                MobilePhone = "111-222-33-44",
                Role = "User"
            };

            var token = ApplicationSetup.GetTestToken(testAccountModel);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);

            var response = await client.GetAsync("api/Auth/LogOut");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AuthorizedUser>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
