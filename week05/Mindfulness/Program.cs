using System;
using System.Collections.Generic;
using System.Threading;

abstract class Activity
{
    private string name;
    private string description;
    private int durationSeconds;

    protected Activity(string name, string description)
    {
        this.name = name;
        this.description = description;
        this.durationSeconds = 0;
    }

    public void SetDuration(int seconds) => durationSeconds = Math.Max(0, seconds);
    protected int Duration() => durationSeconds;

    public void StartMessage()
    {
        Console.Clear();
        Console.WriteLine($"{name}\n");
        Console.WriteLine(description);
        Console.Write("\nEnter duration (seconds): ");
        if (int.TryParse(Console.ReadLine(), out int seconds)) SetDuration(seconds);
        Console.WriteLine("\nGet ready to begin...");
        Spinner();
    }

    public void EndMessage()
    {
        Console.WriteLine("\nWell done.");
        Spinner();
        Console.WriteLine($"\nYou have completed the {name} for {durationSeconds} seconds.");
        Spinner();
    }

    protected void Spinner(int milliseconds = 200, int cycles = 12)
    {
        char[] frames = { '|', '/', '-', '\\' };
        for (int i = 0; i < cycles; i++)
        {
            Console.Write(frames[i % frames.Length]);
            Thread.Sleep(milliseconds);
            Console.Write('\r');
        }
        Console.Write(' '); Console.Write('\r');
    }

    protected void Countdown(int seconds)
    {
        for (int s = seconds; s >= 1; s--)
        {
            Console.Write($" {s} ");
            Thread.Sleep(1000);
            Console.Write('\r');
        }
        Console.Write("    \r");
    }

    public abstract void Run();
}

class BreathingActivity : Activity
{
    public BreathingActivity() : base(
        "Breathing Activity",
        "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing."
    ) { }

    public override void Run()
    {
        StartMessage();
        int elapsed = 0;
        while (elapsed < Duration())
        {
            Console.Write("Breathe in...");
            Countdown(4);
            elapsed += 4;
            if (elapsed >= Duration()) break;

            Console.Write("Breathe out...");
            Countdown(4);
            elapsed += 4;
        }
        EndMessage();
    }
}

class ReflectionActivity : Activity
{
    private readonly List<string> prompts = new()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };
    private readonly List<string> questions = new()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times?",
        "What is your favorite thing about this experience?",
        "What could you learn from this for other situations?",
        "What did you learn about yourself?",
        "How can you keep this experience in mind in the future?"
    };
    private readonly Random random = new();

    public ReflectionActivity() : base(
        "Reflection Activity",
        "This activity will help you reflect on times in your life when you have shown strength and resilience."
    ) { }

    public override void Run()
    {
        StartMessage();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine($"\nPrompt: {prompt}\n");
        Spinner();

        int elapsed = 0;
        while (elapsed < Duration())
        {
            string question = questions[random.Next(questions.Count)];
            Console.WriteLine($"\nConsider: {question}");
            Spinner(300, 10);
            elapsed += 3; // approximate time spent per question pause
        }
        EndMessage();
    }
}

class ListingActivity : Activity
{
    private readonly List<string> prompts = new()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };
    private readonly List<string> items = new();
    private readonly Random random = new();

    public ListingActivity() : base(
        "Listing Activity",
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."
    ) { }

    public override void Run()
    {
        StartMessage();
        string prompt = prompts[random.Next(prompts.Count)];
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.WriteLine("Prepare to begin listing...");
        Countdown(5);

        DateTime end = DateTime.Now.AddSeconds(Duration());
        items.Clear();
        while (DateTime.Now < end)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) items.Add(input.Trim());
        }

        Console.WriteLine($"\nYou listed {items.Count} item(s).");
        EndMessage();
    }
}

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1) Breathing Activity");
            Console.WriteLine("2) Reflection Activity");
            Console.WriteLine("3) Listing Activity");
            Console.WriteLine("4) Exit");
            Console.Write("\nChoose an option: ");

            string? choice = Console.ReadLine();
            Activity? activity = choice switch
            {
                "1" => new BreathingActivity(),
                "2" => new ReflectionActivity(),
                "3" => new ListingActivity(),
                "4" => null,
                _ => null
            };
            if (activity == null)
            {
                if (choice == "4") break;
                Console.WriteLine("Invalid option. Press Enter to continue.");
                Console.ReadLine();
                continue;
            }

            activity.Run();
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }
    }
}
