using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UppgiftDatabas.Models;

namespace UppgiftDatabas
{
    internal class ShopCart
    {
        public static void ViewCart(int customerId)
        {
            using (var db = new myDbContext())
            {
                if (customerId == -1)
                {
                    Console.WriteLine("No customer logged in. Please log in first.");
                    Console.ReadKey();
                    return;
                }

                var cart = db.ShoppingCart
                    .Include(sc => sc.CartProduct)
                    .SingleOrDefault(sc => sc.CustomerId == customerId);

                if (cart == null || !cart.CartProduct.Any(cp => !cp.Paid))
                {
                    Console.WriteLine("Your cart is empty or no unpaid products are found.");
                    Console.ReadKey();
                    return;
                }

                Console.Clear();
                Console.WriteLine("Your Shopping Cart:");

                var productsInCart = cart.CartProduct
                    .Where(cp => !cp.Paid) // Filter only unpaid products
                    .GroupBy(cp => cp.ProductId) // Group by product to sum 
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        TotalQuantity = group.Sum(cp => cp.Quantity), // Sum quantities
                        ProductName = db.Product.FirstOrDefault(p => p.Id == group.Key)?.Name,
                        ProductPrice = db.Product.FirstOrDefault(p => p.Id == group.Key)?.Price
                    }).ToList();

                float totalSum = 0;

                foreach (var product in productsInCart)
                {
                    if (product.ProductName != null && product.ProductPrice != null)
                    {
                        Console.WriteLine($"ID: {product.ProductId} | Name: {product.ProductName} | Quantity: {product.TotalQuantity} | Price: {product.ProductPrice}:-");
                        totalSum += (float)(product.ProductPrice * product.TotalQuantity);
                    }
                }

                Console.WriteLine($"Total Sum: {totalSum}:-");
                Console.WriteLine("Options:");
                Console.WriteLine("1. Remove an item");
                Console.WriteLine("2. Checkout");
                Console.WriteLine("0. Go back");

                int choice = Helpers.GetIntInput("Enter your choice: ");

