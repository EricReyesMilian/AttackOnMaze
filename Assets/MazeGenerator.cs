using System;
using System.Collections.Generic;

public enum Algorithm
{
    Prim,
    Dfs
}
// public class MazeGenerator2
// {
//     private int size;
//     private List<List<Shell>> maze;
//     private Random rand = new Random();

//     public MazeGenerator2(int size, List<List<Shell>> maze, List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
//     {
//         this.size = size;
//         this.maze = maze;


//         // Predefinir celdas sin obstáculos
//         foreach (var cell in predefinedEmptyCells)
//         {
//             maze[cell.x][cell.y].obstacle = false;
//         }

//         // Predefinir celdas con obstáculos
//         foreach (var cell in predefinedObstacleCells)
//         {
//             maze[cell.x][cell.y].obstacle = true;
//         }

//         GenerateMazePrim(predefinedEmptyCells, predefinedObstacleCells);
//         EnsureAllCellsAccessible(predefinedObstacleCells);
//     }

//     private void GenerateMazePrim(List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
//     {
//         List<(int x, int y)> walls = new List<(int, int)>();

//         // Empezar desde una celda no predefinida
//         int startX, startY;
//         do
//         {
//             startX = rand.Next(size);
//             startY = rand.Next(size);
//         } while (!maze[startX][startY].obstacle || predefinedObstacleCells.Contains((startX, startY)));

//         maze[startX][startY].obstacle = false;

//         AddWalls(startX, startY, walls, predefinedEmptyCells, predefinedObstacleCells);

//         while (walls.Count > 0)
//         {
//             var (x, y) = walls[rand.Next(walls.Count)];
//             walls.Remove((x, y));

//             if (CountAdjacent(x, y) == 1 && !predefinedEmptyCells.Contains((x, y)) && !predefinedObstacleCells.Contains((x, y)))
//             {
//                 maze[x][y].obstacle = false;
//                 AddWalls(x, y, walls, predefinedEmptyCells, predefinedObstacleCells);
//             }
//         }
//         EnsureAllCellsAccessible(predefinedEmptyCells, predefinedObstacleCells);
//     }
//     private void EnsureAllCellsAccessible(List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
//     {
//         foreach (var cell in predefinedEmptyCells)
//         {
//             if (!IsCellAccessible(cell.x, cell.y))
//             {

//                 ConnectCell(cell.x, cell.y, predefinedObstacleCells);
//             }
//         }
//     }
//     private bool IsCellAccessible(int x, int y)
//     {
//         bool[,] visited = new bool[size, size]; Stack<(int x, int y)> stack = new Stack<(int, int)>(); stack.Push((x, y)); while (stack.Count > 0)
//         {
//             var (cx, cy) = stack.Pop(); if (cx >= 0 && cy >= 0 && cx < size && cy < size && !visited[cx, cy] && !maze[cx][cy].obstacle)
//             {
//                 visited[cx, cy] = true; if ((cx == 0 && cy == 0) || !maze[cx][cy].obstacle)
//                 {
//                     return true; // Si alcanzamos la celda inicial o cualquier celda libre, es accesible 
//                 }
//                 stack.Push((cx + 1, cy)); stack.Push((cx - 1, cy)); stack.Push((cx, cy + 1)); stack.Push((cx, cy - 1));
//             }
//         }
//         return false;
//     }
//     private void ConnectCell(int x, int y, List<(int x, int y)> predefinedObstacleCells)
//     {
//         List<(int x, int y)> walls = new List<(int, int)>(); AddWalls(x, y, walls, new List<(int, int)>(), predefinedObstacleCells);
//         while (walls.Count > 0)
//         {
//             var (nx, ny) = walls[rand.Next(walls.Count)]; walls.Remove((nx, ny));
//             if (CountAdjacent(nx, ny) > 0 && !predefinedObstacleCells.Contains((nx, ny)))
//             {
//                 maze[nx][ny].obstacle = false;
//                 AddWalls(nx, ny, walls, new List<(int, int)>(), predefinedObstacleCells);
//                 return;
//             }
//         }
//     }
//     // private void ConnectCell(int x, int y, List<(int, int)> predefinedObstacleCells)
//     // {
//     //     List<(int x, int y)> directions = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
//     //     foreach (var (dx, dy) in directions)
//     //     {
//     //         int nx = x + dx;
//     //         int ny = y + dy;
//     //         if (nx >= 0 && nx < size && ny >= 0 && ny < size && !predefinedObstacleCells.Contains((nx, ny)))
//     //         {
//     //             if (!maze[nx][ny].obstacle)
//     //             {
//     //                 maze[x][y].obstacle = false;
//     //                 return;
//     //             }
//     //         }
//     //     } // Si no encontramos una celda libre adyacente, creamos una 
//     //     var randomDirection = directions[rand.Next(directions.Count)];
//     //     int newX = x + randomDirection.x;
//     //     int newY = y + randomDirection.y;
//     //     if (newX >= 0 && newX < size && newY >= 0 && newY < size && !predefinedObstacleCells.Contains((newX, newY)))
//     //     {
//     //         maze[newX][newY].obstacle = false;
//     //     }
//     // }

