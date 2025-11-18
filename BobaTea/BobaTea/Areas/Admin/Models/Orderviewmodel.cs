using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BobaTea.Areas.Admin.Models
{
    public class OrderViewModel
    {
        public string MaDon { get; set; }
        public string KhachHang { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
    }
}