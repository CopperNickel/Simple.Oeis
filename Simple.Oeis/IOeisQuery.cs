using System.Numerics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Simple.Oeis.Test")]

namespace Simple.Oeis;

/// <summary>
/// OEIS (Online Encyclopedia of Integer Sequences) Query
/// </summary>
/// <see href="https://oeis.org/"/>
public interface IOeisQuery
{
    /// <summary>
    /// Http Client Name
    /// </summary>
    public const string HttpClientName = "OEIS";

    /// <summary>
    /// Get Sequences from the Encyclopedia with fit given integer sequence
    /// </summary>
    /// <typeparam name="T">Any integer type</typeparam>
    /// <param name="sequence">Integer sequence to be fit</param>
    /// <param name="token">Token to cancel operation</param>
    /// <returns>OEIS sequences which fit given integer sequences</returns>
    Task<IReadOnlyList<IOeisSequence>> QuerySequences<T>(IEnumerable<T> sequence, CancellationToken token = default) where T : IBinaryInteger<T>;

    /// <summary>
    /// Get Sequence by its number in the Encyclopedia 
    /// </summary>
    /// <param name="number">Sequence number</param>
    /// <param name="token">Token to cancel operation</param>
    /// <returns>OEIS sequence</returns>
    Task<IOeisSequence> QuerySequence(int number, CancellationToken token = default);
}
