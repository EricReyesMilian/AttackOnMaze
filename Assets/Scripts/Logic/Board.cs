using System;
using System.Collections.Generic;
public class Board
{
    GameManager gm = GameManager.gameManeger;
    List<List<Cell>> grid;
    public static List<(int x, int y)> predefinedEmptyCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15), (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 9), (8, 6), (8, 10), (10, 8), (10, 6), (6, 10), (10, 10) };
    public static List<(int x, int y)> predefinedCenterCells = new List<(int, int)> { (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 8), (7, 9) };
    public static List<(int x, int y)> predefinedObstacleCells = new List<(int, int)> { (6, 9), (6, 7), (7, 10), (7, 6), (9, 6), (10, 7), (9, 10), (10, 9) };
    public static List<(int x, int y)> startCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15) };
    public static List<(int x, int y)> DoorCells = new List<(int, int)> { (8, 6), (6, 8), (10, 8), (8, 10) };


    public Board(List<List<Cell>> grid)
    {
        this.grid = grid;
    }
    public static List<List<int>> Lee(List<List<Cell>> tablero, int filaInical, int columnaInicial, int speed)
    {
        int size = tablero[0].Count;
        List<List<int>> distancias = new List<List<int>>();
        IniciarDistancias(ref distancias, tablero[0].Count);
        //Marcar casilla inicial
        distancias[filaInical][columnaInicial] = 0;
        //           N   S  E  W
        int[] df = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, 1, -1 };

        bool huboCambio;

        do
        {
            huboCambio = false;
            //Para cada posible celda del tablero
            for (int f = 0; f < size; f++)
            {
                for (int c = 0; c < size; c++)
                {
                    //saltarse las celdas no marcadas
                    if (distancias[f][c] == -1 || distancias[f][c] == speed) continue;
                    //saltarse las invalidas
                    if (tablero[f][c].obstacle) continue;
                    //inspeccionar celdas vecinas
                    for (int d = 0; d < df.Length; d++)
                    {
                        int vf = f + df[d];
                        int vc = c + dc[d];
                        //determinar si es un vecino valido
                        if (PosicionValida(size, vf, vc) && !tablero[vf][vc].hasAplayer
                        && !tablero[vf][vc].obstacle && distancias[vf][vc] == -1)
                        {
                            distancias[vf][vc] = distancias[f][c] + 1;
                            huboCambio = true;
                        }
                    }
                }
            }
        } while (huboCambio);

        for (int f = 0; f < size; f++)
        {
            for (int c = 0; c < size; c++)
            {
                if (tablero[f][c].destroyableObs)
                {
                    distancias[f][c] = -1;
                }
            }
        }
        return distancias;
    }
    public static List<List<int>> ReachPointInSubMatriz(List<List<Cell>> tablero, int playerPosF, int playerPosC)
    {
        List<List<int>> distancias = new List<List<int>>();
        int n = tablero[0].Count;
        IniciarDistancias(ref distancias, tablero[0].Count);
        if (playerPosC < (n) / 2 && playerPosF <= (n) / 2)
        {

            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle && !predefinedEmptyCells.Contains((i, j)))
                        distancias[i][j] = 0;

                }
            }

        }
        else if (playerPosC >= (n) / 2 && playerPosF > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle && !predefinedEmptyCells.Contains((i, j)))
                        distancias[i][j] = 0;


                }
            }

        }
        else if (playerPosC < (n) / 2 && playerPosF > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle && !predefinedEmptyCells.Contains((i, j)))
                        distancias[i][j] = 0;


                }
            }

        }
        else
        {
            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle && !predefinedEmptyCells.Contains((i, j)))
                        distancias[i][j] = 0;


                }
            }

        }
        return distancias;

    }
    public static void IniciarDistancias(ref List<List<int>> distancias, int n)
    {
        for (int f = 0; f < n; f++)
        {
            distancias.Add(new List<int>());
            for (int c = 0; c < n; c++)
            {
                distancias[f].Add(-1);
            }
        }
    }
    private static bool PosicionValida(int n, int f, int c)
    {
        return f >= 0 && f < n && c >= 0 && c < n;
    }
    public static void ColorReachCell(List<List<Cell>> grid, List<List<int>> distancias)
    {
        for (int i = 0; i < distancias[0].Count; i++)
            for (int j = 0; j < distancias[0].Count; j++)
            {
                if (distancias[i][j] != -1)
                {
                    grid[i][j].reach = true;

                }
                else
                {
                    grid[i][j].reach = false;

                }
            }


    }
    public void AddTraps()
    {
        //           N   S  E  W
        bool[,] trapFrontier = new bool[gm.n, gm.n];
        if (gm.trapList.Count > 0)
        {
            //recorre todo el tablero
            for (int i = 0; i < gm.n; i++)
            {
                for (int j = 0; j < gm.n; j++)
                {
                    //comprueba que no es una casilla con un obstaculo o una casilla predefinnida como vacia
                    if (!Board.predefinedEmptyCells.Contains((i, j))
                    && !gm.grid[i][j].trap && !grid[i][j].obstacle && !trapFrontier[i, j] && !gm.grid[i][j].destroyableObs)
                    {
                        int r = new Random().Next(0, 5);
                        int trapIndex = new Random().Next(0, gm.trapList.Count);
                        if (r == 1)
                        {
                            List<(int x, int y)> listadehijos = new List<(int x, int y)>();
                            List<Cell> TrapCells = new List<Cell>();
                            bool canPutTrap = true;
                            for (int iT = i; iT < (i + gm.trapList[trapIndex].range); iT++)
                            {
                                for (int jT = j; jT < (j + gm.trapList[trapIndex].range); jT++)
                                {
                                    if (!(PosicionValida(gm.n, iT, jT) && !gm.grid[iT][jT].trap)
                                    )
                                    {
                                        canPutTrap = false;
                                        break;
                                    }

                                }
                            }
                            if (canPutTrap)
                            {
                                for (int iT = i; iT < (i + gm.trapList[trapIndex].range); iT++)
                                {
                                    for (int jT = j; jT < (j + gm.trapList[trapIndex].range); jT++)
                                    {
                                        if ((PosicionValida(gm.n, iT, jT) && !Board.predefinedEmptyCells.Contains((iT, jT)) && !gm.grid[iT][jT].trap && !grid[iT][jT].obstacle)
                                        )
                                        {
                                            TrapCells.Add(grid[iT][jT]);
                                            grid[iT][jT].trap = true;
                                            grid[iT][jT].trapType = gm.trapList[trapIndex];

                                            listadehijos.Add((iT, jT));

                                        }

                                    }
                                }
                            }

                            for (int l = 0; l < listadehijos.Count; l++)
                            {
                                //           N   S  E  W
                                int[] df = { -1, 1, 0, 0 };
                                int[] dc = { 0, 0, 1, -1 };
                                TrapCells[l].childsTrap = listadehijos;
                                for (int d = 0; d < df.Length; d++)
                                {
                                    int x = listadehijos[l].x + df[d];
                                    int y = listadehijos[l].y + dc[d];
                                    //determinar si es un vecino valido
                                    if (PosicionValida(gm.n, x, y) && trapFrontier[x, y] == false)
                                    {
                                        trapFrontier[x, y] = true;
                                    }
                                }

                            }


                        }
                    }
                }
            }

        }
    }
    public void AddPowerUps()
    {
        if (gm.powerList.Count > 0)
        {
            //recorre todo el tablero
            for (int i = 0; i < gm.n; i++)
            {
                for (int j = 0; j < gm.n; j++)
                {
                    //comprueba que no es una casilla con un obstaculo o una casilla predefinnida como vacia
                    if (!Board.predefinedEmptyCells.Contains((i, j))
                    && !gm.grid[i][j].powerUp && !grid[i][j].obstacle && !gm.grid[i][j].trap && !gm.grid[i][j].destroyableObs)
                    {
                        int r = new Random().Next(0, 10);
                        int powerIndex = new Random().Next(0, gm.powerList.Count);
                        if (r == 1)
                        {

                            grid[i][j].powerUp = true;
                            grid[i][j].powerUpType = gm.powerList[powerIndex];



                        }
                    }
                }
            }

        }

    }
    public void AddKey()
    {
        while (true)
        {
            int i = new Random().Next(0, gm.n);
            int j = new Random().Next(0, gm.n);

            if (!Board.predefinedEmptyCells.Contains((i, j))
                && !gm.grid[i][j].powerUp && !grid[i][j].obstacle && !gm.grid[i][j].trap && !gm.grid[i][j].destroyableObs)
            {
                grid[i][j].powerUp = true;
                grid[i][j].powerUpType = gm.TheKey;
                break;

            }

        }

    }
    public void DropKeyIn(int i, int j)
    {
        grid[i][j].powerUp = true;
        grid[i][j].powerUpType = gm.TheKey;

    }
    public static void AddTrapOn(trap trapType, int i, int j, List<List<Cell>> grid)
    {


        List<(int x, int y)> listadehijos = new List<(int x, int y)>();

        for (int iT = i; iT < (i + trapType.range); iT++)
        {
            for (int jT = j; jT < (j + trapType.range); jT++)
            {
                if ((PosicionValida(grid[0].Count, iT, jT) && !grid[iT][jT].obstacle)
                )
                {
                    grid[iT][jT].trap = true;
                    grid[iT][jT].trapType = trapType;

                    listadehijos.Add((iT, jT));

                }

            }
        }

        for (int l = 0; l < listadehijos.Count; l++)
        {
            grid[listadehijos[l].x][listadehijos[l].y].childsTrap.Add(listadehijos[l]);
        }


    }

}