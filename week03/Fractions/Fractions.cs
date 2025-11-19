using System;

public class Fraction
{
    // Private attributes (encapsulation)
    private int numerator;
    private int denominator;

    // Constructors
    public Fraction() // No-parameter constructor → 1/1
    {
        numerator = 1;
        denominator = 1;
    }

    public Fraction(int top) // One-parameter constructor → top/1
    {
        numerator = top;
        denominator = 1;
    }

    public Fraction(int top, int bottom) // Two-parameter constructor → top/bottom
    {
        numerator = top;
        denominator = bottom != 0 ? bottom : 1; // prevent divide by zero
    }

    // Getters and Setters
    public int GetNumerator()
    {
        return numerator;
    }

    public void SetNumerator(int value)
    {
        numerator = value;
    }

    public int GetDenominator()
    {
        return denominator;
    }

    public void SetDenominator(int value)
    {
        if (value != 0) // prevent divide by zero
        {
            denominator = value;
        }
        else
        {
            Console.WriteLine("Denominator cannot be zero. Keeping previous value.");
        }
    }

    // Methods to return representations
    public string GetFractionString()
    {
        return $"{numerator}/{denominator}";
    }

    public double GetDecimalValue()
    {
        return (double)numerator / denominator;
    }
}
