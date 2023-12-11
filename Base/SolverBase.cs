﻿namespace AdventOfCode.Base;

public abstract class SolverBase
{
    protected string DayDirectory => $@"{AppContext.BaseDirectory[..AppContext.BaseDirectory.IndexOf("bin")]}\{GetType().Name[..5]}\";

    private List<string> Load(string inputFileName)
        => File.ReadAllLines($"{DayDirectory}{inputFileName}").ToList();

    string Save(string outputFileName, string data)
    {
        File.WriteAllText($"{DayDirectory}{outputFileName}", data);
        return data;
    }

    public string ExecutePuzzle1(string inputFileName = "input.txt", string outputFileName = "output1.txt")
    {
        Parse(Load(inputFileName));
        return Save(outputFileName, Solve1()?.ToString());
    }

    public string ExecutePuzzle2(string inputFileName = "input.txt", string outputFileName = "output2.txt")
    {
        Parse(Load(inputFileName));
        return Save(outputFileName, Solve2()?.ToString());
    }

    public void ExecuteExample1(string expectedResult)
    {
        Parse(Load("Example.txt"));
        Assert.Equal(expectedResult, Solve1()?.ToString());
    }

    public void ExecuteExample2(string expectedResult)
    {
        Parse(Load("Example.txt"));
        Assert.Equal(expectedResult, Solve2()?.ToString());
    }

    protected abstract void Parse(List<string> data);

    protected abstract object Solve1();

    protected abstract object Solve2();
}
