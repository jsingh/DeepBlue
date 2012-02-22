using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Controllers.Admin;

[assembly: PreApplicationStartMethod(typeof(DeepBlue.Helpers.EntityHelper), "Initialize")]

namespace DeepBlue.Helpers {

	public class EntityHelper {

		public static List<EntityPermission> Permissions { get; set; }

		public static void Initialize() {

			Permissions = new List<EntityPermission>();

			// Admin
			Permissions.Add(new EntityPermission { TableName = Table.InvestorEntityType, URL = "/Admin/EntityType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.InvestorType, URL = "/Admin/InvestorType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.CommunicationType, URL = "/Admin/CommunicationType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.CommunicationGrouping, URL = "/Admin/CommunicationGrouping", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.FundClosing, URL = "/Admin/FundClosing", IsSystemEntity = false, IsOtherEntity = true });

			// Custom Fields
			Permissions.Add(new EntityPermission { TableName = Table.CustomField, URL = "/Admin/CustomField", IsSystemEntity = false, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.DataType, URL = "/Admin/DataType", IsSystemEntity = true, IsOtherEntity = false });

			// Deal Management
			Permissions.Add(new EntityPermission { TableName = Table.PurchaseType, URL = "/Admin/PurchaseType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.DealClosingCostType, URL = "/Admin/DealClosingCostType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.UnderlyingFundType, URL = "/Admin/UnderlyingFundType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ShareClassType, URL = "/Admin/ShareClassType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.CashDistributionType, URL = "/Admin/CashDistributionType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.FundExpenseType, URL = "/Admin/FundExpenseType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ReportingFrequency, URL = "/Admin/ReportingFrequency", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ReportingType, URL = "/Admin/ReportingType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.Currency, URL = "/Admin/Currency", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.Industry, URL = "/Admin/Industry", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.Contact, URL = "/Admin/DealContact", IsSystemEntity = false, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.EquityType, URL = "/Admin/EquityType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.FixedIncomeType, URL = "/Admin/FixedIncomeType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.SellerType, URL = "/Admin/SellerType", IsSystemEntity = true, IsOtherEntity = true });

			// Document Management
			Permissions.Add(new EntityPermission { TableName = Table.DocumentType, URL = "/Admin/DocumentType", IsSystemEntity = true, IsOtherEntity = true });

			// User Management
			Permissions.Add(new EntityPermission { TableName = Table.USER, URL = "/Admin/Users", IsSystemEntity = false, IsOtherEntity = true });

			// Entity Management
			Permissions.Add(new EntityPermission { TableName = Table.ENTITY, URL = "/Admin/Entities", IsSystemEntity = true, IsOtherEntity = false });

			// Menu
			Permissions.Add(new EntityPermission { TableName = Table.Menu, URL = "/Admin/Menu", IsSystemEntity = true, IsOtherEntity = false });
		}

		public static string EntityName {
			get {
				return (Authentication.CurrentEntity == null ? "DeepBlue" : Authentication.CurrentEntity.EntityName);
			}
		}
	}
}