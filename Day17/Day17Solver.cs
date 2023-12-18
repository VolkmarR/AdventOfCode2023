namespace AdventOfCode.Day17;

public class Day17Tests
{
    private readonly ITestOutputHelper _output;
    public Day17Tests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Step1WithExample() => new Day17Solver().ExecuteExample1("102");

    [Fact]
    public void Step2WithExample() => new Day17Solver().ExecuteExample2("??");

    [Fact]
    public void Step1WithPuzzleInput() => _output.WriteLine(new Day17Solver().ExecutePuzzle1());

    [Fact]
    public void Step2WithPuzzleInput() => _output.WriteLine(new Day17Solver().ExecutePuzzle2());
}

public class Day17Solver : SolverBase
{
    byte[,] _map;
    private int _height;
    private int _width;
    private List<Node> _graph;


    class Node
    {
        public byte Value;
        public Node Straight;
        public Node Left;
        public Node Right;
    }

    class Path
    {
        private static Dictionary<(int dx, int dy), (int dx, int dy)> TurnLeftMap = new()
        {
            { (1, 0), (0, -1) },
            { (0, -1), (-1, 0) },
            { (-1, 0), (0, 1) },
            { (0, 1), (1, 0) },
        };

        private static Dictionary<(int dx, int dy), (int dx, int dy)> TurnRightMap = new()
        {
            { (1, 0), (0, 1) },
            { (0, 1), (-1, 0) },
            { (-1, 0), (0, -1) },
            { (0, -1), (1, 0) },
        };

        private byte[,] _map;
        private int _width;
        private int _height;
        private HashSet<(int x, int y)> _visited;
        private int _dx = 1;
        private int _dy = 0;
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public long Sum { get; private set; } = 0;

        public bool Move()
        {
            _visited.Add((X, Y));
            Sum += _map[X, Y];
            Y += _dy;
            X += _dx;

            return IsValid();
        }

        public bool IsValid()
            => X >= 0 && Y >= 0 && X < _width && Y < _height && !_visited.Contains((X, Y));

        public Path TurnLeft()
        {
            (_dx, _dy) = TurnLeftMap[(_dx, _dy)];
            return this;
        }

        public Path TurnRight()
        {
            (_dx, _dy) = TurnRightMap[(_dx, _dy)];
            return this;
        }

        public Path(int startX, int startY, byte[,] map)
        {
            X = startX;
            Y = startY;
            _visited = new();
            _map = map;
            _width = _map.GetLength(0);
            _height = _map.GetLength(1);
        }

        public Path Clone()
            => new Path(this);

        private Path(Path basePath)
        {
            _map = basePath._map;
            _width = basePath._width;
            _height = basePath._height;

            Sum = basePath.Sum;
            X = basePath.X;
            Y = basePath.Y;
            _dx = basePath._dx;
            _dy = basePath._dy;
            _visited = new(basePath._visited);
        }
    }


    protected override void Parse(List<string> data)
    {
        _map = new byte[data[0].Length, data.Count];
        _width = _map.GetLength(0);
        _height = _map.GetLength(1);
        for (var y = 0; y < _height; y++)
        for (var x = 0; x < _width; x++)
            _map[x, y] = (byte)data[y][x];
    }

    private Path _shortest;
    private int _endX;
    private int _endY;

    void GetShortestPath(Path path)
    {
        for (int i = 0; i < 3; i++)
        {
            if (!path.Move())
                return;

            if (_shortest != null && path.Sum > _shortest.Sum)
                return;
            if (path.X == _endX && path.Y == _endY)
            {
                _shortest = path;
                return;
            }

            GetShortestPath(path.Clone().TurnLeft());
            GetShortestPath(path.Clone().TurnRight());

        }
    }

    protected override object Solve1()
    {
        _shortest = null;
        _endX = _width - 1;
        _endY = _height - 1;
        var path = new Path(0, 0, _map);
        GetShortestPath(path);

        path = new Path(0, 0, _map).TurnLeft();
        GetShortestPath(path);

        return _shortest.Sum;
    }

    protected override object Solve2()
    {
        throw new Exception("Solver error");
    }
}