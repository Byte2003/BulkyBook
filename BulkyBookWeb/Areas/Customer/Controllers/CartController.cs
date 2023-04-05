﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdenity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdenity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = getPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal+= (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);

        }
        public IActionResult Plus(int id)
        {
            var cartItem = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == id);
            _unitOfWork.ShoppingCart.IncrementCount(cartItem, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
		public IActionResult Minus(int id)
		{
			var cartItem = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == id);
            if (cartItem.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartItem);
            } else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cartItem, 1);
			}
		
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
        public IActionResult Remove(int id)
        {
            var cartItem = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == id);
            _unitOfWork.ShoppingCart.Remove(cartItem);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
			var claimsIdenity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdenity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
				OrderHeader = new()
			};
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = getPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
        }
		private double getPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
            {
                return price;
            } else if (price <=100)
            {
                return price50;
            }
            return price100;
        }
    }
}

