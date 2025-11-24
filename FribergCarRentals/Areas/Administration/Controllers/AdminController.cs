using FribergCarRentals.Mvc.Areas.Administration.Views.Admin;
using FribergCarRentals.Core.Helpers;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Mvc.Areas.Administration.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Administration")]
    public class AdminController : Controller
    {
        private readonly ICRUDApiClient<AdminDto> _adminDtoApiClient;
        private readonly IUserApiClient _userApiClient;

        public AdminController(ICRUDApiClient<AdminDto> adminDtoApiClient, IUserApiClient userApiClient)
        {
            _adminDtoApiClient = adminDtoApiClient;
            _userApiClient = userApiClient;
        }

        // GET: Admin
        public IActionResult Index()
        {
            return RedirectToAction("Index", "IdentityUser");
        }

        // GET: Admin/Create/abcd-1234
        [HttpGet("Administration/Admin/Create/{userId}")]
        public IActionResult Create(string? userId)
        {
            if (userId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            CreateAdminViewModel createAdminViewModel = new()
            {
                UserId = userId,
            };
            return View(createAdminViewModel);
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdminViewModel createAdminViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(createAdminViewModel);
            }

            string userId = createAdminViewModel.UserId;
            UserDto userDto = await _userApiClient.GetAsync(userId);
            if (userDto.UserId == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUserIsNull;
                return RedirectToAction("Index");
            }

            AdminDto adminDto = new()
            {
                UserId = userDto.UserId
            };
            await _adminDtoApiClient.PostAsync(adminDto);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminCreated;
            return RedirectToAction("Index");
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            AdminDto? adminDto = await _adminDtoApiClient.GetAsync((int)id);
            if (adminDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            EditAdminViewModel editAdminViewModel = new()
            {
                AdminId = adminDto.Id
            };
            return View(editAdminViewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAdminViewModel editAdminViewModel)
        {
            if (id != editAdminViewModel.AdminId)
            {

                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["WarningMessage"] = UserMessage.WarningInvalidFormData;
                return View(editAdminViewModel);
            }

            AdminDto? adminDto = await _adminDtoApiClient.GetAsync((int)id);
            if (adminDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            await _adminDtoApiClient.PutAsync(adminDto);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminUpdated;
            return RedirectToAction("Index");
        }

        // GET: Admin/DeleteAsync/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorIdIsNull;
                return RedirectToAction("Index");
            }

            AdminDto? adminDto = await _adminDtoApiClient.GetAsync((int)id);
            if (adminDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorAdminIsNull;
                return RedirectToAction("Index");
            }

            DeleteAdminViewModel deleteAdminViewModel = new DeleteAdminViewModel()
            {
                AdminId = adminDto.Id,
            };

            return View(deleteAdminViewModel);
        }

        // POST: Admin/DeleteAsync/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adminDtoApiClient.DeleteAsync(id);

            TempData["SuccessMessage"] = UserMessage.SuccessAdminDeleted;
            return RedirectToAction("Index");
        }
    }
}
