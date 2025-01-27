using System;
using System.Linq;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class ShopAlter
    {
        public static void RemoveProduct()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ Remove Product +++");
                var productList = db.Product;
                foreach (var product in productList)
                {
                    Console.WriteLine($"ID: {product.Id} Name: {product.Name}");
                }

                int removeId = Helpers.GetIntInput("\nProduct ID to remove: ");
                try
                {
                    var productToRemove = db.Product.Single(x => x.Id == removeId);
                    db.Remove(productToRemove);
                    db.SaveChanges();
                    Console.WriteLine("Product removed successfully.");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Error: Product ID not found.");
                }
            }
        }

        public static void ChangeProduct()
        {
            using (var db = new myDbContext())
            {
                Console.Clear();
                var productList = db.Product;
                foreach (var product in productList)
                {
                    Console.WriteLine($"ID: {product.Id}\t Name: {product.Name}\t Color: {product.Color}\t Category: {product.CategoryId}\t Price: {product.Price}:-\t Deliverer: {product.DelivererId}" +
                                      $"\t Amount: {product.Amount}\t Chosen: {product.Chosen}");
                }

                int alterId = Helpers.GetIntInput("ID of product to change: ");
                var alterProduct = db.Product.SingleOrDefault(product => product.Id == alterId);

                if (alterProduct == null)
                {
                    Console.WriteLine("Error: Product ID not found.");
                    return;
                }

                Console.Clear();
                Console.WriteLine($"ID: {alterProduct.Id}\t 1:Name: {alterProduct.Name}\t 2:Color: {alterProduct.Color}\t 3:Category: {alterProduct.CategoryId}\t 4:Price: {alterProduct.Price}:-\t " +
                                  $"5:Deliverer: {alterProduct.DelivererId}\t 6:Amount: {alterProduct.Amount}\t 7:Chosen: {alterProduct.Chosen}");
                Console.WriteLine("Press 0 to exit.");

                int adminChoice = Helpers.GetIntInput("Select what to change: ");
                switch (adminChoice)
                {
                    case 0:
                        return;

                    case 1:
                        Console.Write("New name: ");
                        alterProduct.Name = Console.ReadLine();
                        break;

                    case 2:
                        Console.Write("New color: ");
                        alterProduct.Color = Console.ReadLine();
                        break;

                    case 3:
                        foreach (var category in db.Category)
                        {
                            Console.WriteLine($"ID: {category.Id}\t Name: {category.Name}");
                        }
                        alterProduct.CategoryId = Helpers.GetIntInput("New category ID: ");
                        break;

                    case 4:
                        alterProduct.Price = Helpers.GetFloatInput("New price: ");
                        break;

                    case 5:
                        foreach (var deliverer in db.Deliverer)
                        {
                            Console.WriteLine($"ID: {deliverer.Id}\t Name: {deliverer.Name}");
                        }
                        alterProduct.DelivererId = Helpers.GetIntInput("New deliverer ID: ");
                        break;

                    case 6:
                        alterProduct.Amount = Helpers.GetIntInput("New amount: ");
                        break;

                    case 7:
                        alterProduct.Chosen = Helpers.GetYesNoInput("Is the product chosen (Visible on frontpage)? (y/n): ");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                db.SaveChanges();
                Console.WriteLine("Product updated successfully.");
            }
        }
    }
}