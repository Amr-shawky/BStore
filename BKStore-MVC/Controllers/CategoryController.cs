using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository categoryRepository;
        IBookRepository bookRepository;
        public CategoryController(ICategoryRepository _categoryRepository, IBookRepository _bookRepository)
        {
            categoryRepository = _categoryRepository;
            bookRepository = _bookRepository;
        }
        // DONE !
        public IActionResult Index()
        {
            return View("Index", categoryRepository.GetAll());
        }

        // DONE !
        public IActionResult Details(int id)
        {
           Category categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Category Not Found");
            }
            List<Book> Books = bookRepository.GetBooksByCatgyId(categoryFromDB.CategoryID);

            if (Books == null)
            {
                return NotFound("There is no Books in this Category");
            }

            BookWithCategoryVM bookVM = new BookWithCategoryVM();

            bookVM.CategoryId = categoryFromDB.CategoryID;
            bookVM.CategoryName = categoryFromDB.Name;
            bookVM.books = Books;

            return View("Details", bookVM);
        }

        // DONE !
        public IActionResult New()
        {
            return View("New");
        }

        // DONE !
        public IActionResult SaveNew(int id, Category categoryFromRequest)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(categoryFromRequest);
                categoryRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", categoryFromRequest);
        }

        // DONE !
        public IActionResult Edit(int id)
        {
           Category categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Category Not Found");
            }
            return View("Edit", categoryFromDB); 
        }

        // DONE !
        public IActionResult SaveEdit(int id, Category categoryFromRequest)
        {
            if (ModelState.IsValid)
            {
                Category categoryFromDB = categoryRepository.GetByID(id);
                if (categoryFromDB == null)
                {
                    return NotFound("Not Found");
                }
                categoryFromDB.Name = categoryFromRequest.Name;
                categoryFromDB.Description = categoryFromRequest.Description;
                
                // Save 
                categoryRepository.Update(categoryFromDB);
                categoryRepository.Save();

                // Redirect
                return RedirectToAction("Index");
            }

            return View("Edit", categoryFromRequest);
        }

        // DONE !
        public IActionResult Delete(int id)
        {
           Category categoryFromDB =  categoryRepository.GetByID(id);

            if (categoryFromDB == null)
            {
                return NotFound("Not Found");
            }
            return View("Delete", categoryFromDB);
        }

        // DONE !
        public IActionResult ConfirmDelete(int id)
        {
            Category categoryFromDB = categoryRepository.GetByID(id);
            if (categoryFromDB == null)
            {
                return NotFound("Not Found");
            }

            categoryRepository.Delete(id);
            categoryRepository.Save();
            return RedirectToAction("Index");
        }


    }
}
