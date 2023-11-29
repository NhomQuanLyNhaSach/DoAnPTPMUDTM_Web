using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineBookstore.Models;

namespace OnlineBookstore.Controllers
{
    public class CartController : Controller
    {
        private QL_NhaSachEntities _db = new QL_NhaSachEntities();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int userId = (int)Session["UserId"];
            var cartItems = _db.ChiTietGioHangs.ToList().Where(u => u.GioHang.Id_KH == userId);
            Session["cart"] = cartItems.ToList();
            return View(cartItems);
        }

        public ActionResult AddToCart(int bookId, int quantity)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int userId = (int)Session["UserId"];
            
            var gioHang = _db.GioHangs.SingleOrDefault(g => g.Id_KH == userId);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    Id_KH = userId
                };

                _db.GioHangs.Add(gioHang);
                _db.SaveChanges();
            }

            var chiTietGioHang = _db.ChiTietGioHangs
                .SingleOrDefault(ctgh => ctgh.Id_GH == gioHang.Id && ctgh.Id_Sach == bookId);

            if (chiTietGioHang != null)
            {
                chiTietGioHang.SoLuongMua += quantity;
                chiTietGioHang.ThanhTien = chiTietGioHang.SoLuongMua * chiTietGioHang.DonGia;
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm mới
                var product = _db.Saches.Find(bookId);

                chiTietGioHang = new ChiTietGioHang
                {
                    Id_GH = gioHang.Id,
                    Id_Sach = bookId,
                    SoLuongMua = quantity,
                    DonGia = product.GiaBan,
                    ThanhTien = quantity * product.GiaBan
                };

                _db.ChiTietGioHangs.Add(chiTietGioHang);
            }

            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult CapNhatSoLuong(int sanPhamID, int soLuongMoi)
        {
            var gioHang = LayGioHang();
            var chiTietGioHang = gioHang.ChiTietGioHangs.SingleOrDefault(c => c.Id_Sach == sanPhamID);

            if (chiTietGioHang != null)
            {
                chiTietGioHang.SoLuongMua = soLuongMoi;
                chiTietGioHang.ThanhTien = soLuongMoi * chiTietGioHang.DonGia;

                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xóa 1 sản phẩm ra khỏi giỏ hàng
        public ActionResult XoaKhoiGio(int sanPhamID)
        {
            var gioHang = LayGioHang();
            var chiTietGioHang = gioHang.ChiTietGioHangs.SingleOrDefault(c => c.Id_Sach == sanPhamID);

            if (chiTietGioHang != null)
            {
                _db.ChiTietGioHangs.Remove(chiTietGioHang);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xóa nhiều sản phẩm ra khỏi giỏ hàng
        [HttpPost]
        public ActionResult XoaNhieuKhoiGio(IEnumerable<int> sanPhamIDs)
        {
            var gioHang = LayGioHang();
            var chiTietGioHangs = gioHang.ChiTietGioHangs.Where(c => sanPhamIDs.Contains(c.Id_Sach)).ToList();

            foreach (var chiTietGioHang in chiTietGioHangs)
            {
                _db.ChiTietGioHangs.Remove(chiTietGioHang);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Các hàm khác

        private GioHang LayGioHang()
        {
            // Hàm này để lấy thông tin giỏ hàng của người dùng, bạn có thể điều chỉnh tùy theo cách bạn tổ chức dữ liệu.
            // Ví dụ: Lấy thông tin từ Session, Database, hoặc bất kỳ nguồn dữ liệu nào bạn sử dụng.
            // Trong ví dụ này, giả sử bạn đang lưu thông tin giỏ hàng trong Session.
            var userId = (int)Session["UserId"];
            var gioHang = _db.GioHangs.SingleOrDefault(g => g.Id_KH == userId);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    Id_KH = userId
                };

                _db.GioHangs.Add(gioHang);
                _db.SaveChanges();
            }

            return gioHang;
        }
    }
}