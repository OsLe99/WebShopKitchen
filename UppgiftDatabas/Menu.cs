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

                List<string> chosen1List = new List<string>();
                List<string> chosen2List = new List<string>();
                List<string> chosen3List = new List<string>();

                var categories = new List<int> { 1, 2, 3 };

                foreach (var categoryId in categories)
                {
                    var chosenList = db.Product.Where(x => x.Chosen == true && x.CategoryId == categoryId)
                        .Select(x => $"{x.Name} | {x.Price} | {x.Description}").ToList();

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
                welcomeWindow.Draw();
                categoryWindow.Draw();
                chosen1Window.Draw(); 
                chosen2Window.Draw();
                chosen3Window.Draw();
            }
        }

        //public static void Category1()
        //{
        //    using (var db = new myDbContext())
        //    {
        //        foreach ()
        //    }
        //}
    }
}
