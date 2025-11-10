using System;

class Program
{
    static void Main()
    {
        var p = new Person();
        p._givenName = "John";
        p._familyName = "Doe";
        p.ShowWesternName();
        p.ShowEasternName();
    }
}
