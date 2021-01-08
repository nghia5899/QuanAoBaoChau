using BTL_asp_quanaobaochau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTL_asp_quanaobaochau.Controllers
{
    public class NghiaController : Controller
    {
        DataBaseContextDataContext db = new DataBaseContextDataContext();
        // GET: Nghia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NhapThongTinDatHang()
        {
            return View();
        }

        public ActionResult QuanLyDonHang()
        {
            var listdonhang = (from p in db.DonHangs
                               select p).ToList();


            ViewBag.listdonhang = listdonhang;


            return View();
        }

        public ActionResult QuanLy()
        {
            var listdonhang = (from p in db.DonHangs
                               select p).ToList();

            ViewBag.listdonhang = listdonhang;
            return View();
        }

        public ActionResult searchinQuanLy(string date)
        {
            try
            {
                var donHang_ngay = from x in db.DonHangs
                                   where x.NgayDat.Value.Day == Convert.ToDateTime(date).Day
                                       && x.NgayDat.Value.Month == Convert.ToDateTime(date).Month
                                       && x.NgayDat.Value.Year == Convert.ToDateTime(date).Year
                                   select x;

                ViewBag.donhang = donHang_ngay;
                ViewBag.Find = "Tìm thấy";
            }
            catch (Exception e)
            {
                ViewBag.Find = "Không tìm thấy";
            }
            return View();
        }
        public ActionResult CapNhatGioHang()
        {
            return RedirectToAction("Nghia/muahang=-2");
        }
        public ActionResult DatHangThanhCong(FormCollection fields)
        {
            string hoten = fields["hoten"];
            string diachi = fields["diachi"];
            string email = fields["email"];
            string sdt = fields["dienthoaidd"];

            int tongtien = 0;
            List<GioHang> giohang = (List<GioHang>)Session["giohang"];
            ViewBag.listsp = giohang;
            foreach (GioHang sp in giohang)
            {
                tongtien += (sp.gia * sp.soluong);
            }
            ViewBag.tongtien = tongtien;

            DateTime ngay = DateTime.Parse(DateTime.Now.ToString("dd/MM/yy"));
            TimeSpan t = DateTime.Now - new DateTime(1970, 1, 1);
            int madonhang = (int)t.TotalSeconds;
            DonHang donhang = new DonHang();
            donhang.MaDonHang = madonhang;
            donhang.HoTen = hoten;
            donhang.DiaChi = diachi;
            donhang.Email = email;
            donhang.SoDienThoai = sdt;
            donhang.TongTien = tongtien;
            donhang.NgayDat = ngay;
            db.DonHangs.InsertOnSubmit(donhang);
            db.SubmitChanges();

            foreach (GioHang sp in giohang)
            {
                ChiTietDonHang chiTietDonHang = new ChiTietDonHang();
                chiTietDonHang.MaDonHang = madonhang;
                chiTietDonHang.MaSanPham = sp.Id;
                chiTietDonHang.SoLuongMua = sp.soluong;
                db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
                
            }
            db.SubmitChanges();
            ViewBag.hoten = hoten;
            ViewBag.diachi = diachi;
            ViewBag.email = email;
            ViewBag.sdt = sdt;
            return View();
        }

        public ActionResult Giohang()
        {
            int tongtien = 0;

            if (Session["giohang"] == null)
            {
                ViewBag.tongtien = 0;
            }
            else
            {
                List<GioHang> giohang = (List<GioHang>)Session["giohang"];
                ViewBag.length = giohang.Count;
                ViewBag.listsp = giohang;
                foreach (GioHang sp in giohang)
                {
                    tongtien += (sp.gia * sp.soluong);
                }
                ViewBag.tongtien = tongtien;
            }
            return View();
        }


        public ActionResult muahang(int id)
        {
            int tongtien = 0;
            if (Session["giohang"] == null)
            {
                try
                {
                    GioHang a = new GioHang();
                    var sp = db.SanPhams.Where(s => s.MaSanPham == id).Single();
                    a.Id = sp.MaSanPham;
                    a.ten = sp.TenSanPham;
                    a.soluong = 1;
                    a.gia = Convert.ToInt32(sp.Gia);
                    List<GioHang> giohang = new List<GioHang>();
                    giohang.Add(a);
                    Session["giohang"] = giohang;
                    ViewBag.listsp = giohang;
                }
                catch (InvalidOperationException)
                {
                    ViewBag.listsp = null;
                }
                ViewBag.tongtien = tongtien;
            }
            else
            {
                List<GioHang> giohang = (List<GioHang>)Session["giohang"];
                var index = giohang.FindIndex(sp => sp.Id == id);
                if (index == -1)
                {
                    try
                    {
                        ViewBag.kiemtra = "null";
                        GioHang a = new GioHang();
                        var sp = db.SanPhams.Where(s => s.MaSanPham == id).Single();
                        a.Id = sp.MaSanPham;
                        a.ten = sp.TenSanPham;
                        a.soluong = 1;
                        a.gia = Convert.ToInt32(sp.Gia);
                        giohang.Add(a);
                    }
                    catch (InvalidOperationException)
                    {

                    }
                }
                else
                {
                    giohang[index].soluong++;
                }
                foreach (GioHang sp in giohang)
                {
                    tongtien += (sp.gia * sp.soluong);
                }
                Session["giohang"] = giohang;
                ViewBag.length = giohang.Count;
                ViewBag.listsp = giohang;
                ViewBag.tongtien = tongtien;
            }

            return this.Giohang();
        }

    }
}