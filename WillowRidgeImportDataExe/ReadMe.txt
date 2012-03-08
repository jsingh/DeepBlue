  
DeepBlue->AssemblyInfo.cs

[assembly: InternalsVisibleTo("DeepBlue.Tests")]
[assembly: InternalsVisibleTo("DeepBlue.ImportData")]
  
  begin transaction
  declare @createdDate DateTime
  set @createdDate = '2011-10-02'
  
  delete from InvestorAddress where CreatedDate > @createdDate
  delete from [Address] where CreatedDate > @createdDate
  
  delete from InvestorCommunication where CreatedDate > @createdDate
  delete from Communication where CreatedDate > @createdDate
  
   delete from ContactCommunication where CreatedDate > @createdDate
   delete from Communication where CreatedDate > @createdDate
   delete from InvestorContact where CreatedDate > @createdDate
   delete from Contact where CreatedDate > @createdDate
   
   delete from Investor where CreatedDate > @createdDate
   rollback

   Amberbrook Fund
   ---------------
   The following table is used for importing Amberbrook fund from blue into DeepBlue: 6_10AmberbrookFundInfo
   Also, the Bank account information associated with the Fund is also extracted from the above table
  
   These fields are not present in blue, so we are using default values as defined
   Fund.TaxID = a unique 25 digit random number

   // If these values are not present in blue, the following defaults will be used.
   Fund.InceptionDate = 1/1/1900;

   WARNING: these fields are not present in Blue
   fund.NumofAutoExtensions
   fund.DateClawbackTriggered;
   fund.RecycleProvision;
   fund.MgmtFeesCatchUpDate;
   fund.Carry;
   WARNING: This field is present in blue but not present in Deepblue
   blueFund.AmberbrookFundNo


   Investors
   ----------------------
   The following table is used for importing Investors from blue into DeepBlue: C7_20tblLPPaymentInstructions
   WARNING Blue has the following properties, but DeepBlue doesnt have these
       investor.Reference;

   WARNING: DeepBlue has the following properties, but Blue doesnt have, so we are using the default values
            investor.IsDomestic = true;
            investor.InvestorEntityTypeID = "";
            investor.ResidencyState = "NY";
            investor.Social = a random 25 digit number
            investor.Notes = "Data Conversion from Blue";

            investor.TaxID = 0;
            investor.FirstName = "";
            investor.LastName = "n/a";
            investor.ManagerName = "";
            investor.MiddleName = "";
            investor.PrevInvestorID = 0;
            investor.TaxExempt = false;
	
	An investor can have multiple contacts. The following table is used to extract contacts for the investor: C7_25LPContactinfo

   UnderlyingFund
   ---------------------
   The following table is used for importing Underlying funds from blue into DeepBlue: C7_10tblGPPaymentInstructions

   UnderlyingFund has the following metadata, which is pulled in from the respective tables
	UnderlyingFundType		=>	8_50tblFundType.FundType
	IndustryFocus			=>	8_50tblIndustryFocus.IndustryFocus
	Reporting Types			=>	8_50tblReportingType.ReportingType
   The following meta-data is missing from blue, so we are using the currently available data in blue
	Geography
	ReportingFrequency
	ShareClassType
	InvestmentType

   In DeepBlue, UF belongs to an Issuer. Blue has no concept of Issuer for UF, So when importing the Underlying funds, we create a default issuer, and assign all the
   imported Underlying Funds to it.

   WARNING: Blue doesnt have the following fields, so we are using default values
            //uf.LegalFundName
            //uf.FiscalYearEnd
            //uf.IsDomestic
            //uf.FundStructureId
            //uf.Taxable
            //uf.Exempt
            //uf.AddressID
            //uf.managementfee
            //uf.incentivefee
            //uf.taxrate
            //uf.auditorname
            //uf.managercontactid
            //uf.shareclasstypeid
            //uf.investmenttypeid


   Q: Currently, we dont ask for ShareClassType when creating an underlying fund. We can add Share Class Type in the Admin module.
    Why are we adding the Share class type when we dont 
   intend to use it?

   Q. What is UF.Investment Type? Do we really need this?

   Q: When creating an underlying fund, under the bank information, we have 2 fields, Account Name and Account Number. 
   Why are there 2 fields for Account? Any specific reason?


   To Do: 
   ------------
   We need to create a new field called Website on the underlying fund level. 
   Currently it is on the Contact level, and is being stored as a communication value, which is wrong
   Convert FundSize to money in DB and re-gen the model
      
   

   Deal/CreateUnderlyingFundContact
   Good To Have Consistenccy
   -----------------------------
   we return the new contact id from the controller. Generally if it is a success, we return something like true||23 where 23 is the new id. But in this method we just return 23



   Currently
   -------------
   /Deal/CreateUnderlyingFundContact
   You can have multiple contacts for the UnderlyingFund using the
   UnderlyingFundContact table(I didnt ask for this.)
   My original implementation
   -----------------------------
   I had put 2 fields, ContactID and ManagerContactID for storing the Contacts tied to the UF

