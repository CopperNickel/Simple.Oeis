using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Simple.Oeis.Test.Fixture;

public sealed partial class HttpFixtureFactory {
  private static readonly Dictionary<string, string> KnownArticles = new() {
      { "A000045", "FibonacciArticle" }
  };

  private static readonly Dictionary<string, string> KnownSequences = new() {
      { "1,1,2,3,5,8", "Fibonacci" }
  };

  public static IHttpClientFactory Factory { get; } = new HttpClientFactoryHelper(Response);

  public static HttpClient Client() => Factory.CreateClient(IOeisQuery.HttpClientName);

  private static HttpResponseMessage Response(HttpRequestMessage request) {
    var uri = request.RequestUri;

    if (uri is null ||
        !uri.Scheme.Equals("https", StringComparison.InvariantCulture) ||
        !uri.Authority.Equals("oeis.org", StringComparison.InvariantCulture)) 
      return NotFound();
    
    var query = HttpUtility.ParseQueryString(uri.Query);

    if (uri.LocalPath.Equals("/search", StringComparison.InvariantCulture)) {
      if (query.Count == 2 && query["fmt"] == "json") {
        return new(HttpStatusCode.OK) {
          Content = new StringContent(JsonText(query["q"]))
        };
      }
    }
    else if (ArticleNameRegex().IsMatch(uri.LocalPath)) {
      if (query.Count == 1 && query["fmt"] == "json") {
        return new(HttpStatusCode.OK) {
          Content = new StringContent(JsonTextArticle(uri.LocalPath.TrimStart('/')))
        };
      }
    }

    return NotFound();
  }

  private static string JsonText(string? query) => KnownSequences.TryGetValue(query ?? "", out var resourceName)
      ? JsonFromResource(resourceName)
      : "null";

  private static string JsonTextArticle(string? query) => KnownArticles.TryGetValue(query ?? "", out var resourceName)
      ? JsonFromResource(resourceName)
      : "null";

  private static string JsonFromResource(string name) {
    using var stream = Assembly
        .GetExecutingAssembly()
        .GetManifestResourceStream($"Simple.Oeis.Test.Responses.{name}.json");

    using var reader = new StreamReader(stream!);

    return reader.ReadToEnd();
  }

  private static HttpResponseMessage NotFound() {
    return new HttpResponseMessage(HttpStatusCode.NotFound);
  }

  [GeneratedRegex("^/A[0-9]{6}$", RegexOptions.IgnoreCase)]
  private static partial Regex ArticleNameRegex();
}

