using AutoMapper;
using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace BKStore_MVC.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;
        ICategoryRepository categoryRepository;
        private readonly IGovernorateRepository governorateRepository;
        IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IBookRepository _bookRepository, IWebHostEnvironment webHostEnvironment,
            IGovernorateRepository governorateRepository,
            IMapper mapper,
            ICategoryRepository _categoryRepository)
        {
            bookRepository = _bookRepository;
            categoryRepository = _categoryRepository;
            _mapper = mapper;
            this.governorateRepository = governorateRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int? page, string sortOrder)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = (page ?? 1); // Default to page 1 if no page is specified

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            var books = from b in bookRepository.GetAll()
                        select b;

            switch (sortOrder)
            {
                case "date_desc":
                    books = books.OrderByDescending(b => b.Publishdate);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Publishdate);
                    break;
            }

            BookCategVM bookCategVM = new BookCategVM();
            bookCategVM.categories = categoryRepository.GetAll();
            bookCategVM.books = books.ToPagedList(pageNumber, pageSize);

            return View("Index", bookCategVM);
        }

        public IActionResult Details(int Bookid)
        {
            Book book = bookRepository.GetByID(Bookid);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            Category category = categoryRepository.GetByID(book.CategoryID);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            var bookVM = _mapper.Map<BookWithAuthorWithPuplisherWithCategVM>(book);
            _mapper.Map(category, bookVM); // Map category properties to the view model

            return View("Details", bookVM);
        }
        [HttpGet]
        public IActionResult SearchBooks(string name)
        {
            var books = bookRepository.GetAll()
                .Where(b => b.Title.Contains(name))
                .Select(b => new { b.BookID, b.Title })
                .ToList();

            return Json(books);
        }

        [HttpGet]
        public IActionResult New()
        {
            ViewData["Categories"] = categoryRepository.GetAll();

            return View("New");
        } // Add New Book
        public IActionResult GetAllToAdmin()
        {
            return View("GetAllToAdmin",bookRepository.GetAll());
        }

        [HttpGet]
        public IActionResult SearchByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var categories = categoryRepository.GetAll();
                var books = bookRepository.GetByNameList(name);

                var bookCategVM = new BookCategVM
                {
                    categories = categories,
                    books = books.ToPagedList(1, 10), // Assuming you want the first page with 10 items per page
                    SearchName = name
                };

                return View("Index", bookCategVM);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SaveNew(Book bookFromRequest, IFormFile ImagePath)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagePath != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagePath.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagePath.CopyToAsync(fileStream);
                        }
                        bookFromRequest.ImagePath = uniqueFileName;
                    }

                    if (bookFromRequest.Publishdate == null)
                        bookFromRequest.Publishdate = DateTime.Now;

                    bookRepository.Add(bookFromRequest);
                    bookRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                }
            }

            ViewData["Categories"] = categoryRepository.GetAll();
            return View("New", bookFromRequest);
        }
        //[ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public IActionResult RateBook([FromBody] RatingModel ratingModel)
        {
            if (ratingModel == null || ratingModel.BookId <= 0 || ratingModel.Rating <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }
            if (User.Identity.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                bookRepository.RateBook(ratingModel.BookId, ratingModel.Rating, userId);
            }
            else
            {
                bookRepository.RateBook(ratingModel.BookId, ratingModel.Rating, null);
            }
            bookRepository.Save();

            var book = bookRepository.GetByID(ratingModel.BookId);
            return Ok(new { success = true, averageRating = book.AverageRating });
        }

        public IActionResult Edit(int id)
        {
            var bookModel = bookRepository.GetByID(id);

            var bookVM = _mapper.Map<BookWithAuthorWithPuplisherWithCategVM>(bookModel);
            bookVM.categories = categoryRepository.GetAll();

            return View("Edit", bookVM);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SaveEdit(int id, BookWithAuthorWithPuplisherWithCategVM bookFromRequest)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                    var bookFromDB = bookRepository.GetByID(id);
                    if (bookFromDB == null)
                    {
                        return NotFound();
                    }

                    if (bookFromRequest.ImagePath != null)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + bookFromRequest.ImagePath.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await bookFromRequest.ImagePath.CopyToAsync(fileStream);
                        }
                        bookFromDB.ImagePath = uniqueFileName;
                        bookFromRequest.BookImagePath = uniqueFileName; // Update the ViewModel
                    }
                    else
                    {
                        bookFromRequest.BookImagePath = bookFromDB.ImagePath; // Retain the existing image path
                    }

                    // Map the properties from bookFromRequest to bookFromDB, excluding ImagePath
                    _mapper.Map(bookFromRequest, bookFromDB);
                    bookRepository.Update(bookFromDB);
                    bookRepository.Save();
                    return RedirectToAction("GetAllToAdmin");
                //}
                //catch (Exception ex)
                //{
                //    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                //    ModelState.AddModelError(string.Empty, errorMessage);
                //}
            }

            bookFromRequest.categories = categoryRepository.GetAll();
            return View("Edit", bookFromRequest);
        }

        public IActionResult Delete(int id)
        {
            // Fetch the book to delete
            Book bookFromDB = bookRepository.GetByID(id);

            // If the book does not exist, return a NotFound view or error
            if (bookFromDB == null)
            {
                return NotFound();
            }
            bookRepository.Delete(id);
            bookRepository.Save();

            return RedirectToAction("GetAllToAdmin"); // Display confirmation before deletion
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
        public IActionResult AddToCart(int bookId ,int Quantity)
        {
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
            }
            else
            {
                // Initialize a new list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }
            Book book = bookRepository.GetByID(bookId);
            // Add the new item to the list
            cartItems.Add(new BookCartItem { BookId = bookId,Quantity = Quantity ,
                ImagePath= book.ImagePath,Title=book.Title,Price=book.Price
            });

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(cartItems);

            // Create or update the cookie
            Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });

            return View("Cart", cartItems);
            //return RedirectToAction(nameof(ShowCart));

        }
        public IActionResult ShowCart()
        {
            // Retrieve the existing cookie
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
            }
            else
            {
                // Initialize an empty list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }

            
            // Pass the ViewModel to the view
            return View("Cart",cartItems);
        }
        public IActionResult RemoveFromCart(int bookId)
        {
            // Retrieve the existing cookie
            var cookie = Request.Cookies["Cart"];
            List<BookCartItem> cartItems;

            if (cookie != null)
            {
                // Deserialize the existing cookie value
                cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
            }
            else
            {
                // Initialize an empty list if the cookie does not exist
                cartItems = new List<BookCartItem>();
            }

            // Find the item to remove
            var itemToRemove = cartItems.Find(item => item.BookId == bookId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(cartItems);

            // Create or update the cookie
            Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });

            // Calculate new totals
            var newSubtotal = cartItems.Sum(item => item.Price * item.Quantity);
            var newTotal = newSubtotal + 50; // Assuming a fixed shipping cost of 50 EGP

            // Return the new totals as JSON
            return Json(new { newSubtotal, newTotal });
        }
        public IActionResult GetBooksCategory(int ID)
        {
            BookCategVM bookCategVM = new BookCategVM();
            bookCategVM.categories = categoryRepository.GetAll();
            var books = bookRepository.GetBooksByCatgyId(ID);
            bookCategVM.books = books.ToPagedList(pageNumber: 1, pageSize: 10); // Adjust pageNumber and pageSize as needed
            return View("Index", bookCategVM);
        }
        public IActionResult DetailedBookForAdmin(int ID)
        {
            return View("DetailedBookForAdmin",bookRepository.GetByID(ID));
        }

    }
}
#region MyImportantTests
//public IActionResult Index(int? page)
//{
//    int pageSize = 10; // Number of items per page
//    int pageNumber = (page ?? 1); // Default to page 1 if no page is specified

