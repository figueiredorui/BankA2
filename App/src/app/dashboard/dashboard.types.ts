
export interface CashFlow {
  Year: number;
  Month: number;
  MonthYear: string;
  DebitAmount: number;
  CreditAmount: number;
}

export interface Transaction {
  TransactionId: number;
  AccountId: number;
  TransactionDate: Date;
  TransactionType: string;
  Description: string;
  DebitAmount: number;
  CreditAmount: number;
  Tag: string;
  Balance: number;
}

export interface ImportCsvDefinition {
  HasHeaders: boolean;
  TransactionDate_Index: number;
  TransactionType_Index: number;
  Description_Index: number;
  DebitAmount_Index: number;
  CreditAmount_Index: number;
  Amount_Index: number;
}

export interface TransactionResult {
  Count: number;
  Page: number;
  ItemsPerPage: number;
  Data: Transaction[];
}

export interface TransactionSearch {
  Page: number;
  Query: string;
}