                switch (choice)
                {
                    case 1:
                        RemoveItem(customerId);
                        break;
                    case 2:
                        Checkout(customerId, totalSum);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Returning...");
                        break;
                }
            }
        }

        public static void AddToCart(int loggedInCustomerId, int productId)
        {
            using (var db = new myDbContext())
            {
                if (loggedInCustomerId == -1)
                {
                    Console.WriteLine("No customer logged in. Please switch customers first.");
                    return;
                }

                // Get the active shopping cart for the logged-in customer
                var cart = db.ShoppingCart.FirstOrDefault(c => c.CustomerId == loggedInCustomerId);
                if (cart == null)
                {
                    Console.WriteLine("No cart found for this customer. Creating a new cart.");
                    cart = new ShoppingCart { CustomerId = loggedInCustomerId, SumCart = 0 };
                    db.ShoppingCart.Add(cart);
                    db.SaveChanges();
                }

                // Ensure that the cart exists and CartProduct collection exists
                if (cart.CartProduct == null)
                {
                    cart.CartProduct = new List<CartProduct>();
                }

                // Ensure that product exists
                var product = db.Product.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {
                    Console.WriteLine("Product not found in the database.");
                    return;
                }

                // Ensure that there is stock available
                if (product.Amount <= 0)
                {
                    Console.WriteLine("Product is out of stock.");
                    return;
                }
                Console.WriteLine("");
                int quantity = Helpers.GetIntInput($"Enter quantity for {product.Name} (Max {product.Amount}): ");
                if (quantity <= 0 || quantity > product.Amount)
                {
                    Console.WriteLine("Invalid quantity.");
                    return;
                }

                // Check if the product already exists in the cart, update the quantity if true
                var existingCartProduct = cart.CartProduct.SingleOrDefault(cp => cp.ProductId == productId && !cp.Paid);
                if (existingCartProduct != null)
                {
                    existingCartProduct.Quantity += quantity;
                    Console.WriteLine($"{quantity} additional {product.Name} added to your cart.");
                }
                else
                {
                    // Add product to cart if it doesn't exist
                    var cartProduct = new CartProduct
                    {
                        CartId = cart.Id,
                        ProductId = productId,
                        Quantity = quantity,
                        Paid = false
                    };

                    db.CartProduct.Add(cartProduct);
                    Console.WriteLine($"{quantity}x {product.Name} added to your cart.");
                }

                // Update inventory
                product.Amount -= quantity;
                db.SaveChanges();
            }
        }

        private static void RemoveItem(int customerId)
        {
            using (var db = new myDbContext())
            {
                // Fetch the customer's cart and CartProducts
                var cart = db.ShoppingCart
                    .Include(sc => sc.CartProduct)  // Ensure CartProduct relation is loaded
                    .SingleOrDefault(sc => sc.CustomerId == customerId);

                if (cart == null || !cart.CartProduct.Any(cp => !cp.Paid))
                {
                    Console.WriteLine("No unpaid items in your cart.");
                    return;
                }

                // Ask the user for the Product ID to remove
                int productId = Helpers.GetIntInput("Enter the Product ID to remove: ");

                // Find matching CartProducts with the given ProductId and unpaid status
                var cartProducts = cart.CartProduct.Where(cp => cp.ProductId == productId && !cp.Paid).ToList();

                if (cartProducts.Count == 0)
                {
                    Console.WriteLine("Item not found in your cart.");
                    return;
                }

                // If multiple matching CartProducts are found
                if (cartProducts.Count > 1)
                {
                    Console.WriteLine($"There are {cartProducts.Count} identical items in your cart. Do you want to remove all of them? (y/n)");
                    string removeAll = Console.ReadLine()?.ToLower();

                    if (removeAll == "y")
                    {
                        foreach (var cartProduct in cartProducts)
                        {
                            db.CartProduct.Remove(cartProduct);  // Remove each matched item
                        }
                        db.SaveChanges();
                        Console.WriteLine($"{cartProducts.Count} items removed from your cart.");
                    }
                    else
                    {
                        Console.WriteLine("No items were removed.");
                    }
                }
                else
                {
                    // If only one matching CartProduct, remove it
                    var cartProduct = cartProducts.First();
                    var product = db.Product.SingleOrDefault(p => p.Id == productId);

                    if (product != null)
                    {
                        db.CartProduct.Remove(cartProduct);
                        db.SaveChanges();
                        Console.WriteLine($"Item '{product.Name}' removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Product not found in the inventory.");
                    }
                }
            }
        }

        private static void Checkout(int customerId, float totalSum)
        {
            using (var db = new myDbContext())
            {
                // Ensure the customer is logged in
                if (customerId == -1)
                {
                    Console.WriteLine("No customer logged in.");
                    Console.ReadKey();
                    return;
                }

                // Fetch the cart for the logged-in customer, and eagerly load CartProduct collection
                var cart = db.ShoppingCart
                             .Include(sc => sc.CartProduct)  // Ensure CartProduct is included
                             .SingleOrDefault(sc => sc.CustomerId == customerId);

                if (cart == null)
                {
                    Console.WriteLine("No cart found for this customer.");
                    Console.ReadKey();
                    return;
                }

                // Ensure CartProduct is not null before continuing
                if (cart.CartProduct == null)
                {
                    Console.WriteLine("No products in the cart.");
                    Console.ReadKey();
                    return;
                }

                // Display the delivery options
                Console.WriteLine("\nSelect Delivery Method:");
                Console.WriteLine("1. Standard Delivery (50:-)");
                Console.WriteLine("2. Express Delivery (100:-)");
                Console.WriteLine("3. Pickup (Free)");

                // Get the delivery choice from the user
                int choice = Helpers.GetIntInput("Enter your choice: ");
                float deliveryCost = choice switch
                {
                    1 => 50,
                    2 => 100,
                    3 => 0,
                    _ => 0
                };

                // Add the delivery cost to the total
                totalSum += deliveryCost;
                Console.WriteLine($"Total cost including delivery: {totalSum}:-");

                // Make sure the cart contains unpaid items before proceeding
                var unpaidItems = cart.CartProduct.Where(cp => !cp.Paid).ToList();
                if (!unpaidItems.Any())
                {
                    Console.WriteLine("Your cart has no unpaid items.");
                    return;
                }

                Console.WriteLine("\nSelect Payment Method:");
                Console.WriteLine("1. Invoice");
                Console.WriteLine("2. Direct Card Payment");

                int paymentChoice = Helpers.GetIntInput("Enter your choice: ");
                string paymentMethod = paymentChoice switch
                {
                    1 => "Invoice",
                    2 => "Direct Card Payment",
                    _ => "Unknown"
                };

                if (paymentMethod == "Unknown")
                {
                    Console.WriteLine("Invalid payment method selected.");
                    return;
                }

                // Proceed with marking items as paid
                foreach (var cartProduct in unpaidItems)
                {
                    cartProduct.Paid = true; // Mark as paid
                }

                db.SaveChanges();
                Console.WriteLine("Checkout complete! Thank you for your purchase!");
                Console.WriteLine($"Payment Method: {paymentMethod}");
                Console.WriteLine("\nYour final cart details:");
                foreach (var cartProduct in unpaidItems)
                {
                    var product = db.Product.SingleOrDefault(p => p.Id == cartProduct.ProductId);
                    if (product != null)
                    {
                        Console.WriteLine($"{product.Name} | Quantity: {cartProduct.Quantity} | Price: {product.Price * cartProduct.Quantity}:-");
                    }
                }
                Console.WriteLine($"Total cost: {totalSum}:-");
                Console.ReadKey();
            }
        }
    }
}
