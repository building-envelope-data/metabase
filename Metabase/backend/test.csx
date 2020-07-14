#nullable enable

using System;

public enum E
{
    X,
    Y
}

public class T
{
    public string S { get; set; }
    public bool B { get; set; }
    public char C { get; set; }
    public float F { get; set; }
    public E E { get; set; }

#nullable disable
    public T() { }
#nullable enable
}

private var t = new T();

Console.WriteLine(t.S == null);
Console.WriteLine(t.B);
Console.WriteLine(t.C);
Console.WriteLine(t.F);
Console.WriteLine(t.E);

Console.WriteLine(default(string) == null);
Console.WriteLine(default(bool));
Console.WriteLine(default(char));
Console.WriteLine(default(float));
Console.WriteLine(default(E));
Console.WriteLine(default(DateTime));
