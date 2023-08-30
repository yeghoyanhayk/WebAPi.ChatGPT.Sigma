namespace ChatGPTPractice;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User not found")
    {
    }
}

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException() : base("Email already exists")
    {
    }
}

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Password must have at least 8 characters and contain letters, numbers, and special characters")
    {
    }
}