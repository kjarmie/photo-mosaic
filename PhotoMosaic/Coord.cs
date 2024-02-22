namespace PhotoMosaic;

public struct Coord
{
    public int row;
    public int col;

    public Coord(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public override string ToString()
    {
        return $"({col}, {row})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Coord && ((Coord)obj).row == row && ((Coord)obj).col == col;
    }

    public override int GetHashCode()
    {
        var result = 11;
        result *= row * 37;
        result *= col * 19;
        return result;
    }
}