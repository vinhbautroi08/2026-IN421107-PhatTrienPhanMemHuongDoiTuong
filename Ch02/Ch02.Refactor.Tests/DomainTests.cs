using EduHub.Ch02;
using EduHub.Ch02.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange (chuẩn bị) - Act (thực hiện) - Assert (kiểm tra).
// Mỗi test chỉ kiểm tra MỘT hành vi. Đặt tên: TênMethod_TìnhHuống_KếtQuảMongĐợi.
public class DomainTests
{
    // Test mẫu đã viết sẵn: chạy được ngay trên ClubManager hiện tại.
    [Fact]
    public void CreateClub_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
    var name = "";

    // Act
    var act = () => Club.Create(name, "mô tả");

    // Assert
    Assert.Throws<ArgumentException>(act);
    }

    // TODO (Nhiệm vụ 4): bỏ Skip và viết thân test cho từng trường hợp dưới đây.
    // Cần ≥ 6 test AAA cho lớp Domain (Club, Event), trong đó có cả trường hợp lỗi.

    [Fact] public void Club_Create_WithValidName_ReturnsClub()
    {
        var club = Club.Create("Tech Club", "mô tả");
        Assert.Equal("Tech Club", club.Name);
        Assert.NotEqual(Guid.Empty, club.Id);
    }

    [Fact] public void Club_Create_WithInvalidName_ThrowsArgumentException()
    {
        var tooLong = new string('a', Club.MaxNameLength + 1);
        Assert.Throws<ArgumentException>(() => Club.Create(tooLong, "x"));
    }

    [Fact] public void Club_HasRoom_AtMaxMembers_ReturnsFalse()
    {
        var club = Club.Create("Tech Club", "x");
        Assert.True(club.HasRoom(49));
        Assert.False(club.HasRoom(50));      // biên
    }

    [Fact] public void Event_Register_NewMember_ReturnsTrue()
    {
        var ev = new Event { Title = "Workshop" };
        Assert.True(ev.Register(Guid.NewGuid()));
    }

    [Fact] public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        var ev = new Event { Title = "Workshop" }; var id = Guid.NewGuid();
        ev.Register(id);
        Assert.False(ev.Register(id));
    }

    [Fact] public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        var ev = new Event { Title = "Workshop" };
        Assert.Throws<InvalidOperationException>(() => ev.CheckIn(Guid.NewGuid()));
    }
}
