// See https://aka.ms/new-console-template for more information

using PikSystems.Ksef.Client;
using PikSystems.Ksef.Client.Contracts;

var client = KsefApiClient.Create(
    "https://piksystems-dev-api-kseflab.azurewebsites.net",
    "sk_test_cb941cd6fb126a7f33972a703f8c50f34bc74adbc10af170"); //deactivated ;) kept only for example :)

var invoice = new InvoiceRequest
{
    Profile = InvoiceProfile.BasicVat,
    IssueDate = DateTime.UtcNow.Date,
    Currency = "PLN",
    Buyer = new PartyRequest
    {
        Name = "Acme Sp. z o.o.",
        Nip = "3558828768",
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
Console.WriteLine($"Invoice created: {created.InvoiceId}");
var currentMonth = await client.GetInvoicesForCurrentMonthAsync();
var previousMonth = await client.GetInvoicesForPreviousMonthAsync();

// status by invoice id
var status = await client.GetInvoiceAsync(created.InvoiceId);
Console.ReadKey();