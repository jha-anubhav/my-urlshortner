namespace MyUrlShortner
{
public interface IUrlShortner
{
    string ShortenUrl(string originalUrl);
    string GetOriginalUrl(string shortenedUrl);
    
    bool IsValidUrl(string url);
}
}