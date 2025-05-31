namespace aspnet_short_url.Utilities;

using Visus.Cuid;

public class UtilityFunctions
{
  public static string GenerateId(int length = 10)
  {
    return new Cuid2(length).ToString();
  }
}
