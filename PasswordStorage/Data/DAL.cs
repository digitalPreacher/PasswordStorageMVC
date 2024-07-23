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
        public void CreateContainerItem(Container container, ContainerItem containerItem);

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

        public void CreateContainerItem(Container container, ContainerItem containerItem)
        {
            container.AddContainerItem(containerItem);
            _db.SaveChanges();
        }
    }
}
