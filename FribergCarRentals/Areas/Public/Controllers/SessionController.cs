using FribergCarRentals.Mvc.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Mvc.Areas.Public.Views.Session;
using FribergCarRentals.WebApi.Dtos;
using FribergCarRentals.Core.Interfaces.ApiClients;
using FribergCarRentals.Core.Helpers;

namespace FribergCarRentals.Mvc.Areas.Public.Controllers
{

    [Area("Public")]
    public class SessionController : Controller
    {
        private readonly UserSession _userSession;
        private readonly IUserApiClient _userApiClient;
        public SessionController(UserSession userSession, IUserApiClient userApiClient)
        {
            _userSession = userSession;
            _userApiClient = userApiClient;
        }

        // GET: Public/Session
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Public/Signin
        public IActionResult Signin()
        {
            if (_userSession.IsSignedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            SigninViewModel emptyVM = new();
            return View(emptyVM);
        }

        // POST: Public/Signin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(SigninViewModel populatedSigninViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(populatedSigninViewModel);
            }

            string username = populatedSigninViewModel.Username;
            string password = populatedSigninViewModel.Password;

            UserDto userDto = await _userApiClient.GetByUsernameAsync(username);
            if (userDto.UserId == null)
            {
                await _userApiClient.RegisterAsync(username, password);
                TempData["SuccessMessage"] = UserMessage.SuccessUserCreated;
                return RedirectToAction("Index", "Home");
            }

            JwtTokenDto? jwtTokenDto = await _userApiClient.LoginAsync(username, password);
            if(jwtTokenDto == null)
            {
                TempData["ErrorMessage"] = UserMessage.ErrorUnableToSignIn;
                return View(populatedSigninViewModel);
            }

            _userSession.UserDto = userDto;
            TempData["SuccessMessage"] = UserMessage.SuccessSignedIn;
            return RedirectToAction("Index", "Home");
        }

        // GET: Public/Signout
        public IActionResult Signout()
        {
            _userSession.SignOut();
            TempData["SuccessMessage"] = UserMessage.SuccessSignedOut;
            return RedirectToAction("Index", "Home");
        }
    }
}
