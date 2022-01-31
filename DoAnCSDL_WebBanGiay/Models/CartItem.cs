using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnCSDL_WebBanGiay.Models
{
    public class CartItem
    {
        public int MaSP { get; set; }
        public string Size { get; set; }
        public int MaSizeSp { get; set; }
        public string TenSP { get; set; }
        public string HinhSP { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double ThanhTien
        {
            get
            {
                return SoLuong * DonGia;
            }
        }
    }
}