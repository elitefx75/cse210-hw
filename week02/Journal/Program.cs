using System;

namespace JournalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Journal theJournal = new Journal();

            string choice = "";
            while (choice != "5")
            {
                Console.WriteLine("\nJournal Menu:");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. Display the journal");
                Console.WriteLine("3. Save the journal to a file");
                Console.WriteLine("4. Load the journal from a file");
                Console.WriteLine("5. Quit");
                Console.Write("Choose an option: ");
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Journal handles prompt + entry creation internally
                    theJournal.AddEntry();
                }
                else if (choice == "2")
                {
                    theJournal.DisplayEntries();
                }
                else if (choice == "3")
                {
                    Console.Write("Enter filename: ");
                    string filename = Console.ReadLine();
                    theJournal.Save(filename);
                }
                else if (choice == "4")
                {
                    Console.Write("Enter filename: ");
                    string filename = Console.ReadLine();
                    theJournal.Load(filename);
                }
                else if (choice == "5")
                {
                    Console.WriteLine("Goodbye! Thanks for journaling today.");
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select 1–5.");
                }
            }
        }
    }
}
