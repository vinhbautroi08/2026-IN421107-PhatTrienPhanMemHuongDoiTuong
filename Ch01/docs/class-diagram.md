# Sơ đồ lớp — EduHub (Ch01)

**Tác giả:** <Phan Thế Vinh> — 0024418356

## 1. Sơ đồ

```mermaid
classDiagram
    class Member {
        +Guid Id
        +string Name
        +string Email
        +string StudentCode
        +DateTime JoinedAt
        +MemberStatus Status
        +Create(name, email, studentCode)$ Member
        +IsActive() bool
        +Deactivate() void
    }

    class Club {
        +Guid Id
        +string Name
        +string Description
        +DateTime CreatedAt
        +IReadOnlyList~Member~ Members
        +Create(name, description)$ Club
        +AddMember(member) void
        +MemberCount() int
    }

    class Event {
        +Guid Id
        +Guid ClubId FK
        +string Title
        +DateTime Date
        +int Capacity
        +bool IsOnline
        +Create(clubId, title, date, capacity, isOnline)$ Event
        +HasCapacity(registeredCount) bool
    }

    class Registration {
        +Guid Id
        +Guid MemberId FK
        +Guid EventId FK
        +DateTime RegisteredAt
        +RegistrationStatus Status
        +Create(memberId, eventId)$ Registration
        +Confirm() void
        +Cancel() void
    }

    class CheckIn {
        +Guid Id
        +Guid RegistrationId FK
        +DateTime CheckedInAt
        +string Method
        +Create(registration, method)$ CheckIn
        +Summary() string
    }

    class MemberStatus {
        <<enumeration>>
        Active
        Inactive
    }

    class RegistrationStatus {
        <<enumeration>>
        Pending
        Confirmed
        Cancelled
    }

    Club "*" -- "*" Member : có thành viên
    Club "1" --> "*" Event : tổ chức
    Member "1" --> "*" Registration : đăng ký
    Event "1" --> "*" Registration : nhận đăng ký
    Registration "1" --> "0..1" CheckIn : được điểm danh
    Member ..> MemberStatus
    Registration ..> RegistrationStatus
```

## 2. Bảng quan hệ và khoá ngoại

| Quan hệ | Bội số | Khoá ngoại | Ghi chú |
|---|---|---|---|
| Club — Member | * — * | bảng trung gian `ClubMember` (làm ở Ch04) | Ch01: `Club` giữ danh sách `Members` |
| Club — Event | 1 — * | `Event.ClubId` | Một CLB tổ chức nhiều sự kiện |
| Member — Registration | 1 — * | `Registration.MemberId` | Một thành viên có nhiều phiếu |
| Event — Registration | 1 — * | `Registration.EventId` | Một sự kiện nhận nhiều phiếu |
| Registration — CheckIn | 1 — 0..1 | `CheckIn.RegistrationId` | Có thể chưa check-in |

## 3. Giải thích thiết kế

- Phía "nhiều" giữ khoá ngoại (FK), phía "một" chỉ là nơi được tham chiếu tới.
- Khoá chính của mọi entity là `Id` kiểu `Guid`.
- Tiêu đề (`Title`) và sức chứa (`Capacity`) là thuộc tính của `Event`, không phải entity riêng vì chúng không có hành vi và không tồn tại độc lập.
- Trạng thái dùng enum (`MemberStatus`, `RegistrationStatus`) thay vì chuỗi để tránh gõ sai.
- Vòng đời `Registration`: Pending → Confirmed → (CheckIn); Cancel() được phép từ Pending hoặc Confirmed.
- Ký hiệu `$` sau tên method là method `static` (factory `Create`).
