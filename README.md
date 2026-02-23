# PikSystems.Ksef.Client

## Install (NuGet)

```bash
dotnet add package PikSystems.Ksef.Client
```

## Usage

```csharp
using PikSystems.Ksef.Client;
using PikSystems.Ksef.Client.Contracts;

var client = KsefApiClient.Create(
    "https://piksystems-dev-api-kseflab.azurewebsites.net",
    "YOUR_API_KEY");

var invoice = new InvoiceRequest
{
    Profile = InvoiceProfile.BasicVat,
    IssueDate = DateTime.UtcNow.Date,
    Currency = "PLN",
    Buyer = new PartyRequest
    {
        Name = "Acme Sp. z o.o.",
        Nip = "1234567890",
        Address = "Warszawa"
    },
    Items = new List<InvoiceItemRequest>
    {
        new InvoiceItemRequest
        {
            Name = "Usługa",
            Quantity = 1,
            NetPrice = 100,
            VatRate = 23,
            Unit = "szt"
        }
    }
};

var created = await client.SendInvoiceAsync(invoice);
var currentMonth = await client.GetInvoicesForCurrentMonthAsync();
var previousMonth = await client.GetInvoicesForPreviousMonthAsync();

// status by invoice id
var status = await client.GetInvoiceAsync(created.InvoiceId);
```

## Invoice Status After Import

You can import invoice metadata and then fetch full status from the API.

```csharp
var imported = await client.GetInvoicesForCurrentMonthAsync(pageSize: 50);
if (imported.Invoices.Count > 0)
{
    // If you store invoiceId in your system, call by id
    var status = await client.GetInvoiceAsync(created.InvoiceId);
}
```

## Previous Month Import

```csharp
var previous = await client.GetInvoicesForPreviousMonthAsync(pageOffset: 0, pageSize: 50);
```

## Get By KSeF Number

```csharp
var statusByNumber = await client.GetInvoiceByKsefNumberAsync("KSEF_NUMBER");
```

## Pagination

`GetInvoicesForCurrentMonthAsync` and `GetInvoicesForPreviousMonthAsync` accept:

- `pageOffset`
- `pageSize`
- `subjectType` (passed as query parameter)

## Errors

Non-2xx responses throw `HttpRequestException` with the response payload.

## Notes

- The client signs requests using `Authorization` + `X-KSeF-*` headers.
- `GetInvoicesForCurrentMonthAsync` calls `/api/invoices/import`.
- `GetInvoicesForPreviousMonthAsync` calls `/api/invoices/import?period=last`.
