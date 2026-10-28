using System;

public static class Gateways
{
    public static string Real(decimal amount)
    {
        string id = "TX-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
        Console.WriteLine($"[BANK] Charged {amount:F2}, transaction {id}");
        return id;
    }
}

public class FakeGateway
{
    public int Calls { get; private set; }
    public decimal LastAmount { get; private set; }

    public string Call(decimal amount)
    {
        Calls++;
        LastAmount = amount;
        return "FAKE-" + Calls;
    }
}
