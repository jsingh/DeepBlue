using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Controllers.Member
{
    public interface IMemberControllerRepository {
        int GetSomethingFromDatabase(int id);
    }

    public class MemberControllerRepository : IMemberControllerRepository {
        public int GetSomethingFromDatabase(int id) {
            return 0;
        }
    }

    public class MemberController : Controller {

        public IMemberControllerRepository MemberControllerRepository { get; set; }

        public MemberController() {
            this.MemberControllerRepository = new MemberControllerRepository();
        }

        // karan: 02/15/2011
        // Follow the Unit testing standards document for the naming conventions of the Actions.
        // As detailed in the document, the following names should be used
        //*Index - the main "landing" page. This is also the default endpoint.
        //* List - a list of all the members.
        //* Show – show a particular member.
        //* Edit - an edit page for the member
        //* New - a create page for the member
        //* Create - creates a new member (and saves it if you're using a DB)
        //* Update - updates the member
        //* Delete - deletes the member

        //
        // GET: /Member/
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        //
        // GET: /Member/List
        [HttpGet]
        public ActionResult List() {
            return View();
        }

        //
        // GET: /Member/Show/1
        [HttpGet]
        public ActionResult Show(int id) {
            return View();
        }

        //
        // GET: /Member/New
        [HttpGet]
        public ActionResult New() {
            return View();
        }

        //
        // POST: /Member/Create
        [HttpPost]
        public ActionResult Create() {
            return View();
        }

        //
        // GET: /Member/Edit/1
        [HttpGet]
        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Member/Update/1
        [HttpPost]
        public ActionResult Update(int id) {
            return View();
        }

        //
        // GET: /Member/Delete/5
        [HttpGet]
        public ActionResult Delete(int id) {
            return View();
        }
    }
}
