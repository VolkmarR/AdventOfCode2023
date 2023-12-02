using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day02;

public class Day02Tests
{
    private readonly ITestOutputHelper _output;
    public Day02Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day02Solver().ExecuteExample1("8");

    [Fact]
    public void Step2WithExample() => new Day02Solver().ExecuteExample2("2286");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day02Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day02Solver().ExecutePuzzle2());
}

public class Day02Solver : SolverBase
{
    List<Game> _data;

    int CubeCount(string color) => color switch
    {
        "red" => 12,
        "green" => 13,
        "blue" => 14,
        _ => throw new Exception($"Invalid Color {color}")
    };

    class Game
    {
        public int Id { get; set; }
        public List<Dictionary<string, int>> Plays { get; set; } = [];
    }

    protected override void Parse(List<string> data)
    {
        _data = new();
        var gamePlaysRegEx = new Regex("Game (?'game'\\d+): (?'plays'.+)");

        foreach (var line in data)
        {
            var gamePlays = gamePlaysRegEx.Matches(line)[0];
            var game = new Game() { Id = gamePlays.Groups["game"].Value.ToInt() };
            _data.Add(game);
            foreach (var play in gamePlays.Groups["plays"].Value.Split("; "))
            {
                var playValues = new Dictionary<string, int>();
                game.Plays.Add(playValues);
                foreach (var cube in play.Split(", "))
                {
                    var parts = cube.Split(" ");
                    var (count, color) = (parts[0].ToInt(), parts[1]);
                    if (playValues.TryGetValue(color, out var current))
                        playValues[color] = current + count;
                    else
                        playValues[color] = count;
                }
            }
        }
    }

    protected override object Solve1()
    {
        bool GameValid(Game game)
        {
            foreach (var play in game.Plays)
            {
                if (play.Any(q => CubeCount(q.Key) < q.Value))
                    return false;
            }

            return true;
        }

        return _data.Where(q => GameValid(q)).Sum(q => q.Id);
    }

    protected override object Solve2()
    {
        long CubePower(Game game)
        {
            var maxCubeCount = new Dictionary<string, int>() { { "red", 0 }, { "green", 0 }, { "blue", 0 } };
            foreach (var play in game.Plays)
            {
                foreach (var playValues in play)
                    if (maxCubeCount[playValues.Key] < playValues.Value)
                        maxCubeCount[playValues.Key] = playValues.Value;
            }

            var result = 1;
            foreach (var value in maxCubeCount.Values)
                result *= value;
            
            return result;
        }

        return _data.Sum(q => CubePower(q));
    }
}