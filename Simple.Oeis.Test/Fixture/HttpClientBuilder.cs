namespace Simple.Oeis.Test.Fixture;

/// <summary>
/// HttpClient with desired response
/// </summary>
public sealed class HttpClientBuilder : HttpMessageHandler {
  private Func<HttpRequestMessage, HttpResponseMessage> HttpResponse { get; }

  /// <summary>
  /// Creates HttpClient with desired response
  /// </summary>
  /// <param name="httpResponse">Http Response function</param>
  /// <returns>HttpClient with desired response</returns>
  public static HttpClient Create(Func<HttpRequestMessage, HttpResponseMessage> httpResponse) =>
      new(new HttpClientBuilder(httpResponse), false);

  protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
      Task.FromResult(HttpResponse(request));

  private HttpClientBuilder(Func<HttpRequestMessage, HttpResponseMessage> httpResponse) {
    HttpResponse = httpResponse;
  }
}