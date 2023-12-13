using DwellingAPI.AppSettings;
using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.UOW;
using DwellingAPI.NUnit.Application;
using DwellingAPI.NUnit.Exceptions;
using DwellingAPI.NUnit.ResponseWrapper;
using DwellingAPI.ResponseWrapper.Implementation;
using DwellingAPI.Services.UOW;
using DwellingAPI.Shared.Models;
using DwellingAPI.StartupSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit
{
    public class ContactsControllerTest
    {
        [Test]
        public async Task GetAllAsync_IfCountGreaterThanZero()
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
                    uowMock.Setup(x => x.ContactsRepo.GetAllAsync()).ReturnsAsync(new List<Contact>()
                    {
                        new Contact
                        {
                           Id = Guid.NewGuid(),
                           ContactOptionName = "name",
                           ContactOptionValue = "value",
                           CreationDate = DateTime.Now,
                           LastlyUpdatedDate = DateTime.Now,
                        }
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
            var response = await client.GetAsync("api/Contacts");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<IEnumerable<ContactModel>>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task GetAllAsync_IfCountIsZero()
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
                    uowMock.Setup(x => x.ContactsRepo.GetAllAsync()).ThrowsAsync(new TestOperationFailedException("Test error"));

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
            var response = await client.GetAsync("api/Contacts");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<IEnumerable<ContactModel>>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNull(baseResponse.Data);
            Assert.IsNotEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task InsertAsync_ReturnSuccess()
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

                uowMock.Setup(x => x.ContactsRepo.InsertAsync(It.IsAny<Contact>())).ReturnsAsync(new Contact()
                {
                    Id = Guid.NewGuid(),
                    ContactOptionName = "name",
                    ContactOptionValue = "value",
                    CreationDate = DateTime.Now,
                    LastlyUpdatedDate = DateTime.Now
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
                Role = "Admin"
            };

            var token = ApplicationSetup.GetTestToken(testAccountModel);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);

            var content = new StringContent(JsonConvert.SerializeObject(new Contact()
            {
                Id = Guid.NewGuid(),
                ContactOptionName = "name",
                ContactOptionValue = "value",
                CreationDate = DateTime.Now,
                LastlyUpdatedDate = DateTime.Now
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/Contacts", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ContactModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task DeleteAsync_IfExists()
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

                    uowMock.Setup(x => x.ContactsRepo.DeleteAsync(It.IsAny<string>())).ReturnsAsync(new Contact()
                    {
                        Id = Guid.NewGuid(),
                        ContactOptionName = "name",
                        ContactOptionValue = "value",
                        CreationDate = DateTime.Now,
                        LastlyUpdatedDate = DateTime.Now
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
                Role = "Admin"
            };

            var token = ApplicationSetup.GetTestToken(testAccountModel);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
            var response = await client.DeleteAsync("api/Contacts/someId");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ContactModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task DeleteAsync_IfNotExists()
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

                    var response = new ResponseWrapper<Contact>(new List<string>()
                    {
                        new string("List is empty")
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);

                    uowMock.Setup(x => x.ContactsRepo.DeleteAsync(It.IsAny<string>())).ThrowsAsync(new TestOperationFailedException("Test error"));

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
                Role = "Admin"
            };

            var token = ApplicationSetup.GetTestToken(testAccountModel);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
            var response = await client.DeleteAsync("api/Contacts/someId");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ContactModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNull(baseResponse.Data);
            Assert.IsNotEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        
    }
}
