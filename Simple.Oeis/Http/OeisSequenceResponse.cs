using System.Text.Json.Serialization;

namespace Simple.Oeis.Http;

internal sealed class OeisSequenceResponse {
  [JsonPropertyName("number")]
  public int Number { get; init; }

  [JsonPropertyName("data")]
  public string? Data { get; init; }

  [JsonPropertyName("name")]
  public string? Name { get; init; }

  [JsonPropertyName("comment")]
  public string[]? Comments { get; init; }

  [JsonPropertyName("reference")]
  public string[]? References { get; init; }

  [JsonPropertyName("link")]
  public string[]? Links { get; init; }

  [JsonPropertyName("formula")]
  public string[]? Formulae { get; init; }
}