# Handling Temporal Coupling

- `OldCheckout/` — Task 1: original class + wrong call sequences (`dotnet run --project OldCheckout`)
- `NewCheckout/` — Tasks 2–4: staged immutable records, `Func<decimal,string>` gateway, real + fake gateway, wrong sequences as comments in `Program.cs` (`dotnet run --project NewCheckout`)

## Task 1 results
| Sequence | Result |
|---|---|
| Discount twice | charges 810.00 — silent |
| Discount forgotten | charges 1000.00 — silent |
| Charge twice | money leaves twice — silent |
| Charge before Validate | InvalidOperationException — the only one noticed, and only at run time |

## Task 4 results
| Sequence | Compiler |
|---|---|
| ApplyDiscount twice | CS1503: cannot convert 'Priced' to 'Validated' |
| Charge(Validated) (no discount) | CS1503: cannot convert 'Validated' to 'Priced' |
| Charge(Draft) (before Validate) | CS1503: cannot convert 'Draft' to 'Priced' |
| Charge twice on same Priced | compiles — still possible |

## Questions
1. **Temporal coupling** is a hidden dependency on the order of calls. The runtime check in Charge catches only one wrong order, only at run time, and relies on mutable flags that can go stale.
2. Each function accepts only the previous stage type, so the signature is the order. Not prevented: Charge can be called many times on the same `Priced`, and anyone can create `new Priced(x)` with a public constructor, skipping earlier steps.
3. The gateway is a parameter, so tests pass a fake that counts calls — no bank, no mocking framework. Pure parts: Validate, ApplyDiscount and the records. Only the gateway has side effects.
4. Each stage is a different fact, so a new immutable value fits; mutation would bring back hidden state. Cost: more types and allocations. Benefit: compiler-checked order, no stale state, thread-safe sharing, easy testing. Downside: old values can be reused (double Charge).
