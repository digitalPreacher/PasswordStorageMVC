using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using NuGet.Frameworks;
using PasswordStorage.Controllers;
using PasswordStorage.Data;
using PasswordStorage.Models;
using System.Net;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace PasswordStorageTests
{
    public class ContainersControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<IDAL> _idal;
        private ContainersController _controller;

        public ContainersControllerTests()
        {
            _userManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _idal = new Mock<IDAL>();

            _controller = new ContainersController(_idal.Object, _userManager.Object);
        }

        [Fact]
        public void GetIndexViewNotNull()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test"}));

            _idal.Setup(x => x.GetContainers(It.IsAny<int>())).Returns(GetTestContainers());

            var result = _controller.Index(null);

            Assert.NotNull(result);
        }

        private async Task<List<Container>> GetTestContainers()
        {

            var containers = new List<Container>()
            { 
                new Container {
                    Id = 1,
                    Title = "test",
                    CreateAt = DateTime.Now,
                    Description = "test",
                    Items = null,
                    UpdateAt = DateTime.Now,
                    User = null
                },
                new Container {
                    Id = 2,
                    Title = "test2",
                    CreateAt = DateTime.Now,
                    Description = "test",
                    Items = null,
                    UpdateAt = DateTime.Now,
                    User = null
                }
            };

            return containers;
        }


        [Fact]
        public void GetCreateViewNotNull()
        {
            ViewResult result = _controller.Create() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async void PostCreateViewResult()
        {
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Title", "test" },
                { "Description", "test" }
            });

            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            _idal.Setup(x => x.CreateContainer(formCollection, new ApplicationUser { LoginName = "test" }));

            var result = await _controller.Create(formCollection);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void GetItemsListViewNotNull()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            _idal.Setup(x => x.GetContainer(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new Container { Id = 1, Title = "test" }));

            _idal.Setup(x => x.GetContainerItems(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GetTestContainerItems());

            var result = await _controller.ItemsList(1, 1, null);

            Assert.NotNull(result);
        }
        private async Task<List<ContainerItem>> GetTestContainerItems()
        {
            var containerItems = new List<ContainerItem>
            {
                new ContainerItem { Id = 1, LoginName = "test" },
                new ContainerItem { Id = 2, LoginName = "test2" },
            };

            return containerItems;
        }

        [Fact]
        public void GetCreateItemView()
        {
            var result = _controller.CreateItem(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async void PostCreateItemView()
        {
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Title", "test" },
                { "Description", "test" },
                { "LoginName", "test" },
                { "Password", "test" }
            });

            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).
                Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            _idal.Setup(x => x.GetContainer(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new Container { Id = 1, Title = "test" }));

            var result = await _controller.CreateItem(1, formCollection);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("ItemsList", redirectToActionResult.ActionName);
        }

        [Fact]
        public void GetEditViewNotNull()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            _idal.Setup(x => x.GetContainer(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new Container { Id = 1, Title = "test" }));

            var result = _controller.Edit(1);

            Assert.NotNull(result); 
        }

        [Fact]
        public async void PostEditViewReturnRedirect()
        {
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Title", "test" },
                { "Description", "test" }
            });

            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            var result = await _controller.Edit(formCollection);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Fact]
        public async void PostDeleteReturnredirect()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            var result = await _controller.Delete(1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Fact]
        public void GetEditItemViewNotNull()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            _idal.Setup(x => x.GetContainerItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new ContainerItem { Id = 1 }));

            var result = _controller.EditItem(1, 1);

            Assert.NotNull(result);

        }

        [Fact]  
        public async void PostEditItemReturnRedirect()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "Id", "1" },
                { "Title", "test" },
                { "Description", "test" },
                { "LoginName", "test" },
                { "Password", "test" }
            });

            var result = await _controller.EditItem(1, formCollection);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ItemsList", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void PostDeleteItemReturnRedirect()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            var result = await _controller.DeleteItem(1, 1);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ItemsList", redirectToActionResult.ActionName);
        }

        [Fact]
        public void GetDetailsItemViewNotNull()
        {
            _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
              .Returns(Task.FromResult(new ApplicationUser { LoginName = "test" }));

            var result = _controller.DetailsItem(1, 1);

            Assert.NotNull(result);
        }

    }
}