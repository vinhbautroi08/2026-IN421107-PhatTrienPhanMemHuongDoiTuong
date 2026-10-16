namespace EduHub.Ch01.Domain;

public enum MemberStatus
{
    Active,
    Inactive
}

public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public static class CheckInMethod
{
    public const string Manual = "Manual";
    public const string QR = "QR";
}
