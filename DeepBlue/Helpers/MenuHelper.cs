using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DeepBlue.Models.Admin;
using System.IO;

namespace DeepBlue.Helpers {
	public class MenuHelper {

		public static List<MenuModel> GetMenus() {
			string fileName = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/"), "web.sitemap");
			FileInfo fileInfo = new System.IO.FileInfo(fileName);
			string key = string.Format("DeepBlueMenu_{0}_{1}", (Authentication.IsSystemEntityUser == true ? "SystemEntity" : "OtherEntity"), fileInfo.LastWriteTime.ToString("MM_dd_yyyy_hh_mm_ss_tt"));
			ICacheManager cacheManager = new MemoryCacheManager();
			return cacheManager.Get(key, () => {
				List<MenuModel> menues = new List<MenuModel>();
				XDocument xmlDoc = XDocument.Load(fileName);
				ReadMenu(xmlDoc, ref menues);
				return menues;
			});
		}

		public static MenuModel FirstLeftMenu(string topMenuName) {
			return GetMenus().Where(m => m.Name == topMenuName).Select(m => m.Childs.FirstOrDefault()).FirstOrDefault();
		}

		private static void AppendMenu(MenuModel menu, ref System.Text.StringBuilder sb) {
			sb.AppendLine();
			sb.AppendFormat("<menu name=\"{0}\" displayname=\"{1}\" action=\"{2}\" controllername=\"{3}\" isadmin=\"{4}\" istopmenu=\"{5}\">", menu.Name, menu.DisplayName, menu.ActionName, menu.ControllerName, menu.IsAdmin, menu.IsTopMenu);
			AppendMenuAttrs(menu.HtmlAttributes, "htmlattributes", ref sb);
			sb.AppendLine();
			AppendMenuAttrs(menu.HtmlAttributes, "routevalues", ref sb);
			foreach (MenuModel cm in menu.Childs) {
				AppendMenu(cm, ref sb);
			}
			sb.AppendFormat("</menu>");
		}

		private static void AppendMenuAttrs(object atts, string attName, ref System.Text.StringBuilder sbparent) {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			var pros = atts.GetType().GetProperties();
			foreach (var p in pros) {
				sb.AppendFormat("<attr key=\"{0}\" value=\"{1}\"></attr>", p.Name, p.GetValue(atts, null));
			}
			if (string.IsNullOrEmpty(sb.ToString()) == false) {
				sbparent.AppendFormat("<{0}>", attName);
				sb.AppendLine();
				sbparent.Append(sb.ToString());
				sb.AppendLine();
				sbparent.AppendFormat("</{0}>", attName);
			}
		}

		private static void ReadMenu(XDocument element, ref List<MenuModel> menuList) {
			var q = element.Descendants("menu");
			var menus = (from c in q select c).ToArray();
			foreach (var m in menus) {
				int? c = m.Attributes().ToArray().Where(a => a.Name.ToString() == "istopmenu" && a.Value == "True").Count();
				if (c > 0) {
					MenuModel menu = GetMenuModel(m);
					ReadMenu(m, ref menu);
					if (CheckMenuPermission(menu)) {
						menuList.Add(menu);
					}
				}
			}
		}

		private static bool CheckMenuPermission(MenuModel menu) {
			bool isNotAppendMenu = false;
			if (Authentication.IsSystemEntityUser) {
				if (menu.IsSystemEntity == false) {
					isNotAppendMenu = true;
				}
			}
			if (isNotAppendMenu == false) {
				if (menu.IsOtherEntity == false && Authentication.IsSystemEntityUser == false) {
					isNotAppendMenu = true;
				}
			}
			if (isNotAppendMenu == false) {
				if (menu.IsAdmin && AdminAuthorizeHelper.IsAdmin == false) {
					isNotAppendMenu = true;
				}
			}
			return !(isNotAppendMenu);
		}

		private static void ReadMenu(XElement element, ref MenuModel parentMenu) {
			var menus = (from c in element.Elements("menu") select c).ToArray();
			foreach (var m in menus) {
				MenuModel menu = GetMenuModel(m);
				ReadMenu(m, ref menu);
				if (CheckMenuPermission(menu)) {
					parentMenu.Childs.Add(menu);
				}
			}
		}

		private static MenuModel GetMenuModel(XElement element) {
			MenuModel menu = new MenuModel();
			var arr = element.Attributes().ToArray();
			bool bvalue = false;
			foreach (var a in arr) {
				switch (a.Name.ToString()) {
					case "name":
						menu.Name = a.Value; break;
					case "displayname":
						menu.DisplayName = a.Value; break;
					case "action":
						menu.ActionName = a.Value; break;
					case "controllername":
						menu.ControllerName = a.Value; break;
					case "isadmin":
						bool.TryParse(a.Value, out bvalue);
						menu.IsAdmin = bvalue;
						break;
					case "istopmenu":
						bool.TryParse(a.Value, out bvalue);
						menu.IsTopMenu = bvalue;
						break;
					case "issystementity":
						bool.TryParse(a.Value, out bvalue);
						menu.IsSystemEntity = bvalue;
						break;
					case "isotherentity":
						bool.TryParse(a.Value, out bvalue);
						menu.IsOtherEntity = bvalue;
						break;
				}
			}
			ReadAttributes(ref menu, element, "htmlattributes");
			ReadAttributes(ref menu, element, "routevalues");
			return menu;
		}

		private static void ReadAttributes(ref MenuModel menu, XElement element, string elementName) {
			var htmlattrs = (from h in element.Elements(elementName) select h).ToArray();
			foreach (var ha in htmlattrs) {
				var atts = (from attr in ha.Elements("attr") select attr).ToArray();
				foreach (var at in atts) {
					var j = at.Attributes().ToArray();
					string key = string.Empty;
					string value = string.Empty;
					foreach (var z in j) {
						switch (z.Name.ToString()) {
							case "key": key = z.Value; break;
							case "value": value = z.Value; break;
						}
					}
					if (elementName == "htmlattributes") {
						menu.HtmlAttributes.Add(key, value);
					}
					else {
						menu.RouteValues.Add(key, value);
					}
				}
			}
		}

		public static List<MenuModel> GetLeftMenus(string menuName) {
			List<MenuModel> menus = GetMenus();
			MenuModel submenu = (from menu in menus
								 where menu.Childs.Where(m => m.Name == menuName).Count() > 0
								 select menu.Childs.FirstOrDefault())
					.FirstOrDefault();
			return (submenu != null ? submenu.Childs : null);
		}
	}
}