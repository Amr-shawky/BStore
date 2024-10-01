using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BKStore_MVC.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        ICategoryRepository categoryRepository;

        public BookController(IBookRepository _bookRepository, ICategoryRepository _categoryRepository)
        {
            bookRepository = _bookRepository;
            categoryRepository = _categoryRepository;
        }

        public IActionResult Index()
        {
            BookCategVM bookCategVM = new BookCategVM();
            bookCategVM.categories = categoryRepository.GetAll();
            bookCategVM.books = bookRepository.GetAll();
            return View("Index", bookCategVM);
        } // Show All Books

        public IActionResult Details(int Bookid)
        {
            Book book = bookRepository.GetByID(Bookid);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            Category category = categoryRepository.GetByID(book.CategoryID);

            BookWithAuthorWithPuplisherWithCategVM bookVM =
                new BookWithAuthorWithPuplisherWithCategVM();

            // Pass Book Props to Book View Model Class
            bookVM.BookID = book.BookID;
            bookVM.BookImagePath = book.ImagePath;
            bookVM.Title = book.Title;
            bookVM.Price = book.Price;
            bookVM.StockQuantity = book.StockQuantity;
            bookVM.Description = book.Description;
            bookVM.CategoryID = category.CategoryID;
            bookVM.CategoryName = category.Name;

            return View("Details", bookVM);
        } // Show Book by id

        [HttpGet]
        public IActionResult New()
        {
            ViewData["Categories"] = categoryRepository.GetAll();
            //  ViewData["DeptList"] =DepartmentRepository.GetAll();

            return View("New");
        } // Add New Book

        [HttpGet]
        public IActionResult SearchByName(string name)
        {
            BookCategVM bookViewModel = new BookCategVM();
            bookViewModel.categories = categoryRepository.GetAll();
            bookViewModel.books = bookRepository.GetByName(name);
            bookViewModel.SearchName = name;

            return View("Index", bookViewModel);
        }

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
            ViewData["CategoryName"] = categoryRepository.GetAll();
            return RedirectToAction("New", bookFromRequest);
        } // Save Data

        public IActionResult Edit(int id)
        {
            Book bookModel = bookRepository.GetByID(id);

            BookWithAuthorWithPuplisherWithCategVM bookVM =
                new BookWithAuthorWithPuplisherWithCategVM();

            bookVM.BookID = bookModel.BookID;
            bookVM.Title = bookModel.Title;
            bookVM.AuthorName = bookModel.AuthorName;
            bookVM.StockQuantity = bookModel.StockQuantity;
            bookVM.Price = bookModel.Price;
            bookVM.BookImagePath = bookModel.ImagePath;
            bookVM.categories = categoryRepository.GetAll();
            bookVM.PublisherName = bookModel.PublisherName;
            bookVM.Description = bookModel.Description;
            bookVM.CategoryID = bookModel.CategoryID;
            bookVM.categories = categoryRepository.GetAll();

            return View("Edit", bookVM);
        }

        [HttpPost]
        public IActionResult SaveEdit(int id, BookWithAuthorWithPuplisherWithCategVM bookFromRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Book bookFromDB =
                        bookRepository.GetByID(id);

                    bookFromDB.Title = bookFromRequest.Title;
                    bookFromDB.AuthorName = bookFromRequest.AuthorName;
                    bookFromDB.StockQuantity = bookFromRequest.StockQuantity;
                    bookFromDB.Price = bookFromRequest.Price;
                    bookFromDB.PublisherName = bookFromRequest.PublisherName;
                    bookFromDB.Description = bookFromRequest.Description;
                    bookFromDB.ImagePath = bookFromRequest.BookImagePath;
                    bookFromDB.CategoryID = bookFromRequest.CategoryID;

                    bookRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            bookFromRequest.categories = categoryRepository.GetAll();
            return View("Edit", bookFromRequest);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            // Fetch the book to delete
            Book bookFromDB = bookRepository.GetByID(id);

            // If the book does not exist, return a NotFound view or error
            if (bookFromDB == null)
            {
                return NotFound();
            }

            return View("Delete", bookFromDB); // Display confirmation before deletion
        }

        public IActionResult DeleteConfirmed(int id)
        {
            // Fetch the book to delete
            Book bookFromDB = bookRepository.GetByID(id);

            // Check if the book exists
            if (bookFromDB != null)
            {
                // Delete the book from the database
                bookRepository.Delete(id);

                // Save changes to the database
                bookRepository.Save();
            }

            // Redirect to Index after deletion
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddToCart(int bookId)
        {
            // Set the cookie with the book ID
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7) // Set the cookie to expire in 7 days
            };
            Response.Cookies.Append("BookID", bookId.ToString(), options);

            // Redirect to the Cart action
            return RedirectToAction("Cart");
        }
        public IActionResult Cart()
        {
            //var cart = Request.Cookies["cart"];
            //var cartItems = string.IsNullOrEmpty(cart) ? new List<BookCartItem>() : JsonConvert.DeserializeObject<List<BookCartItem>>(cart);
            //var bookId = Request.Cookies["BookID"];

            // Your logic to get the book details using the bookId
            //var book = bookRepository.GetByID(int.Parse(bookId??"0"));
            //BookCartItem cartItem1 = new BookCartItem();
            //cartItem1.Title = book.Title;
            //cartItem1.Price= book.Price;
            //cartItem1.ImagePath = book.ImagePath;
            //cartItem1.BookId = book.BookID;
            //cartItem1.Quantity = 1;
            var bookId = Request.Cookies["BookID"];

            // Your logic to get the book details using the bookId
            var book = bookRepository.GetByID(int.Parse(bookId ?? "0"));

            // Create a list of BookCartItem objects
            var cartItems = new List<BookCartItem>
        {
            new BookCartItem
            {
                BookId = book.BookID,
                Title = book.Title??"",
                Price = book.Price,
                Quantity = 1 // Example quantity
            }
        };

            return View("Cart", cartItems);
        }

    }
}
