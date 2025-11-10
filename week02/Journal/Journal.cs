using System;
using System.Collections.Generic;
using System.IO;

namespace JournalApp
{
    public class Journal
    {
        private List<Entry> _entries = new List<Entry>();
        private PromptGenerator _promptGenerator = new PromptGenerator();

        public void AddEntry()
        {
            string prompt = _promptGenerator.GetRandomPrompt();
            Console.WriteLine(prompt);
            Console.Write("> ");
            string response = Console.ReadLine();

            string date = DateTime.Now.ToString("yyyy-MM-dd");

            Entry entry = new Entry(prompt, response, date);
            _entries.Add(entry);
        }

        public void DisplayEntries()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("No entries yet.");
                return;
            }

            foreach (var entry in _entries)
            {
                Console.WriteLine(entry.ToString());
                Console.WriteLine();
            }
        }

        public void Save(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (var entry in _entries)
                    {
                        writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
                    }
                }
                Console.WriteLine($"Journal saved to {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving: {ex.Message}");
            }
        }

        public void Load(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            try
            {
                List<Entry> loaded = new List<Entry>();
                string[] lines = File.ReadAllLines(filename);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        string date = parts[0];
                        string prompt = parts[1];
                        string response = parts[2];
                        for (int i = 3; i < parts.Length; i++)
                        {
                            response += "|" + parts[i];
                        }

                        Entry entry = new Entry(prompt, response, date);
                        loaded.Add(entry);
                    }
                }
                _entries = loaded;
                Console.WriteLine($"Journal loaded from {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading: {ex.Message}");
            }
        }
    }
}
