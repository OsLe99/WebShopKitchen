using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class ShopAlter
    {
        public static void ChangeProduct()
        {
            using (var db = new myDbContext())
            {
                Console.Clear();
                var productList = db.Product;
                // Display and select product
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
                bool menuLoop = true;
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
                        while (menuLoop == true)
                        {
                            foreach (var category in db.Category)
                            {
                                Console.WriteLine($"ID: {category.Id}\t Name: {category.Name}");
                            }
                            alterProduct.CategoryId = Helpers.GetIntInput("New category ID: ");
                            if (db.Category.Where(x => x.Id == alterProduct.CategoryId).Select(x => x.Id).SingleOrDefault() == alterProduct.CategoryId)
                            {
                                menuLoop = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid ID. Press any button to continue...");
                                Console.ReadKey();
                            } 
                        }
                        break;

                    case 4:
                        alterProduct.Price = Helpers.GetFloatInput("New price: ");
                        break;

                    case 5:
                        while (menuLoop == true)
                        {
                            foreach (var deliverer in db.Deliverer)
                            {
                                Console.WriteLine($"ID: {deliverer.Id}\t Name: {deliverer.Name}");
                            }
                            alterProduct.DelivererId = Helpers.GetIntInput("New deliverer ID: ");
                            if (db.Deliverer.Where(y => y.Id == alterProduct.DelivererId).Select(y => y.Id).SingleOrDefault() == alterProduct.DelivererId)
                            {
                                menuLoop = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid ID. Press any button to continue...");
                                Console.ReadKey();
                            } 
                        }
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
                Console.WriteLine("Product updated successfully. Press any button to continue...");
                Console.ReadKey();
            }
        }

        public static void ChangeDeliverer()
        {
            using (var db = new myDbContext())
            {
                var delivererList = db.Deliverer;
                foreach (var deliverer in delivererList)
                {
                    Console.WriteLine($"ID: {deliverer.Id} Name: {deliverer.Name}");
                }
                int alterDelivererId = Helpers.GetIntInput("ID of deliverer to change: ");
                var alterDeliverer = db.Deliverer.SingleOrDefault(deliverer => deliverer.Id == alterDelivererId);
                if (alterDeliverer == null)
                {
                    Console.WriteLine("Error: Deliverer ID not found.");
                    return;
                }
                Console.Write("New name: ");
                alterDeliverer.Name = Console.ReadLine();
                db.SaveChanges();
                Console.WriteLine("Deliverer updated successfully. Press any button to continue...");
                Console.ReadKey();
            }
        }

        public static void ChangeCategory()
        {
            using (var db = new myDbContext())
            {
                var categoryList = db.Category;
                foreach (var category in categoryList)
                {
                    Console.WriteLine($"ID: {category.Id} Name: {category.Name}");
                }
                int alterCategoryId = Helpers.GetIntInput("ID of category to change: ");
                var alterCategory = db.Category.SingleOrDefault(category => category.Id == alterCategoryId);
                if (alterCategory == null)
                {
                    Console.WriteLine("Error: Category ID not found.");
                    return;
                }
                Console.Write("New name: ");
                alterCategory.Name = Console.ReadLine();
                db.SaveChanges();
                Console.WriteLine("Category updated successfully. Press any button to continue...");
                Console.ReadKey();
            }
        }
    }
}