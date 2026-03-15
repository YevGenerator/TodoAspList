using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TodoListApp.WebApp.Services;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;

    public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.configuration = configuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = this.httpContextAccessor.HttpContext?.User;

        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            var token = this.GenerateJwtToken(user);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private string GenerateJwtToken(ClaimsPrincipal user)
    {
        var secretKey = this.configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT Secret Key is missing.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty),
            new Claim(ClaimTypes.Name, user.Identity?.Name ?? string.Empty),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
