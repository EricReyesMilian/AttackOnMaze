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

    public MazeGenerator(int size, int startX, int startY, List<List<Cell>> maze, Algorithm alg, List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
    {
        this.size = size;
        this.startX = startX;
        this.startY = startY;
        this.maze = maze;

        switch (alg)
        {
            case Algorithm.Prim: GenerateMazePrim(startX, startY, predefinedEmptyCells, predefinedObstacleCells); break;
            case Algorithm.Dfs: GenerateMazeDfs(startX, startY); break;

        }
    }
    private void GenerateMazeDfs(int startX, int startY)
    {

    }
    private void GenerateMazePrim(int startX, int startY, List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
    {
        List<(int x, int y)> walls = new List<(int, int)>();



        // Predefinir celdas sin obstáculos 
        foreach (var cell in predefinedEmptyCells)
        {
            maze[cell.x][cell.y].obstacle = false;
        }
        // Predefinir celdas con obstáculos 
        foreach (var cell in predefinedObstacleCells)
        {
            maze[cell.x][cell.y].obstacle = true;
        }

        maze[startX][startY].obstacle = false;
        AddWalls(startX, startY, walls, predefinedEmptyCells, predefinedObstacleCells);

        while (walls.Count > 0)
        {
            var (x, y) = walls[rand.Next(walls.Count)];
            walls.Remove((x, y));

            if (CountAdjacent(x, y, predefinedEmptyCells, predefinedObstacleCells) == 1 && !predefinedEmptyCells.Contains((x, y)) && !predefinedObstacleCells.Contains((x, y)))
            {
                maze[x][y].obstacle = false;
                AddWalls(x, y, walls, predefinedEmptyCells, predefinedObstacleCells);
            }
        }

        // Asegurar que todas las celdas sean alcanzables
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i][j].obstacle && CountAdjacent(i, j, predefinedEmptyCells, predefinedObstacleCells) == 1 && !predefinedObstacleCells.Contains((i, j)))
                {
                    maze[i][j].obstacle = false;
                    // AddWalls(i, j, walls, predefinedEmptyCells, predefinedObstacleCells);
                }
            }
        }

        Conec(predefinedEmptyCells, predefinedObstacleCells);
        foreach (var cell in predefinedEmptyCells)
        {
            maze[cell.x][cell.y].obstacle = false;
        }

    }
    void Conec(List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
    {
        int size = maze[0].Count;
        //Marcar casilla inicial
        //           N   S  E  W
        int[] df = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, 1, -1 };


        //Para cada posible celda del maze
        for (int i = 0; i < predefinedEmptyCells.Count; i++)
        {
            for (int f = 0; f < size; f++)
            {
                for (int c = 0; c < size; c++)
                {

                    //inspeccionar celdas vecinas
                    if ((f, c) == predefinedEmptyCells[i])
                    {
                        for (int d = 0; d < df.Length; d++)
                        {
                            int vf = f + df[d];
                            int vc = c + dc[d];
                            //determinar si es un vecino valido
                            if (PosicionValida(size, vf, vc)
                            && !predefinedObstacleCells.Contains((vf, vc)) && !predefinedEmptyCells.Contains((vf, vc)))
                            {
                                if (maze[vf][vc].obstacle)
                                {
                                    predefinedEmptyCells.Add((vf, vc));

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

    private void AddWalls(int x, int y, List<(int, int)> walls, List<(int, int)> predefinedEmptyCells, List<(int, int)> predefinedObstacleCells)
    {
        if (x > 0 && maze[x - 1][y].obstacle && !predefinedEmptyCells.Contains((x - 1, y)) && !predefinedObstacleCells.Contains((x - 1, y)))
            walls.Add((x - 1, y));
        if (y > 0 && maze[x][y - 1].obstacle && !predefinedEmptyCells.Contains((x, y - 1)) && !predefinedObstacleCells.Contains((x, y - 1)))
            walls.Add((x, y - 1));
        if (x < size - 1 && maze[x + 1][y].obstacle && !predefinedEmptyCells.Contains((x + 1, y)) && !predefinedObstacleCells.Contains((x + 1, y)))
            walls.Add((x + 1, y));
        if (y < size - 1 && maze[x][y + 1].obstacle && !predefinedEmptyCells.Contains((x, y + 1)) && !predefinedObstacleCells.Contains((x, y + 1)))
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

    private int CountAdjacent(int x, int y, List<(int, int)> predefinedEmptyCells, List<(int, int)> predefinedObstacleCells)
    {
        int count = 0;
        if (x > 0 && !maze[x - 1][y].obstacle && !predefinedEmptyCells.Contains((x - 1, y)) && !predefinedObstacleCells.Contains((x - 1, y))) count++;
        if (y > 0 && !maze[x][y - 1].obstacle && !predefinedEmptyCells.Contains((x, y - 1)) && !predefinedObstacleCells.Contains((x, y - 1))) count++;
        if (x < size - 1 && !maze[x + 1][y].obstacle && !predefinedEmptyCells.Contains((x + 1, y)) && !predefinedObstacleCells.Contains((x + 1, y))) count++;
        if (y < size - 1 && !maze[x][y + 1].obstacle && !predefinedEmptyCells.Contains((x, y + 1)) && !predefinedObstacleCells.Contains((x, y + 1))) count++;
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


