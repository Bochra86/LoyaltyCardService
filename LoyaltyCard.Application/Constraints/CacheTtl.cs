
namespace LoyaltyCard.Application.Constraints;

public static class CacheTtl
{
    public static readonly TimeSpan Short = TimeSpan.FromMinutes(1);
    public static readonly TimeSpan Meduim = TimeSpan.FromMinutes(10);
    public static readonly TimeSpan Long = TimeSpan.FromMinutes(60);
}
