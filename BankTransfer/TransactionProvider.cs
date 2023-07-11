using BankTransfer.Interfaces;
using BankTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer
{
    public class TransactionProvider : ITransactionProvider
    {
        public void From(IAccount account, double amount)
        {
            account.AddTransaction(new Transaction(TransactionType.Debit, account, amount));
        }

        public void To(IAccount account, double amount)
        {
            account.AddTransaction(new Transaction(TransactionType.Credit, account, amount));
        }
    }
}
