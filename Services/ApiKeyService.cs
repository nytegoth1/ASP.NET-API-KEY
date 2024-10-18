using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Make sure to include this if you're using EF Core

public class ApiKeyService
{
    private readonly ApiKeyContext _context;

    public ApiKeyService(ApiKeyContext context)
    {
        _context = context;
    }

    public string GenerateApiKey()
    {
        byte[] randomBytes = new byte[32];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes).Replace("+", "").Replace("/", "").Replace("=", "");
    }

    public async Task<string> GenerateOrUpdateApiKeyAsync(string email)
    {
        // Check if the API key exists for the email
        var existingApiKey = await _context.ApiKeys.SingleOrDefaultAsync(a => a.Email == email);
        string newApiKey = GenerateApiKey(); // Generate a new API key

        if (existingApiKey != null)
        {
            // User exists, overwrite the API key
            existingApiKey.Key = newApiKey;
            _context.ApiKeys.Update(existingApiKey);
        }
        else
        {
            // User does not exist, create a new entry
            existingApiKey = new ApiKey { Key = newApiKey, Email = email };
            await _context.ApiKeys.AddAsync(existingApiKey);
        }

        await _context.SaveChangesAsync();
        return newApiKey; // Return the new API key
    }

    public async Task StoreApiKeyAsync(string apiKey, string email)
    {
        var newApiKey = new ApiKey { Key = apiKey, Email = email }; // Include the email
        _context.ApiKeys.Add(newApiKey);
        await _context.SaveChangesAsync();
    }
}



