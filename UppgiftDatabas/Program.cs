namespace UppgiftDatabas
{
    internal class Program
    {
        public static int LoggedInCustomerId;
        public static bool IsAdmin;
        static async Task Main(string[] args)
        {
            LoggedInCustomerId = Menu.SwitchCustomer();

            IsAdmin = await Menu.IsUserAdminAsync(LoggedInCustomerId);

            bool runProgram = true;
            while (runProgram == true)
            {
                Menu.MainMenu();
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case '1':
                        Menu.Category1(LoggedInCustomerId);
                        Console.ReadKey();
                        break;
                    case '2':
                        Menu.Category2(LoggedInCustomerId);
                        Console.ReadKey();
                        break;
                    case '3':
                        Menu.Category3(LoggedInCustomerId);
                        Console.ReadKey();
                        break;
                    case 'a': // Admin menu with check
                        if (IsAdmin)
                        {
                            Menu.AdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("You do not have permission to access the admin menu.");
                            Console.ReadKey();
                        }
                        break;

                    case 's': // Search
                        await Menu.SearchProductAsync();
                        break;
                    case 'c': // View cart
                        ShopCart.ViewCart(LoggedInCustomerId);
                        break;
                    case 'h': // View order history
                        await Menu.ViewPastPurchasesAsync(LoggedInCustomerId);
                        break;
                    case 't': // Switch customer
                        Menu.SwitchCustomer();
                        break;
                    case 'y': // New customer
                        ShopAdd.NewCustomer();
                        break;
                    case 'q': // Exit the program
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
