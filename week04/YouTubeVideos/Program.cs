using System;
using System.Collections.Generic;

public class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }
    private List<Comment> Comments { get; set; }

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
        Comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return Comments.Count;
    }

    public List<Comment> GetComments()
    {
        return Comments;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create videos
        var video1 = new Video("Intro to Abstraction", "Alice", 600);
        var video2 = new Video("C# Classes Explained", "Bob", 900);
        var video3 = new Video("Object-Oriented Programming Basics", "Charlie", 1200);

        // Add comments
        video1.AddComment(new Comment("John", "Great explanation!"));
        video1.AddComment(new Comment("Sarah", "Very helpful, thanks."));
        video1.AddComment(new Comment("Mike", "Can you cover inheritance next?"));

        video2.AddComment(new Comment("Anna", "Clear and concise."));
        video2.AddComment(new Comment("Tom", "Loved the examples."));
        video2.AddComment(new Comment("Lucy", "This made OOP easier to understand."));

        video3.AddComment(new Comment("David", "Perfect for beginners."));
        video3.AddComment(new Comment("Emma", "I finally get abstraction!"));
        video3.AddComment(new Comment("Chris", "Looking forward to more content."));

        // Store videos in a list
        var videos = new List<Video> { video1, video2, video3 };

        // Display details
        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthSeconds} seconds");
            Console.WriteLine($"Number of Comments: {video.GetCommentCount()}");

            foreach (var comment in video.GetComments())
            {
                Console.WriteLine($" - {comment.CommenterName}: {comment.Text}");
            }

            Console.WriteLine(new string('-', 40));
        }
    }
}
