using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTL_asp_quanaobaochau.Models
{
    public class GioHang
    {
        public int Id { get; set; }
        public string ten { get; set; }
        public int soluong { get; set; }
        public int gia { get; set; }

        public GioHang() { }
    }
}