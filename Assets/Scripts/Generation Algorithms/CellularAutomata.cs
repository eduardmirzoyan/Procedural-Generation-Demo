

public class CellularAutomata
{
    public int[,] Generate(int seed, int width, int height, int cullPercentage, int numSteps)
    {
        System.Random rng = new(seed);

        // 0 == Wall | 1 == Floor
        int[,] tiles = new int[width, height];

        // Random set starting position
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (rng.Next(100) < cullPercentage)
                {
                    tiles[i, j] = 1;
                }
            }
        }

        // Iterate cellular automata steps
        for (int i = 0; i < numSteps; i++)
        {
            Simulate(tiles);
        }

        return tiles;
    }

    private void Simulate(int[,] tiles)
    {
        var copy = (int[,])tiles.Clone();

        for (int i = 0; i < copy.GetLength(0); i++)
        {
            for (int j = 0; j < copy.GetLength(1); j++)
            {
                int numNeighbors = GetNumNeighbors(i, j, copy);

                if (numNeighbors > 4)
                {
                    tiles[i, j] = 0;
                }
                else
                {
                    tiles[i, j] = 1;
                }
            }
        }
    }

    private int GetNumNeighbors(int x, int y, int[,] tiles)
    {
        int count = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int a = x + i;
                int b = y + j;

                if (a < 0 || b < 0 || a >= tiles.GetLength(0) || b >= tiles.GetLength(1))
                    continue;

                // if wall
                if (tiles[a, b] == 0)
                    count++;
            }
        }

        return count;
    }
}
