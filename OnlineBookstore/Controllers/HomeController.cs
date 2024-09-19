using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookstore.Models;
using PagedList;

namespace OnlineBookstore.Controllers
{
    public class HomeController : Controller
    {
        private QL_NhaSachEntities _db = new QL_NhaSachEntities();
        // GET: Home
        public ActionResult Index(int? page)
        {
            int pageSize = 12; // Số sản phẩm hiển thị trên mỗi trang
            int pageNumber = (page ?? 1);

            var allBooks = _db.Saches.ToList();

            var model = allBooks.ToPagedList(pageNumber, pageSize);

            int? userId = Session["UserId"] as int?;

            if (userId.HasValue)
            {
                int totalQuantity = (int)_db.ChiTietGioHangs
                                        .Where(ct => ct.GioHang.Id_KH == userId)
                                        .Count();

                Session["totalQuantity"] = totalQuantity;
            }
            else
            {
                Session["totalQuantity"] = 0;
            }

            return View(model);
        }
        public ActionResult BookDetail(int id)
        {
            var sach = _db.Saches.Find(id);

            if (sach == null)
            {
                return HttpNotFound();
            }

            return View(sach);
        }

    }
}