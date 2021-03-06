﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningSystem.Models;
using System.Threading;
using Domain.Abstract;
using Domain;
using System.Web.Security;

namespace ELearningSystem.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        IElearningSystemRepository _repository;
        MembershipProvider _provider;

        public AccountController(IElearningSystemRepository repo, MembershipProvider membershipProvider)
        {
            _repository = repo;
            _provider = membershipProvider;
        }

        public PartialViewResult Login(UserInformation info)
        {
            if (info == null)
                return PartialView(new AccountModel());
            else if (info.IsStudent)
            {
                return PartialView("StudentProfile", new AccountModel() { UserName = info.UserName });
            }
            else
            {
                return PartialView("LecturerProfile", new AccountModel() { UserName = info.UserName });
            }
        }

        [HttpPost]
        public PartialViewResult LoggingIn(AccountModel account)
        {
            bool res = _provider.ValidateUser(account.UserName, account.Password);
            if (res)
            {
                bool isStudent = CheckIfUserIsStudent(account.UserName);
                Session["username"] = account.UserName;
                Session["isStudent"] = isStudent;
                if (isStudent)
                {
                    Session["userId"] = _repository.Students.Where(x => x.Login == account.UserName).First().ID;
                }
                else
                {
                    Session["userId"] = _repository.Lecturers.Where(x => x.Login == account.UserName).First().ID;
                }
                if (isStudent) return PartialView("StudentProfile", account);
                else return PartialView("LecturerProfile", account);
                //return PartialView("Profile", account);
            }
            else
            {
                ModelState.AddModelError("LoginError", "Login or password is incorrect.");
                return PartialView("Login", account);
            }
        }

        public ActionResult Registration()
        {
            return View(new IsStudentModel() { IsStudent = true });
        }

        public RedirectToRouteResult LogOut(string returnUrl)
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public PartialViewResult ConcreteRegistration(IsStudentModel model)
        {
            if (model.IsStudent) return PartialView("StudentRegistration");
            else return PartialView("LecturerRegistration");
        }

        public PartialViewResult RegisterStudent(Student model)
        {
            if (!ModelState.IsValid) return PartialView("StudentRegistration");
            _repository.SaveStudent(model);
            return PartialView("RegistrationFinished", new RegistrationFinishedModel()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email
            });
        }

        public PartialViewResult RegisterLecturer(Lecturer model)
        {
            if (!ModelState.IsValid) return PartialView("LecturerRegistration");
            _repository.SaveLecturer(model);
            return PartialView("RegistrationFinished", new RegistrationFinishedModel()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email
            });
        }

        public ActionResult EditProfile(UserInformation user)
        {
            if (user != null)
            {
                if (user.IsStudent)
                {
                    try
                    {
                        StudentEditProfileModel model = (from x in _repository.Students
                                                         where x.ID == user.UserId
                                                         select new StudentEditProfileModel
                                                         {
                                                             Id = x.ID,
                                                             Information = x.Information,
                                                             Interests = x.Interests,
                                                             Name = x.Name,
                                                             Surname = x.Surname
                                                         }).First();
                        return View("EditStudentProfile", model);

                    }
                    catch
                    {
                        return View("Error");
                    }
                }
                else
                {
                    try
                    {
                        LecturerEditProfileModel model = (from x in _repository.Lecturers
                                                          where x.ID == user.UserId
                                                          select new LecturerEditProfileModel
                                                          {
                                                              Id = x.ID,
                                                              Information = x.Information,
                                                              Interests = x.Interests,
                                                              Name = x.Name,
                                                              Surname = x.Surname,
                                                              IsAcademic = x.IsAcademic
                                                          }).First();
                        return View("EditLecturerProfile", model);

                    }
                    catch
                    {
                        return View("Error");
                    }
                }
            }
            else
            {
                return View("UnauthorizedAccess");
            }
        }

        [HttpPost]
        public ActionResult EditStudentProfile(UserInformation user, StudentEditProfileModel model)
        {
            if (user != null)
            {
                if (!user.IsStudent)
                {
                    return View("Error");
                }
                else
                {
                    try
                    {
                        Student student = _repository.Students.Where(x => x.ID == user.UserId).First();
                        student.Information = model.Information;
                        student.Interests = model.Interests;
                        student.Name = model.Name;
                        student.Surname = model.Surname;
                        _repository.SaveStudent(student);
                        return View("ChangesSaved");
                    }
                    catch
                    {
                        return View("Error");
                    }
                }
            }
            else
            {
                return View("UnauthorizedAccess");
            }
        }

        [HttpPost]
        public ActionResult EditLecturerProfile(UserInformation user, LecturerEditProfileModel model)
        {
            if (user != null)
            {
                if (user.IsStudent)
                {
                    return View("Error");
                }
                else
                {
                    try
                    {
                        Lecturer lecturer = _repository.Lecturers.Where(x => x.ID == user.UserId).First();
                        lecturer.Information = model.Interests;
                        lecturer.Interests = model.Interests;
                        lecturer.Name = model.Name;
                        lecturer.Surname = model.Surname;
                        lecturer.IsAcademic = model.IsAcademic;
                        _repository.SaveLecturer(lecturer);
                        return View("ChangesSaved");
                    }
                    catch
                    {
                        return View("Error");
                    }
                }
            }
            else
            {
                return View("UnauthorizedAccess");
            }
        }

        private bool CheckIfUserIsStudent(string userName)
        {
            if (_repository.Students.Where(x => x.Login == userName).Count() > 0) return true;
            return false;
        }

        public JsonResult ValidateUniqueLoginName(string login)
        {
            if (_repository.Students.Where(x => x.Login == login).Count() == 0
                && _repository.Lecturers.Where(x => x.Login == login).Count() == 0) return Json(true, JsonRequestBehavior.AllowGet);
            return Json("This login is already used.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateUniqueEmail(string email)
        {
            if (_repository.Students.Where(x => x.Email == email).Count() == 0
                && _repository.Lecturers.Where(x => x.Email == email).Count() == 0) return Json(true, JsonRequestBehavior.AllowGet);
            return Json("ValidateUniqueEmail", JsonRequestBehavior.AllowGet);
        }
    }
}
