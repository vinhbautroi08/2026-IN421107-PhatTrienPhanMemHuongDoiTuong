# User Stories — EduHub (Ch01)

**Hệ thống:** EduHub — quản lý câu lạc bộ (CLB) và sự kiện sinh viên
**Tác giả:** <Phan Thế Vinh> — 0024418356

## Các vai trò (actor)

| Vai trò | Mô tả |
|---|---|
| Sinh viên | Người tham gia CLB, đăng ký và dự sự kiện |
| Ban CLB | Người tổ chức sự kiện, xử lý phiếu đăng ký |
| Admin | Người quản trị thành viên và CLB của hệ thống |

---

## Sinh viên

### US-01 · Huỷ đăng ký sự kiện

**Là** sinh viên, **tôi muốn** huỷ phiếu đăng ký một sự kiện, **để** nhường chỗ cho bạn khác khi tôi không thể đến.

**Tiêu chí nghiệm thu**
1. **Given** phiếu đăng ký của tôi đang Pending hoặc Confirmed, **When** tôi huỷ, **Then** phiếu chuyển sang Cancelled.
2. **Given** phiếu đã Cancelled, **When** tôi huỷ lần nữa, **Then** hệ thống từ chối và báo lỗi.

### US-02 · Xem tóm tắt lượt điểm danh của mình

**Là** sinh viên, **tôi muốn** xem tóm tắt lượt check-in của mình, **để** biết mình đã được ghi nhận tham dự lúc nào và bằng cách nào.

**Tiêu chí nghiệm thu**
1. **Given** tôi đã check-in thành công, **When** tôi xem tóm tắt, **Then** thấy thời điểm check-in và hình thức (Manual hoặc QR).
2. **Given** phiếu của tôi chưa được xác nhận, **When** tôi cố check-in, **Then** hệ thống từ chối nên không có tóm tắt nào để xem.

---

## Ban CLB

### US-03 · Xác nhận phiếu đăng ký

**Là** ban CLB, **tôi muốn** xác nhận phiếu đăng ký của sinh viên, **để** chỉ những người được duyệt mới được check-in.

**Tiêu chí nghiệm thu**
1. **Given** phiếu đang Pending, **When** tôi xác nhận, **Then** phiếu chuyển sang Confirmed.
2. **Given** phiếu đã Cancelled, **When** tôi xác nhận, **Then** hệ thống từ chối và báo lỗi (phiếu đã huỷ không sống lại).

### US-04 · Kiểm tra sự kiện còn chỗ

**Là** ban CLB, **tôi muốn** biết sự kiện còn chỗ hay đã đủ, **để** quyết định có nhận thêm đăng ký hay không.

**Tiêu chí nghiệm thu**
1. **Given** sức chứa 50 và đã có 49 phiếu, **When** tôi kiểm tra, **Then** kết quả là còn chỗ.
2. **Given** sức chứa 50 và đã có 50 phiếu, **When** tôi kiểm tra, **Then** kết quả là hết chỗ.

---

## Admin

### US-05 · Tạo hồ sơ thành viên mới

**Là** admin, **tôi muốn** tạo hồ sơ thành viên với thông tin hợp lệ, **để** dữ liệu sinh viên trong hệ thống luôn sạch và đúng định dạng.

**Tiêu chí nghiệm thu**
1. **Given** tên, email có dấu `@` và mã số sinh viên đầy đủ, **When** tôi tạo, **Then** thành viên được tạo ở trạng thái Active, email được chuẩn hoá về chữ thường.
2. **Given** tên trống hoặc email không có `@`, **When** tôi tạo, **Then** hệ thống từ chối và báo lỗi, không có thành viên nào được tạo.

### US-06 · Tạo câu lạc bộ mới

**Là** admin, **tôi muốn** tạo một CLB mới có tên rõ ràng, **để** sinh viên có nơi tham gia và ban CLB có nơi tổ chức sự kiện.

**Tiêu chí nghiệm thu**
1. **Given** tên CLB hợp lệ, **When** tôi tạo, **Then** CLB được tạo với danh sách thành viên rỗng (MemberCount = 0).
2. **Given** tên CLB trống, **When** tôi tạo, **Then** hệ thống từ chối và báo lỗi.

---

## Liên hệ story với code (để dùng ở các bước sau)

| Story | Class / method sẽ hiện thực |
|---|---|
| US-01 | `Registration.Cancel()` |
| US-02 | `CheckIn.Create()`, `CheckIn.Summary()` |
| US-03 | `Registration.Confirm()` |
| US-04 | `Event.HasCapacity()` |
| US-05 | `Member.Create()` |
| US-06 | `Club.Create()`, `Club.MemberCount()` |