//    BookCategVM bookCategVM = new BookCategVM();
//    bookCategVM.categories = categoryRepository.GetAll();
//    bookCategVM.books = bookRepository.GetAll().ToPagedList(pageNumber, pageSize);

//    return View("Index", bookCategVM);
//} // Show All Books

//public IActionResult SearchByName(string name)
//{
//        if (name != null)
//        {
//            var categories = categoryRepository.GetAll();
//            var books = bookRepository.GetByName(name);

//            var bookCategVM = new BookCategVM
//            {
//                categories = categories,
//                books = _mapper.Map<List<Book>>(books),
//                SearchName = name
//            };

//            return View("Index", bookCategVM);
//        }
//        return RedirectToAction(nameof(Index));
//}

//public IActionResult SaveNew(Book bookFromRequest)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            //save
//            if (bookFromRequest.Publishdate==null)
//            bookFromRequest.Publishdate= DateTime.Now;
//            bookRepository.Add(bookFromRequest);
//            bookRepository.Save();
//            return RedirectToAction("Index");
//        }
//        catch (Exception ex)
//        {
//            ModelState.AddModelError(string.Empty, ex.InnerException.Message);
//        }
//    }
//    ViewData["CategoryName"] = categoryRepository.GetAll();
//    return RedirectToAction("New", bookFromRequest);
//} // Save Data
//public IActionResult BuyNow(int bookId, int Quantity)
//{
//    Book book = bookRepository.GetByID(bookId);
//    BookCartItem cartItem = new BookCartItem()
//    {
//        BookId=bookId,
//        Quantity = Quantity,
//        ImagePath=book.ImagePath,
//        Title = book.Title,
//        Price = book.Price

