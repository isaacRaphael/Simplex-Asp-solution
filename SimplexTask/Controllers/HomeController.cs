using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplexTask.Contracts;
using SimplexTask.Models;
using SimplexTask.Models.Entities;
using SimplexTask.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager, 
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }


        [Authorize]
        public async Task<IActionResult> UserDashboard(string email, int filter = 0)
        {
            if(filter == 0)
            {
                var user = await _userRepository.GetByEmail(email);
                return View(user);
            }
            var userf = await _userRepository.GetByFilter(email, filter);
            return View(userf);

        }

        
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDash(string filter = "")
        {
            if (filter == "")
            {
                var books = await _bookRepository.GetAll();
                ViewBag.Books = books;
                return View();
            }
            var booksf = await _bookRepository.GetAll();
            var filtered = booksf.Where(x => x.CreatedDate.ToString("yyyy-MM-dd").Equals(filter)).ToList();
            ViewBag.Books = filtered;
            return View();

        }



        public async  Task<IActionResult> NewBook(string Email)
        {
            var user = await _userRepository.GetByEmail(Email);
            if(user is null)
            {
                ViewBag.BookError = "could'nt process";
                return RedirectToAction("UserDashboard");
            }


            ViewBag.User = user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewBook(NewBookViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            var book = new Book
            {
                Author = model.Author,
                Name = model.BookTitle,
                Rating = model.Rating,
                User = user,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow
            };


            var result = await _bookRepository.Add(book);

            if(result is null)
            {
                ModelState.AddModelError("", "Could not create book");
                return View(model);
            }
            return RedirectToAction("UserDashboard", new { email = model.UserEmail});
        }


        public async Task<IActionResult> BookDelete(Guid Id)
        {
            var book = await  _bookRepository.GetById(Id);
            await _bookRepository.Remove(Id);
            return RedirectToAction("UserDashboard", new { email = book.User.Email });
        }

        public async Task<IActionResult> BookEdit(Guid Id)
        {
            var book = await _bookRepository.GetById(Id);
            return View(book);

        }
        [HttpPost]
        public async Task<IActionResult> BookEdit(Book book)
        {
            if(book.Rating < 1)
                return View(book);

            var toUpdate = await _bookRepository.GetById(book.Id);
            toUpdate.Rating = book.Rating;

            await _bookRepository.Update(toUpdate);
            return RedirectToAction("UserDashboard", new { email = toUpdate.User.Email });

        }

        [HttpPost]
        public  IActionResult RatingFilter(User model)
        {
            return RedirectToAction("UserDashboard", new { email = model.Email, filter = model.PhoneNumber == "*" ? 0 : int.Parse(model.PhoneNumber) });
        }
        [HttpPost]
        public IActionResult DateFilter(FilterViewModel model)
        {
            return RedirectToAction("AdminDash", new {  filter = model.Date });
        }

        public IActionResult Privacy()
        {
            return View();
        }


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
