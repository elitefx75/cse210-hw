using System;
using System.Collections.Generic;
using System.Linq;

public class Scripture
{
    private Reference reference;
    private List<Word> words;
    private Random rng = new Random();

    public Scripture(Reference reference, string text)
    {
        this.reference = reference;
        this.words = text.Split(' ').Select(w => new Word(w)).ToList();
    }

    public string GetDisplayText()
    {
        string refText = reference.ToString();
        string wordsText = string.Join(" ", words.Select(w => w.GetDisplayText()));
        return $"{refText} {wordsText}";
    }

    public void HideRandomWords(int count)
    {
        var available = words
            .Select((word, idx) => new { word, idx })
            .Where(x => !x.word.IsHidden())
            .ToList();

        if (available.Count == 0)
            return;

        for (int i = available.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            var tmp = available[i];
            available[i] = available[j];
            available[j] = tmp;
        }

        int toHide = Math.Min(count, available.Count);
        for (int i = 0; i < toHide; i++)
        {
            words[available[i].idx].Hide();
        }
    }

    public bool AllHidden()
    {
        return words.All(w => w.IsHidden());
    }
}
