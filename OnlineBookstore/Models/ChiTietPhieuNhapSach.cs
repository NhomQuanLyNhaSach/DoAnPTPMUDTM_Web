//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineBookstore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietPhieuNhapSach
    {
        public int Id_PNS { get; set; }
        public int Id_Sach { get; set; }
        public Nullable<int> SoLuongNhap { get; set; }
        public Nullable<int> GiaNhap { get; set; }
        public Nullable<int> ThanhTien { get; set; }
    
        public virtual PhieuNhapSach PhieuNhapSach { get; set; }
        public virtual Sach Sach { get; set; }
    }
}