Issuer
--------------
The following table is used for importing Underlying funds from blue into DeepBlue: 4_20tblStockTable
We basically use distinct  4_20tblStockTable.Company as the name of the issuer


TO DO:
-----
Make IsGP as Not null, and set default value to 1

Equity
------------------
The following table is used for importing Underlying funds from blue into DeepBlue: 4_20tblStockTable
Equities in DeepBlue have the following 2 properties, which take their value from:
EquityType			=> 4_20tblStockTable.Security seems to have EquityType and ShareClassType clubbed together. We are exporting Security to EquityType	
ShareClassType		=> The imported equities dont have any values
We also assign 4_20tblStockTable.Company as the Issuer to the Equity.

WARNING:
----------------
In database, USD is det to CurrencyID = 6.. also in the enum. Why is that? I set the USD as CurrencyID = 1
I had to use the following script
  
  set identity_insert currency on
  insert into Currency (CurrencyID, EntityID, Currency, Enabled, CreatedDate, CreatedBy)
  Values(6,1, 'USD', 1, GETDATE(), 1)
  set identity_insert currency oFF




The following fields are present in Deepblue, but absent in Blue
eq.Public;
eq.ShareClassTypeID;
eq.IndustryID;
eq.CurrencyID;
eq.ISIN;
eq.Comments; // we will use default value when importing

 // Blue has StockSymbol, Ticker, StockName, so we are using StockSymbol to populate the Equity.Symbol

 DeepBlue has Convept of Public, which should get imported with 4-20tblStockTable.PrivateStock. However, when creating an equity, there is no UI element that lets the user choose that. 
 TO DO:
 -------------
 When creating an equity, you need to choose an issuer. However, you can change the Country, and the name of the issuer. I dont think that should be allowed.
 If the user wants, he should edit the issuer from another screen.

 Direct
 --------------
 TODO: Verify that you should not be able to create a direct with same (issuerId, EquityTypeId, ShareClassTypeID) pair. Is the three fields enough to uniquely define a direct?

 There are fields from Direct[7-10tblGPPaymentInstructions] in Blue that have a value in the DB, and that are not present in DeepBlue:
 Vintage Year, TerminationDate, WebSite, FeesInside,Reporting, Reporting Type


 /Deal/CreateSellerInfo currently doesnt return the new DealSellerContactID. Ask prasanna to do that.


 // Capital Distribution
 // WARNING: The following fields are present in DeepBlue but are absent from blue
 // NOTE: the fields are a rollup of the line items
            //deepBlueCD.CapitalDistributionProfit;
            //deepBlueCD.CapitalReturn;
            //deepBlueCD.DistributionNumber;
            //deepBlueCD.LPProfits;
            //deepBlueCD.PreferredCatchUp;
            //deepBlueCD.Profits;
            //deepBlueCD.ReturnFundExpenses;
            
            // The following fields is present in blue but absent from DeepBlue
            //blueCapitalDist.TotalUnusedCapital;

