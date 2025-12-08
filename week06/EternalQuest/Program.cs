using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    private string _name;
    private string _description;
    private int _points;
    private bool _isComplete;

    protected Goal(string name, string description, int points)
    {
        _name = name;
        _description = description;
        _points = points;
        _isComplete = false;
    }

    public string Name => _name;
    public string Description => _description;
    public int Points => _points;
    public bool IsComplete => _isComplete;

    protected void MarkComplete() => _isComplete = true;

    // Polymorphic recording: returns points awarded for this event
    public abstract int RecordEvent();

    // Status text shown in list
    public abstract string GetStatus();

    // Serialize type and fields to a single line
    public abstract string Serialize();

    // Factory-like deserialization
    public static Goal Deserialize(string line)
    {
        // Format examples:
        // Simple|Name|Desc|Points|IsComplete
        // Eternal|Name|Desc|Points
        // Checklist|Name|Desc|Points|Target|Count|Bonus
        var parts = line.Split('|');
        var type = parts[0];

        if (type == "Simple")
        {
            string name = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            bool complete = bool.Parse(parts[4]);
            var g = new SimpleGoal(name, desc, points);
            if (complete) g.ForceComplete();
            return g;
        }
        else if (type == "Eternal")
        {
            string name = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            return new EternalGoal(name, desc, points);
        }
        else if (type == "Checklist")
        {
            string name = parts[1];
            string desc = parts[2];
            int points = int.Parse(parts[3]);
            int target = int.Parse(parts[4]);
            int count = int.Parse(parts[5]);
            int bonus = int.Parse(parts[6]);
            return new ChecklistGoal(name, desc, points, target, bonus, count);
        }
        else
        {
            throw new InvalidOperationException($"Unknown goal type: {type}");
        }
    }
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override int RecordEvent()
    {
        if (IsComplete)
        {
            // Already complete; no additional points
            return 0;
        }
        MarkComplete();
        return Points;
    }

    public override string GetStatus()
    {
        string box = IsComplete ? "[X]" : "[ ]";
        return $"{box} {Name} ({Description})";
    }

    public override string Serialize()
    {
        return $"Simple|{Name}|{Description}|{Points}|{IsComplete}";
    }

    // Helper for deserialization
    public void ForceComplete() => MarkComplete();
}

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override int RecordEvent()
    {
        // Never complete, always award base points
        return Points;
    }

    public override string GetStatus()
    {
        return $"[∞] {Name} ({Description})";
    }

    public override string Serialize()
    {
        return $"Eternal|{Name}|{Description}|{Points}";
    }
}

class ChecklistGoal : Goal
{
    private int _targetCount;
    private int _currentCount;
    private int _bonusPoints;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints, int currentCount = 0)
        : base(name, description, points)
    {
        _targetCount = Math.Max(1, targetCount);
        _currentCount = Math.Max(0, currentCount);
        _bonusPoints = Math.Max(0, bonusPoints);
        if (_currentCount >= _targetCount) MarkComplete();
    }

    public override int RecordEvent()
    {
        if (IsComplete)
        {
            // Already complete; no further points
            return 0;
        }

        _currentCount++;

        int awarded = Points;
        if (_currentCount >= _targetCount)
        {
            // Award bonus and mark complete
            awarded += _bonusPoints;
            MarkComplete();
        }
        return awarded;
    }

    public override string GetStatus()
    {
        string box = IsComplete ? "[X]" : "[ ]";
        return $"{box} {Name} ({Description}) — Completed {_currentCount}/{_targetCount}";
    }

    public override string Serialize()
    {
        return $"Checklist|{Name}|{Description}|{Points}|{_targetCount}|{_currentCount}|{_bonusPoints}";
    }
}

