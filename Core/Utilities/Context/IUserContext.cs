namespace Core.Utilities.Context
{
    public interface IUserContext
    {
        string? UserId { get; } 
        bool IsAuthenticated { get; }
    }
}
