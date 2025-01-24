using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class ShopAdd
    {
        public static void NewProduct()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t +++ New Product +++");
                Console.Write("Product name: ");
                string productName = Console.ReadLine();
                Console.Write("Product price: ");
                float productPrice = float.Parse(Console.ReadLine());
                Console.Write("Product color: ");
                string productColor = Console.ReadLine();
                Console.Write("Product description: ");
                string productDesc = Console.ReadLine();
                Console.Write("How many stored in inventory: ");
                int productAmount = int.Parse(Console.ReadLine());

                var delivererList = from Deliverer in db.Deliverer
                                    select Deliverer;

                foreach (var deliverer in delivererList)
                {
                    Console.WriteLine($"ID: {deliverer.Id} Name: {deliverer.Name}");
                }
                Console.Write("Supplier ID: ");
                int delivererId = int.Parse(Console.ReadLine());

                var categoryList = from Category in db.Category
                                   select Category;
                foreach (var category in categoryList)
                {
                    Console.WriteLine($"ID: {category.Id} Category: {category.Name}");
                }
                Console.Write("Category ID: ");
                int categoryId = int.Parse(Console.ReadLine());

                Console.Write("Is product chosen (visible on frontpage)? y/n ");
                char choice = (char)Console.Read();
                bool productChosen;
                if (choice == 'y')
                {
                    productChosen = true;
                }
                else if (choice == 'n')
                {
                    productChosen = false;
                }
                else
                {
                    Console.WriteLine("WARNING!\n Invalid choice, chosen set to false. Press any button to continue.");
                    productChosen = false;
                    Console.ReadKey();
                }
                Console.Clear();
                var delivererName = (from Deliverer in db.Deliverer
                                     where Deliverer.Id == delivererId
                                     select Deliverer.Name).SingleOrDefault();

                Console.Write($"Name: {productName} \nPrice: {productPrice} \nColor: {productColor}\n Description: {productDesc} \nAmount: {productAmount} \nDeliverer: {delivererName} \nChosen: {productChosen} \n Save product? y/n");
                ConsoleKeyInfo choice2 = Console.ReadKey(true);
                if (choice2.KeyChar == 'y')
                {
                    Console.WriteLine("\t Saving product.");
                    Models.Product products = new Models.Product()
                    {
                        Name = productName,
                        Price = productPrice,
                        Color = productColor,
                        Description = productDesc,
                        Amount = productAmount,
                        DelivererId = delivererId,
                        CategoryId = categoryId,
                        Chosen = productChosen,
                    };
                    db.Product.Add(products);
                    db.SaveChanges();
                    Console.WriteLine("Product saved. Press any button to continue.");
                    Console.ReadKey();
                }
                else if (choice2.KeyChar == 'n')
                {
                    Console.WriteLine("Product was not saved and user will be sent back to last menu. \nPress any button to continue.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("WARNING! \nInvalid choice, product was not saved and user will be sent back to last menu. \nPress any button to continue.");
                    Console.ReadKey();
                }
                Console.Clear();
            }
        }

        public static void NewDeliverer()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t +++ New Deliverer +++");
                Console.Write("Name: ");
                string delivererName = Console.ReadLine();

                Models.Deliverer deliverers = new Models.Deliverer()
                {
                    Name = delivererName
                };
                db.Deliverer.Add(deliverers);
                db.SaveChanges();
            }
        }

        public static void NewCategory()
        {
            using (var db = new myDbContext())
            {
                Console.WriteLine("\t +++ New Category +++");
                Console.Write("Name: ");
                string newCategory = Console.ReadLine();

                Models.Category categories = new Models.Category()
                {
                    Name = newCategory
                };
                db.Category.Add(categories);
                db.SaveChanges();
            }
        }
    }
}
