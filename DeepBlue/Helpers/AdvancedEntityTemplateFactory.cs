using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.IO;

namespace DeepBlue.Helpers {
	public class AdvancedEntityTemplateFactory : System.Web.DynamicData.EntityTemplateFactory {
		public override string BuildEntityTemplateVirtualPath(string templateName, DataBoundControlMode mode) {
			string path = base.BuildEntityTemplateVirtualPath(templateName, mode);

			if (File.Exists(HttpContext.Current.Server.MapPath(path)))
				return path;

			return path.Replace("_" + mode.ToString(), "");
		}

		public override EntityTemplateUserControl CreateEntityTemplate(MetaTable table, DataBoundControlMode mode, string uiHint) {
			return base.CreateEntityTemplate(table, mode, uiHint);
		}

		public override string GetEntityTemplateVirtualPath(MetaTable table, DataBoundControlMode mode, string uiHint) {
			return base.GetEntityTemplateVirtualPath(table, mode, uiHint);
		}
	}
}