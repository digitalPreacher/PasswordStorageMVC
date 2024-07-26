using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordStorage.Models;
using System.Diagnostics.CodeAnalysis;

namespace PasswordStorage.Data
{
    public interface IDAL
    {
        public Task<List<Container>> GetContainers();

        public Task<List<ContainerItem>> GetContainerItems(int containerId);
        public Task<Container> GetContainer(int containerId);

        public void CreateContainer(IFormCollection form);

        public void UpdateContainer(IFormCollection form);

        public void DeleteContainer(int containerId);
        public void CreateContainerItem(Container container, ContainerItem containerItem);

        public void DeleteContainerItem(int itemId);
        public Task<ContainerItem> GetContainerItem(int itemId);
        public void UpdateContainerItem(IFormCollection form);

    }

    public class DAL : IDAL
    {
        private readonly ApplicationDbContext _db;

        public DAL(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<List<Container>> GetContainers()
        {
            return _db.Containers.ToList();
        }

        public async Task<List<ContainerItem>> GetContainerItems(int containerId)
        {
            var container = _db.Containers.Where(x => x.Id == containerId);
            var items = container.SelectMany(x => x.Items.Select(x => x.Id));
            return _db.Items.Where(x => items.Contains(x.Id)).ToList();
        }

        public async Task<Container> GetContainer(int containerId)
        {
            return await _db.Containers.FirstOrDefaultAsync(x => x.Id == containerId);
        }

        public void CreateContainer(IFormCollection form)
        {
            var newContainer = new Container(form);
            _db.Containers.Add(newContainer);
            _db.SaveChanges();
        }

        public void UpdateContainer( IFormCollection form)
        {
            var container = _db.Containers.FirstOrDefault(x => x.Id == int.Parse(form["Id"]));
            container.UpdateContainer(form);
            _db.SaveChanges();
        }
        public void DeleteContainer(int containerId) 
        {
            var container = _db.Containers.Where(x => x.Id == containerId);
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

        public Task<ContainerItem> GetContainerItem(int itemId)
        {
            return _db.Items.FirstOrDefaultAsync(x => x.Id == itemId);
        }

        public void DeleteContainerItem(int itemId)
        {
            var containerItem = _db.Items.FirstOrDefault(x => x.Id == itemId);
            _db.Items.Remove(containerItem);
            _db.SaveChanges();
        }


        public void UpdateContainerItem(IFormCollection form)
        {
            var item = _db.Items.FirstOrDefault(x => x.Id == int.Parse(form["Id"]));
            item.UpdateContainerItem(form);
            _db.SaveChanges();
        }
    }
}
