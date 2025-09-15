using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Models;
using DBContext;
using Interface;
public class urlShortnerService : IUrlShortner
{
    private readonly UrlShortenerDbContext _context;
    private readonly string _baseUrl;
    private readonly string _shortingKey;

    public urlShortnerService(UrlShortenerDbContext context, IOptions<UrlSetting> options)
    {
        _context = context;
        _baseUrl = options.Value.BaseUrl;
        _shortingKey = options.Value.ShortingKey;
    }

    public string ShortenUrl(string originalUrl)
    {
        if(originalUrl == null)
            throw new ArgumentNullException(nameof(originalUrl));
        
        bool proceed = IsValidUrl(originalUrl);
        if(!proceed)
            throw new Exception("Invalid URL");
        
        long id = _context.UrlShort.Count() + 1; // Simple auto-increment logic
        string shortCode = GenerateShortCode(id);
        string shortenedUrl = $"{_baseUrl}{shortCode}";
        return shortenedUrl;    
        // Implement URL shortening logic here
    }
    public string GetOriginalUrl(string shortenedUrl)
    {
        if (shortenedUrl == null)
            throw new ArgumentNullException(nameof(shortenedUrl));
        string originalUrl = _context.UrlShort
            .Where(u => u.ShortenedUrl == shortenedUrl)
            .Select(u => u.OriginalUrl)
            .FirstOrDefault() ?? throw new Exception("URL not found");
        return originalUrl;
        // Implement logic to retrieve the original URL from the shortened URL
    }

    public bool IsValidUrl(string url)
    {
        // Implement URL validation logic here
        if(string.IsNullOrEmpty(url))
            return false;
        bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                       && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return isValid;
    }
    public string GenerateShortCode(long id)
    {
        var shortCode = new StringBuilder();
        var keyLength = _shortingKey.Length;

        while (id > 0)
        {
            shortCode.Insert(0, _shortingKey[(int)(id % keyLength)]);
            id /= keyLength;
        }

        return shortCode.ToString();
    }
}