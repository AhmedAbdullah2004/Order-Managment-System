public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);
}

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepo;
    private readonly TokenGenerator _tokenGenerator;

    public AuthService(IRepository<User> userRepo, TokenGenerator tokenGenerator)
    {
        _userRepo = userRepo;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var users = await _userRepo.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        return _tokenGenerator.GenerateToken(user);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return password == passwordHash;
    }
}

