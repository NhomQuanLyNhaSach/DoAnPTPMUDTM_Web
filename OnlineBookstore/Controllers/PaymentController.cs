using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookstore.Models;

namespace OnlineBookstore.Controllers
{
    public class PaymentController : Controller
    {
        QL_NhaSachEntities _db = new QL_NhaSachEntities();
        // GET: Payment
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(DonHang dh)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var lstCart = (List<ChiTietGioHang>)Session["cart"];
                dh.NgayLap = DateTime.Now;
                dh.Id_NV = null;
                dh.Id_KH = (int)Session["UserId"];
                dh.TrangThaiDonHang = 1;
                dh.DaXoa = false;
                if (dh.PhuongThucThanhToan == false)
                {
                    dh.TrangThaiThanhToan = false;
                }
                else
                {
                    dh.TrangThaiThanhToan = true;
                }
                _db.DonHangs.Add(dh);
                _db.SaveChanges();

                int oderId = dh.Id;
                List<ChiTietDonHang> lstOderDetail = new List<ChiTietDonHang>();

                foreach (var item in lstCart)
                {
                    ChiTietDonHang obj = new ChiTietDonHang();
                    obj.Id_DH = oderId;
                    obj.Id_Sach = item.Id_Sach;
                    obj.SoLuongMua = item.SoLuongMua;
                    obj.DonGia = item.DonGia;
                    obj.ThanhTien = item.ThanhTien;
                    lstOderDetail.Add(obj);
                }

                _db.ChiTietDonHangs.AddRange(lstOderDetail);
                _db.SaveChanges();
            }
            
            return RedirectToAction("SucessPayment", "Payment");
        }
        public ActionResult SucessPayment()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}