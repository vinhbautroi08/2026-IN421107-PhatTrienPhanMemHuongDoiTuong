# Phân tích vi phạm SOLID trong `ClubManager` (starter)

`ClubManager` là "God class": một class lo lưu dữ liệu, kiểm tra nghiệp vụ, ghi log, gửi thông báo và in báo cáo.
Bảng dưới liệt kê 7 vi phạm trong file `ClubManager.cs` gốc của starter.

| # | Vị trí (method) | Nguyên tắc | Vì sao là vi phạm | Hậu quả | Cách sửa trong bài |
|---|---|---|---|---|---|
| 1 | `CreateClub`: validate tên, lưu, ghi log, gửi thông báo trong một method | **SRP** | Method có nhiều lý do để thay đổi (luật tên, nơi lưu, định dạng log, kênh thông báo) | Sửa luật tên có thể làm hỏng phần gửi thông báo; khó test từng việc riêng | Luật tên chuyển vào `Club.Create`; `ClubService` chỉ điều phối; log và thông báo qua interface |
| 2 | `CreateClub`: chuỗi `if/else` kiểm tra `@gmail.com`, `@yahoo.com` | **OCP** | Thêm kênh hoặc nhà cung cấp mới phải sửa method đã chạy ổn | Dễ sinh lỗi hồi quy; method phình to dần | `INotificationService` + `NotificationServiceFactory` (Factory); thêm kênh = thêm class và một nhánh trong factory |
| 3 | `CreateClub`, `AddMemberToClub`, `CreateEvent`, `PrintClubReport`: gọi thẳng `File.AppendAllText` | **DIP** | Logic nghiệp vụ phụ thuộc vào chi tiết hạ tầng (file) thay vì abstraction | Không test được nếu không ghi file thật; muốn đổi sang console hay database phải sửa mọi method | `IAppLogger` với `ConsoleLogger`, `FileLogger`; nhận qua constructor |
| 4 | `AddMemberToClub`: `Console.WriteLine` cho email và Slack, luật tối đa 50 thành viên viết cứng | **SRP + OCP** | Vừa làm nghiệp vụ vừa gửi thông báo; thêm kênh mới phải sửa method; luật nghiệp vụ nằm ngoài Domain | Luật "50 thành viên" khó tái sử dụng và khó test | `Club.HasRoom` giữ luật; thông báo qua `INotificationService` |
| 5 | `CreateEvent`: tự gửi email cho từng thành viên | **SRP + OCP** | Tạo sự kiện biết luôn cách thông báo cho ai và bằng kênh nào | Thêm một cách thông báo (ghi log, SMS) phải sửa `CreateEvent` | Observer: `IEventObserver`, `EmailNotifier`, `LogNotifier`; `EventService.Subscribe` |
| 6 | `PrintClubReport`: trộn thống kê và in ra console | **SRP + OCP** | Gộp việc tính số liệu và việc định dạng; muốn xuất CSV phải sửa method | Không dùng lại số liệu cho định dạng khác | `ReportService.Generate(IReportStrategy)` với `ConsoleReportStrategy`, `CsvReportStrategy` (Strategy) |
| 7 | Trường `_clubs`, `_members` là `List` giữ trực tiếp, `GetAllClubs()` trả về chính danh sách | **DIP + ISP** (và đóng gói) | Phụ thuộc vào kiểu lưu cụ thể; bên ngoài sửa được danh sách bên trong | Khó đổi sang database; dữ liệu có thể bị sửa ngoài ý muốn | `IClubRepository`, `IMemberRepository`, `IEventRepository` (interface nhỏ) và bản `InMemory*` |

## Sau khi sửa

| Nguyên tắc | Cách đạt được |
|---|---|
| SRP | Mỗi class một việc: `ClubService`, `EventService`, `ReportService`, `IAppLogger`, repository |
| OCP | Thêm kênh thông báo, định dạng báo cáo hay observer mới mà không sửa code cũ |
| LSP | Mọi `INotificationService`, `IReportStrategy`, `IEventObserver` thay thế nhau được |
| ISP | Interface nhỏ, mỗi repository chỉ khai báo việc liên quan |
| DIP | Service phụ thuộc interface, nhận qua constructor; `Program.cs` là nơi duy nhất `new` class cụ thể |

## Bốn Design Pattern đã áp dụng

| Pattern | Thành phần | Giải quyết vấn đề |
|---|---|---|
| Factory | `INotificationServiceFactory`, `NotificationServiceFactory` | Thay chuỗi `if/else` chọn kênh thông báo |
| Decorator | `LoggingClubRepository` bọc `IClubRepository` | Thêm log truy cập mà không sửa `InMemoryClubRepository` |
| Strategy | `IReportStrategy`, `ConsoleReportStrategy`, `CsvReportStrategy` | Đổi định dạng báo cáo bằng cách đổi strategy |
| Observer | `IEventObserver`, `EmailNotifier`, `LogNotifier`, `EventService.Subscribe` | Tạo sự kiện xong báo cho nhiều bên mà `EventService` không biết họ làm gì |
