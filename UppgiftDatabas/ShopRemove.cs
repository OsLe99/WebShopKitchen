using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class ShopRemove
    {
        public static void RemoveProduct()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ Remove Product +++");
                var productList = db.Product;
                // Display and select product
                foreach (var product in productList)
                {
                    Console.WriteLine($"ID: {product.Id}\t Name: {product.Name}");
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
                    Console.WriteLine("Error: Product ID not found. \nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        public static void RemoveCategory()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ Remove Category +++");
                var categoryList = db.Category;
                // Display and select category
                foreach (var category in categoryList)
                {
                    Console.WriteLine($"ID: {category.Id}\t Name: {category.Name}");
                }

                int removeId = Helpers.GetIntInput("\nCategory ID to remove: ");
                try
                {
                    var categoryToRemove = db.Category.Single(x => x.Id == removeId);
                    db.Remove(categoryToRemove);
                    db.SaveChanges();
                    Console.WriteLine("Category removed successfully.");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Error: Category ID not found. \nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public static void RemoveDeliverer()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t+++ Remove Deliverer +++");
                var delivererList = db.Deliverer;
                // Display and select deliverer
                foreach (var deliverer in delivererList)
                {
                    Console.WriteLine($"ID: {deliverer.Id}\t Name: {deliverer.Name}");
                }

                int removeId = Helpers.GetIntInput("\nDeliverer ID to remove: ");
                try
                {
                    var delivererToRemove = db.Deliverer.Single(x => x.Id == removeId);
                    db.Remove(delivererToRemove);
                    db.SaveChanges();
                    Console.WriteLine("Deliverer removed successfully.");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Error: Deliverer ID not found. \nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
