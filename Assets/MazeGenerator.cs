using System;
using System.Collections.Generic;

public enum Algorithm
{
    Prim,
    Dfs
}
public class MazeGenerator
{
    private int size;
    private int startX;
    private int startY;
    private List<List<Shell>> maze;
    private Random rand = new Random();

    public MazeGenerator(int size, int startX, int startY, List<List<Shell>> maze, Algorithm alg)
    {
        this.size = size;
        this.startX = startX;
        this.startY = startY;
        this.maze = maze;

        switch (alg)
        {
            case Algorithm.Prim: GenerateMazePrim(startX, startY); break;
            case Algorithm.Dfs: GenerateMazeDfs(startX, startY); break;

        }
    }
    private void GenerateMazeDfs(int startX, int startY)
    {
        Stack<(int x, int y)> stack = new Stack<(int, int)>();
        maze[startX][startY].obstacle = false;
        stack.Push((startX, startY));
        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();
            List<(int x, int y)> neighbors = GetNeighbors(x, y);
            if (neighbors.Count > 0)
            {
                stack.Push((x, y));
                var (nx, ny) = neighbors[rand.Next(neighbors.Count)];
                if (CountAdjacent(nx, ny) == 1)
                {
                    maze[nx][ny].obstacle = false;
                    stack.Push((nx, ny));
                }
            }
        }
    }
    private void GenerateMazePrim(int startX, int startY)
    {
        List<(int x, int y)> walls = new List<(int, int)>();

        maze[startX][startY].obstacle = false;

        AddWalls(startX, startY, walls);

        while (walls.Count > 0)
        {
            var (x, y) = walls[rand.Next(walls.Count)];
            walls.Remove((x, y));

            if (CountAdjacent(x, y) == 1)
            {
                maze[x][y].obstacle = false;
                AddWalls(x, y, walls);
            }
        }

        // Asegurar que todas las celdas sean alcanzables
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i][j].obstacle && CountAdjacent(i, j) == 1)
                {
                    maze[i][j].obstacle = false;
                    AddWalls(i, j, walls);
                }
            }
        }
    }

    private void AddWalls(int x, int y, List<(int, int)> walls)
    {
        if (x > 0 && maze[x - 1][y].obstacle) walls.Add((x - 1, y));
        if (y > 0 && maze[x][y - 1].obstacle) walls.Add((x, y - 1));
        if (x < size - 1 && maze[x + 1][y].obstacle) walls.Add((x + 1, y));
        if (y < size - 1 && maze[x][y + 1].obstacle) walls.Add((x, y + 1));
    }
    private List<(int x, int y)> GetNeighbors(int x, int y)
    {
        List<(int x, int y)> neighbors = new List<(int, int)>();
        if (x > 0) neighbors.Add((x - 1, y));
        if (y > 0) neighbors.Add((x, y - 1));
        if (x < size - 1) neighbors.Add((x + 1, y));
        if (y < size - 1) neighbors.Add((x, y + 1));
        return neighbors;
    }

    private int CountAdjacent(int x, int y)
    {
        int count = 0;
        if (x > 0 && !maze[x - 1][y].obstacle) count++;
        if (y > 0 && !maze[x][y - 1].obstacle) count++;
        if (x < size - 1 && !maze[x + 1][y].obstacle) count++;
        if (y < size - 1 && !maze[x][y + 1].obstacle) count++;
        return count;
    }

    public bool[,] PrintMaze(int n)
    {
        bool[,] info = new bool[n, n];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[j][i].obstacle)
                {
                    info[i, j] = true;
                }
                else
                {
                    info[i, j] = false;
                }
            }
        }
        return info;
    }
}


