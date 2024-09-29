using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BKStore_MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IGovernorateRepository governorateRepository;
        private readonly IOrderBookRepository orderBookRepository;
        private readonly IOrderRepository orderRepository;

        public CustomerController(IBookRepository bookRepository, ICustomerRepository customerRepository,
            IGovernorateRepository governorateRepository, IOrderBookRepository orderBookRepository,
            IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.customerRepository = customerRepository;
            this.governorateRepository = governorateRepository;
            this.orderBookRepository = orderBookRepository;
            this.orderRepository = orderRepository;
        }

        public IActionResult AddCustomer(int BookId, int Quantity)
        {
            ViewData["Governoratelst"] = governorateRepository.GetAll();
            CustomerOrderVM customerOrderVM = new CustomerOrderVM();
            customerOrderVM.Quantity = Quantity;
            customerOrderVM.Book = bookRepository.GetByID(BookId);
            return View("AddCustomer", customerOrderVM);
        }
        public IActionResult SaveAdd(CustomerOrderVM customerOrderVM)
        {
            if (ModelState.IsValid)
            {
                if (customerOrderVM.Address != null)
                {
                    Customer customer = new Customer();
                    customer.Address = customerOrderVM.Address;
                    customer.Name = customerOrderVM.Name;
                    customer.Phone = customerOrderVM.Phone;
                    customer.GovernorateID = customerOrderVM.GovernorateID;
                    customerRepository.Add(customer);
                    customerRepository.Save();
                    Order order = new Order();
                    order.CustomerID = customerRepository.GetByName(customerOrderVM.Name).ID;
                    order.OrderDate = DateTime.Now;
                    order.DelivaryStatus = "Pending";
                    order.TotalAmount = bookRepository.GetByID(customerOrderVM.BookID ?? 2).Price * (customerOrderVM.Quantity ?? 1);
                    orderRepository.Add(order);
                    orderRepository.Save();
                    OrderBook orderBook = new OrderBook();
                    orderBook.BookID = customerOrderVM.BookID ?? 2;
                    orderBook.Quantity = customerOrderVM.Quantity ?? 1;
                    orderBook.TSubPrice = bookRepository.GetByID(customerOrderVM.BookID ?? 2).Price * (customerOrderVM.Quantity ?? 1);
                    orderBook.OrderID = order.OrderId;
                    orderBookRepository.Add(orderBook);
                    orderBookRepository.Save();
                    return RedirectToAction(nameof(Index), nameof(Book));
                }

            }
            ViewData["Governoratelst"] = governorateRepository.GetAll();
            return View("AddCustomer", customerOrderVM);
        }
        public IActionResult Details(Customer customer)
        {
            return View();
        }
    }
}
