using System.Text;
using EduHub.Ch01.Domain;

Console.OutputEncoding = Encoding.UTF8;

void Section(string title) => Console.WriteLine($"\n=== {title} ===");

// Chạy một thao tác dự kiến sẽ bị từ chối, in thông báo lỗi thay vì làm chương trình sập
void ExpectRejected(string label, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[!] {label}: KHÔNG bị từ chối (cần xem lại luật nghiệp vụ)");
    }
    catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
    {
        Console.WriteLine($"[Từ chối] {label}: {ex.Message}");
    }
}

// ---------- 1. Tạo câu lạc bộ ----------
Section("1. Tạo câu lạc bộ");
var club = Club.Create("CLB Lập trình", "Sinh hoạt học thuật về lập trình");
Console.WriteLine($"Đã tạo: {club.Name} (số thành viên: {club.MemberCount()})");

// ---------- 2. Thêm thành viên ----------
Section("2. Thêm thành viên");
var an   = Member.Create("Nguyễn Văn An", "an@example.com", "SV001");
var binh = Member.Create("Trần Thị Bình", "binh@example.com", "SV002");
var chi  = Member.Create("Lê Minh Chi", "chi@example.com", "SV003");
var dung = Member.Create("Phạm Quốc Dũng", "dung@example.com", "SV004");

var members = new List<Member> { an, binh, chi, dung };
foreach (var m in members)
{
    club.AddMember(m);
    Console.WriteLine($"+ {m.Name} ({m.StudentCode})");
}
Console.WriteLine($"Tổng số thành viên: {club.MemberCount()}");

var nameById = members.ToDictionary(m => m.Id, m => m.Name);

// ---------- 3. Tạo sự kiện (sức chứa nhỏ để demo hết chỗ) ----------
Section("3. Tạo sự kiện");
var ev = Event.Create(club.Id, "Workshop Git cơ bản", DateTime.Now.AddDays(7), capacity: 3);
Console.WriteLine($"Sự kiện: {ev.Title} | Sức chứa: {ev.Capacity} | Online: {ev.IsOnline}");

// ---------- 4. Đăng ký (Pending) ----------
Section("4. Đăng ký sự kiện");
var registrations = new List<Registration>();

foreach (var m in members)
{
    int active = registrations.Count(r => r.Status != RegistrationStatus.Cancelled);

    if (!ev.HasCapacity(active))
    {
        Console.WriteLine($"- {m.Name}: HẾT CHỖ ({active}/{ev.Capacity}), không đăng ký được");
        continue;
    }

    var reg = Registration.Create(m.Id, ev.Id);
    registrations.Add(reg);
    Console.WriteLine($"+ {m.Name}: đã đăng ký, trạng thái {reg.Status}");
}

// ---------- 5. Xác nhận (Confirmed) ----------
Section("5. Ban CLB xác nhận phiếu");
var regAn   = registrations.First(r => r.MemberId == an.Id);
var regBinh = registrations.First(r => r.MemberId == binh.Id);
var regChi  = registrations.First(r => r.MemberId == chi.Id);

regAn.Confirm();
regBinh.Confirm();
// Cố ý không xác nhận Chi để demo "chưa được duyệt"
Console.WriteLine($"{an.Name}: {regAn.Status}");
Console.WriteLine($"{binh.Name}: {regBinh.Status}");
Console.WriteLine($"{chi.Name}: {regChi.Status} (chưa được xác nhận)");

// ---------- 6. Check-in (chỉ người Confirmed) ----------
Section("6. Check-in tại sự kiện");
var checkIns = new List<CheckIn>();

var ciAn = CheckIn.Create(regAn, CheckInMethod.QR);
var ciBinh = CheckIn.Create(regBinh); // mặc định Manual
checkIns.Add(ciAn);
checkIns.Add(ciBinh);

Console.WriteLine($"{an.Name}: {ciAn.Summary()}");
Console.WriteLine($"{binh.Name}: {ciBinh.Summary()}");

// ---------- 7. Báo cáo tham dự ----------
Section("7. Báo cáo tham dự");
int registered = registrations.Count(r => r.Status != RegistrationStatus.Cancelled);
Console.WriteLine($"Sự kiện: {ev.Title}");
Console.WriteLine($"Đăng ký {registered}/{ev.Capacity}, đã check-in {checkIns.Count}");

foreach (var r in registrations)
{
    bool checkedIn = checkIns.Any(c => c.RegistrationId == r.Id);
    string state = checkedIn ? "đã check-in" : "chưa check-in";
    Console.WriteLine($"  - {nameById[r.MemberId],-16} | {r.Status,-9} | {state}");
}

// ---------- 8. Thử vi phạm luật nghiệp vụ ----------
Section("8. Thử vi phạm luật (mong đợi bị từ chối)");

ExpectRejected("Check-in phiếu chưa xác nhận (Chi)",
    () => CheckIn.Create(regChi));

regChi.Cancel();
Console.WriteLine($"Đã huỷ phiếu của {chi.Name}: {regChi.Status}");

ExpectRejected("Huỷ phiếu đã huỷ (Chi)",
    () => regChi.Cancel());

ExpectRejected("Xác nhận phiếu đã huỷ (Chi)",
    () => regChi.Confirm());

ExpectRejected("Thêm trùng thành viên vào CLB (An)",
    () => club.AddMember(an));

ExpectRejected("Tạo thành viên với email sai",
    () => Member.Create("Hoàng Văn E", "email-sai", "SV005"));

Console.WriteLine("\nHoàn tất demo.");
