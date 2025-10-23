using System.Net;

namespace Simple.Oeis.Http;

internal sealed class DefaultHttpFactory : IHttpClientFactory {
  private static readonly Lazy<HttpClientHandler> HttpClientHandlerBuilder = new(() => new() {
    CookieContainer = new CookieContainer(),
    Credentials = CredentialCache.DefaultCredentials
  });

  private HttpClient? Client { get; }

  public DefaultHttpFactory() { }

  public DefaultHttpFactory(HttpClient? client) : this() {
    Client = client;
  }

  public HttpClient CreateClient(string name) => Client ?? new HttpClient(HttpClientHandlerBuilder.Value, false);
}