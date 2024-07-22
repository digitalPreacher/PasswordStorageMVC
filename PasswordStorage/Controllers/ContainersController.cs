using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasswordStorage.Data;
using PasswordStorage.Models;

namespace PasswordStorage.Controllers
{
    public class ContainersController : Controller
    {
        private readonly IDAL _dal;

        public ContainersController(IDAL dal)
        {
            _dal = dal;
        }

        // GET: Containers
        public async Task<IActionResult> Index()
        {

            return View(await _dal.GetContainers());
            
              //return _context.Containers != null ? 
              //            View(await _context.Containers.ToListAsync()) :
              //            Problem("Entity set 'ApplicationDbContext.Containers'  is null.");
        }


        public int ContainerId { get; set; }

        public async Task<IActionResult> ItemsList(int containerId)
        {
            ContainerId = containerId;
            var container = await _dal.GetContainerItems(ContainerId);
            return View(container);
        }

        //    // GET: Containers/Details/5
        //    public async Task<IActionResult> Details(int? id)
        //    {
        //        if (id == null || _context.Containers == null)
        //        {
        //            return NotFound();
        //        }

        //        var container = await _context.Containers
        //            .FirstOrDefaultAsync(m => m.Id == id);
        //        if (container == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(container);
        //    }

        // GET: Containers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            if (!ModelState.IsValid)
            {
                _dal.CreateContainer(form);
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Containers/ItemList/CreateItem
        public IActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(IFormCollection form)
        {
            var container = _dal.GetContainer(ContainerId);
            if (container != null)
            {
                _dal.CreateContainerItem(container.Id, form);
                return RedirectToAction("ItemsList", new { containerId = ContainerId});
            }

            return View();

        }

        //    // POST: Containers/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("Id,Title,Description")] Container container)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var newContainer = new Container { Id = container.Id,
        //                Title = container.Title,
        //                Description = container.Description,
        //                CreateAt = container.CreateAt.ToUniversalTime()};

        //            _context.Add(newContainer);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View(container);
        //    }

        //    // GET: Containers/Edit/5
        //    public async Task<IActionResult> Edit(int? id)
        //    {
        //        if (id == null || _context.Containers == null)
        //        {
        //            return NotFound();
        //        }

        //        var container = await _context.Containers.FindAsync(id);
        //        if (container == null)
        //        {
        //            return NotFound();
        //        }
        //        return View(container);
        //    }

        //    // POST: Containers/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreateAt")] Container container)
        //    {
        //        if (id != container.Id)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(container);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!ContainerExists(container.Id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View(container);
        //    }

        //    // GET: Containers/Delete/5
        //    public async Task<IActionResult> Delete(int? id)
        //    {
        //        if (id == null || _context.Containers == null)
        //        {
        //            return NotFound();
        //        }

        //        var container = await _context.Containers
        //            .FirstOrDefaultAsync(m => m.Id == id);
        //        if (container == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(container);
        //    }

        //    // POST: Containers/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        if (_context.Containers == null)
        //        {
        //            return Problem("Entity set 'ApplicationDbContext.Containers'  is null.");
        //        }
        //        var container = await _context.Containers.FindAsync(id);
        //        if (container != null)
        //        {
        //            _context.Containers.Remove(container);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool ContainerExists(int id)
        //    {
        //      return (_context.Containers?.Any(e => e.Id == id)).GetValueOrDefault();
        //    }
    }
}