//    };
//    List<BookCartItem> cartItems = new List<BookCartItem>();
//    cartItems.Add(item: cartItem);
//    ViewData["Governoratelst"] = governorateRepository.GetAll();
//    CustomerOrderVM customerOrderVM = new CustomerOrderVM
//    {
//        BookItems = cartItems,
//        TotalAmount = (decimal?)(book.Price * Quantity)
//    };
//    return View("AddCustomer", customerOrderVM);
//}

//[HttpPost]
//public IActionResult SaveEdit(int id, BookWithAuthorWithPuplisherWithCategVM bookFromRequest)
//{
//    if (ModelState.IsValid)
//    {
//        try
//        {
//            Book bookFromDB =
//                bookRepository.GetByID(id);

//            bookFromDB.Title = bookFromRequest.Title;
//            bookFromDB.AuthorName = bookFromRequest.AuthorName;
//            bookFromDB.StockQuantity = bookFromRequest.StockQuantity;
//            bookFromDB.Price = bookFromRequest.Price;
//            bookFromDB.PublisherName = bookFromRequest.PublisherName;
//            bookFromDB.Description = bookFromRequest.Description;
//            bookFromDB.ImagePath = bookFromRequest.BookImagePath;
//            bookFromDB.CategoryID = bookFromRequest.CategoryID;

//            bookRepository.Save();
//            return RedirectToAction("Index");
//        }
//        catch (Exception ex)
//        {
//            string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
//            ModelState.AddModelError(string.Empty, errorMessage);
//        }
//    }

//    bookFromRequest.categories = categoryRepository.GetAll();
//    return View("Edit", bookFromRequest);
//}

// Delete
//public IActionResult Edit(int id)
//{
//    Book bookModel = bookRepository.GetByID(id);

//    BookWithAuthorWithPuplisherWithCategVM bookVM =
//        new BookWithAuthorWithPuplisherWithCategVM();

//    bookVM.BookID = id;
//    bookVM.Title = bookModel.Title;
//    bookVM.AuthorName = bookModel.AuthorName;
//    bookVM.StockQuantity = bookModel.StockQuantity;
//    bookVM.Price = bookModel.Price;
//    bookVM.BookImagePath = bookModel.ImagePath;
//    bookVM.categories = categoryRepository.GetAll();
//    bookVM.PublisherName = bookModel.PublisherName;
//    bookVM.Description = bookModel.Description;
//    bookVM.CategoryID = bookModel.CategoryID;
//    bookVM.categories = categoryRepository.GetAll();

