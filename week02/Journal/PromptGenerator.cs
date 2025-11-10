using System;
using System.Collections.Generic;

namespace JournalApp
{
    public class PromptGenerator
    {
        private readonly List<string> _prompts;
        private readonly Random _rand;

        public PromptGenerator()
        {
            _rand = new Random();
            _prompts = new List<string>
            {
                "Who was the most interesting person I interacted with today?",
                "What was the best part of my day?",
                "How did I see the hand of the Lord in my life today?",
                "What was the strongest emotion I felt today?",
                "If I had one thing I could do over today, what would it be?"
            };
        }

        public string GetRandomPrompt()
        {
            int index = _rand.Next(_prompts.Count);
            return _prompts[index];
        }

        public void AddPrompt(string prompt)
        {
            if (!string.IsNullOrWhiteSpace(prompt))
            {
                _prompts.Add(prompt);
            }
        }
    }
}
