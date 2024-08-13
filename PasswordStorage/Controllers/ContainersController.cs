using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordStorage.Data;
using PasswordStorage.Models;

namespace PasswordStorage.Controllers
{
    [Authorize]
    public class ContainersController : Controller
    {
        private readonly IDAL _dal;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContainersController(IDAL dal, UserManager<ApplicationUser> userManager)
        {
            _dal = dal;
            _userManager = userManager;
        }

        // GET: Containers?pageNumber={id}
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var pageSize = 10;
            var currentPage = pageNumber ?? 1;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var containers = await _dal.GetContainers(user.Id);
            if (containers != null)
            {
                var totalPages = (int)Math.Ceiling(containers.Count() / (double)pageSize);
                var currentContainers = containers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.TotalPages = totalPages;

                return View(currentContainers);
            }

            return View();
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
                if (user == null)
                {
                    return BadRequest();
                }

                _dal.CreateContainer(form, user);

                return RedirectToAction("Index");
            }

            return View();
        }

        //GET: Containers/ItemsList/{id}
        public async Task<IActionResult> ItemsList(int id, int? pageNumber, string? searchItem)
        {
            var pageSize = 10;
            var currentPage = pageNumber ?? 1;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var container = await _dal.GetContainer(id, user.Id);
            if (container == null)
            {
                return NotFound();
            }

            ViewBag.ContainerId = container.Id;

            var containerItem = await _dal.GetContainerItems(id, user.Id);
            if(containerItem == null)
            {
                return NotFound();
            }

            ViewBag.TotalPages = (int)Math.Ceiling(containerItem.Count() / (double)pageSize);
            var currentItems = containerItem.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            if (searchItem != null)
            {
                var searchItems = containerItem.Where(x => x.Title.ToUpper().Contains(searchItem.ToUpper())).ToList();
                var currentSearchItems = searchItems.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                ViewBag.TotalPages = (int)Math.Ceiling(searchItems.Count() / (double)pageSize);
                ViewBag.SearchItem = searchItem;

                return View(currentSearchItems);
            }

            return View(currentItems);

        }

        // GET: Containers/ItemList/CreateItem/{id}
        public IActionResult CreateItem(int id)
        {
            ViewBag.ContainerId = id;

            return View();
        }

        // POST: Containers/ItemList/CreateItem/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItem(int id, IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return BadRequest();
            }

            if (ModelState.IsValid) 
            {
                var container = await _dal.GetContainer(id, user.Id);
                if (container != null)
                {
                    var containerItem = new ContainerItem
                    {
                        Title = form["Title"].ToString(),
                        Description = form["Description"].ToString(),
                        LoginName = form["LoginName"].ToString(),
                        Password = form["Password"].ToString()
                    };

                    ViewBag.ContainerId = id;

                    _dal.CreateContainerItem(container, containerItem);

                    return RedirectToAction("ItemsList", new { id = id });
                }
            }

            return BadRequest();
        }

        // GET: Containers/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            var container = await _dal.GetContainer(id, user.Id);
            if (container == null)
            {
                return NotFound();
            }

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
                if (user == null)
                {
                    return BadRequest();
                }

                 _dal.UpdateContainer(form, user);

                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        //POST: Containers/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _dal.DeleteContainer(id, user.Id);

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        // GET: /Containers/EditItem/{id}?containerId={id}
        public async Task<IActionResult> EditItem(int id, int containerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }
            
            var containerItem = await _dal.GetContainerItem(id, containerId, user.Id);
            if (containerItem == null) 
            { 
                return NotFound();
            }

            ViewBag.ContainerId = containerId;

            ViewBag.ContainerItemName = containerItem.Title;

            return View(containerItem);
        }

        // POST: /Containers/EditItem/{id}?containerId={id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(int? containerId, IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return BadRequest();
            }

            if (containerId == null || !int.TryParse(form["Id"], out int itemId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dal.UpdateContainerItem(form, containerId, user.Id);

                return RedirectToAction("ItemsList", new { id = containerId });
            }

            return View();

        }

        // POST: /Containers/Delete/{id}?containerId={id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int? id, int? containerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            if (id != null && containerId != null)
            {
                _dal.DeleteContainerItem(id, containerId, user.Id);

                return RedirectToAction("ItemsList", new { id = containerId });
            } 
            else
            {
                return NotFound();
            }

        }

        // GET: /Containers/DetailsItem/{id}?containerId={id}
        public async Task<IActionResult> DetailsItem(int id, int containerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            { 
                return BadRequest();
            }

            var containerItem = await _dal.GetContainerItem(id, containerId, user.Id);
            if (containerItem == null)
            {
                return NotFound();
            }
                
            ViewBag.ContainerId = containerId;

            return View(containerItem);
        }
    }
}

      
