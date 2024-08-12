using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordStorage.Models;
using System.Diagnostics.CodeAnalysis;

namespace PasswordStorage.Data
{
    public interface IDAL
    {
        public Task<List<Container>> GetContainers(int userId);

        public Task<List<ContainerItem>> GetContainerItems(int containerId, int userId);
        public Task<Container> GetContainer(int containerId, int userId);

        public void CreateContainer(IFormCollection form, ApplicationUser user);

        public void UpdateContainer(IFormCollection form, ApplicationUser user);

        public void DeleteContainer(int containerId, int userId);
        public void CreateContainerItem(Container container, ContainerItem containerItem);

        public void DeleteContainerItem(int? id, int? containerId, int userId);
        public Task<ContainerItem> GetContainerItem(int id, int containerId, int userId);
        public void UpdateContainerItem(IFormCollection form, int? containerId, int userId);
        public Task<SignInResult> UserLoginAsync(SignInModel signInModel);

        public Task<IdentityResult> UserSignUpAsync(SignUpModel signUpModel);

        public Task<ApplicationUser> GetUserByEmailAsync(string email);


    }

    public class DAL : IDAL
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DAL(ApplicationDbContext db, UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<List<Container>> GetContainers(int userId)
        {
            return _db.Containers.OrderByDescending(x => x.Id).Where(x => x.User.Id == userId).ToList();
        }

        public async Task<List<ContainerItem>> GetContainerItems(int containerId, int userId)
        {
            var container = _db.Containers.Where(x => x.Id == containerId && x.User.Id == userId);
            var items = container.SelectMany(x => x.Items.Select(x => x.Id));
            return _db.Items.OrderByDescending(x => x.Id).Where(x => items.Contains(x.Id)).ToList();
        }

        public async Task<Container> GetContainer(int containerId, int userId)
        {
            return await _db.Containers.FirstOrDefaultAsync(x => x.Id == containerId && x.User.Id == userId);
        }

        public void CreateContainer(IFormCollection form, ApplicationUser user)
        {
            var newContainer = new Container(form, user);
            _db.Containers.Add(newContainer);
            _db.SaveChanges();
        }

        public void UpdateContainer( IFormCollection form, ApplicationUser user)
        {
            
            var container = _db.Containers.FirstOrDefault(x => x.Id == int.Parse(form["Id"]));
            container.UpdateContainer(form, user);
            _db.SaveChanges();
        }
        public void DeleteContainer(int containerId, int userId) 
        {
            var container = _db.Containers.Where(x => x.Id == containerId && x.User.Id == userId);
            var containerItems = container.SelectMany(x => x.Items.Select(x => x.Id));
            var deleteItems = _db.Items.Where(item => containerItems.Contains(item.Id));
            _db.Containers.RemoveRange(container);
            _db.Items.RemoveRange(deleteItems);
            _db.SaveChanges();
        }

        public void CreateContainerItem(Container container, ContainerItem containerItem)
        {
            container.AddContainerItem(containerItem);
            _db.SaveChanges();
        }

        public async Task<ContainerItem> GetContainerItem(int id, int containerId, int userId)
        {
            var container = _db.Containers.Include(x => x.Items).FirstOrDefault(x => x.Id == containerId && x.User.Id == userId);
            if (container == null)
            {
                return null;
            }
            else
            {
                var containerItem = container.Items.FirstOrDefault(x => x.Id == id);

                return containerItem;
            }
        }

        public void DeleteContainerItem(int? id, int? containerId, int userId)
        {
            var container = _db.Containers.Include(x => x.Items).FirstOrDefault(x => x.Id == containerId && x.User.Id == userId);
            var containerItem = container.Items.FirstOrDefault(x => x.Id == id);
            _db.Items.Remove(containerItem);
            _db.SaveChanges();
        }


        public void UpdateContainerItem(IFormCollection form, int? containerId, int userId)
        {
            var itemId = int.Parse(form["Id"]);
            var container = _db.Containers.Include(x => x.Items).FirstOrDefault(x => x.Id == containerId && x.User.Id == userId);
            var item = container.Items.FirstOrDefault(x => x.Id == itemId);
            item.UpdateContainerItem(form);
            _db.SaveChanges();
        }

        public async Task<SignInResult> UserLoginAsync(SignInModel user)
        {
            return await _signInManager.PasswordSignInAsync(user.LoginName, user.Password, user.RememberMe, false);
        }

        public async Task<IdentityResult> UserSignUpAsync(SignUpModel signUpModel)
        {
            var user = new ApplicationUser
            {
                LoginName = signUpModel.LoginName,
                SecretWord = signUpModel.SecretWord,
                CreateAt = DateTime.Now.ToUniversalTime(),
                Email = signUpModel.Email,
                UserName = signUpModel.LoginName
            };

            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            return result;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