//     private void AddWalls(int x, int y, List<(int, int)> walls, List<(int, int)> predefinedEmptyCells, List<(int, int)> predefinedObstacleCells)
//     {
//         if (x > 0 && maze[x - 1][y].obstacle && !predefinedEmptyCells.Contains((x - 1, y)) && !predefinedObstacleCells.Contains((x - 1, y))) walls.Add((x - 1, y));
//         if (y > 0 && maze[x][y - 1].obstacle && !predefinedEmptyCells.Contains((x, y - 1)) && !predefinedObstacleCells.Contains((x, y - 1))) walls.Add((x, y - 1));
//         if (x < size - 1 && maze[x + 1][y].obstacle && !predefinedEmptyCells.Contains((x + 1, y)) && !predefinedObstacleCells.Contains((x + 1, y))) walls.Add((x + 1, y));
//         if (y < size - 1 && maze[x][y + 1].obstacle && !predefinedEmptyCells.Contains((x, y + 1)) && !predefinedObstacleCells.Contains((x, y + 1))) walls.Add((x, y + 1));
//     }

//     private int CountAdjacent(int x, int y)
//     {
//         int count = 0;
//         if (x > 0 && !maze[x - 1][y].obstacle) count++;
//         if (y > 0 && !maze[x][y - 1].obstacle) count++;
//         if (x < size - 1 && !maze[x + 1][y].obstacle) count++;
//         if (y < size - 1 && !maze[x][y + 1].obstacle) count++;
//         return count;
//     }

//     private void EnsureAllCellsAccessible(List<(int x, int y)> predefinedObstacleCells)
//     {
//         for (int i = 0; i < size; i++)
//         {
//             for (int j = 0; j < size; j++)
//             {
//                 if (maze[i][j].obstacle && CountAdjacent(i, j) == 1 && !predefinedObstacleCells.Contains((i, j)))
//                 {
//                     maze[i][j].obstacle = false;
//                     List<(int, int)> walls = new List<(int, int)>();
//                     AddWalls(i, j, walls, new List<(int, int)>(), predefinedObstacleCells);

//                     while (walls.Count > 0)
//                     {
//                         var (x, y) = walls[rand.Next(walls.Count)];
//                         walls.Remove((x, y));

//                         if (CountAdjacent(x, y) == 1)
//                         {
//                             maze[x][y].obstacle = false;
//                             AddWalls(x, y, walls, new List<(int, int)>(), predefinedObstacleCells);
//                         }
//                     }
//                 }
//             }
//         }
//     }

//     public void PrintMaze()
//     {
//         for (int i = 0; i < size; i++)
//         {
//             for (int j = 0; j < size; j++)
//             {
//                 Console.Write(maze[j][i].obstacle ? "#" : " ");
//             }
//             Console.WriteLine();
//         }
//     }
// }


public class MazeGenerator
{
    private int size;
    private int startX;
    private int startY;
    private List<List<Shell>> maze;
    private Random rand = new Random();

    public MazeGenerator(int size, int startX, int startY, List<List<Shell>> maze, Algorithm alg, List<(int x, int y)> predefinedEmptyCells, List<(int x, int y)> predefinedObstacleCells)
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
        // foreach (var ecell in predefinedEmptyCells)
        // {
        //     var aux = ecell;
        //     predefinedEmptyCells.Remove(ecell);

        //     //conectar con el laberinto
        // }


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


