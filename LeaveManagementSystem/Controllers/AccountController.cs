﻿using LeaveManagementSystem.DomainModel.Identity;
using LeaveManagementSystem.ServiceLayer;
using LeaveManagementSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        IEmployeeService es;

        public AccountController(IEmployeeService es)
        {
            this.es = es;
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count >= 1)
                {
                    var file = Request.Files[0];
                    var imgBytes = new Byte[file.ContentLength];
                    file.InputStream.Read(imgBytes, 0, file.ContentLength);
                    var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
                    rvm.UserImg = base64String;
                }
                int? id = this.es.InsertEmployee(rvm);
                if (id!=null)
                    return Content("<script language='javascript' type='text/javascript'>alert('Employee Created');</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Employee Not Created');</script>");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View();
            }
        }
        public ActionResult Update(int id)
        {
            EmployeeViewModel evm = this.es.GetEmployeeByID(id);
            UpdateEmployeeViewModel uevm = new UpdateEmployeeViewModel() { EmployeeId =evm.EmployeeID,Name = evm.Name , Address = evm.Address, DOB = evm.DOB , Phone = evm.Phone,Email = evm.Email};
            return View(uevm);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Update(UpdateEmployeeViewModel uevm)
        {
            if (ModelState.IsValid)
            {
                this.es.UpdateEmployeeDetails(uevm);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid data");
                return View(uevm);
            }
        }
    }
}