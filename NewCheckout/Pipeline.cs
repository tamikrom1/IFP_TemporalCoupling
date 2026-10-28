using System;

public static class Pipeline
{
    public static Validated Validate(Draft d)
    {
        if (d.Amount <= 0) throw new ArgumentException("Amount must be positive.", nameof(d));
        return new Validated(d.Amount);
    }

    public static Priced ApplyDiscount(Validated v, decimal percent) =>
        new Priced(v.Amount - v.Amount * percent);

    public static Receipt Charge(Priced p, Func<decimal, string> gateway)
    {
        string txId = gateway(p.Amount);
        return new Receipt(txId, p.Amount);
    }
}
