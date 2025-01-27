using System;
using System.Collections.Generic;
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
                List<string> welcomeText = new List<string>
                {
                    "Welcome to Kitchen Store!"
                };
                var welcomeWindow = new Window("", 0, 0, welcomeText);
               
                // Display category window
                var categoryList = db.Category.Select(x => $"ID: {x.Id} | {x.Name}").ToList();
                var categoryWindow = new Window("Category", 0, (welcomeText.Count + 2), categoryList);

                List<string> chosen1List = new List<string>
                {
                    ""
                };
                if (db.Product.Where(x => x.CategoryId == 1).Any(x => x.Chosen == true))
                {
                    chosen1List = db.Product.Where(x => x.Chosen == true && x.CategoryId == 1).Select(x => $"{x.Name} | {x.Price}:- | {x.Description}").ToList();

                }
                else
                {
                    chosen1List = new List<string>
                    {
                        "Currently no discounts."
                    };
                }
                var chosen1Window = new Window("Cookware Discounts", 30, 0, chosen1List);

                welcomeWindow.Draw();
                categoryWindow.Draw();
                chosen1Window.Draw();
            }
        }
    }
}
