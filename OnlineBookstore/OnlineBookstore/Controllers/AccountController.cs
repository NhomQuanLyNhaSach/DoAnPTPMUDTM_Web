using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookstore.Models;

namespace OnlineBookstore.Controllers
{
    public class AccountController : Controller
    {
        private QL_NhaSachEntities _db = new QL_NhaSachEntities();
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KhachHang model)
        {
            if (ModelState.IsValidField("TenDangNhap") && ModelState.IsValidField("MatKhau"))
            {
                var user = _db.KhachHangs.FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);

                if (user != null)
                {
                    

                    if (BCrypt.Net.BCrypt.Verify(model.MatKhau, user.MatKhau))
                    {
                        if (user.DaXoa == true)
                        {
                            ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                            return View(model);
                        }
                        Session["UserId"] = user.Id;
                        Session["UserName"] = user.TenDangNhap;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                if (_db.KhachHangs.Any(u => u.TenDangNhap == model.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);

                var newUser = new KhachHang
                {
                    HoTen = model.HoTen,
                    TenDangNhap = model.TenDangNhap,
                    MatKhau = hashedPassword,
                    Email = model.Email,
                    SDT = model.SDT,
                    DiaChi = model.DiaChi,
                    DaXoa = false
                };

                _db.KhachHangs.Add(newUser);
                _db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}