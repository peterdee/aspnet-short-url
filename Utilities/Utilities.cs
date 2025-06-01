namespace aspnet_short_url.Utilities
{
  using Visus.Cuid;

  public class UtilityFunctions
  {
    public static string GenerateId(int length = 10)
    {
      return new Cuid2(length).ToString();
    }

    public static bool IsValidUrl(string url)
    {
      var partials = url.Split("://");
      if (partials.Length < 2)
      {
        return false;
      }
      if (!(partials[0] == "http" || partials[0] == "https"))
      {
        return false;
      }
      return Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }
  }
}
