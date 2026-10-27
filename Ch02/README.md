# EduHub Ch02: Refactoring với SOLID và Design Patterns

Bài tập Chương 02, môn **Phát triển phần mềm hướng đối tượng (IN4211)**.

- Họ tên: `Phan Thế Vinh`
- MSSV: `0024418356`
- Lớp: `ĐHCNTT24C-IT`

## Mục tiêu

Dự án xuất phát từ lớp `ClubManager` ("God class") vi phạm nhiều nguyên lý SOLID. Bài làm refactor lớp này thành nhiều class nhỏ, phụ thuộc nhau qua interface, và áp dụng 4 Design Pattern: **Factory, Decorator, Strategy, Observer**. Phần Domain có unit test viết theo mẫu Arrange-Act-Assert.

## Yêu cầu môi trường

- [.NET SDK 10](https://dotnet.microsoft.com/download) (`net10.0`)
- Git

Kiểm tra phiên bản:

```bash
dotnet --version
```

## Cách chạy

```bash
git clone https://github.com/vinhbautroi08/2026-IN421107-PhatTrienPhanMemHuongDoiTuong.git
cd 2026-IN421107-PhatTrienPhanMemHuongDoiTuong
git switch feature/solid-refactor
cd Ch02

dotnet build Ch02.Refactor.sln       # biên dịch
dotnet run --project Ch02.Refactor   # chạy chương trình demo
dotnet test Ch02.Refactor.sln        # chạy unit test
```

Khi chạy `dotnet run`, chương trình in ra các khối kết quả: tạo CLB và thêm thành viên, Observer báo khi tạo sự kiện, báo cáo theo hai định dạng (Console, CSV), các kênh thông báo từ Factory, và kiểm tra lỗi nghiệp vụ. Mã định danh (GUID) thay đổi mỗi lần chạy.

## Cấu trúc thư mục

```
Ch02/
├── Ch02.Refactor.sln
├── docs/
│   └── solid-analysis.md          # Nhiệm vụ 1: phân tích vi phạm SOLID
├── Ch02.Refactor/
│   ├── Program.cs                 # Composition root: nơi duy nhất `new` class cụ thể
│   ├── Domain/                    # Club, Member, Event (quy tắc nghiệp vụ)
│   ├── Logging/                   # IAppLogger, ConsoleLogger, FileLogger
│   ├── Repositories/              # I*Repository, InMemory*Repository, LoggingClubRepository
│   ├── Notifications/             # INotificationService, Email/Sms/Slack, Factory
│   ├── Services/                  # ClubService, EventService, ReportService
│   └── Patterns/
│       ├── Strategies/            # IReportStrategy, Console/Csv, ClubStat
│       └── Observers/             # IEventObserver, LogNotifier, EmailNotifier
└── Ch02.Refactor.Tests/
    └── DomainTests.cs             # Unit test (xUnit, mẫu AAA)
```

## SOLID áp dụng như thế nào

| Nguyên lý | Vi phạm trong `ClubManager` | Cách sửa |
|---|---|---|
| **S**: Single Responsibility | `CreateClub` vừa validate, lưu, ghi log, gửi thông báo | Tách `ClubService`, `EventService`, `ReportService`; luật kiểm tra tên đưa về `Club.Create` |
| **O**: Open/Closed | Chuỗi `if/else` chọn kênh gửi; báo cáo cứng một định dạng | `INotificationService` + Factory; `IReportStrategy` |
| **D**: Dependency Inversion | Gọi thẳng `File.AppendAllText`, `Console.WriteLine`, dùng danh sách trong class | Phụ thuộc vào `IAppLogger`, `INotificationService`, `I*Repository`, nhận qua constructor |
| **I**: Interface Segregation | Một class ôm cả dữ liệu, thông báo và báo cáo | Interface nhỏ theo từng việc (`IClubRepository`, `IMemberRepository`, `IEventRepository`...) |

Phân tích chi tiết kèm số dòng nằm trong [`docs/solid-analysis.md`](docs/solid-analysis.md).

## Design Pattern đã áp dụng

| Pattern | Vị trí | Giải quyết vấn đề gì |
|---|---|---|
| **Factory** | `Notifications/NotificationServiceFactory.cs` | Tạo `INotificationService` theo `NotificationChannel`, thay chuỗi `if/else`. Thêm kênh mới chỉ cần thêm class và một nhánh |
| **Decorator** | `Repositories/LoggingClubRepository.cs` | Bọc `IClubRepository` để ghi log mỗi lần truy cập mà không sửa class gốc |
| **Strategy** | `Patterns/Strategies/` | Cùng một dữ liệu thống kê, đổi cách xuất báo cáo (Console, CSV) bằng cách đổi strategy |
| **Observer** | `Patterns/Observers/` + `EventService` | Khi tạo sự kiện, `EventService` báo cho các observer đã đăng ký; không cần biết họ làm gì |

## Unit test

Test nằm trong `Ch02.Refactor.Tests/DomainTests.cs`, dùng xUnit, mỗi test theo mẫu **Arrange - Act - Assert** và đặt tên `TênMethod_TìnhHuống_KếtQuảMongĐợi`. Các trường hợp được kiểm tra:

- `Club.Create` với tên hợp lệ, tên rỗng, tên dài quá 100 ký tự
- `Club.HasRoom` ở biên 49 và 50 thành viên
- `Event.Register` thành viên mới và đăng ký trùng
- `Event.CheckIn` khi thành viên chưa đăng ký (ném `InvalidOperationException`)

## Quy trình Git

- Nhánh làm bài: `feature/solid-refactor`
- Commit theo [Conventional Commits](https://www.conventionalcommits.org/) (`feat`, `fix`, `refactor`, `test`, `docs`)
- Rebase lên `main` trước khi mở Pull Request
- Pull Request có mô tả What / Why / How to test và các nhận xét review

## Ghi chú

Các file `bin/`, `obj/` và `club-log.txt` được bỏ qua bằng `.gitignore`, không đưa lên repository.
