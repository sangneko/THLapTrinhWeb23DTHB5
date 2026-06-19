using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebBanHang.Models
{
    public class Order
    {
        public int Id { get; set; } // [cite: 283]

        public string UserId { get; set; } // Đổi lại UserId để khớp với tài liệu trang 8, 10 [cite: 284, 348]

        public DateTime OrderDate { get; set; } // [cite: 284]

        public decimal TotalPrice { get; set; } // Đổi từ TotalAmount thành TotalPrice giống tài liệu [cite: 285, 350]

        public string ShippingAddress { get; set; } // [cite: 290]

        public string Notes { get; set; } // Thêm trường ghi chú khách hàng nhập từ form [cite: 291, 318]

        public string OrderStatus { get; set; } = "Pending"; // Giữ nguyên thuộc tính trạng thái đơn hàng của bạn

        [ForeignKey("UserId")] // Khai báo khóa ngoại liên kết với User (Theo tài liệu trang 8) [cite: 292]
        [ValidateNever] // Bỏ qua kiểm tra Validate dữ liệu Identity khi gửi Form (Theo tài liệu trang 9) [cite: 295]
        public ApplicationUser ApplicationUser { get; set; } // [cite: 296]

        public List<OrderDetail> OrderDetails { get; set; } // [cite: 296]
    }
}