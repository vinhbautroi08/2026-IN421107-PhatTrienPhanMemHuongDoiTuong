namespace EduHub.Ch01.Domain;

public class Registration
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }
    public Guid EventId { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public RegistrationStatus Status { get; private set; }

    private Registration(Guid memberId, Guid eventId)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = DateTime.UtcNow;
        Status = RegistrationStatus.Pending;
    }

    public static Registration Create(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Thiếu thành viên đăng ký.", nameof(memberId));
        if (eventId == Guid.Empty)
            throw new ArgumentException("Thiếu sự kiện đăng ký.", nameof(eventId));

        return new Registration(memberId, eventId);
    }

    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException(
                $"Chỉ xác nhận được phiếu đang Pending (hiện tại: {Status}).");

        Status = RegistrationStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đăng ký đã bị huỷ trước đó.");

        Status = RegistrationStatus.Cancelled;
    }
}
