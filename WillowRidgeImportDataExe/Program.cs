using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using DeepBlue.Models.Investor;

using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using DeepBlue.Models.Deal;
using System.Globalization;

namespace DeepBlue.ImportData {
	class Program {
	
		static void Main(string[] args) {

			try {
				
				//// 1. Investor Import
				InvestorImport.ImportInvestors(Globals.CookieContainer);

				//// 2. Fund import
				FundImport.ImportFunds(Globals.CookieContainer);

				////3. Investor Fund Import
				InvestorFundImport.ImportInvestorFunds(Globals.CookieContainer);

				////4. Underlying Fund Import
				UnderlyingFundImport.ImportFunds(Globals.CookieContainer);

				////5. Direct Import
				DirectsImport.ImportEquities(Globals.CookieContainer);

				//6. Deals
				DealImport.ImportDeals(Globals.CookieContainer);

				//7. Capital Calls
				//Before calling InvestorFundImport, Make sure InvestorImport.ImportInvestors has already been run
				//Before calling CapitalCallImport, Make sure InvestorFundImport.ImportInvestorFunds has already been run
				CapitalCallImport.ImportCapitalCall(Globals.CookieContainer);

				//8. Capital Distribution import
				CapitalDistributionImport.ImportCapitalDistribution(Globals.CookieContainer);

				//9 Post record date transactions
				//9a. PRDCC
				UnderlyingFundCapitalCallImport.ImportPostRecordCapitalCall(Globals.CookieContainer);

				//9b PRDCD
				CashDistributionImport.ImportPostRecordDateCashDistribution(Globals.CookieContainer);
 
				//10. Underlying Fund Capital Call
				UnderlyingFundCapitalCallImport.ImportCapitalCall(Globals.CookieContainer);

				//11. Cash Distributions
				CashDistributionImport.ImportCashDistribution(Globals.CookieContainer);

				Console.WriteLine("Press any key to continue........");
				Console.ReadLine();

			}
			catch (Exception ex) {
				Util.Log("Exception: " + Environment.NewLine + ex.Message);
				Util.Log("Press any key to continue........");
				Console.ReadLine();
			}
		}

		//private static void ConvertViaDB(string username, string password, string entityCode) {
		//    Globals.LogOn(username, password, entityCode);
		//    InvestorImport.ConvertInvestorViaDB();
		//}

		//private static void ConvertViaWeb(string username, string password, string entityCode) {
		//    // we need this even in case of Web,as we need to set the CurrentUsr
		//    Globals.LogOn(username, password,entityCode);
		//    CookieCollection container = new CookieCollection();
		//    HttpWebRequestUtil.LoginPortal(username, password, entityCode, container);
		//    InvestorImport.ImportInvestors(container);
		//    Util.Log(string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", InvestorImport.TotalConversionRecords, InvestorImport.RecordsConvertedSuccessfully, InvestorImport.Errors.Count));
		//    Util.Log(string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", InvestorImport.TotalImportRecords, InvestorImport.RecordsImportedSuccessfully, InvestorImport.ImportErrors.Count));
		//}

		//private static void ConvertFundViaWeb(string username, string password) {
		//    // we need this even in case of Web,as we need to set the CurrentUsr
		//    Globals.LogOn(username, password);
		//    CookieCollection container = new CookieCollection();
		//    HttpWebRequestUtil.LoginPortal(username, password, container);
		//    Fund.ConvertFundViaWeb(container);
		//    Util.Log(string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", Fund.TotalConversionRecords, Fund.RecordsConvertedSuccessfully, Fund.Errors.Count));
		//    Util.Log(string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", Fund.TotalImportRecords, Fund.RecordsImportedSuccessfully, Fund.ImportErrors.Count));

		//}
	}
}
