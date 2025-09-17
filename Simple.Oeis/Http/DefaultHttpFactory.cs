using System.Net;

namespace Simple.Oeis.Http;

internal sealed class DefaultHttpFactory : IHttpClientFactory
{
    private static readonly Lazy<HttpClient> HttpClientBuilder = new(() =>
    {
        var handler = new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            Credentials = CredentialCache.DefaultCredentials
        };

        return new HttpClient(handler, false);
    });

    private HttpClient? Client { get; }

    public DefaultHttpFactory() { }

    public DefaultHttpFactory(HttpClient? client) : this()
    {
        Client = client;
    }

    public HttpClient CreateClient(string name) => Client ?? HttpClientBuilder.Value;
}