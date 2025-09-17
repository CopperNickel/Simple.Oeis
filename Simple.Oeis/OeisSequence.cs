using System.Globalization;
using System.Numerics;
using Simple.Oeis.Http;

namespace Simple.Oeis;

/// <summary>
/// OEIS (Online Encyclopedia of Integer Sequences) Sequence Article
/// </summary>
public sealed class OeisSequence : IOeisSequence
{
    #region Private Fields

    private string[] RawData { get; } = [];

    #endregion Private Fields

    #region Properties

    /// <inheritdoc cref="IOeisSequence.Number" />
    public int Number { get; }

    /// <inheritdoc cref="IOeisSequence.Name" />
    public string Name { get; } = "A000000";

    /// <inheritdoc cref="IOeisSequence.Title" />
    public string Title { get; } = "";

    /// <inheritdoc cref="IOeisSequence.Comments" />
    public IReadOnlyList<string> Comments { get; } = [];

    /// <inheritdoc cref="IOeisSequence.References" />
    public IReadOnlyList<string> References { get; } = [];

    /// <inheritdoc cref="IOeisSequence.Links" />
    public IReadOnlyList<string> Links { get; } = [];

    /// <inheritdoc cref="IOeisSequence.Formulae" />
    public IReadOnlyList<string> Formulae { get; } = [];

    #endregion Properties

    #region Create

    internal OeisSequence()
    {
    }

    internal OeisSequence(OeisSequenceResponse response)
    {
        Number = response.Number;
        Name = $"A{response.Number.ToString("d6", CultureInfo.InvariantCulture)}";
        Title = response.Name ?? "";
        Comments = response.Comments ?? [];

        References = response.References ?? [];
        Links = response.Links ?? [];
        Formulae = response.Formulae ?? [];

        RawData = (response.Data ?? "").Split(',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    #endregion Create

    #region Public Methods

    /// <inheritdoc cref="T:IOeisQuery.FirstItems{T}()" />
    public IEnumerable<T> FirstItems<T>() where T : IBinaryInteger<T>
    {
        return RawData
            .Select(item =>
            {
                var success = T.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out var value);

                return (success, value);
            })
            .TakeWhile(pair => pair.success)
            .Select(pair => pair.value!);
    }

    /// <summary>
    /// To String (debug usage only) representation
    /// </summary>
    /// <returns>String representation</returns>
    public override string ToString()
    {
        return $"{Name}: {Title}";
    }

    #endregion Public Methods

    #region IEquatable<IOeisSequence>

    /// <summary>
    /// If sequences articles are equal
    /// </summary>
    /// <param name="other">Sequence article to compare with</param>
    /// <returns>True if sequences are equal, false otherwise</returns>
    public bool Equals(IOeisSequence? other) => other is not null && other.Number == Number;

    /// <summary>
    /// If sequences articles are equal
    /// </summary>
    /// <param name="obj">Sequence article to compare with</param>
    /// <returns>True if sequences are equal, false otherwise</returns>
    public override bool Equals(object? obj) => Equals(obj as OeisSequence);

    /// <summary>
    /// Hash code computation
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode() => Number;

    #endregion IEquatable<IOeisSequence>

    #region IComparable<IOeisSequence>

    /// <summary>
    /// Compare sequences articles by their numbers
    /// </summary>
    /// <param name="other">Sequence article to compare</param>
    /// <returns>Negative if current sequence is less than less, 0 if they are equal, positive if current sequence article is greater</returns>
    public int CompareTo(IOeisSequence? other)
    {
        if (other is null)
            return 1;

        return Number.CompareTo(other.Number);
    }

    #endregion IComparable<IOeisSequence>
}