using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;
using DeepBlue.Models.Admin.Enums;

namespace DeepBlue.Controllers.Admin {
	public class AdminController : Controller {

		public IAdminRepository AdminRepository { get; set; }

		public AdminController()
			: this(new AdminRepository()) {
		}

		public AdminController(IAdminRepository repository) {
			AdminRepository = repository;
		}   

		//
		// GET: /Admin/

		public ActionResult Index() {
			ViewData["MenuName"] = "Admin";
			ListModel model = new ListModel();
			model.ControllerType = ControllerType.EntityType;
			switch (model.ControllerType) {
				case ControllerType.EntityType:
					model.EntityTypes = AdminRepository.GetAllInvestorEntityTypes();
					break;
			}
			return View(model);
		}

		//
		// GET: /Admin/List
		[HttpGet]
		public ActionResult List(int id) {
			ListModel model = new ListModel();
			model.ControllerType = (ControllerType)id;
			switch(model.ControllerType){	
				case ControllerType.EntityType:
					model.EntityTypes = AdminRepository.GetAllInvestorEntityTypes();
					break;
			}
			return View(model);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EntityType(int page,int row) {
			return View();
		}

	}
}
