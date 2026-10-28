using System;

public static class Program
{
    public static void Main()
    {
        var receipt = Pipeline.Charge(
            Pipeline.ApplyDiscount(Pipeline.Validate(new Draft(1000m)), 0.10m),
            Gateways.Real);
        Console.WriteLine(receipt);

        var fake = new FakeGateway();
        var priced = Pipeline.ApplyDiscount(Pipeline.Validate(new Draft(1000m)), 0.10m);
        var fakeReceipt = Pipeline.Charge(priced, fake.Call);
        Console.WriteLine(fakeReceipt);
        Console.WriteLine($"Fake gateway: calls = {fake.Calls}, last amount = {fake.LastAmount:F2}");

        try { Pipeline.Validate(new Draft(0m)); }
        catch (ArgumentException ex) { Console.WriteLine("Validate(0) -> " + ex.Message.Split('\n')[0]); }

        // Task 4: wrong sequences from Task 1

        //Discount applied twice:
        //   var p2 = Pipeline.ApplyDiscount(Pipeline.ApplyDiscount(Pipeline.Validate(new Draft(1000m)), 0.1m), 0.1m);
        //   -> compile error CS1503: cannot convert from 'Priced' to 'Validated'. REJECTED.

        //Discount forgotten:
        //   Pipeline.Charge(Pipeline.Validate(new Draft(1000m)), fake.Call);
        //   -> compile error CS1503: cannot convert from 'Validated' to 'Priced'. REJECTED.

        //Charge before Validate:
        //   Pipeline.Charge(new Draft(1000m), fake.Call);
        //   -> compile error CS1503: cannot convert from 'Draft' to 'Priced'. REJECTED.

        // Charge called twice:
        //   Pipeline.Charge(priced, fake.Call); Pipeline.Charge(priced, fake.Call);
        //   -> COMPILES. NOT rejected: Priced is an ordinary value and can be reused.

        Pipeline.Charge(priced, fake.Call);
        Pipeline.Charge(priced, fake.Call);
        Console.WriteLine($"Charging the same Priced twice more: fake.Calls = {fake.Calls} (double charge possible)");

        var sneaky = new Priced(-5m);
        Console.WriteLine(Pipeline.Charge(sneaky, fake.Call));
    }
}
