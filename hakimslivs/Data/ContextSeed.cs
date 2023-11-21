using hakimslivs.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace hakimslivs.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@example.com",
                FirstName = "Admin",
                LastName = "Administrator",
                Street = "The Street",
                StreetNumber = 1,
                PostalCode = 12345,
                City = "The city",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }

            }
        }
        public static Task InitializeProductAsync(ApplicationDbContext database)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            if (database.Items.Any())
            {
                return Task.CompletedTask;
            }

            if (!database.Categories.Any())
            {
                string[] categoryLines = File.ReadAllLines("Data/Category.csv", Encoding.GetEncoding("ISO-8859-1")).Skip(1).ToArray();
                foreach (var category in categoryLines)
                {
                    string[] parts = category.Split(';');
                    Category c = new Category
                    {
                        Name = parts[0],
                        Icon = parts[1]
                    };
                    database.Categories.Add(c);
                }

                database.SaveChanges();
            }
            

            string[] itemLines = File.ReadAllLines("Data/Item.csv", Encoding.GetEncoding("ISO-8859-1")).Skip(1).ToArray();
            foreach (string line in itemLines)
            {
                string[] parts = line.Split(';');

                Category category = new();
                try 
                {
                    category = database.Categories.First(c => c.Name == parts[0]);
                }
                catch
                {
                    category = null;
                }

                string product = parts[1];
                decimal price = decimal.Parse(parts[2]);
                int stock = int.Parse(parts[3]);
                string description = parts[4];
                string imageURL = parts[5];

                Item i = new Item
                {
                    Category = category,
                    Product = product,
                    Price = price,
                    Stock = stock,
                    Description = description,
                    ImageURL = imageURL
                };

                database.Items.Add(i);
            }

            database.SaveChanges();
            return Task.CompletedTask;
        }
        public static async Task InitializeUserAsync(ApplicationDbContext database, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            if (database.Users.Any())
            {
                return;
            }
            string[] userLines = File.ReadAllLines("Data/User.csv", Encoding.GetEncoding("ISO-8859-1")).Skip(1).ToArray();

            foreach (string line in userLines)
            {
                string[] parts = line.Split(';');

                string password = parts[9];
                string role = parts[10];

                ApplicationUser user = new ApplicationUser
                {
                    UserName = parts[0],
                    FirstName = parts[1],
                    LastName = parts[2],
                    DOB = DateTime.Parse(parts[3]),
                    Street = parts[4],
                    StreetNumber = int.Parse(parts[5]),
                    PostalCode = int.Parse(parts[6]),
                    City = parts[7],
                    Email = parts[8],
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(user, password);
                if (role == "Admin")
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                }
            }
        }

        public static Task InitializeOrderStatusAsync(ApplicationDbContext database)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            if (database.OrdersStatuses.Any())
            {
                return Task.CompletedTask;
            }

            if (!database.OrdersStatuses.Any())
            {
                string[] orderstatusLines = File.ReadAllLines("Data/OrderStatuses.csv", Encoding.GetEncoding("ISO-8859-1")).Skip(1).ToArray();
                foreach (var ordersStatus in orderstatusLines)
                {
                    string[] parts = ordersStatus.Split(';');
                    OrderStatus os = new OrderStatus
                    {
                        OrderStatusName = parts[0]
                    };
                    database.OrdersStatuses.Add(os);
                }

                database.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
}
