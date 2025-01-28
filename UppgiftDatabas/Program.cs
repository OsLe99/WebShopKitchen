namespace UppgiftDatabas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runProgram = true;
            while (runProgram == true)
            {
                Menu.MainMenu();
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case '1':
                        Menu.Category1();
                        Console.ReadLine();
                        break;
                    case '2':
                        Menu.Category2();
                        Console.ReadLine();
                        break;
                    case '3':
                        Menu.Category3();
                        Console.ReadLine();
                        break;
                    case 'a':
                        Menu.AdminMenu();
                        break;
                }
            }
        }
    }
}
