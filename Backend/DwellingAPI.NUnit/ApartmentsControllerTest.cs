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
    public class ApartmentsControllerTest
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

                    var response = new ResponseWrapper<IEnumerable<Apartment>>(new List<Apartment>{ new Apartment
                    {

                        Id = Guid.NewGuid(),
                        Number = "326",
                        Rooms = 2,
                        Address = "test",
                        City = "test",
                        ImageUrl = "test",
                        Price = 1,
                        IsActive = true,
                        Details = new ApartmentDetails
                        {
                            ApartmentId = Guid.NewGuid(),
                            Description = "test",
                            CreationDate = DateTime.Now,
                            LastlyUpdatedDate = DateTime.Now,
                            RealtorName = "test",
                            RealtorPhone = "test",
                        },
                        Photos = new List<ApartmentPhoto>
                        {
                            new ApartmentPhoto()
                            {
                                Id = Guid.NewGuid(),
                                ApartmentId = Guid.NewGuid(),
                                ImageUrl = "test",
                                CreationDate = DateTime.Now,
                                LastlyUpdatedDate = DateTime.Now,
                            }
                        }
                    }
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.ApartmentRepo.GetAllAsync()).ReturnsAsync(response);

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
            var response = await client.GetAsync("api/Apartments");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<IEnumerable<ContactModel>>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [Test]
        public async Task GetbyIdAsync_IfExists()
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

                    var response = new ResponseWrapper<Apartment>(new Apartment
                    {

                        Id = Guid.NewGuid(),
                        Number = "326",
                        Rooms = 2,
                        Address = "test",
                        City = "test",
                        ImageUrl = "test",
                        Price = 1,
                        IsActive = true,
                        Details = new ApartmentDetails
                        {
                            ApartmentId = Guid.NewGuid(),
                            Description = "test",
                            CreationDate = DateTime.Now,
                            LastlyUpdatedDate = DateTime.Now,
                            RealtorName = "test",
                            RealtorPhone = "test",
                        },
                        Photos = new List<ApartmentPhoto>
                        {
                            new ApartmentPhoto()
                            {
                                Id = Guid.NewGuid(),
                                ApartmentId = Guid.NewGuid(),
                                ImageUrl = "test",
                                CreationDate = DateTime.Now,
                                LastlyUpdatedDate = DateTime.Now,
                            }
                        }
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.ApartmentRepo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(response);
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
            var response = await client.GetAsync("api/Apartments/id");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ApartmentModel>>(stringResponse);

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

                    var response = new ResponseWrapper<Apartment>(new Apartment
                    {

                        Id = Guid.NewGuid(),
                        Number = "326",
                        Rooms = 2,
                        Address = "test",
                        City = "test",
                        ImageUrl = "test",
                        Price = 1,
                        IsActive = true,
                        Details = new ApartmentDetails
                        {
                            ApartmentId = Guid.NewGuid(),
                            Description = "test",
                            CreationDate = DateTime.Now,
                            LastlyUpdatedDate = DateTime.Now,
                            RealtorName = "test",
                            RealtorPhone = "test",
                        },
                        Photos = new List<ApartmentPhoto>
                        {
                            new ApartmentPhoto()
                            {
                                Id = Guid.NewGuid(),
                                ApartmentId = Guid.NewGuid(),
                                ImageUrl = "test",
                                CreationDate = DateTime.Now,
                                LastlyUpdatedDate = DateTime.Now,
                            }
                        }
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.ApartmentRepo.DeleteAsync(It.IsAny<string>())).ReturnsAsync(response);
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
            var response = await client.DeleteAsync("api/Apartments/id");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ApartmentModel>>(stringResponse);

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

                    var response = new ResponseWrapper<Apartment>(new Apartment
                    {

                        Id = Guid.NewGuid(),
                        Number = "326",
                        Rooms = 2,
                        Address = "test",
                        City = "test",
                        ImageUrl = "test",
                        Price = 1,
                        IsActive = true,
                        Details = new ApartmentDetails
                        {
                            ApartmentId = Guid.NewGuid(),
                            Description = "test",
                            CreationDate = DateTime.Now,
                            LastlyUpdatedDate = DateTime.Now,
                            RealtorName = "test",
                            RealtorPhone = "test",
                        },
                        Photos = new List<ApartmentPhoto>
                        {
                            new ApartmentPhoto()
                            {
                                Id = Guid.NewGuid(),
                                ApartmentId = Guid.NewGuid(),
                                ImageUrl = "test",
                                CreationDate = DateTime.Now,
                                LastlyUpdatedDate = DateTime.Now,
                            }
                        },
                        ApartmentOrders = new List<OrderApartment>()
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.ApartmentRepo.InsertAsync(It.IsAny<Apartment>())).ReturnsAsync(response);
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

            var content = new StringContent(JsonConvert.SerializeObject(new ApartmentModel
            {

                Id = Guid.NewGuid().ToString(),
                Number = "326",
                Rooms = 2,
                Address = "test",
                City = "test",
                ImageUrl = "test",
                Price = 1,
                IsActive = true,
                Description = "test",
                CreationDate = DateTime.Now,
                LastlyUpdatedDate = DateTime.Now,
                RealtorName = "test",
                RealtorPhone = "test",
                Photos = new List<ApartmentPhotoModel>(),
                Orders = new List<OrderModel>()
            }), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
            var response = await client.PostAsync("api/Apartments", content);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ApartmentModel>>(stringResponse);

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

                    var response = new ResponseWrapper<Apartment>(new Apartment
                    {

                        Id = Guid.NewGuid(),
                        Number = "326",
                        Rooms = 2,
                        Address = "test",
                        City = "test",
                        ImageUrl = "test",
                        Price = 1,
                        IsActive = true,
                        Details = new ApartmentDetails
                        {
                            ApartmentId = Guid.NewGuid(),
                            Description = "test",
                            CreationDate = DateTime.Now,
                            LastlyUpdatedDate = DateTime.Now,
                            RealtorName = "test",
                            RealtorPhone = "test",
                        },
                        Photos = new List<ApartmentPhoto>
                        {
                            new ApartmentPhoto()
                            {
                                Id = Guid.NewGuid(),
                                ApartmentId = Guid.NewGuid(),
                                ImageUrl = "test",
                                CreationDate = DateTime.Now,
                                LastlyUpdatedDate = DateTime.Now,
                            }
                        },
                        ApartmentOrders = new List<OrderApartment>()
                    }
                    );

                    appStartMock.Setup(x => x.CreateDefaultRoles()).ReturnsAsync(true);
                    appStartMock.Setup(x => x.CreateDefaultAdmin()).ReturnsAsync(true);
                    uowMock.Setup(x => x.ApartmentRepo.DeleteAsync(It.IsAny<string>())).ReturnsAsync(response);
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
            var response = await client.DeleteAsync("api/Apartments/id");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<TestResponseWrapper<ApartmentModel>>(stringResponse);

            Assert.IsNotNull(baseResponse);
            Assert.IsNotNull(response);
            Assert.IsNotNull(baseResponse.Data);
            Assert.IsEmpty(baseResponse.Errors);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
