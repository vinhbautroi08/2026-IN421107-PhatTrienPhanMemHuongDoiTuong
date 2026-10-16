namespace EduHub.Ch01.Domain;

public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid RegistrationId { get; private set; }
    public DateTime CheckedInAt { get; private set; }
    public string Method { get; private set; }

    private CheckIn(Guid registrationId, string method)
    {
        Id = Guid.NewGuid();
        RegistrationId = registrationId;
        CheckedInAt = DateTime.UtcNow;
        Method = method;
    }

    public static CheckIn Create(Registration registration, string method = CheckInMethod.Manual)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException(
                $"Chỉ phiếu Confirmed mới được check-in (hiện tại: {registration.Status}).");
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Hình thức check-in không được để trống.", nameof(method));

        return new CheckIn(registration.Id, method);
    }

    public string Summary() =>
        $"Check-in lúc {CheckedInAt.ToLocalTime():HH:mm dd/MM/yyyy} bằng {Method}";
}
