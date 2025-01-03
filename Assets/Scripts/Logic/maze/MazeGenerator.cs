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
    private List<List<Cell>> maze;
    private Random rand = new Random();

    public MazeGenerator(int size, int startX, int startY, List<List<Cell>> maze, Algorithm alg)
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

    }
    private void GenerateMazePrim(int startX, int startY)
    {
        List<(int x, int y)> walls = new List<(int, int)>();

        foreach (var cell in Board.predefinedEmptyCells)
        {
            maze[cell.x][cell.y].obstacle = false;
        }
        foreach (var cell in Board.predefinedObstacleCells)
        {
            maze[cell.x][cell.y].obstacle = true;
        }

        maze[startX][startY].obstacle = false;
        AddWalls(startX, startY, walls);

        while (walls.Count > 0)
        {
            var (x, y) = walls[rand.Next(walls.Count)];
            walls.Remove((x, y));

            if (CountAdjacent(x, y) == 1 && !Board.predefinedEmptyCells.Contains((x, y)) && !Board.predefinedObstacleCells.Contains((x, y)))
            {
                maze[x][y].obstacle = false;
                AddWalls(x, y, walls);
            }
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i][j].obstacle && CountAdjacent(i, j) == 1 && !Board.predefinedObstacleCells.Contains((i, j)))
                {
                    maze[i][j].obstacle = false;
                }
            }
        }

        Conec();
        foreach (var cell in Board.predefinedEmptyCells)
        {
            maze[cell.x][cell.y].obstacle = false;
        }

    }
    void Conec()
    {
        int size = maze[0].Count;
        //Marcar casilla inicial
        //           N   S  E  W
        int[] df = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, 1, -1 };


        //Para cada posible celda del maze
        for (int i = 0; i < Board.predefinedEmptyCells.Count; i++)
        {
            for (int f = 0; f < size; f++)
            {
                for (int c = 0; c < size; c++)
                {

                    //inspeccionar celdas vecinas
                    if ((f, c) == Board.predefinedEmptyCells[i])
                    {
                        for (int d = 0; d < df.Length; d++)
                        {
                            int vf = f + df[d];
                            int vc = c + dc[d];
                            //determinar si es un vecino valido
                            if (PosicionValida(size, vf, vc)
                            && !Board.predefinedObstacleCells.Contains((vf, vc)) && !Board.predefinedEmptyCells.Contains((vf, vc)))
                            {
                                if (maze[vf][vc].obstacle)
                                {
                                    Board.predefinedEmptyCells.Add((vf, vc));

                                }
                            }
                        }

                    }
                }
            }
        }


    }
    private static bool PosicionValida(int n, int f, int c)
    {
        return f >= 0 && f < n && c >= 0 && c < n;
    }

    private void AddWalls(int x, int y, List<(int, int)> walls)
    {
        if (x > 0 && maze[x - 1][y].obstacle && !Board.predefinedEmptyCells.Contains((x - 1, y)) && !Board.predefinedObstacleCells.Contains((x - 1, y)))
            walls.Add((x - 1, y));
        if (y > 0 && maze[x][y - 1].obstacle && !Board.predefinedEmptyCells.Contains((x, y - 1)) && !Board.predefinedObstacleCells.Contains((x, y - 1)))
            walls.Add((x, y - 1));
        if (x < size - 1 && maze[x + 1][y].obstacle && !Board.predefinedEmptyCells.Contains((x + 1, y)) && !Board.predefinedObstacleCells.Contains((x + 1, y)))
            walls.Add((x + 1, y));
        if (y < size - 1 && maze[x][y + 1].obstacle && !Board.predefinedEmptyCells.Contains((x, y + 1)) && !Board.predefinedObstacleCells.Contains((x, y + 1)))
            walls.Add((x, y + 1));
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
        if (x > 0 && !maze[x - 1][y].obstacle && !Board.predefinedEmptyCells.Contains((x - 1, y)) && !Board.predefinedObstacleCells.Contains((x - 1, y))) count++;
        if (y > 0 && !maze[x][y - 1].obstacle && !Board.predefinedEmptyCells.Contains((x, y - 1)) && !Board.predefinedObstacleCells.Contains((x, y - 1))) count++;
        if (x < size - 1 && !maze[x + 1][y].obstacle && !Board.predefinedEmptyCells.Contains((x + 1, y)) && !Board.predefinedObstacleCells.Contains((x + 1, y))) count++;
        if (y < size - 1 && !maze[x][y + 1].obstacle && !Board.predefinedEmptyCells.Contains((x, y + 1)) && !Board.predefinedObstacleCells.Contains((x, y + 1))) count++;
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


