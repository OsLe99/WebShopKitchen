﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using UppgiftDatabas.Models;
using System.Data.SqlClient;

namespace UppgiftDatabas
{
    internal class Menu
    {
        private static string connectionString = "Server=tcp:kitchenstoreoscar.database.windows.net,1433;Initial Catalog=KitchenStore;Persist Security Info=False;User ID=sqladmin;Password={Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static async Task SearchProductAsync()
        {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Enter product name to search:");
            string searchQuery = Console.ReadLine();

            if (string.IsNullOrEmpty(searchQuery))
            {
                Console.WriteLine("Search query cannot be empty.");
                return;
            }
            sw.Start();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Search query using Dapper 
                var products = await connection.QueryAsync<Product>("SELECT * FROM dbo.Product WHERE Name LIKE @Name", new { Name = "%" + searchQuery + "%" });
                sw.Stop();
                if (products.Any())
                {
                    Console.WriteLine("Search Results:");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"ID: {product.Id} | Name: {product.Name} | Price: {product.Price}:- | Category ID: {product.CategoryId}");
                    }
                }
                else
                {
                    Console.WriteLine("No products found with that name.");
                }
                Console.WriteLine($"Search took: {sw.ElapsedMilliseconds}ms");
                Console.ReadKey();
            }
        }

        // Fetch all products sorted by TotalSold
        public static async Task GetTopSellingProductAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Query to get the most sold product from CartProduct where Paid = 1
                var query = @"
                    SELECT TOP 1 p.Id, p.Name, SUM(cp.Quantity) AS TotalSold
                    FROM Product p
                    INNER JOIN CartProduct cp ON p.Id = cp.ProductId
                    WHERE cp.Paid = 1
                    GROUP BY p.Id, p.Name
                    ORDER BY TotalSold DESC";

                var topSellingProduct = await connection.QuerySingleOrDefaultAsync<dynamic>(query);
                Console.WriteLine("\n");
                if (topSellingProduct != null)
                {
                    Console.WriteLine($"Most Sold Product: {topSellingProduct.Name} | Total Sold: {topSellingProduct.TotalSold}");
                }
                else
                {
                    Console.WriteLine("Something went wrong.");
                }
                Console.ReadKey();
            }
        }

        // Fetch all products sorted by the lowest amount
        public static async Task GetProductsSortedByStockAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Query to get all products sorted by amount
                var query = @"
                    SELECT Id, Name, Amount
                    FROM Product
                    ORDER BY Amount ASC";

                var products = await connection.QueryAsync<Product>(query);
                Console.WriteLine("\n");
                Console.WriteLine("\nProducts Sorted by Lowest Stock (Amount):");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id} | Name: {product.Name} | Amount in Stock: {product.Amount}");
                }
                Console.ReadKey();
            }
        }
        // Fetch all products from a customer where Paid = 1
        public static async Task ViewPastPurchasesAsync(int customerId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                await connection.OpenAsync();

                // Adjust query to join CartProduct with ShoppingCart based on CartId and filter by CustomerId
                var query = @"
                    SELECT p.Name, cp.Quantity, p.Price, cp.Quantity * p.Price AS TotalPrice
                    FROM CartProduct cp
                    INNER JOIN Product p ON cp.ProductId = p.Id
                    INNER JOIN ShoppingCart sc ON cp.CartId = sc.Id
                    WHERE sc.CustomerId = @CustomerId AND cp.Paid = 1";

                var purchases = await connection.QueryAsync(query, new { CustomerId = customerId });
                sw.Stop();
                if (purchases.Any())
                {
                    Console.WriteLine("\nYour Past Purchases:");
                    foreach (var purchase in purchases)
                    {
                        Console.WriteLine($"Product: {purchase.Name} | Quantity: {purchase.Quantity} | Price per unit: {purchase.Price}:- | Total: {purchase.TotalPrice}:-");
                    }
                }
                else
                {
                    Console.WriteLine("You have not made any purchases yet.");
                }
                Console.WriteLine($"Search took: {sw.ElapsedMilliseconds}ms");
                Console.ReadKey();
            }
        }

        public static async Task<bool> IsUserAdminAsync(int customerId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT IsAdmin FROM Customer WHERE Id = @CustomerId";
                var isAdmin = await connection.QuerySingleOrDefaultAsync<bool>(query, new { CustomerId = customerId });

                return isAdmin;
            }
        }

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


               
                // Display category windows
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
                    "'H' for past order history",
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
                    Console.WriteLine("");
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
                    "12. See most sold product\n" +
                    "13. See products sorted by lowest amount left\n" +
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
                    case 12:
                        GetTopSellingProductAsync();
                        break;
                    case 13:
                        GetProductsSortedByStockAsync();
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
                var customerList = db.Customer.ToList();

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

                Program.LoggedInCustomerId = switchCustomer.Id;
                Program.IsAdmin = switchCustomer.IsAdmin;

                Console.WriteLine($"Switched to Customer: {switchCustomer.Name} (Admin: {switchCustomer.IsAdmin})\nPress any key to continue...");
                Console.ReadKey();
                return switchCustomer.Id;
            }
        }
    }
}
