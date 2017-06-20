USE [BankA2]

SET IDENTITY_INSERT [dbo].[BankAccount] ON
INSERT INTO [dbo].[BankAccount]
           ([AccountId],[ChangedBy],[ChangedOn],[Closed],[CreatedBy],[CreatedOn],[Description],[ImportCsvDefinition])
    Select  [AccountId],[ChangedBy],[ChangedOn],[Closed],[CreatedBy],[CreatedOn],[Description],null 
			FROM BankA.[dbo].[BankAccount]
SET IDENTITY_INSERT [dbo].[BankAccount] OFF

SET IDENTITY_INSERT [dbo].[BankFile] ON
INSERT INTO [dbo].[BankFile]
           ([FileId],[AccountId],[ChangedBy],[ChangedOn],[ContentType],[CreatedBy],[CreatedOn],[FileContent],[FileName])
    Select  [FileId],[AccountId],[ChangedBy],[ChangedOn],[ContentType],[CreatedBy],[CreatedOn],[FileContent],[FileName]
			FROM BankA.[dbo].[BankFile]
SET IDENTITY_INSERT [dbo].[BankFile] OFF

SET IDENTITY_INSERT [dbo].[BankTag] ON
INSERT INTO [dbo].[BankTag]
           ([TagId],[ChangedBy],[ChangedOn],[CreatedBy],[CreatedOn],[Description],[Tag])
     Select RuleID, [ChangedBy],[ChangedOn],[CreatedBy],[CreatedOn],[Description],[Tag] 
			FROM  [BankA].dbo.BankTransactionRule
SET IDENTITY_INSERT [dbo].[BankTag] OFF

SET IDENTITY_INSERT [dbo].[BankTransaction] ON
INSERT INTO [dbo].[BankTransaction]
           ([TransactionId],[AccountId],[ChangedBy],[ChangedOn],[CreatedBy],[CreatedOn],[CreditAmount],[DebitAmount],[Description],[FileId],[IsTransfer],[Tag],[TransactionDate],[TransactionType])
     SELECT				 ID,[AccountId],[ChangedBy],[ChangedOn],[CreatedBy],[CreatedOn],[CreditAmount],[DebitAmount],[Description],[FileId],[IsTransfer],[Tag],[TransactionDate],[TransactionType]
	 FROM BankA.[dbo].[BankTransaction]
SET IDENTITY_INSERT [dbo].[BankTransaction] OFF