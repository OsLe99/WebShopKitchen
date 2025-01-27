using System;
using System.Linq;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    public class ShopAdd
    {
        public static void NewProduct()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ New Product +++");

                Console.Write("Product name: ");
                string productName = Console.ReadLine();

                float productPrice = Helpers.GetFloatInput("Product price: ");
                Console.Write("Product color: ");
                string productColor = Console.ReadLine();
                Console.Write("Product description: ");
                string productDesc = Console.ReadLine();

                int productAmount = Helpers.GetIntInput("How many stored in inventory: ");

                // Display and select deliverer
                var delivererList = from Deliverer in db.Deliverer
                                    select Deliverer;
                foreach (var deliverer in delivererList)
                {
                    Console.WriteLine($"ID: {deliverer.Id} Name: {deliverer.Name}");
                }
                int delivererId = Helpers.GetIntInput("Supplier ID: ");

                // Display and select category
                var categoryList = from Category in db.Category
                                   select Category;
                foreach (var category in categoryList)
                {
                    Console.WriteLine($"ID: {category.Id} Category: {category.Name}");
                }
                int categoryId = Helpers.GetIntInput("Category ID: ");

                // Chosen status
                bool productChosen = Helpers.GetYesNoInput("Is product chosen (visible on frontpage)? (y/n): ");

                Console.Clear();
                var delivererName = (from Deliverer in db.Deliverer
                                     where Deliverer.Id == delivererId
                                     select Deliverer.Name).SingleOrDefault();

                Console.WriteLine($"Name: {productName} \nPrice: {productPrice} \nColor: {productColor}");
                Console.WriteLine($"Description: {productDesc} \nAmount: {productAmount}");
                Console.WriteLine($"Deliverer: {delivererName} \nChosen: {productChosen}");
                if (Helpers.GetYesNoInput("Save product? (y/n): "))
                {
                    Console.WriteLine("\tSaving product...");
                    var product = new Product
                    {
                        Name = productName,
                        Price = productPrice,
                        Color = productColor,
                        Description = productDesc,
                        Amount = productAmount,
                        DelivererId = delivererId,
                        CategoryId = categoryId,
                        Chosen = productChosen
                    };
                    db.Product.Add(product);
                    db.SaveChanges();
                    Console.WriteLine("Product saved successfully. Press any key to continue...");
                }
                else
                {
                    Console.WriteLine("Product was not saved. Press any key to return to the menu...");
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void NewDeliverer()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ New Deliverer +++");
                Console.Write("Name: ");
                string delivererName = Console.ReadLine();

                var deliverer = new Deliverer
                {
                    Name = delivererName
                };
                db.Deliverer.Add(deliverer);
                db.SaveChanges();
                Console.WriteLine("Deliverer added successfully. Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void NewCategory()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ New Category +++");
                Console.Write("Name: ");
                string newCategory = Console.ReadLine();

                var category = new Category
                {
                    Name = newCategory
                };
                db.Category.Add(category);
                db.SaveChanges();
                Console.WriteLine("Category added successfully. Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
