using System;

public class Checkout
{
    private decimal _total;
    private bool _validated;
    public void Start(decimal amount) { _total = amount; }
    public void Validate() { _validated = _total > 0; }
    public void ApplyDiscount(decimal percent) { _total -= _total * percent; }
    public void Charge()
    {
        if (!_validated) throw new InvalidOperationException("Not validated.");
        Console.WriteLine($"Charged {_total:F2}");
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Correct order");
        var c = new Checkout();
        c.Start(1000m); c.Validate(); c.ApplyDiscount(0.10m); c.Charge();

        Console.WriteLine("1. Discount applied twice");
        c = new Checkout();
        c.Start(1000m); c.Validate(); c.ApplyDiscount(0.10m); c.ApplyDiscount(0.10m); c.Charge();

        Console.WriteLine("2. Discount forgotten");
        c = new Checkout();
        c.Start(1000m); c.Validate(); c.Charge();

        Console.WriteLine("3. Charge called twice");
        c = new Checkout();
        c.Start(1000m); c.Validate(); c.ApplyDiscount(0.10m); c.Charge(); c.Charge();

        Console.WriteLine("4. Charge before Validate");
        try
        {
            c = new Checkout();
            c.Start(1000m); c.ApplyDiscount(0.10m); c.Charge();
        }
        catch (InvalidOperationException ex) { Console.WriteLine("Exception: " + ex.Message); }

        Console.WriteLine("5. Bonus: discount after Validate on a re-Start (stale flag)");
        c = new Checkout();
        c.Start(1000m); c.Validate(); c.Start(-50m); c.Charge();
    }
}
