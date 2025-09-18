using Simple.Oeis.Test.Fixture;

namespace Simple.Oeis.Test;

public sealed class OeisSequenceTest {
  [Fact]
  public async Task FirstItems_FibonacciSequence_Generated() {
    // Arrange
    var article = await Fibonacci();

    // Act
    var actual = article
        .FirstItems<byte>()
        .ToArray();

    // Assert
    byte[] expected = [0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233];

    Assert.Equal(expected, actual);
  }

  [Fact]
  public void FirstItems_UnknownSequence_NoItems() {
    // Arrange
    var unknownArticle = IOeisSequence.None;

    // Act
    var actual = unknownArticle
        .FirstItems<byte>()
        .ToArray();

    // Assert
    Assert.Empty(actual);
  }

  private static Task<IOeisSequence> Fibonacci() {
    const int articleNumber = 45;

    OeisQuery query = new(HttpFixtureFactory.Factory);

    return query.QuerySequence(articleNumber);
  }
}

