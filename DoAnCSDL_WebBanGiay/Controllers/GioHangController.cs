using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCSDL_WebBanGiay.Models;
using System.Net;
using System.Net.Mail;

namespace DoAnCSDL_WebBanGiay.Controllers
{
    public class GioHangController : Controller
    {
        DoAn_WebBanHangEntities db = new DoAn_WebBanHangEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            List<CartItem> gioHang = Session["gioHang"] as List<CartItem>;
            return View(gioHang);
        }
        public RedirectToRouteResult AddToCart(int MaSP, string txtsize)
        {
            if (txtsize== "")
                return RedirectToAction("Index");
            if (Session["gioHang"] == null)
            {
                Session["gioHang"] = new List<CartItem>();
            }
            List<CartItem> gioHang = Session["gioHang"] as List<CartItem>;
            var SLSize = from s in gioHang
                         where s.MaSP == MaSP && s.Size == txtsize
                         select s; //Tìm kiếm có sản phẩm tương tự trong giỏ
            List<SoLuongSize> slSize = db.SoLuongSizes.ToList();
            var MaSize = from s in slSize
                         where s.MaSP == MaSP && s.Size == txtsize
                         select s.MaSizeSP; //Lấy mã sản phẩm và size chi tiết
            if (SLSize.FirstOrDefault() == null)
            {
                SanPham sp = db.SanPhams.Find(MaSP);
                CartItem item = new CartItem();
                item.MaSP = MaSP;
                item.TenSP = sp.TenSP;
                item.DonGia = Convert.ToDouble(sp.Gia);
                item.SoLuong = 1;
                item.HinhSP = sp.HinhAnh;
                item.Size = txtsize;
                item.MaSizeSp = MaSize.FirstOrDefault();
                gioHang.Add(item);
            }
            else
            {
                CartItem item = gioHang.FirstOrDefault(m => m.MaSizeSp == MaSize.FirstOrDefault());
                item.SoLuong++;
            }
            Session["gioHang"] = gioHang;
            return RedirectToAction("Index");

        }
        public RedirectToRouteResult UpdateCart(int MaszSP, int txtSoLuong)
        {
            List<CartItem> gioHang = Session["gioHang"] as List<CartItem>;

            CartItem item = gioHang.FirstOrDefault(m => m.MaSizeSp == MaszSP);
            if (item != null)
            {
                item.SoLuong = txtSoLuong;
                Session["gioHang"] = gioHang;
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Delete(int MaSPsize)
        {
            List<CartItem> gioHang = Session["gioHang"] as List<CartItem>;
            CartItem item = gioHang.FirstOrDefault(m => m.MaSizeSp == MaSPsize);
            if (item != null)
            {
                gioHang.Remove(item);
                Session["gioHang"] = gioHang;
            }
            return RedirectToAction("Index");
        }
        public RedirectToRouteResult Order(string Email, string Phone)
        {
            List<CartItem> gioHang = Session["gioHang"] as List<CartItem>;
            //Ghi vào hóa đơn
            DonDatHang hd = new DonDatHang();;
            List<DonDatHang> cts = db.DonDatHangs.ToList();
            string MaDon;
            if (cts.FirstOrDefault() == null)
                MaDon = "DH1";
            else
                MaDon ="DH"+(Int64.Parse(cts.LastOrDefault().MaDDH.Substring(2)) +1).ToString();
            hd.NgayDat = DateTime.Now;
            hd.MaKH = 2;
            hd.MaDDH = MaDon;
            db.DonDatHangs.Add(hd);
            db.SaveChanges();
            foreach (var item in gioHang)
            {
                ChiTietDonDatHang ct = new ChiTietDonDatHang();
                ct.SoLuong = item.SoLuong;
                ct.DonGia = (Decimal)item.DonGia;
                ct.MaSP = item.MaSP;
                ct.TenSP = item.TenSP;
                ct.Size = item.Size;
                ct.MaDDH = MaDon;
                ct.MaSizeSP = item.MaSizeSp;
                db.ChiTietDonDatHangs.Add(ct);
                db.SoLuongSizes.Find(item.MaSizeSp).SoLuong -= item.SoLuong;
                db.SanPhams.Find(item.MaSP).SoLanMua += 1;
                db.SaveChanges();
            }
            Session["gioHang"] = null;
            //Gửi mail
            string sMsg = "<html><body><table><caption>Thông tin đặt hàng</caption>";
            sMsg += "<tr><th>STT</th> <th>Tên hàng</th><th>Size</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
            int i = 0;
            double tongTien = 0;
            foreach (var item in gioHang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.Size + "</td>";
                sMsg += "<td>" + item.SoLuong + "</td>";
                sMsg += "<td>" + item.DonGia.ToString("#,###") + "</td>";
                sMsg += "<td>" + item.ThanhTien.ToString("#,###") + "</td>";
                sMsg += "/<tr>";
                tongTien += item.ThanhTien;
            }
            sMsg += "<tr><th colspan='5'>Tổng cộng" + tongTien.ToString("#,###") + "</th></tr>";
            sMsg += "</table></body></html>";
            MailMessage mail = new MailMessage("nhatduong242001@gmail.com", Email, "Thông tin đặt hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nhatduong242001@gmail.com", "nhat2442001");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("TatCaSanPham", "SanPham");
        }
    }
}