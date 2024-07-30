using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasswordStorage.Data;
using PasswordStorage.Models;

namespace PasswordStorage.Controllers
{
    [Authorize]
    public class ContainersController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContainersController(IDAL dal,
            UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _userManager = userManager;
        }

        // GET: Containers
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _dal.GetContainers(user.Id));
        }


        // GET: Containers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Containers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                _dal.CreateContainer(form, user);
                return RedirectToAction("Index");
            }

            return View();
        }

        //GET: Containers/ItemsList/{id}
        public async Task<IActionResult> ItemsList(int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var container = await _dal.GetContainer(id);
            ViewBag.ContainerId = container.Id;

            var containerItem = await _dal.GetContainerItems(id);

            return View(containerItem);
        }

        // GET: Containers/ItemList/CreateItem/{id}
        public IActionResult CreateItem()
        {
            return View();
        }

        // POST: Containers/ItemList/CreateItem/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(int id, IFormCollection form)
        {
            ViewBag.ContainerId = id;

            if (ModelState.IsValid) {
                var container = await _dal.GetContainer(id);
                if (container != null)
                {
                    var containerItem = new ContainerItem
                    {
                        Title = form["Title"].ToString(),
                        LoginName = form["LoginName"].ToString(),
                        Password = form["Password"].ToString()
                    };

                    _dal.CreateContainerItem(container, containerItem);

                    return RedirectToAction("ItemsList", new { id = id });
                } 
            }

            return RedirectToAction("Index");
        }

        // GET: Containers/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            { 
                return NotFound(); 
            }

            var container = await _dal.GetContainer(id);
            return View(container);
        }

        // POST: Containers/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);
                _dal.UpdateContainer(form, user);
            }

            return RedirectToAction("Index");
        }

        //POST: Containers/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            { 
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dal.DeleteContainer(id);
            }

            return RedirectToAction("Index");
        }

        // GET: /Containers/EditItem/{id}?containerId={id}
        public async Task<IActionResult> EditItem(int id, int containerId)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.ContainerId = containerId;

            var containerItem = await _dal.GetContainerItem(id);

            return View(containerItem);
        }

        // POST: /Containers/EditItem/{id}?containerId={id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int containerId, IFormCollection form)
        {

            if (ModelState.IsValid)
            {
                _dal.UpdateContainerItem(form);
                return RedirectToAction("ItemsList", new { id = containerId });   
            }

            return View();

        }

        // POST: /Containers/Delete/{id}?containerId={id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int id, int containerId)
        {
            if (ModelState.IsValid) 
            {
                _dal.DeleteContainerItem(id);
            }

            return RedirectToAction("ItemsList", new { id = containerId });
        }
    }
}

      
