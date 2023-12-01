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
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.NUnit
{
    public class AgreementControllerTest
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

                    var response = new ResponseWrapper<IEnumerable<Agreement>>(new List<Agreement>()
                    {
                        new Agreement
                        {
                           Id = Guid.NewGuid(),
                           SumPerMonth = 111,
                           RealtorPaymentSum = 111,
                           ApartmentCity = "City",
                           ApartmentAddress = "Address",
                           AccountId = Guid.NewGuid().ToString(),
                           MonthCountBeforeExpiration = 5,
                           PaymentsMadeCount = 4,
                           PaymentsToMakeCount = 4,
                           CreationDate = DateTime.Now,
                           LastlyUpdatedDate = DateTime.Now,
                        }
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AgreementRepo.GetAllAsync()).ReturnsAsync(response);
                    uowMock.Setup(x => x.CommitAsync()).ReturnsAsync(new CommitResponse(1));

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
            var response = await client.GetAsync("api/Agreement");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<IEnumerable<AgreementModel>>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task GetAllByAccountIdAsync_IfCountGreaterThanZero()
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

                    var response = new ResponseWrapper<IEnumerable<Agreement>>(new List<Agreement>()
                    {
                        new Agreement
                        {
                           Id = Guid.NewGuid(),
                           SumPerMonth = 111,
                           RealtorPaymentSum = 111,
                           ApartmentCity = "City",
                           ApartmentAddress = "Address",
                           AccountId = Guid.NewGuid().ToString(),
                           MonthCountBeforeExpiration = 5,
                           PaymentsMadeCount = 4,
                           PaymentsToMakeCount = 4,
                           CreationDate = DateTime.Now,
                           LastlyUpdatedDate = DateTime.Now,
                        }
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AgreementRepo.GetAllByAccountIdAsync(It.IsAny<string>())).ReturnsAsync(response);

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
            var response = await client.GetAsync("api/Agreement/ByAccountId/someId");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<IEnumerable<AgreementModel>>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task GetByIdAsync_IfExists()
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

                    var response = new ResponseWrapper<Agreement>(new Agreement()
                    {
                        Id = Guid.NewGuid(),
                        SumPerMonth = 111,
                        RealtorPaymentSum = 111,
                        ApartmentCity = "City",
                        ApartmentAddress = "Address",
                        AccountId = Guid.NewGuid().ToString(),
                        MonthCountBeforeExpiration = 5,
                        PaymentsMadeCount = 4,
                        PaymentsToMakeCount = 4,
                        CreationDate = DateTime.Now,
                        LastlyUpdatedDate = DateTime.Now,
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AgreementRepo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(response);
                    uowMock.Setup(x => x.CommitAsync()).ReturnsAsync(new CommitResponse(1));

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
            var response = await client.GetAsync("api/Agreement/someId");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AgreementModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task InsertAsync_IfNotExists()
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

                    var response = new ResponseWrapper<Agreement>(new Agreement()
                    {
                        Id = Guid.NewGuid(),
                        SumPerMonth = 111,
                        RealtorPaymentSum = 111,
                        ApartmentCity = "City",
                        ApartmentAddress = "Address",
                        AccountId = Guid.NewGuid().ToString(),
                        MonthCountBeforeExpiration = 5,
                        PaymentsMadeCount = 4,
                        PaymentsToMakeCount = 4,
                        CreationDate = DateTime.Now,
                        LastlyUpdatedDate = DateTime.Now,
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AgreementRepo.InsertAsync(It.IsAny<Agreement>())).ReturnsAsync(response);
                    uowMock.Setup(x => x.CommitAsync()).ReturnsAsync(new CommitResponse(1));

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

            var content = new StringContent(JsonConvert.SerializeObject(new AgreementModel()
            {
                Id = string.Empty,
                SumPerMonth = 111,
                RealtorPaymentSum = 111,
                ApartmentCity = "City",
                ApartmentAddress = "Address",
                AccountId = Guid.NewGuid().ToString(),
                MonthCountBeforeExpiration = 5,
                PaymentsMadeCount = 4,
                PaymentsToMakeCount = 4,
                CreationDate = DateTime.Now,
                LastlyUpdatedDate = DateTime.Now,
            }), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
            var response = await client.PostAsync("api/Agreement", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AgreementModel>>(stringResponse);

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

                    var response = new ResponseWrapper<Agreement>(new Agreement()
                    {
                        Id = Guid.NewGuid(),
                        SumPerMonth = 111,
                        RealtorPaymentSum = 111,
                        ApartmentCity = "City",
                        ApartmentAddress = "Address",
                        AccountId = Guid.NewGuid().ToString(),
                        MonthCountBeforeExpiration = 5,
                        PaymentsMadeCount = 4,
                        PaymentsToMakeCount = 4,
                        CreationDate = DateTime.Now,
                        LastlyUpdatedDate = DateTime.Now,
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.AgreementRepo.DeleteAsync(It.IsAny<string>())).ReturnsAsync(response);
                    uowMock.Setup(x => x.CommitAsync()).ReturnsAsync(new CommitResponse(1));

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
            var response = await client.DeleteAsync("api/Agreement/someId");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<AgreementModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
