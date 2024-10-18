using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ApiKeyController : ControllerBase
{
    private readonly ApiKeyService _apiKeyService;

    public ApiKeyController(ApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateApiKey([FromBody] ApiKeyRequest request)
    {
        if (!IsValidEmail(request.Email))
        {
            return BadRequest(new { message = "Invalid email format" });
        }

        string apiKey = await _apiKeyService.GenerateOrUpdateApiKeyAsync(request.Email);
        return Ok(new { ApiKey = apiKey });
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    // Create a request model to accept email
    public class ApiKeyRequest
    {
        public required string Email { get; set; }
    }
}