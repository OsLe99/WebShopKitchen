using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class Menu
    {
        public static void MainMenu()
        {
            using (var db = new myDbContext())
            {
                // Welcome window
                Console.Clear();
                List<string> welcomeText = new List<string>
                {
                    "Welcome to Cyber Kitchen!"
                };
                var welcomeWindow = new Window("", 0, 0, welcomeText);


               
                // Display category window
                var categoryList = db.Category.Select(x => $"ID: {x.Id} | {x.Name}").ToList();
                var categoryWindow = new Window("Category", 0, (welcomeText.Count + 2), categoryList);

                List<string> chosen1List = new List<string>();
                List<string> chosen2List = new List<string>();
                List<string> chosen3List = new List<string>();

                var categories = new List<int> { 1, 2, 3 };
                // Add chosen products into lists based on Id
                foreach (var categoryId in categories)
                {
                    var chosenList = db.Product.Where(x => x.Chosen == true && x.CategoryId == categoryId)
                        .Select(x => $"{x.Name} | {x.Price}:- | {x.Description}").ToList();

                    if (!chosenList.Any())
                    {
                        chosenList = new List<string> { "Currently no discounts."};
                    }

                    switch (categoryId)
                    {
                        case 1:
                            chosen1List = chosenList;
                            break;
                        case 2:
                            chosen2List = chosenList;
                            break;
                        case 3:
                            chosen3List = chosenList;
                            break;
                    }
                }
                var chosen1Window = new Window("Cookware Discounts", 30, 0, chosen1List);
                var chosen2Window = new Window("Utensils Discounts", 30, (chosen1List.Count + 2), chosen2List);
                var chosen3Window = new Window("Small Appliances Discounts", 30, (chosen1List.Count + chosen2List.Count + 4), chosen3List);

                // User guide
                List<string> guide = new List<string>
                {
                    "'A' for admin",
                    "'S' for search",
                    "'C' for checkout",
                    "'T' to change customer",
                    "'Y' to add customer",
                    "'Q to quit"
                };
                var guideWindow = new Window("Guide", 0, categoryList.Count + welcomeText.Count + 4, guide);

                welcomeWindow.Draw();
                guideWindow.Draw();
                chosen1Window.Draw(); 
                chosen2Window.Draw();
                chosen3Window.Draw();
                categoryWindow.Draw();
            }
        }

        public static void DisplayCategoryProducts(int categoryId, int LoggedInCustomerId)
        {
            using (var db = new myDbContext())
            {
                Console.Clear();
                var categoryName = db.Category
                    .Where(c => c.Id == categoryId)
                    .Select(c => c.Name)
                    .FirstOrDefault();

                // Get all products in the category
                var productList = db.Product
                    .Where(x => x.CategoryId == categoryId)
                    .Select(x => new { x.Id, x.Name, x.Price })
                    .ToList();

                // Display the products
                var productWindow = new Window(categoryName, 0, 0, productList.Select(p => $"ID: {p.Id} | {p.Name} | {p.Price}:-").ToList());
                productWindow.Draw();

                int productId = Helpers.GetIntInput("Enter the ID of a product to view details, or press 0 to go back: ");

                if (productId == 0)
                {
                    Console.WriteLine("Returning to the previous menu...");
                    return;
                }

                // Check if a valid product ID
                var selectedProduct = db.Product
                    .Where(x => x.CategoryId == categoryId && x.Id == productId)
                    .Select(x => new { x.Id, x.Name, x.Price, x.Description, x.Amount })
                    .FirstOrDefault();

                // Check if the product exists
                if (selectedProduct != null)
                {
                    // Display the selected product details
                    var selectedProductWindow = new Window("Product Details", 0, productList.Count + 4, new List<string>
            {
                $"ID: {selectedProduct.Id} | {selectedProduct.Name} | {selectedProduct.Price}:- | {selectedProduct.Description} | {selectedProduct.Amount} in storage."
            });
                    selectedProductWindow.Draw();

                    // Ask if they want to add the product to their cart
                    Console.WriteLine("Would you like to add this product to your cart? (y/n)");
                    var addToCartChoice = Console.ReadLine()?.ToLower();

                    if (addToCartChoice == "y")
                    {
                        // Only call AddToCart once here
                        ShopCart.AddToCart(LoggedInCustomerId, productId);
                    }
                }
                else
                {
                    Console.WriteLine("Product not found. Press any key to return.");
                    Console.ReadKey();
                }
            }
        }

        public static void Category1(int LoggedInCustomerId)
        {
            DisplayCategoryProducts(1, LoggedInCustomerId);
        }


        public static void Category2(int LoggedInCustomerId)
        {
            DisplayCategoryProducts(2, LoggedInCustomerId);  
        }

        public static void Category3(int LoggedInCustomerId)
        {
            DisplayCategoryProducts(3, LoggedInCustomerId);
        }


        public static void AdminMenu()
        {
            bool adminMenuLoop = true;
            while (adminMenuLoop == true)
            {
                Console.Clear();
                Console.WriteLine("\t+++ Admin Menu +++");
                Console.WriteLine
                    (
                    "1. Add product\n" +
                    "2. Alter product\n" +
                    "3. Remove product\n" +
                    "4. Add deliverer\n" +
                    "5. Alter deliverer\n" +
                    "6. Remove deliverer\n" +
                    "7. Add category\n" +
                    "8. Alter category\n" +
                    "9. Remove category\n" +
                    "10. Add customer\n" +
                    "11. Alter customer\n" +
                    "0. To exit Admin menu"
                    );

                int choice = Helpers.GetIntInput("Choice: ");

                switch (choice)
                {
                    case 1:
                        ShopAdd.NewProduct();
                        break;
                    case 2:
                        ShopAlter.ChangeProduct();
                        break;
                    case 3:
                        ShopRemove.RemoveProduct();
                        break;
                    case 4:
                        ShopAdd.NewDeliverer();
                        break;
                    case 5:
                        ShopAlter.ChangeDeliverer();
                        break;
                    case 6:
                        ShopRemove.RemoveDeliverer();
                        break;
                    case 7:
                        ShopAdd.NewCategory();
                        break;
                    case 8:
                        ShopAlter.ChangeCategory();
                        break;
                    case 9:
                        ShopRemove.RemoveCategory();
                        break;
                    case 10:
                        ShopAdd.NewCustomer();
                        break;
                    case 11:
                        ShopAlter.ChangeCustomer();
                        break;
                    case 0:
                        adminMenuLoop = false;
                        Console.WriteLine("Exiting Admin Menu...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static int SwitchCustomer()
        {
            using (var db = new myDbContext())
            {
                Console.Clear();
                var customerList = db.Customer;
                foreach (var customer in customerList)
                {
                    Console.WriteLine($"ID: {customer.Id}\t Name: {customer.Name}\t Email: {customer.Email}");
                }

                int switchId = Helpers.GetIntInput("Enter the ID of the customer: ");
                var switchCustomer = db.Customer.SingleOrDefault(customer => customer.Id == switchId);

                if (switchCustomer == null)
                {
                    Console.WriteLine("Error: Customer ID not found.");
                    return -1;
                }

                Console.WriteLine($"Switched to Customer: {switchCustomer.Name}\n Press any key to continue...");
                Console.ReadKey();
                return switchCustomer.Id;
            }
        }
    }
}
