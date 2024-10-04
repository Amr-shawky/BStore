using BKStore_MVC.Models;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        //public IActionResult AddCustomer(int BookId, int Quantity,double total)
        //{
        //    ViewData["Governoratelst"] = governorateRepository.GetAll();
        //    CustomerOrderVM customerOrderVM = new CustomerOrderVM();
        //    customerOrderVM.Quantity = Quantity;
        //    customerOrderVM.Book = bookRepository.GetByID(BookId);
        //    customerOrderVM.TotalAmount= total;
        //    return View("AddCustomer", customerOrderVM);
        //}
        public IActionResult AddCustomer(decimal TotalAmount)
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


            ViewData["Governoratelst"] = governorateRepository.GetAll();
            CustomerOrderVM customerOrderVM = new CustomerOrderVM
            {
                BookItems = cartItems,
                TotalAmount = TotalAmount
            };
            return View("AddCustomer", customerOrderVM);
        }
        [HttpPost]
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
                    order.TotalAmount = ((double?)customerOrderVM.TotalAmount);
                    orderRepository.Add(order);
                    orderRepository.Save();
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
                    
                    foreach(var item in cartItems.ToList())
                    {
                        OrderBook orderBook = new OrderBook
                        {
                            BookID = item.BookId??0,
                            Quantity = item.Quantity ?? 0,
                            TSubPrice = (item.Price * item.Quantity)??0,
                            OrderID = order.OrderId
                        };
                        orderBookRepository.Add(orderBook);
                        orderBookRepository.Save();
                    }
                    
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
        public IActionResult GetAll()
        {
            return View("GetAll",customerRepository.GetAll());
        }
    }
}
