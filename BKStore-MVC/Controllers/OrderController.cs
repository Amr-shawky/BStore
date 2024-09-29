using BKStore_MVC.Models;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using BKStore_MVC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class OrderController : Controller
    {
        IOrderBookRepository orderBookRepository;
        IOrderRepository orderRepository;
        ICustomerRepository customerRepository;
        IBookRepository bookRepository;
        IGovernorateRepository governorateRepository;
        public OrderController(IOrderRepository orderRepository ,
            ICustomerRepository customerRepository ,IBookRepository bookRepository,
            IOrderBookRepository orderBookRepository, IGovernorateRepository governorateRepository)
        {
            this.orderBookRepository = orderBookRepository;
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.bookRepository = bookRepository;
            this.governorateRepository = governorateRepository;
        }
        //[Authorize(Roles = "Delivery")]
        public IActionResult GetAll()
        {
            return View("GetAll",orderRepository.GetAll());
        }
        public IActionResult DetailedOrder(int OrderId) {
            OrderDetailVM orderDetailVM = new OrderDetailVM();
            orderDetailVM.BookName = bookRepository.GetByID(orderBookRepository.GetByID(OrderId).BookID).Title??"nothing";
            orderDetailVM.CustomerName = customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID??0).Name;
            orderDetailVM.Quantity=orderBookRepository.GetByID(OrderId).Quantity;
            orderDetailVM.TotalPrice = orderRepository.GetByID(OrderId).TotalAmount??0;
            orderDetailVM.CustomerAddress= customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID ?? 0).Address;
            orderDetailVM.Governorate = governorateRepository.GetByID(customerRepository.GetByID(orderRepository.GetByID(OrderId).CustomerID ?? 0).GovernorateID??0).Name;
            return View("DetailedOrder", orderDetailVM);
        }
        public IActionResult DeliverOrder(string CustomerName)
        {

            Order order = orderRepository.GetByCustomerID(customerRepository.GetByName(CustomerName).ID);
            order.DelivaryStatus = "Delivering";
            order.DeliveryClientsID = customerRepository.GetByName(CustomerName).ID;
            orderRepository.Update(order);
            return View("GetAll", orderRepository.GetAll());
        }
    }
}
