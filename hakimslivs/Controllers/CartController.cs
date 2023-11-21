using hakimslivs.Data;
using hakimslivs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hakimslivs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartreadController : Controller
    {
        ApplicationDbContext context;
        UserManager<ApplicationUser> _userManager;
        public CartreadController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/GetCartItems/{json?}")]
        public object GetCartItems([FromBody] object jObject)
        {
            // Lista som ska returneras
            var items = new List<CartItems>();
            if (jObject != null)
            {
                items = GetListWithItems(jObject);
            }

            // Returnera listan till javascriptet i JSON form
            return JsonConvert.SerializeObject(items);
        }

        [HttpPost]
        [Route("/GenerateOrder/{json?}")]
        public async Task<object> GenerateOrderAsync([FromBody] object jObject)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var success = false;
            var items = new List<CartItems>();
            int orderID = 0;

            if (jObject != null)
            {
                items = GetListWithItems(jObject);
                if (items.Count == 0)
                {
                    return success;
                }

                OrderStatus status = context.OrdersStatuses.First(o => o.OrderStatusName == "Mottagen");

                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = await _userManager.FindByIdAsync(userID);

                Order newOrder = new Order
                {
                    OrderDate = DateTime.UtcNow.AddHours(2),
                    OrderStatus = status,
                    PaymentOk = false,
                    User = customer
                };
                
                context.Orders.Add(newOrder);
                await context.SaveChangesAsync();

                foreach (var item in items)
                {
                    ItemQuantity iq = new ItemQuantity
                    {
                        OrderID = newOrder.ID,
                        ItemID = item.Item.ID,
                        Quantity = item.Amount
                    };
                    context.ItemQuantities.Add(iq);

                    var item2Change = context.Items.Find(item.Item.ID);
                    item2Change.Stock -= iq.Quantity;
                }
                await context.SaveChangesAsync();
                success = true;
                orderID = newOrder.ID;
            }

            return JsonConvert.SerializeObject(orderID);
        }

        private List<CartItems> GetListWithItems(object jObject)
        {
            List<CartItems> items = new();
            // Konvertera objektet till string och sedan till en Dictionary
            var jsonString = jObject.ToString();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            foreach (var item in data)
            {
                if (item.Key == "shopping-cart")
                {
                    var cart = item.Value;
                    // Konvertera shoppingkarten till en Dictionary
                    var cartList = JsonConvert.DeserializeObject<Dictionary<string, int>>(cart);
                    foreach (var cartItem in cartList)
                    {
                        // Omvandla ID till int då den skickas in som sträng
                        _ = int.TryParse(cartItem.Key, out int id);
                        // Hitta varan som kunden vill ha
                        var curItem = context.Items.FirstOrDefault(item => item.ID == id);
                        if (curItem != null)
                        {
                            // Lägg till varan i listan om den inte är null
                            items.Add(new CartItems { Item = curItem, Amount = cartItem.Value });
                        }
                    }
                }
            }

            // Sortera varorna i listan efter namn
            items = items.OrderBy(item => item.Item.Product).ToList();
            return items;
        }
    }
}
