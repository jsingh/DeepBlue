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
			Permissions.Add(new EntityPermission { TableName = Table.InvestorEntityType, ControllerName = "Admin", ActionName = "EntityType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.InvestorType, ControllerName = "Admin", ActionName = "InvestorType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.CommunicationType, ControllerName = "Admin", ActionName = "CommunicationType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.CommunicationGrouping, ControllerName = "Admin", ActionName = "CommunicationGrouping", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.FundClosing, ControllerName = "Admin", ActionName = "FundClosing", IsSystemEntity = false, IsOtherEntity = true });

			// Custom Fields
			Permissions.Add(new EntityPermission { TableName = Table.CustomField, ControllerName = "Admin", ActionName = "CustomField", IsSystemEntity = false, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.DataType, ControllerName = "Admin", ActionName = "DataType", IsSystemEntity = true, IsOtherEntity = false });

			// Deal Management
			Permissions.Add(new EntityPermission { TableName = Table.PurchaseType, ControllerName = "Admin", ActionName = "PurchaseType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.DealClosingCostType, ControllerName = "Admin", ActionName = "DealClosingCostType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.UnderlyingFundType, ControllerName = "Admin", ActionName = "UnderlyingFundType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ShareClassType, ControllerName = "Admin", ActionName = "ShareClassType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.CashDistributionType, ControllerName = "Admin", ActionName = "CashDistributionType", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.FundExpenseType, ControllerName = "Admin", ActionName = "FundExpenseType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ReportingFrequency, ControllerName = "Admin", ActionName = "ReportingFrequency", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.ReportingType, ControllerName = "Admin", ActionName = "ReportingType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.Currency, ControllerName = "Admin", ActionName = "Currency", IsSystemEntity = true, IsOtherEntity = false });
			Permissions.Add(new EntityPermission { TableName = Table.Industry, ControllerName = "Admin", ActionName = "Industry", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.Contact, ControllerName = "Admin", ActionName = "DealContact", IsSystemEntity = false, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.EquityType, ControllerName = "Admin", ActionName = "EquityType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.FixedIncomeType, ControllerName = "Admin", ActionName = "FixedIncomeType", IsSystemEntity = true, IsOtherEntity = true });
			Permissions.Add(new EntityPermission { TableName = Table.SellerType, ControllerName = "Admin", ActionName = "SellerType", IsSystemEntity = true, IsOtherEntity = true });

			// Document Management
			Permissions.Add(new EntityPermission { TableName = Table.DocumentType, ControllerName = "Admin", ActionName = "DocumentType", IsSystemEntity = true, IsOtherEntity = true });

			// User Management
			Permissions.Add(new EntityPermission { TableName = Table.USER, ControllerName = "Admin", ActionName = "Users", IsSystemEntity = false, IsOtherEntity = true });

			// Entity Management
			Permissions.Add(new EntityPermission { TableName = Table.ENTITY, ControllerName = "Admin", ActionName = "Entities", IsSystemEntity = true, IsOtherEntity = false });
		}

		public static string EntityName {
			get {
				return (Authentication.CurrentEntity == null ? "DeepBlue" : Authentication.CurrentEntity.EntityName);
			}
		}
	}
}