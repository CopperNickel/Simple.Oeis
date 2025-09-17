# [OEIS Encyclopedia](https://oeis.org/) sequences queries

![OEIS icon](Icon.png)

This package designed to query for __integer sequences_.

## Article by its number

```cs
const int FibonacciArticleNumber = 45;

var query = new OeisQuery();

var sequence = await query.QuerySequence(FibonacciArticleNumber);
```
## Article by its name

```cs
const string FibonacciArticleName = "A000045";

var query = new OeisQuery();

var sequences = await query.QuerySequence(FibonacciArticleNumber);
```
## Articles sequence items

When the _exact article is not known_ one can try finding sequences by known items.

```cs
var int[] knownItems = [1, 1, 2, 3, 5, 8, 13];

var query = new OeisQuery();

var sequences = await query.QuerySequence(knownItems);
```
## Sequence Items

Having a sequence we can generate its items (usually their top 10 .. 100 items), e.g. 

```cs
var query = new OeisQuery();

var sequences = await query.QuerySequence(1, 1, 2, 3, 5, 8);

var fibo = sequences
  .First(sequence => sequence.Title.Contains("Fibonacci"));

var moreItems = fibo
  .FirstItems<int>();

Console.WriteLine(string.Join(", ", moreItems));
```
