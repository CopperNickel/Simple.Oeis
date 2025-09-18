using System.Numerics;

namespace Simple.Oeis;

/// <summary>
/// OEIS (Online Encyclopedia of Integer Sequences) Sequence Article
/// </summary>
/// <see href="https://oeis.org/"/>
public interface IOeisSequence : IEquatable<IOeisSequence>, IComparable<IOeisSequence> {
  /// <summary>
  /// Inexistent sequence
  /// </summary>
  public static IOeisSequence None { get; } = new OeisSequence();

  /// <summary>
  /// Sequence number in the Encyclopedia
  /// </summary>
  int Number { get; }

  /// <summary>
  /// Sequence Name in the Encyclopedia
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Title of the sequence
  /// </summary>
  string Title { get; }

  /// <summary>
  /// Comments for the article in the Encyclopedia
  /// </summary>
  public IReadOnlyList<string> Comments { get; }

  /// <summary>
  /// References for the article in the Encyclopedia
  /// </summary>
  public IReadOnlyList<string> References { get; }

  /// <summary>
  /// Links for the article in the Encyclopedia
  /// </summary>
  public IReadOnlyList<string> Links { get; }

  /// <summary>
  /// Formulae for the article in the Encyclopedia
  /// </summary>
  public IReadOnlyList<string> Formulae { get; }

  /// <summary>
  /// Some first numbers of the sequence
  /// </summary>
  /// <typeparam name="T">Any integer type</typeparam>
  /// <returns>First numbers of the sequence</returns>
  public IEnumerable<T> FirstItems<T>() where T : IBinaryInteger<T>;
}