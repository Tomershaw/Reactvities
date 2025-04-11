namespace Application.Interfaces
{
    // Interface that provides access to the username of the currently authenticated user.
    // This is typically used in application logic where user identity is needed.

    public interface IUserAccessor
    {
        // Retrieves the username of the currently logged-in user.
        string GetUsername();
    }
}
