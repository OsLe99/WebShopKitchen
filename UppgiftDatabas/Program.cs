namespace UppgiftDatabas
{
    internal class Program
    {
        static int LoggedInCustomerId;
        static async Task Main(string[] args)
        {
            LoggedInCustomerId = Menu.SwitchCustomer();

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
                    case 'a': // Admin
                        Menu.AdminMenu();
                        break;
                    case 's': // Search
                        await Menu.SearchProductAsync();
                        break;
                    case 'c':
                        ShopCart.ViewCart(LoggedInCustomerId);
                        break;
                    case 't': // Switch customer
                        Menu.SwitchCustomer();
                        break;
                    case 'y': // New customer
                        ShopAdd.NewCustomer();
                        break;
                    case 'q':
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
