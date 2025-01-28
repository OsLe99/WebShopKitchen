namespace UppgiftDatabas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runProgram = true;
            while (runProgram == true)
            {
                Console.Clear();
                Menu.MainMenu();
                ConsoleKeyInfo input = Console.ReadKey();
                switch (input.KeyChar)
                {
                    case '1':
                        Console.Write("1");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
