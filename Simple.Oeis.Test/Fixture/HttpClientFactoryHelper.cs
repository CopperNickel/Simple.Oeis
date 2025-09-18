namespace Simple.Oeis.Test.Fixture;

public sealed class HttpClientFactoryHelper : IHttpClientFactory {
  private Func<HttpRequestMessage, HttpResponseMessage> HttpResponse { get; }

  public HttpClientFactoryHelper(Func<HttpRequestMessage, HttpResponseMessage> httpResponse) {
    ArgumentNullException.ThrowIfNull(httpResponse);

    HttpResponse = httpResponse;
  }

  public HttpClient CreateClient(string name) => string.Equals(IOeisQuery.HttpClientName, name, StringComparison.InvariantCulture)
      ? HttpClientBuilder.Create(HttpResponse)
      : throw new ArgumentException($"Invalid client name {name} when {IOeisQuery.HttpClientName} expected.", nameof(name));
}

