using Azure.Core;
using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

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
            CustomerOrderVM customerOrderVM = new CustomerOrderVM
            {
                BookItems = cartItems,
                TotalAmount = TotalAmount
            };

            ViewData["Governoratelst"] = governorateRepository.GetAll();
            if (GetCustomerID() != null)
            {
                int CustomerID = int.Parse(GetCustomerID());
                customerOrderVM.Address = customerRepository.GetByID(CustomerID).Address;
                customerOrderVM.GovernorateID = customerRepository.GetByID(CustomerID).GovernorateID;
                customerOrderVM.Name = customerRepository.GetByID(CustomerID).Name;
                customerOrderVM.Nationalnumber = customerRepository.GetByID(CustomerID).Nationalnumber;
                customerOrderVM.Phone = customerRepository.GetByID(CustomerID).Phone;
                return View("AddCustomer", customerOrderVM);
            }
            return View("AddCustomer", customerOrderVM);
        }
        public IActionResult AddToCartBuy(int bookId, int Quantity)
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
            cartItems.Add(new BookCartItem
            {
                BookId = bookId,
                Quantity = Quantity,
                ImagePath = book.ImagePath,
                Title = book.Title,
                Price = book.Price
            });

            // Serialize the updated list
            string serializedCartItems = JsonConvert.SerializeObject(cartItems);

            // Create or update the cookie
            Response.Cookies.Append("Cart", serializedCartItems, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
            });
            BookCartItem cartItem = new BookCartItem()
            {
                BookId = bookId,
                Quantity = Quantity,
                ImagePath = book.ImagePath,
                Title = book.Title,
                Price = book.Price

            };
            List<BookCartItem> BookCartItem = new List<BookCartItem>();
            BookCartItem.Add(item: cartItem);
            ViewData["Governoratelst"] = governorateRepository.GetAll();
            CustomerOrderVM customerOrderVM = new CustomerOrderVM
            {
                BookItems = BookCartItem,
                TotalAmount = (decimal?)(book.Price * Quantity + 50)
            };
            if (GetCustomerID() != null)
            {
                int CustomerID = int.Parse(GetCustomerID());
                customerOrderVM.Address = customerRepository.GetByID(CustomerID).Address;
                customerOrderVM.GovernorateID = customerRepository.GetByID(CustomerID).GovernorateID;
                customerOrderVM.Name = customerRepository.GetByID(CustomerID).Name;
                customerOrderVM.Nationalnumber = customerRepository.GetByID(CustomerID).Nationalnumber;
                customerOrderVM.Phone = customerRepository.GetByID(CustomerID).Phone;
                return View("AddCustomer", customerOrderVM);
            }
            return View("AddCustomer", customerOrderVM);            //return RedirectToAction(nameof(ShowCart));

        }


        [HttpPost]
        public IActionResult SaveAdd(CustomerOrderVM customerOrderVM)
        {
            if (ModelState.IsValid)
            {
                if (customerOrderVM.Address != null)
                {
                    var customerIDCookie = Request.Cookies["CustomerID"];
                    string customerID;
                    if (customerIDCookie != null)
                    {
                        // Use the existing cookie value
                        customerID = JsonConvert.DeserializeObject<string>(customerIDCookie);
                        Customer customer = customerRepository.GetByID(int.Parse(customerID??" "));
                        customer.Name = customerOrderVM.Name;
                        customer.Address = customerOrderVM.Address;
                        customer.Phone= customerOrderVM.Phone;
                        customer.GovernorateID= customerOrderVM.GovernorateID;
                        customer.Nationalnumber= customerOrderVM.Nationalnumber;
                        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        customer.UserID = userID;
                        customerRepository.Update(customer);
                        customerRepository.Save();
                    }
                    else
                    {
                        // Create a new cookie with the CustomerID
                        customerID = customerRepository.GetByName(customerOrderVM.Name).ID.ToString();
                        string serializedID = JsonConvert.SerializeObject(customerID);
                        Response.Cookies.Append("CustomerID", serializedID, new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddDays(7) // Set the cookie to expire in 7 days
                        });
                        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        Customer customer = new Customer
                        {
                            Address = customerOrderVM.Address,
                            Name = customerOrderVM.Name,
                            Phone = customerOrderVM.Phone,
                            GovernorateID = customerOrderVM.GovernorateID,
                            UserID = userID,
                            Nationalnumber = customerOrderVM.Nationalnumber
                        };
                        customerRepository.Add(customer);
                        customerRepository.Save();

                    }


                    Order order = new Order
                    {
                        CustomerID = customerRepository.GetByName(customerOrderVM.Name).ID,
                        OrderDate = DateTime.Now,
                        DelivaryStatus = "Pending",
                        TotalAmount = (double?)customerOrderVM.TotalAmount
                    };
                    orderRepository.Add(order);
                    orderRepository.Save();

                    // Check if the CustomerID cookie exists
                    
                    var cartCookie = Request.Cookies["Cart"];
                    List<BookCartItem> cartItems;
                    if (cartCookie != null)
                    {
                        // Deserialize the existing cookie value
                        cartItems = JsonConvert.DeserializeObject<List<BookCartItem>>(cartCookie);
                    }
                    else
                    {
                        // Initialize an empty list if the cookie does not exist
                        cartItems = new List<BookCartItem>();
                    }

                    if (customerOrderVM.BookItems.Count > 1)
                    {
                        foreach (var item in cartItems.ToList())
                        {
                            OrderBook orderBook = new OrderBook
                            {
                                BookID = item.BookId ?? 0,
                                Quantity = item.Quantity ?? 0,
                                TSubPrice = (item.Price * item.Quantity) ?? 0,
                                OrderID = order.OrderId
                            };
                            orderBookRepository.Add(orderBook);
                            orderBookRepository.Save();
                        }
                    }
                    else
                    {
                        var carts = cartItems.LastOrDefault();
                        OrderBook orderBook = new OrderBook
                        {
                            BookID = carts.BookId ?? 0,
                            Quantity = carts.Quantity ?? 0,
                            TSubPrice = (carts.Price * carts.Quantity) ?? 0,
                            OrderID = order.OrderId
                        };
                        orderBookRepository.Add(orderBook);
                        orderBookRepository.Save();
                    }

                    return RedirectToAction("GetAllByCustomerID", "Order");
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
        private string GetCustomerID()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    return customerRepository.GetByUserID(userId).ID.ToString();
                }
            }

            var customerIDCookie = Request.Cookies["CustomerID"];
            if (customerIDCookie != null)
            {
                return JsonConvert.DeserializeObject<string>(customerIDCookie);
            }

            return string.Empty;
        }

    }
}
