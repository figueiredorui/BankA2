select 'new BankAccount() { BankName = "' + BankName + '", Description = "' + Description + '" };' from BankA_Dev.dbo.BankAccount

select 'new BankTransaction() { AccountId = ' + CONVERT(nvarchar(10), AccountId) + ', TransactionDate = DateTime.Parse("' + CONVERT(nvarchar(10), TransactionDate, 120) + '"), TransactionType = "' + Isnull(TransactionType,'') + '", Description="' + Description + '", Tag="' + Tag + '", CreditAmount=' + CONVERT(nvarchar(10), CreditAmount) + 'm, DebitAmount=' + CONVERT(nvarchar(10), DebitAmount) + 'm },' 
from BankA_Dev.dbo.BankTransaction
