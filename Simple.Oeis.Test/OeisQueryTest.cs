using Simple.Oeis.Test.Fixture;

namespace Simple.Oeis.Test;

public sealed class OeisQueryTest {
  private const int FibonacciArticleNumber = 45;
  private const int UnknownArticleNumber = 999999;

  private static readonly string FibonacciArticleName = $"A0000{FibonacciArticleNumber}";
  private static readonly string UnknownArticleName = $"A0000{UnknownArticleNumber}";
  private static readonly IReadOnlyList<int> FibonacciSequence = [1, 1, 2, 3, 5, 8];
  private static readonly IReadOnlyList<int> UnknownSequence = [-1, 3, 9, -4789];

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequence_FibonacciNumber_FibonacciArticle(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var article = await query.QuerySequence(FibonacciArticleNumber);

    // Assert
    Assert.Equal(FibonacciArticleNumber, article.Number);
    Assert.Equal(FibonacciArticleName, article.Name);
    Assert.Equal("Fibonacci numbers: F(n) = F(n-1) + F(n-2) with F(0) = 0 and F(1) = 1.", article.Title);

    Assert.Contains("Also sometimes called Lamé's sequence.", article.Comments);
    Assert.Contains("H. Halberstam and K. F. Roth, Sequences, Oxford, 1966; see Appendix.", article.References);
    Assert.Contains("\u003ca href=\"/index/Tu#2wis\"\u003eIndex entries for two-way infinite sequences\u003c/a\u003e", article.Links);
    Assert.Contains("F(n) = F(n-1) + F(n-2) = -(-1)^n F(-n).", article.Formulae);
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequence_FibonacciName_FibonacciArticle(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var article = await query.QuerySequence(FibonacciArticleName);

    // Assert
    Assert.Equal(FibonacciArticleNumber, article.Number);
    Assert.Equal(FibonacciArticleName, article.Name);
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequence_UnknownNumber_NoneArticle(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var article = await query.QuerySequence(UnknownArticleNumber);

    // Assert
    Assert.Equal(IOeisSequence.None, article);
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequence_UnknownName_NoneArticle(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var article = await query.QuerySequence(UnknownArticleName);

    // Assert
    Assert.Equal(IOeisSequence.None, article);
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequences_FibonacciItems_FibonacciArticleIncluded(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var articles = await query.QuerySequences(FibonacciSequence);

    // Assert
    var expectedArticle = articles.Single(article => article.Number == FibonacciArticleNumber);

    Assert.NotNull(expectedArticle);

    Assert.Equal(FibonacciArticleNumber, expectedArticle.Number);
    Assert.Equal(FibonacciArticleName, expectedArticle.Name);
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public async Task QuerySequences_UnknownItems_NoArticles(bool useFactory) {
    // Arrange
    OeisQuery query = useFactory
        ? new(HttpFixtureFactory.Factory)
        : new(HttpFixtureFactory.Client());

    // Act
    var articles = await query.QuerySequences(UnknownSequence);

    // Assert
    Assert.Empty(articles);
  }
}