//    return View("Edit", bookVM);
//}

//public IActionResult RemoveFromCart(int bookId)
//{
//    // Retrieve the existing cookie
//    var cookie = Request.Cookies["Cart"];
//    List<BookCartItem> cartItems;

//    if (cookie != null)
//    {
//        // Deserialize the existing cookie value
//        cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cookie);
//    }
//    else
//    {
//        // Initialize an empty list if the cookie does not exist
//        cartItems = new List<BookCartItem>();
//    }

//    // Find the item to remove
//    var itemToRemove = cartItems.Find(item => item.BookId == bookId);
//    if (itemToRemove != null)
//    {
//        cartItems.Remove(itemToRemove);
//    }

//    // Serialize the updated list
//    string serializedCartItems = JsonConvert.SerializeObject(cartItems);

//    // Create or update the cookie
//    Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
//    {
//        Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
//    });

//    return RedirectToAction("ShowCart");
//}

//public IActionResult Details(int Bookid)
//{
//    Book book = bookRepository.GetByID(Bookid);
//    if (book == null)
//    {
//        return NotFound("Book not found.");
//    }

//    Category category = categoryRepository.GetByID(book.CategoryID);

//    BookWithAuthorWithPuplisherWithCategVM bookVM =
//        new BookWithAuthorWithPuplisherWithCategVM();

//    // Pass Book Props to Book View Model Class
//    bookVM.BookID = book.BookID;
//    bookVM.BookImagePath = book.ImagePath;
//    bookVM.Title = book.Title;
//    bookVM.Price = book.Price;
//    bookVM.StockQuantity = book.StockQuantity;
//    bookVM.Description = book.Description;
//    bookVM.CategoryID = category.CategoryID;
//    bookVM.CategoryName = category.Name;

//    return View("Details", bookVM);
//} // Show Book by id

//public IActionResult Cart(int Quantity)
//{

//var bookID = Request.Cookies["BookID"].ToList();
//var cartitem = new List<BookCartItem>();
//foreach (var item in bookID) {
//    Book book = bookRepository.GetByID(item);
//    BookCartItem Cart = new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title ?? "",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    };
//    cartitem.Add(Cart);
//}

//return View("Cart", cartitem);

//    var cartItems = new List<BookCartItem>
//{
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title??"",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    }
//};

//    string DeliveryIDValue = UserID;
//    CookieOptions options = new CookieOptions
//    {
//        Expires = DateTime.Now.AddDays(1)
//    };
//    Response.Cookies.Append("Did", DeliveryIDValue, options);


//    var bookId = Request.Cookies["BookID"];

//    // Your logic to get the book details using the bookId
//    var book = bookRepository.GetByID(int.Parse(bookId ?? "0"));

//    // Create a list of BookCartItem objects
//    var cartItems = new List<BookCartItem>
//{
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title??"",
//        Price = book.Price,
//        Quantity = 1 // Example quantity
//    }
//};

//}

// Set the cookie with the book ID
//CookieOptions options = new CookieOptions
//{
//    Expires = DateTime.Now.AddDays(7) // Set the cookie to expire in 7 days
//};
//Response.Cookies.Append("BookID", bookId.ToString(), options);

//Book book = bookRepository.GetByID(bookId);

//BookCartItem cart =
//    new BookCartItem
//    {
//        BookId = book.BookID,
//        Title = book.Title ?? "",
//        Price = book.Price,
//        Quantity = Quantity // Example quantity
//    };
//var cartItems =new List<BookCartItem>();
//cartItems.Add(cart);
//return View("Cart", cartItems);
////return RedirectToAction("Cart", Quantity);
#endregion