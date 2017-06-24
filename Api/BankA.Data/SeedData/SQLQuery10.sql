
--DatePart(Year,TransactionDate) AS Year, 
--				 DatePart(Month,TransactionDate) AS Month, 

Select 

	AccountId,
--	TransactionDate,
	DatePart(Year,TransactionDate),  DatePart(Month,TransactionDate),
	SUM(CreditAmount) AS CreditAmount, 
	SUM(TransferIn) AS TransferIn,
	SUM(DebitAmount) AS DebitAmount,
	SUM(TransferOut) AS TransferOut


FROM (
		SELECT
			AccountId,
			TransactionDate,
			CreditAmount, 
			0 as TransferIn,
			DebitAmount,
			0 as TransferOut
		FROM        BankTransaction
		where  istransfer = 0
	
	union all
	
		SELECT      
			AccountId,   
			TransactionDate, 
			0 as CreditAmount,
			CreditAmount as TransferIn, 
			0 as DebitAmount,
			DebitAmount AS TransferOut
		FROM         BankTransaction
		where  istransfer = 1
	) AS T2 
--GROUP BY TransactionDate, AccountId
--order by TransactionDate
GROUP by AccountId, DatePart(Year,TransactionDate),  DatePart(Month,TransactionDate)
order by DatePart(Year,TransactionDate),  DatePart(Month,TransactionDate)

--SELECT      
--			AccountId,   
--			TransactionDate, 
--			0 as CreditAmount,
--			CreditAmount as TransferIn, 
--			0 as DebitAmount,
--			DebitAmount AS TransferOut
--		FROM         BankTransaction
--		where  istransfer = 1
--		and TransactionDate = '2012-09-03 00:00:00'

Select 

	AccountId,
	TransactionDate,
	SUM(CreditAmount) AS CreditAmount, 
	SUM(DebitAmount) AS DebitAmount
	
	from BankTransaction where TransactionDate = '2012-09-03 00:00:00'
	GROUP BY TransactionDate, AccountId

select * from BankTransaction where TransactionDate = '2012-09-03 00:00:00'