namespace Application.Interfaces
{
    // Interface that provides access to user identity information.
    // This interface is implemented by classes that retrieve details about the currently authenticated user.
    // It is typically used in application logic where the username of the logged-in user is required.

    public interface IUserAccessor
    {
        // Retrieves the username of the currently logged-in user.
        // This method is used to identify the user making a request in scenarios such as auditing or authorization.
        // Returns:
        //   - A string representing the username of the authenticated user.
        string GetUsername();
    }
}
