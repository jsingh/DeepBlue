using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Investor;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.IO;


namespace DeepBlue.ImportData {
	public class Globals {
		public static int DefaultCountryID;
		public static int DefaultStateID;
		public static List<STATE> States;
		public static List<COUNTRY> Countries;
		public static List<UnderlyingFundType> UnderlyingFundTypes;
		public static List<Industry> Industries;
		public static List<Geography> Geograpies;
		public static List<ReportingFrequency> ReportingFrequencies;
		public static List<ReportingType> ReportingTypes;
		public static List<DeepBlue.Models.Deal.DirectListModel> Issuers;
		public static List<EquityType> EquityTypes;
		//public static List<ShareClassType> ShareClassTypes;
		//public static List<InvestmentType> InvestmentTypes;
		public static List<PurchaseType> PurchaseTypes;
		public static List<SellerType> SellerTypes;
		public static List<DeepBlue.Models.Entity.DealClosingCostType> DealClosingCostTypes;
		public static int DefaultInvestorEntityTypeID;

	 	public static string LogFile = string.Empty;

		public static string ConsoleLogFile = string.Empty;
		public static string ConsoleErrorLogFile = string.Empty;
		public static string ConsoleWarningLogFile = string.Empty;
		public static string ConsoleSuccessLogFile = string.Empty;

	 	public static string MessageFile = string.Empty;
		public static string DefaultString = "Data Conversion from Blue";
		public static string DefaultStringValue = "Data not found";
		public static int DefaultEntityID = Convert.ToInt16(ConfigurationManager.AppSettings["DefaultEntityID"]);
		public static int DefaultUserID = Convert.ToInt16(ConfigurationManager.AppSettings["DefaultUserID"]);
		public static string DefaultAddress1 = "Data Conversion default";
		public static string DefaultCity = "New York";
		public static string DefaultZip = "10280";
		public static USER CurrentUser = new USER() { EntityID = DefaultEntityID, UserID = DefaultUserID };
		public static string BaseUrl = string.Empty;
		public static CookieCollection CookieContainer = null;
		public static string LegalFee = "Legal Fee";

		static Globals() {
			Init();

		}

		public static void Init() {
			
			//using (DeepBlueEntities context = new DeepBlueEntities()) {
			//Countries = context.COUNTRies.ToList();
			//States = context.STATEs.ToList();
			DefaultCountryID = 225; //Countries.Where(x => x.CountryCode == "US").First().CountryID; 225
			DefaultStateID = 33; //States.Where(x => x.Abbr == "NY").First().StateID; 33
			DefaultInvestorEntityTypeID = 2; // Corporation // context.InvestorEntityTypes.First().InvestorEntityTypeID;
			//UnderlyingFundTypes = context.UnderlyingFundTypes.ToList();
			//Industries = context.Industries.Where(x => x.EntityID == DefaultEntityID).ToList();
			//Geograpies = context.Geographies.Where(x => x.EntityID == DefaultEntityID).ToList();
			//ReportingFrequencies = context.ReportingFrequencies.Where(x => x.EntityID == DefaultEntityID).ToList();
			//ReportingTypes = context.ReportingTypes.Where(x => x.EntityID == DefaultEntityID).ToList();
			//ShareClassTypes = context.ShareClassTypes.Where(x => x.EntityID == DefaultEntityID).ToList();
			//InvestmentTypes = context.InvestmentTypes.Where(x => x.EntityID == DefaultEntityID).ToList();

			string prefix = string.Format("{0}.{1}.{2}.{3}.", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Millisecond);
			LogFile = prefix + ConfigurationManager.AppSettings["ConversionErrorLog"];
			MessageFile = prefix + ConfigurationManager.AppSettings["MessageLog"];
			string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
			string directory = codeBase.Substring(0, codeBase.LastIndexOf("/"));


			ConsoleLogFile = "ConsoleLogFile.html";

			ConsoleErrorLogFile = "ConsoleErrorLogFile_" + LogFile;
			ConsoleWarningLogFile = "ConsoleWarningLogFile_" + LogFile;
			ConsoleSuccessLogFile = "ConsoleSuccessLogFile_" + LogFile;

			LogFile = directory + "/" + LogFile;
			LogFile = LogFile.Replace("file:///", string.Empty);

			ConsoleErrorLogFile = directory + "/" + ConsoleErrorLogFile;
			ConsoleErrorLogFile = ConsoleErrorLogFile.Replace("file:///", string.Empty);

			ConsoleWarningLogFile = directory + "/" + ConsoleWarningLogFile;
			ConsoleWarningLogFile = ConsoleWarningLogFile.Replace("file:///", string.Empty);

			ConsoleSuccessLogFile = directory + "/" + ConsoleSuccessLogFile;
			ConsoleSuccessLogFile = ConsoleSuccessLogFile.Replace("file:///", string.Empty);

			BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];

			string[] creds = ConfigurationManager.AppSettings["LoginCredentials"].Split(',');
			string username = creds[0].Trim();
			string password = creds[1].Trim();
			string entitycode = creds[2].Trim();
			CookieContainer = new CookieCollection();


			HttpWebRequestUtil.LoginPortal(username, password, entitycode, CookieContainer);

			UnderlyingFundTypeImport.SyncUnderlyingFundTypes(CookieContainer);
			IndustryFocusImport.SyncIndustryFocuses(CookieContainer);
			ReportingTypeImport.SyncReportingTypes(CookieContainer);
			UnderlyingFundTypes = UnderlyingFundTypeImport.GetUnderlyingFundTypesFromDeepBlue(CookieContainer); ;
			Industries = IndustryFocusImport.GetIndustriesFromDeepBlue(CookieContainer);
			Geograpies = GeographyImport.GetGeographiesFromDeepBlue(CookieContainer);
			ReportingFrequencies = ReportingFrequencyImport.GetReportingFrequenciesFromDeepBlue(CookieContainer);
			ReportingTypes = ReportingTypeImport.GetReportingTypesFromDeepBlue(CookieContainer);
			EquityTypeImport.SyncEquityTypes(CookieContainer);
			EquityTypes = EquityTypeImport.GetEquityTypesFromDeepBlue(CookieContainer);
			IssuerImport.SyncIssuers(CookieContainer);
			Issuers = IssuerImport.GetIssuersFromDeepBlue(CookieContainer);
			PurchaseTypeImport.SyncPurchaseTypes(CookieContainer);
			PurchaseTypes = PurchaseTypeImport.GetPurchaseTypesFromDeepBlue(CookieContainer);
			DealClosingCostTypeImport.SyncDealClosingCostTypes(CookieContainer);
			DealClosingCostTypes = DealClosingCostTypeImport.GetDealClosingCostTypesFromDeepBlue(CookieContainer);
			SellerTypeImport.SynSellerTypes(CookieContainer);
			SellerTypes = SellerTypeImport.GetSellerTypesFromDeepBlue(CookieContainer);

		}

		 

	}
}
