using DwellingAPI.DAL.Entities;
using DwellingAPI.DAL.Repositories;
using DwellingAPI.xUnit.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwellingAPI.xUnit.Repositories
{
    public class AccountRepositoryTest
    {
        [Fact]
        public async void InsertAsync_IfAccountNotExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);

            var signInManager = new SignInManager<Account>(userManagerMock.Object, 
                new HttpContextAccessor(), new Mock<IUserClaimsPrincipalFactory<Account>>().Object, 
                new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<Account>>>().Object, 
                new Mock<IAuthenticationSchemeProvider>().Object);

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Account>())).ReturnsAsync(new TestIdentityResult(true));

            // Act
            var repo = new AccountRepository(userManagerMock.Object, signInManager);
            var result = await repo.InsertAsync(new Account() { UserName = "TEst"});

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Errors);

        }
        [Fact]
        public async void InsertAsync_IfAccountExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Account>())).ReturnsAsync(IdentityResult.Failed(new IdentityError() { Description = "failed"}));

            var signInManager = new SignInManager<Account>(userManagerMock.Object,
                new HttpContextAccessor(), new Mock<IUserClaimsPrincipalFactory<Account>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<Account>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            // Act
            var repo = new AccountRepository(userManagerMock.Object, signInManager);
            var result = await repo.InsertAsync(It.IsAny<Account>());

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotEmpty(result.Errors);
        }
        [Fact]
        public async void UpdateAsync_IfAccountNotExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void UpdateAsync_IfAccountExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void SetPasswordAsync_IfAccountNotExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void SetPasswordAsync_IfAccountExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void DeleteAsync_IfAccountNotExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void DeleteAsync_IfAccountExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void GetByIdAsync_IfAccountNotExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void GetByIdAsync_IfAccountExists()
        {
            // Arrange


            // Act


            // Assert

        }
        [Fact]
        public async void LogInAsync_IfAccountNotExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);

            // Act


            // Assert

        }
        [Fact]
        public async void LogInAsync_IfAccountExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);

            // Act


            // Assert

        }
        [Fact]
        public async void LogOutAsync_IfAccountNotExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);

            // Act


            // Assert

        }
        [Fact]
        public async void LogOutAsync_IfAccountExists()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<Account>>(
                new Mock<IUserStore<Account>>().Object, null, null, null, null, null, null, null, null);


            // Act


            // Assert

        }
    }
}
