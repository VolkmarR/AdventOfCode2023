using System.Text.RegularExpressions;

namespace AdventOfCode.Day19;

public class Day19Tests
{
    private readonly ITestOutputHelper _output;
    public Day19Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day19Solver().ExecuteExample1("19114");

    [Fact]
    public void Step2WithExample() => new Day19Solver().ExecuteExample2("167409079868000");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day19Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day19Solver().ExecutePuzzle2());
}

public class Day19Solver : SolverBase
{
    class Rule
    {
        public char Name { get; set; }
        public char Operator { get; set; }
        public int Value { get; set; }
        public string NextWorkflow { get; set; }
    }


    private Dictionary<string, List<Rule>> _workflows;
    List<Dictionary<char, int>> _data;
    private static readonly char[] WorkflowSpSeparator1 = new[] { '{', ',', '}' };

    protected override void Parse(List<string> data)
    {
        _data = new();
        _workflows = new();
        var ruleRegex = new Regex(@"(?'name'\w+)(?'operator'[<>])(?'value'\d+):(?'destination'\w+)",
            RegexOptions.Compiled);

        var i = 0;
        for (; i < data.Count && !string.IsNullOrEmpty(data[i]); i++)
        {
            var parts = data[i].Split(WorkflowSpSeparator1, StringSplitOptions.RemoveEmptyEntries);
            var rules = new List<Rule>();
            _workflows.Add(parts[0], rules);
            foreach (var ruleRaw in parts.Skip(1))
            {
                var ruleParts = ruleRegex.Match(ruleRaw);
                Rule rule;
                if (ruleParts.Success)
                    rule = new()
                    {
                        Name = ruleParts.Groups["name"].Value[0],
                        Operator = ruleParts.Groups["operator"].Value.First(),
                        Value = ruleParts.Groups["value"].Value.ToInt(),
                        NextWorkflow = ruleParts.Groups["destination"].Value,
                    };
                else
                    rule = new() { Name = ' ', NextWorkflow = ruleRaw };
                rules.Add(rule);
            }
        }

        i++;
        for (; i < data.Count; i++)
        {
            var parts = data[i].Split(new[] { '{', '=', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
            var values = new Dictionary<char, int>();
            for (int j = 0; j < parts.Length; j += 2)
                values[parts[j][0]] = parts[j + 1].ToInt();
            _data.Add(values);
        }
    }

    bool ExecuteWorkflow(Dictionary<char, int> values)
    {
        bool RuleIsTrue(Rule rule)
        {
            if (rule.Name == ' ')
                return true;

            var value = values[rule.Name];
            return
                (rule.Operator == '<' && value < rule.Value) ||
                (rule.Operator == '>' && value > rule.Value);
        }

        var workflowName = "in";
        while (true)
        {
            var rules = _workflows[workflowName];
            var rule = rules.First(RuleIsTrue);
            if (rule.NextWorkflow == "R")
                return false;
            if (rule.NextWorkflow == "A")
                return true;
            workflowName = rule.NextWorkflow;
        }
    }

    int PartSum(Dictionary<char, int> values)
        => values.Values.Sum();

    protected override object Solve1()
        => _data.Where(ExecuteWorkflow).Sum(PartSum);

    protected override object Solve2()
    {
        var result = 0L;
        var values = new Dictionary<char, int>();
        for (int x = 1; x < 4000; x++)
        {
            values['x'] = x;
            for (int m = 1; m < 4000; m++)
            {
                values['m'] = m;
                for (int a = 1; a < 4000; a++)
                {
                    values['a'] = a;
                    for (int s = 1; s < 4000; s++)
                    {
                        values['s'] = s;
                        if (ExecuteWorkflow(values))
                            result += PartSum(values);
                    }
                }
            }
        }

        return result;
    }
}