// Are the following fields ok?
deepBlueCD.CapitalDistributionDate = blueCapitalDist.NoticeDate;
            deepBlueCD.CapitalDistributionDueDate = blueCapitalDist.EffectiveDate;

6-30tblLPDistribution records both cash as well as stock distribution from  AMB fund to investors. WE dont have stock distribution implemented yet

 deepBlueCDLineItem.PaidON = blueCapitalDistLineItem.TransactionDate; ??

  // WARNING: The following fields are present in DeepBlue but are not present in Blue
            //deepBlueCDLineItem.LPProfits;
            //deepBlueCDLineItem.PreferredCatchUp;
            //deepBlueCDLineItem.PreferredReturn;
            //deepBlueCDLineItem.Profits;
            //deepBlueCDLineItem.ReconciliationMethod;
            //deepBlueCDLineItem.ReturnFundExpenses;
            //deepBlueCDLineItem.ReturnManagementFees;


// UF->Capital Call
// UnderlyingFundCapitalCallImport.cs
is the mapping ok?
 if (blueCapitalCall.Paid.HasValue) {
                deepBlueCC.IsReconciled = blueCapitalCall.Paid.Value;
            }
            deepBlueCC.PaidON = blueCapitalCall.PaymentDate;

            
            //WARNING: The following values are present on DeepBlue but are not present in Blue
            //deepBlueCC.IsDeemedCapitalCall 
            //deepBlueCC.ReconciliationMethod;

            //WARNING: The following fields are present in Blue but are not present in DeepBlue
            // There is actually a "Due Date" on the UI, but it is actually saved in Notice Date
            //blueCapitalCall.DueDate;
            //blueCapitalCall.Fees;
            //blueCapitalCall.Called;
            //blueCapitalCall.DateCalled;



// Capital Call
// WARNING: Following fields are present in DeepBlue but not in blue
            // The following fields are present in UI and should be provided
            //deepBlueCCLineItem.ManagementFeeInterest;
            //deepBlueCCLineItem.InvestedAmountInterest;

            // Capital Call Reconciliation( actually it is Capital Call Line Item reconciliation)
            // When doing the reconciliation on the capital call line item, there is a Payment Date on the UI.
            // That Payment Date is actually the CapitalCall.ReceivedDate associated with the CapitalCallLineItem
            // Moreover, that field is editable, However, when you save the reconciliation, that changes on the PaymentDate will not have any effect, which the correct behavior
            // We should make the payment date non-editable

            // The following fields are not present in Blue
            //deepBlueCCLineItem.ChequeNumber;
            //deepBlueCCLineItem.ReconciliationMethod

            // The following fields are not asked from the manual capital call.
            // Should we add these fields to the UI?
            // Probably they shud be on the deepBlueCC level
            //deepBlueCCLineItem.InvestmentAmount; // This is actually sum of New + Existing Investment Amount
            //deepBlueCCLineItem.NewInvestmentAmount;
            //deepBlueCCLineItem.ExistingInvestmentAmount;
			

11/17/11 InvestorFund

Questions to ask pulak 11/20/11
-----------------------------
You can create a deal, you can add Pre/post transactions. Post transaction means transactions


CASH Distribution
------------------------
In blue, It looks like the cash distribution is for fnd as well as direct. I say this cos when doing the data conversion 
Post record cash distribution (C1_30tblPostRecordDateTransactions),
C1_30tblPostRecordDateTransactions.Fund(which points to 5-10tblDealOrigination.Fund, which points to either to Fund,
 or direct (in 7-10tblGPPaymentInstructions.Fund distinguished by FundType(Direct/Venture/Buyout etc))
contains name of directs also. In deepblue, we dont have the concept of Cash distribution for directs

When doing the conversion, if the app is run multiple times, the PRCD will be re-run again.
I dont have a way to see PRCD in deepblue. This brings up another question. Should there not be a way to 
see PRCD in the system?
