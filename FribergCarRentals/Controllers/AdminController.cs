using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            AdminControlPanelViewModel adminControlPanelViewModel = new AdminControlPanelViewModel();
            List<ApplicationUser> applicationUsers = _applicationDbContext.Users.ToList();
            foreach (ApplicationUser user in applicationUsers)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                bool isUser = await _userManager.IsInRoleAsync(user, "User");
                adminControlPanelViewModel.UsersWithRoles.Add(new UserWithRolesViewModel(){
                    ApplicationUser = user,
                    IsAdmin = isAdmin,
                    IsUser = isUser
                });
            }
            return View(adminControlPanelViewModel);
        }

        // POST: AdminController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser applicationUser = _applicationDbContext.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();
            bool isAdmin = await _userManager.IsInRoleAsync(applicationUser, "Admin");
            bool isUser = await _userManager.IsInRoleAsync(applicationUser, "User");
            UserWithRolesViewModel userWithRoles = new() { 
                ApplicationUser = applicationUser, 
                IsAdmin = isAdmin, 
                IsUser = isUser };
            return View(userWithRoles);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
