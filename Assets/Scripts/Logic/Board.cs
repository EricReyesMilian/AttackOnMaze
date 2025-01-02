using System;
using UnityEngine;
using System.Collections.Generic;
public class Board
{
    public static List<List<Cell>> grid = new List<List<Cell>>();
    public static List<(int x, int y)> predefinedEmptyCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15), (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 9), (8, 6), (8, 10), (10, 8), (10, 6), (6, 10), (10, 10) };
    public static List<(int x, int y)> predefinedCenterCells = new List<(int, int)> { (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 8), (7, 9) };
    public static List<(int x, int y)> predefinedObstacleCells = new List<(int, int)> { (6, 9), (6, 7), (7, 10), (7, 6), (9, 6), (10, 7), (9, 10), (10, 9) };
    public static List<(int x, int y)> startCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15) };
    public static List<(int x, int y)> DoorCells = new List<(int, int)> { (8, 6), (6, 8), (10, 8), (8, 10) };
    public static List<List<int>> distancia = new List<List<int>>();
    public static List<List<int>> distanciaToCenter = new List<List<int>>();

    public static void AddCellsToMatriz(int n)
    {
        for (int i = 0; i < n; i++)
        {
            grid.Add(new List<Cell>());
            for (int j = 0; j < n; j++)
            {
                grid[i].Add(new Cell());
                grid[i][j].coord = new Vector2(i, j);

            }
        }
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
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle && !predefinedCenterCells.Contains((i, j)))
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
    public static void ColorReachCell()
    {
        for (int i = 0; i < distancia[0].Count; i++)
            for (int j = 0; j < distancia[0].Count; j++)
            {
                if (distancia[i][j] != -1)
                {
                    grid[i][j].reach = true;

                }
                else
                {
                    grid[i][j].reach = false;

                }
            }


    }
    public static void AddTraps()
    {
        //           N   S  E  W
        bool[,] trapFrontier = new bool[GameManager.gameManeger.n, GameManager.gameManeger.n];
        if (GameManager.gameManeger.trapList.Count > 0)
        {
            //recorre todo el tablero
            for (int i = 0; i < GameManager.gameManeger.n; i++)
            {
                for (int j = 0; j < GameManager.gameManeger.n; j++)
                {
                    //comprueba que no es una casilla con un obstaculo o una casilla predefinnida como vacia
                    if (!Board.predefinedEmptyCells.Contains((i, j))
                    && !grid[i][j].trap && !grid[i][j].obstacle && !trapFrontier[i, j] && !grid[i][j].destroyableObs)
                    {
                        int trapIndex = new System.Random().Next(0, GameManager.gameManeger.trapList.Count);
                        int r = new System.Random().Next(0, GameManager.gameManeger.trapList[trapIndex].rarity);
                        if (r == 1)
                        {
                            List<(int x, int y)> listadehijos = new List<(int x, int y)>();
                            List<Cell> TrapCells = new List<Cell>();
                            bool canPutTrap = true;
                            for (int iT = i; iT < (i + GameManager.gameManeger.trapList[trapIndex].range); iT++)
                            {
                                for (int jT = j; jT < (j + GameManager.gameManeger.trapList[trapIndex].range); jT++)
                                {
                                    if (!(PosicionValida(GameManager.gameManeger.n, iT, jT) && !grid[iT][jT].trap)
                                    )
                                    {
                                        canPutTrap = false;
                                        break;
                                    }

                                }
                            }
                            if (canPutTrap)
                            {
                                for (int iT = i; iT < (i + GameManager.gameManeger.trapList[trapIndex].range); iT++)
                                {
                                    for (int jT = j; jT < (j + GameManager.gameManeger.trapList[trapIndex].range); jT++)
                                    {
                                        if ((PosicionValida(GameManager.gameManeger.n, iT, jT) && !predefinedEmptyCells.Contains((iT, jT)) && !grid[iT][jT].trap && !grid[iT][jT].obstacle)
                                        )
                                        {
                                            TrapCells.Add(grid[iT][jT]);
                                            grid[iT][jT].trap = true;
                                            grid[iT][jT].trapType = GameManager.gameManeger.trapList[trapIndex];

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
                                    if (PosicionValida(GameManager.gameManeger.n, x, y) && trapFrontier[x, y] == false)
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
    public static void AddPowerUps()
    {
        if (GameManager.gameManeger.powerList.Count > 0)
        {
            //recorre todo el tablero
            for (int i = 0; i < GameManager.gameManeger.n; i++)
            {
                for (int j = 0; j < GameManager.gameManeger.n; j++)
                {
                    //comprueba que no es una casilla con un obstaculo o una casilla predefinnida como vacia
                    if (!Board.predefinedEmptyCells.Contains((i, j))
                    && !grid[i][j].powerUp && !grid[i][j].obstacle && !grid[i][j].trap && !grid[i][j].destroyableObs)
                    {
                        int powerIndex = new System.Random().Next(0, GameManager.gameManeger.powerList.Count);
                        int r = new System.Random().Next(0, GameManager.gameManeger.powerList[powerIndex].rarity);
                        if (r == 1)
                        {

                            grid[i][j].powerUp = true;
                            grid[i][j].powerUpType = GameManager.gameManeger.powerList[powerIndex];



                        }
                    }
                }
            }

        }

    }
    public static void AddKey()
    {
        while (true)
        {
            int i = new System.Random().Next(0, GameManager.gameManeger.n);
            int j = new System.Random().Next(0, GameManager.gameManeger.n);

            if (!Board.predefinedEmptyCells.Contains((i, j))
                && !grid[i][j].powerUp && !grid[i][j].obstacle && !grid[i][j].trap && !grid[i][j].destroyableObs)
            {
                grid[i][j].powerUp = true;
                grid[i][j].powerUpType = GameManager.gameManeger.TheKey;
                break;

            }

        }

    }
    public static void DropKeyIn(int i, int j)
    {
        grid[i][j].powerUp = true;
        grid[i][j].powerUpType = GameManager.gameManeger.TheKey;

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
    public static void CloseDoors()
    {
        for (int i = 0; i < DoorCells.Count; i++)
        {
            if (grid[DoorCells[i].x][DoorCells[i].y].endurence > 0)
            {
                grid[DoorCells[i].x][DoorCells[i].y].obstacle = true;

            }

        }


    }
    public static void SetBattleZone(List<PlayerManager> players)
    {
        ClearMatrizNearPlayers();
        for (int p = 0; p < players.Count; p++)
        {
            if (p != GameManager.gameManeger.turn)
            {
                int i = (int)((players[p].Pos.x));
                int j = (int)((players[p].Pos.y));

                if (i > 0)
                {
                    if (Board.grid[i - 1][j].reach)
                    {
                        Board.grid[i - 1][j].nearPlayer = true;
                        Board.grid[i - 1][j].NearPlayers.Add(players[p]);
                    }
                }
                if (j > 0)
                {
                    if (Board.grid[i][j - 1].reach)
                    {
                        Board.grid[i][j - 1].nearPlayer = true;
                        Board.grid[i][j - 1].NearPlayers.Add(players[p]);
                    }



                }
                if (i < GameManager.gameManeger.n - 1)
                {
                    if (Board.grid[i + 1][j].reach)
                    {
                        Board.grid[i + 1][j].nearPlayer = true;
                        Board.grid[i + 1][j].NearPlayers.Add(players[p]);
                    }

                }

                if (j < GameManager.gameManeger.n - 1)
                {
                    if (Board.grid[i][j + 1].reach)
                    {
                        Board.grid[i][j + 1].nearPlayer = true;
                        Board.grid[i][j + 1].NearPlayers.Add(players[p]);
                    }

                }
            }
        }
    }
    public static bool ReachPoint(Vector2 target)
    {
        return Board.grid[(int)target.x][(int)target.y].reach;
    }
    public static void ClearMatrizNearPlayers()
    {
        for (int i = 0; i < GameManager.gameManeger.n; i++)
        {
            for (int j = 0; j < GameManager.gameManeger.n; j++)
            {
                Board.grid[i][j].NearPlayers.Clear();

            }
        }
    }
    public static void PlacePlayerIn(int i, int j, int index)
    {
        Board.grid[i][j].hasAplayer = true;
        Board.grid[i][j].player = GameManager.gameManeger.players[index];

    }
    public static void UnPlacePlayer(int i, int j)
    {
        Board.grid[i][j].hasAplayer = false;
        Board.grid[i][j].player = null;

    }
    public static void InitReachCell()
    {
        for (int i = 0; i < GameManager.gameManeger.n; i++)
        {
            for (int j = 0; j < GameManager.gameManeger.n; j++)
            {
                Board.grid[i][j].reach = false;
                Board.grid[i][j].nearPlayer = false;
                Board.grid[i][j].special = false;
            }
        }

    }
    public static void OpenDoorsToSavior()
    {
        for (int i = 0; i < DoorCells.Count; i++)
        {
            grid[DoorCells[i].x][DoorCells[i].y].obstacle = false;


        }
    }
    public static bool isTrap(Vector2 target)
    {
        return grid[(int)target.x][(int)target.y].trap && grid[(int)target.x][(int)target.y].enableTrap;
    }
    public static bool isPowerUp(Vector2 target)
    {
        return grid[(int)target.x][(int)target.y].powerUp;
    }
    public static void AssignDestroyableWalls()
    {
        for (int i = 0; i < DoorCells.Count; i++)
        {
            grid[DoorCells[i].x][DoorCells[i].y].obstacle = true;
            grid[DoorCells[i].x][DoorCells[i].y].destroyableObs = true;

        }

    }
    public static void SetDist()
    {
        for (int i = 0; i < GameManager.gameManeger.n; i++)
        {
            for (int j = 0; j < GameManager.gameManeger.n; j++)
            {
                distancia[i][j] = -1;
            }
        }
    }
    public static void SavePath(Vector2 target, Vector2 initPos)
    {
        GameManager.gameManeger.wayToPoint.Add(target);
        if (target != initPos && Board.distancia[(int)target.x][(int)target.y] != 0)
        {
            bool taken = false;
            if (target.y + 1 < GameManager.gameManeger.n && !taken && Board.distancia[(int)target.x][(int)target.y + 1] - Board.distancia[(int)target.x][(int)target.y] == -1)
            {
                SavePath(new Vector2(target.x, target.y + 1), initPos);
                taken = true;
            }
            if (target.y - 1 >= 0 && !taken && Board.distancia[(int)target.x][(int)target.y - 1] - Board.distancia[(int)target.x][(int)target.y] == -1)
            {
                SavePath(new Vector2(target.x, target.y - 1), initPos);
                taken = true;
            }
            if (target.x + 1 < GameManager.gameManeger.n && !taken && Board.distancia[(int)target.x + 1][(int)target.y] - Board.distancia[(int)target.x][(int)target.y] == -1)
            {
                SavePath(new Vector2(target.x + 1, target.y), initPos);
                taken = true;
            }
            if (target.x - 1 >= 0 && !taken && Board.distancia[(int)target.x - 1][(int)target.y] - Board.distancia[(int)target.x][(int)target.y] == -1)
            {
                SavePath(new Vector2(target.x - 1, target.y), initPos);
                taken = true;
            }
        }
    }

}