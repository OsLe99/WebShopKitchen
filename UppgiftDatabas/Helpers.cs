using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UppgiftDatabas
{
    public static class Helpers
    {
        public static int GetIntInput(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    return int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Input value is too large. Please enter a smaller number.");
                }
            }
        }

        public static float GetFloatInput(string prompt)
        {
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    return float.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Input value is too large. Please enter a smaller number.");
                }
            }
        }

        public static bool GetYesNoInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (choice == 'y' || choice == 'Y')
                    return true;
                if (choice == 'n' || choice == 'N')
                    return false;

                Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            }
        }
    }
}
