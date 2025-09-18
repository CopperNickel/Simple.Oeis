using System.Globalization;
using System.Numerics;

namespace Simple.Oeis;

/// <summary>
///     OEIS (Online Encyclopedia of Integer Sequences) Query extensions
/// </summary>
public static class OeisQueryExtensions {
  /// <summary>
  /// Get Sequences from the Encyclopedia with fit given integer sequence
  /// </summary>
  /// <typeparam name="T">Any integer type</typeparam>
  /// <param name="query">OEIS query</param>
  /// <param name="sequence">Integer sequence to be fit</param>
  /// <returns>OEIS sequences which fit given integer sequences</returns>
  public static Task<IReadOnlyList<IOeisSequence>> QuerySequences<T>(this IOeisQuery query, params IEnumerable<T> sequence)
      where T : IBinaryInteger<T> {
    ArgumentNullException.ThrowIfNull(query);
    ArgumentNullException.ThrowIfNull(sequence);

    return query.QuerySequences(sequence, CancellationToken.None);
  }

  /// <summary>
  /// Get Sequence by its number in the Encyclopedia
  /// </summary>
  /// <param name="query">OEIS query</param>
  /// <param name="name">Sequence name in Ax...xxx format, where x...xxx is OEIS sequence number</param>
  /// <param name="token">Token to cancel operation</param>
  /// <returns>OEIS sequence</returns>
  public static Task<IOeisSequence> QuerySequence(this IOeisQuery query, ReadOnlySpan<char> name, CancellationToken token = default) {
    ArgumentNullException.ThrowIfNull(query);

    name = name.Trim();

    if (name.Length > 1 && (name[0] == 'a' || name[0] == 'A'))
      if (int.TryParse(name[1..], NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
        return query.QuerySequence(number, token);

    return Task.FromResult(IOeisSequence.None);
  }

  /// <summary>
  /// Get Sequence by its number in the Encyclopedia
  /// </summary>
  /// <param name="query">OEIS query</param>
  /// <param name="name">Sequence name in Ax...xxx format, where x...xxx is OEIS sequence number</param>
  /// <param name="token">Token to cancel operation</param>
  /// <returns>OEIS sequence</returns>
  public static Task<IOeisSequence> QuerySequence(this IOeisQuery query, string name, CancellationToken token = default) {
    ArgumentNullException.ThrowIfNull(query);

    if (string.IsNullOrWhiteSpace(name))
      return Task.FromResult(IOeisSequence.None);

    return QuerySequence(query, name.AsSpan(), token);
  }
}