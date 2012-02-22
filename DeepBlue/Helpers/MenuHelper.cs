using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using DeepBlue.Models.Admin;
using System.IO;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Helpers {
	public class MenuHelper {

		public const string ENTITYMENUKEY = "EntityMenu-{0}";

		public static List<EntityMenuModel> GetMenus() {
			string key = string.Format(ENTITYMENUKEY, Authentication.CurrentEntity.EntityID);
			ICacheManager cacheManager = new MemoryCacheManager();
			List<EntityMenuModel> menus = cacheManager.Get(key, () => {
				IAdminRepository adminRepository = new AdminRepository();
				return adminRepository.GetAllEntityMenus();
			});
			return menus;
		}

		public static EntityMenuModel GetMenu(string url) {
			List<EntityMenuModel> menus = GetMenus();
			EntityMenuModel topmenu = (from menu in menus
									   where menu.URL == url
									   select menu).FirstOrDefault();
			EntityMenuModel leftmenu = null;
			if (topmenu != null) {
				leftmenu = (from menu in menus
							where menu.ParentMenuID == topmenu.MenuID
							select menu).FirstOrDefault();
			}
			if (leftmenu == null)
				return topmenu;
			else
				return leftmenu;
		}


	}
}