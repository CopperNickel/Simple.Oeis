using Simple.Oeis.Http;
using System.Globalization;
using System.Numerics;
using System.Text.Json;

namespace Simple.Oeis;

/// <summary>
/// OEIS (Online Encyclopedia of Integer Sequences) Query
/// </summary>
public sealed class OeisQuery : IOeisQuery {
  #region Properties

  /// <summary>
  /// Http Client Factory to create Http Client when it's required
  /// </summary>
  public IHttpClientFactory Factory { get; }

  #endregion Properties

  #region Create

  /// <summary>
  /// Create OEIS Query with http client factory
  /// </summary>
  /// <param name="factory">Factory to create http clients, static client as a default</param>
  public OeisQuery(IHttpClientFactory? factory = default) {
    Factory = factory ?? new DefaultHttpFactory();
  }

  /// <summary>
  /// Create OEIS Query with given http client
  /// </summary>
  /// <param name="client">Http client; static client as a default</param>
  public OeisQuery(HttpClient? client) {
    Factory = new DefaultHttpFactory(client);
  }

  #endregion Create

  #region IOeisQuery

  /// <inheritdoc cref="T:IOeisQuery.QuerySequences{T}(IEnumerable{T},CancellationToken)" />
  public Task<IReadOnlyList<IOeisSequence>> QuerySequences<T>(
      IEnumerable<T> sequence,
      CancellationToken token)
      where T : IBinaryInteger<T> {
    ArgumentNullException.ThrowIfNull(sequence);

    var query = string.Join(",", sequence
        .Select(item => item.ToString(null, CultureInfo.InvariantCulture)));

    return CoreQuerySequence(query, token);
  }

  /// <inheritdoc cref="IOeisQuery.QuerySequence" />
  public Task<IOeisSequence> QuerySequence(int number, CancellationToken token = default) {
    if (number <= 0 || number >= 10_000_000) {
      return Task.FromResult(IOeisSequence.None);
    }

    return CoreQueryItem(number.ToString("d6", CultureInfo.InvariantCulture), token);
  }

  #endregion IOeisQuery

  #region Algorithm

  private async Task<IReadOnlyList<IOeisSequence>> CoreQuerySequence(string query, CancellationToken token = default) {
    if (string.IsNullOrEmpty(query)) 
      return [];
    
    using var client = Factory.CreateClient(IOeisQuery.HttpClientName);

    var address = $"https://oeis.org/search?q={string.Join(",", query)}&fmt=json";

    using var response = await client.GetAsync(new Uri(address), token).ConfigureAwait(false);

    response.EnsureSuccessStatusCode();

    await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);

    var results = await JsonSerializer
        .DeserializeAsync<OeisSequenceResponse?[]>(stream, JsonSerializerOptions.Default, token).ConfigureAwait(false);

    var answer = new List<OeisSequence>(results?.Length ?? 0);

    foreach (var result in results ?? []) {
      token.ThrowIfCancellationRequested();

      if (result is not null) 
        answer.Add(new OeisSequence(result));
    }

    return answer;
  }

  private async Task<IOeisSequence> CoreQueryItem(string query, CancellationToken token = default) {
    if (string.IsNullOrEmpty(query)) 
      return IOeisSequence.None;
    
    using var client = Factory.CreateClient(IOeisQuery.HttpClientName);

    var address = $"https://oeis.org/A{query}?fmt=json";

    using var response = await client.GetAsync(new Uri(address), token).ConfigureAwait(false);

    response.EnsureSuccessStatusCode();

    await using var stream = await response.Content.ReadAsStreamAsync(token).ConfigureAwait(false);

    var result = await JsonSerializer
        .DeserializeAsync<OeisSequenceResponse>(stream, JsonSerializerOptions.Default, token).ConfigureAwait(false);

    return result is null
        ? IOeisSequence.None
        : new OeisSequence(result);
  }

  #endregion Algorithm
}