[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    var token = await _authService.LoginAsync(request.Username, request.Password);
    var user = (await _authService.GetUserAsync(request.Username));

    return Ok(new
    {
        Token = token,
        Role = user.Role
    });
}
