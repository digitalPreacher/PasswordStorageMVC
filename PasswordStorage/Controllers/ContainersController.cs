using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
                _dal.CreateContainer(form);
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
                _dal.UpdateContainer(form);
            }

            return RedirectToAction("Index");
        }

        //POST: Containers/Delte/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            { 
                return NotFound();
            }

            _dal.DeleteContainer(id);

            return RedirectToAction("Index");
        }
    }
}

      