class Program
{
    // Score and goals encapsulated within Program
    private static int _score = 0;
    private static readonly List<Goal> _goals = new();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Eternal Quest");
            Console.WriteLine("-------------");
            Console.WriteLine($"Score: {_score}");
            Console.WriteLine();
            Console.WriteLine("1) Create New Goal");
            Console.WriteLine("2) List Goals");
            Console.WriteLine("3) Record Event");
            Console.WriteLine("4) Save Goals");
            Console.WriteLine("5) Load Goals");
            Console.WriteLine("6) Exit");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine() ?? "";
            switch (choice)
            {
                case "1": CreateGoalMenu(); break;
                case "2": ListGoals(); Pause(); break;
                case "3": RecordEventMenu(); break;
                case "4": SaveGoals(); Pause(); break;
                case "5": LoadGoals(); Pause(); break;
                case "6": return;
                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    private static void CreateGoalMenu()
    {
        Console.Clear();
        Console.WriteLine("Create New Goal");
        Console.WriteLine("---------------");
        Console.WriteLine("a) Simple Goal (one-time)");
        Console.WriteLine("b) Eternal Goal (repeating)");
        Console.WriteLine("c) Checklist Goal (target count + bonus)");
        Console.Write("\nChoose type: ");
        string type = (Console.ReadLine() ?? "").Trim().ToLower();

        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "Untitled";

        Console.Write("Description: ");
        string desc = Console.ReadLine() ?? "";

        Console.Write("Base points per completion: ");
        int points = ReadInt(min: 0);

        switch (type)
        {
            case "a":
            case "simple":
                _goals.Add(new SimpleGoal(name, desc, points));
                break;

            case "b":
            case "eternal":
                _goals.Add(new EternalGoal(name, desc, points));
                break;

            case "c":
            case "checklist":
                Console.Write("Target count to complete: ");
                int target = ReadInt(min: 1);

                Console.Write("Bonus points on completion: ");
                int bonus = ReadInt(min: 0);

                _goals.Add(new ChecklistGoal(name, desc, points, target, bonus));
                break;

            default:
                Console.WriteLine("Unknown type.");
                break;
        }

        Console.WriteLine("Goal created.");
        Pause();
    }

    private static void ListGoals()
    {
        Console.Clear();
        Console.WriteLine("Goals");
        Console.WriteLine("-----");
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals yet.");
            return;
        }

        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetStatus()}");
        }
    }

    private static void RecordEventMenu()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("\nNo goals to record.");
            Pause();
            return;
        }

        ListGoals();
        Console.Write("\nChoose a goal number to record: ");
        int index = ReadInt(min: 1, max: _goals.Count) - 1;

        var goal = _goals[index];
        int awarded = goal.RecordEvent();
        _score += awarded;

        Console.WriteLine($"\nRecorded: {goal.Name}");
        Console.WriteLine($"Points awarded: {awarded}");
        Console.WriteLine($"New score: {_score}");
        Pause();
    }

    private static void SaveGoals()
    {
        Console.Write("\nEnter filename to save (e.g., goals.txt): ");
        string path = Console.ReadLine() ?? "goals.txt";

        try
        {
            using var sw = new StreamWriter(path);
            sw.WriteLine(_score);
            sw.WriteLine(_goals.Count);
            foreach (var g in _goals)
            {
                sw.WriteLine(g.Serialize());
            }
            Console.WriteLine($"Saved to {Path.GetFullPath(path)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving: {ex.Message}");
        }
    }

    private static void LoadGoals()
    {
        Console.Write("\nEnter filename to load (e.g., goals.txt): ");
        string path = Console.ReadLine() ?? "goals.txt";

        if (!File.Exists(path))
        {
            Console.WriteLine("File not found.");
            return;
        }

        try
        {
            using var sr = new StreamReader(path);
            _score = int.Parse(sr.ReadLine() ?? "0");
            int count = int.Parse(sr.ReadLine() ?? "0");

            _goals.Clear();
            for (int i = 0; i < count; i++)
            {
                string line = sr.ReadLine() ?? "";
                _goals.Add(Goal.Deserialize(line));
            }
            Console.WriteLine("Loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading: {ex.Message}");
        }
    }

    private static int ReadInt(int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;
            Console.Write($"Enter a valid number ({min}–{max}): ");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
