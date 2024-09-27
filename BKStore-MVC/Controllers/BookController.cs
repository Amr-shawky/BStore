using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        IAuthorRepository authorRepository;
        IPublisherRepository publisherRepository;
        ICategoryRepository categoryRepository;
        public BookController(IBookRepository _bookRepository, IAuthorRepository _authorRepository, IPublisherRepository _publisherRepository, ICategoryRepository _categoryRepository)
        {
            bookRepository = _bookRepository;
            authorRepository = _authorRepository;
            publisherRepository = _publisherRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index()
        {
            return View("Index", bookRepository.GetAll());  
        } // Show All Books

        public  IActionResult Details(int Bookid)
        {
             Book book =  bookRepository.GetByID(Bookid);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            Author author = authorRepository.GetByID(book.AuthorID);
            Category category = categoryRepository.GetByID(book.CategoryID);
            Publisher publisher = publisherRepository.GetByID(book.PublisherID);

            BookWithAuthorWithPuplisherWithCategVM bookVM =
                new BookWithAuthorWithPuplisherWithCategVM();

            // Pass Book Props to Book View Model Class
            bookVM.BookID = book.BookID;
            bookVM.BookImagePath = book.ImagePath;
            bookVM.Title = book.Title;
            bookVM.Price = book.Price;
            bookVM.StockQuantity = book.StockQuantity;
            bookVM.Description = book.Description;
            bookVM.PublisherID = publisher.ID;
            bookVM.PublisherName = publisher.Name;
            bookVM.CategoryID = category.CategoryID;
            bookVM.CategoryName = category.Name;
            bookVM.AuthorId = author.AuthorId;
            bookVM.AuthorName = author.Name;

            return View("Details", bookVM);
        } // Show Book by id

        [HttpGet]
        public IActionResult New()
        {
            ViewData["AuthorName"] = authorRepository.GetAll();
            ViewData["Categories"] = categoryRepository.GetAll();
           //  ViewData["DeptList"] =DepartmentRepository.GetAll();
            ViewData["PublisherName"] = publisherRepository.GetAll();

            return View("New");
        } // Add New Book

        [HttpPost]
        public IActionResult SaveNew(Book bookFromRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //save
                    bookRepository.Add(bookFromRequest);
                    bookRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            ViewData["AuthorName"] = authorRepository.GetAll();
            ViewData["CategoryName"] = categoryRepository.GetAll();
            ViewData["PublisherName"] = publisherRepository.GetAll();

            return RedirectToAction("New", bookFromRequest);
        } // Save Data

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult SaveEdit(Book bookFromRequest)
        {
            return View();
        }


        //public IActionResult Delete(int id)
        //{
        //    return View();
        //} // Later

    }
}
