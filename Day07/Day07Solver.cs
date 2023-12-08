namespace AdventOfCode.Day07;

public class Day07Tests
{
    private readonly ITestOutputHelper _output;
    public Day07Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day07Solver().ExecuteExample1("6440");

    [Fact]
    public void Step2WithExample() => new Day07Solver().ExecuteExample2("5905");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day07Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day07Solver().ExecutePuzzle2());
}

public class Day07Solver : SolverBase
{
    private const int Joker1 = 11;
    private const int Joker2 = 1;

    static int MapCard(char card) => card switch
    {
        'T' => 10,
        'J' => 11,
        'Q' => 12,
        'K' => 13,
        'A' => 14,
        _ => (int)char.GetNumericValue(card),
    };

    static int[] MapCardsForGame2(int[] cards)
        => cards.Select(q => q == Joker1 ? Joker2 : q).ToArray();

    private enum HandType
    {
        FiveOfAKind = 1,
        FourOfAKind = 2,
        FullHouse = 3,
        TreeOfAKind = 4,
        TwoPair = 5,
        OnePair = 6,
        HighCard = 7,
    }

    private HandType GetHandType(int[] hand)
    {
        var grouped = hand.GroupBy(q => q).Select(q => q.Count()).OrderByDescending(q => q).ToArray();
        return grouped.Length switch
        {
            1 => HandType.FiveOfAKind,
            2 when grouped[0] == 4 => HandType.FourOfAKind,
            2 when grouped[0] == 3 => HandType.FullHouse,
            3 when grouped[0] == 3 => HandType.TreeOfAKind,
            3 when grouped[0] == 2 => HandType.TwoPair,
            4 => HandType.OnePair,
            _ => HandType.HighCard
        };
    }

    private HandType GetHandTypeGame2(int[] hand)
    {
        var jokers = hand.Count(q => q == Joker2);
        if (jokers == 0)
            return GetHandType(hand);

        var grouped = hand.GroupBy(q => q).Select(q => q.Count()).OrderByDescending(q => q).ToArray();
        return grouped.Length switch
        {
            1 or 2 => HandType.FiveOfAKind,
            3 when jokers >= 2 => HandType.FourOfAKind,
            3 when jokers == 1 && grouped[0] == 3 => HandType.FourOfAKind,
            3 => HandType.FullHouse,
            4 => HandType.TreeOfAKind,
            _ => HandType.OnePair
        };
    }

    private class Hand
    {
        public int[] Cards { get; set; }
        public int Bid { get; set; }
        public HandType HandType { get; set; }
    }

    List<Hand> _data1;
    List<Hand> _data2;

    protected override void Parse(List<string> data)
    {
        _data1 = new();
        _data2 = new();
        foreach (var line in data)
        {
            var elements = line.Split(' ');
            var cards = elements[0].Select(MapCard).ToArray();
            var hand1 = new Hand()
            {
                Cards = cards,
                Bid = elements[1].ToInt(),
                HandType = GetHandType(cards),
            };
            _data1.Add(hand1);

            cards = MapCardsForGame2(hand1.Cards);
            var hand2 = new Hand()
            {
                Cards = cards,
                Bid = hand1.Bid,
                HandType = GetHandTypeGame2(cards),
            };
            _data2.Add(hand2);
        }
    }

    private long GetTotalWinning(List<Hand> hands)
    {
        var sortedHands = hands
            .OrderByDescending(q => q.HandType)
            .ThenBy(q => q.Cards[0])
            .ThenBy(q => q.Cards[1])
            .ThenBy(q => q.Cards[2])
            .ThenBy(q => q.Cards[3])
            .ThenBy(q => q.Cards[4])
            .ToArray();

        var rank = 0;
        return sortedHands.Sum(q => (long)(q.Bid * ++rank));
    }

    protected override object Solve1() => GetTotalWinning(_data1);

    protected override object Solve2() => GetTotalWinning(_data2);
}