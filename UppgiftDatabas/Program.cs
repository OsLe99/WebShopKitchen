namespace UppgiftDatabas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runProgram = true;
            while (runProgram == true)
            {
                ShopAlter.ChangeProduct();
                ShopAlter.ChangeProduct();
                Menu.MainMenu();
                Console.ReadKey();
            }
        }
    }
}
