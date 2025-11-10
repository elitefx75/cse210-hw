using System;

class Journal
{
    public void AddEntry(string text)
    {
        Console.WriteLine($"Entry added: {text}");
    }
}

class Entry
{
    public string Content { get; set; } = "Default entry text";

    public void Display()
    {
        Console.WriteLine($"Entry: {Content}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal theJournal = new Journal();
        Entry anEntry = new Entry();
        anEntry.Display();

        Console.WriteLine("Hello World! This is the Journal Project.");
    }
}
