using BankTransfer;

var bank = new Bank();

var account1 = bank.CreateAccount();
var account2 = bank.CreateAccount();
var account3 = bank.CreateAccount();

var provider = new TransactionProvider();

await account1.TransferAsync(account2, 100, provider);
await account2.TransferAsync(account3, 52, provider);
await account1.TransferAsync(account3, 12, provider);
await account3.TransferAsync(account1, 82, provider);

Console.ReadLine();