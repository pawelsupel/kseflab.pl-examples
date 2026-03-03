// See https://aka.ms/new-console-template for more information

using PikSystems.Ksef.Client;
using PikSystems.Ksef.Client.Contracts;

var client = KsefApiClient.Create(
    "https://piksystems-dev-api-kseflab.azurewebsites.net",
    "sk_test_78a14c91a9348b2912bb8f612cebb3d54eeb3225aaf8853a"); //kept only for dev testing :)

// await client.StoreKsefCredentialsAsync(new KsefCredentialsRequest()
// {
//     Type = "Token",
//     Value = "YOUR_KSEF_TOKEN"
// });

var invoice = new InvoiceRequest
{
    Profile = InvoiceProfile.BasicVat,
    IssueDate = DateTime.UtcNow.Date,
    Currency = "PLN",
    InvoiceNumber = $"FV/{DateTime.UtcNow:yyyy/MM/dd}/2",
    Buyer = new PartyRequest
    {
        Name = "Acme Sp. z o.o.",
        Nip = "7740001454",
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
    },
};


var created = await client.SendInvoiceAsync(invoice);
Console.WriteLine($"Invoice created: {created.InvoiceId}");

var currentMonth = await client.GetInvoicesForCurrentMonthAsync();
Console.WriteLine(currentMonth.Invoices.Count);

var previousMonth = await client.GetInvoicesForPreviousMonthAsync();
Console.WriteLine(currentMonth.Invoices.Count);

// status by invoice id
var status = await client.GetInvoiceAsync(created.InvoiceId);
Console.WriteLine(status.Status);
Console.ReadKey();