using UnityEngine;
using System.Collections.Generic;

public class TitanAI
{
    public List<List<List<int>>> distanciaToDoor = new List<List<List<int>>>();

    public List<(int x, int y)> predefinedAgroCells = new List<(int, int)> { (8, 5), (5, 8), (11, 8), (8, 11) };
    public List<(int x, int y)> predefinedCenterCells = new List<(int, int)> { (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 8), (7, 9) };

    public List<(int x, int y)> DoorCells = new List<(int, int)> { (8, 6), (6, 8), (10, 8), (8, 10) };
    public List<List<int>> distanciaToCenter = new List<List<int>>();



    public TitanAI()
    {
        InitDistToDoors();
    }

    void InitDistToDoors()
    {
        distanciaToDoor.Clear();
        for (int l = 0; l < 4; l++)
        {
            distanciaToDoor.Add(new List<List<int>>());
            for (int i = 0; i < Board.grid.Count; i++)
            {
                distanciaToDoor[l].Add(new List<int>());
                for (int j = 0; j < Board.grid.Count; j++)
                {
                    distanciaToDoor[l][i].Add(-1);
                }
            }


        }
        for (int l = 0; l < 4; l++)
        {
            distanciaToDoor[l] = Board.Lee(predefinedAgroCells[l].x, predefinedAgroCells[l].y, Board.grid[0].Count * Board.grid[0].Count, true);

        }

    }
    public int GetMove(PlayerManager titan, List<(int x, int y)> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            for (int j = 0; j < predefinedCenterCells.Count; j++)
            {
                if (targets[i] == predefinedCenterCells[j])
                {
                    return i;
                }
            }
        }
        for (int i = 0; i < DoorCells.Count; i++)
        {
            if (((int)titan.Pos.x, (int)titan.Pos.y) == predefinedAgroCells[i])
            {
                if (Board.grid[DoorCells[i].x][DoorCells[i].y].obstacle)
                {
                    Board.grid[DoorCells[i].x][DoorCells[i].y].TakeDamage(1);
                    AudioManager.speaker.Play(Resources.Load<AudioClip>("wall"));

                    return -1;

                }
            }
        }
        InitDistToDoors();
        switch (titan.TitanIQ)
        {
            case 0:
                return IQTonto(titan, targets);
            case 1:
                return IQ1(titan, targets);
            case 2:
                return IQ2(titan, targets);
            case 3:
                return IQ3(titan, targets);
            // case 4:
            //     return IQ4(titan, targets);
            default:
                return IQ3(titan, targets);

        }
    }
    int IQTonto(PlayerManager titan, List<(int x, int y)> targets)//se mueve y actua aleatoriamente
    {
        return Random.Range(0, targets.Count);

    }
    int IQ1(PlayerManager titan, List<(int x, int y)> targets)//se mueve rapidamente con libertad ignorando a su entorno
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {

            if (Board.distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = Board.distancia[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    int IQ2(PlayerManager titan, List<(int x, int y)> targets)//se mueve rapidamente con libertad y se distrae facilmente
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (Board.grid[coord.x][coord.y].nearPlayer)
            {
                if (!Board.grid[coord.x][coord.y].NearPlayers[0].isTitan)
                {
                    huboCambio = true;
                    return targets.IndexOf(coord);

                }
            }

            if (Board.distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = Board.distancia[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    // int IQ3(PlayerManager titan, List<(int x, int y)> targets)//avanza con ligeras distracciones hacia las puertas de las murallas
    // {
    //     int max = int.MinValue;
    //     int min = int.MaxValue;
    //     int i = 0;
    //     bool huboCambio = false;
    //     int agroIndex = 0;

    //     for (int l = 0; l < 4; l++)
    //     {
    //         int min4 = int.MaxValue;
    //         int[] df = { -1, 1, 0, 0 };
    //         int[] dc = { 0, 0, 1, -1 };

    //         for (int r = 0; r < 4; r++)
    //         {
    //             if (((int)titan.Pos.x + df[r]) < Board.grid[0].Count - 1 && ((int)titan.Pos.x + df[r]) >= 0
    //              && ((int)titan.Pos.y + dc[r]) < Board.grid[0].Count - 1 && ((int)titan.Pos.y + dc[r]) >= 0)
    //             {
    //                 if (distanciaToDoor[l][(int)titan.Pos.x + df[r]][(int)titan.Pos.y + dc[r]] < min4)
    //                 {
    //                     if (Board.grid[predefinedAgroCells[l].x][predefinedAgroCells[l].y].hasAplayer &&
    //                     Board.grid[predefinedAgroCells[l].x][predefinedAgroCells[l].y].player.isTitan)
    //                     {

    //                     }
    //                     else
    //                     {
    //                         if (distanciaToDoor[l][(int)titan.Pos.x + df[r]][(int)titan.Pos.y + dc[r]] >= 0)
    //                             min4 = distanciaToDoor[l][(int)titan.Pos.x + df[r]][(int)titan.Pos.y + dc[r]];

    //                     }
    //                 }

    //             }
    //         }
    //         if (min4 < min)
    //         {
    //             min = min4;
    //             agroIndex = l;
    //         }


    //     }
    //     Board.distanciaToCenterAux = distanciaToDoor[agroIndex];
    //     distanciaToCenter = distanciaToDoor[agroIndex];
    //     foreach (var coord in targets)
    //     {
    //         if (Board.grid[coord.x][coord.y].nearPlayer)
    //         {
    //             if (!Board.grid[coord.x][coord.y].NearPlayers[0].isTitan)
    //             {
    //                 huboCambio = true;
    //                 return targets.IndexOf(coord);

    //             }
    //         }
    //         if (Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y] >= max)
    //         {
    //             if (!titan.lastMove.Contains(coord))
    //             {
    //                 i = targets.IndexOf(coord);
    //                 max = Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y];
    //                 huboCambio = true;
    //             }
    //         }

    //     }
    //     if (!huboCambio)
    //     {
    //         titan.lastMove.Clear();
    //     }
    //     return Random.Range(0, targets.Count);

    // }
    int IQ3(PlayerManager titan, List<(int x, int y)> targets)
    {
        int max = int.MinValue;
        int min = int.MaxValue;
        int i = 0;
        bool huboCambio = false;
        int agroIndex = 0;

        for (int l = 0; l < 4; l++)
        {
            int min4 = int.MaxValue;
            int[] df = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, 1, -1 };

            for (int r = 0; r < 4; r++)
            {
                int newX = (int)titan.Pos.x + df[r];
                int newY = (int)titan.Pos.y + dc[r];

                if (newX < Board.grid[0].Count && newX >= 0 && newY < Board.grid[0].Count && newY >= 0)
                {
                    if (distanciaToDoor[l][newX][newY] < min4)
                    {
                        if (!(Board.grid[predefinedAgroCells[l].x][predefinedAgroCells[l].y].hasAplayer &&
                              Board.grid[predefinedAgroCells[l].x][predefinedAgroCells[l].y].player.isTitan))
                        {
                            if (distanciaToDoor[l][newX][newY] >= 0)
                            {
                                min4 = distanciaToDoor[l][newX][newY];
                            }
                        }
                    }
                }
            }

            if (min4 < min)
            {
                min = min4;
                agroIndex = l;
            }
        }

        Board.distanciaToCenterAux = distanciaToDoor[agroIndex];
        distanciaToCenter = distanciaToDoor[agroIndex];

        foreach (var coord in targets)
        {
            if (Board.grid[coord.x][coord.y].nearPlayer)
            {
                if (!Board.grid[coord.x][coord.y].NearPlayers[0].isTitan)
                {
                    huboCambio = true;
                    return targets.IndexOf(coord);
                }
            }

            if (Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y];
                    huboCambio = true;
                }
            }
        }

        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }

        return UnityEngine.Random.Range(0, targets.Count);
    }

    int IQ4(PlayerManager titan, List<(int x, int y)> targets)//avanza sin distraccion hacia el centro de las murallas si hay un agujero
    {
        int max = int.MinValue;
        int min = int.MaxValue;
        int i = 0;
        bool huboCambio = false;
        int agroIndex = 0;
        for (int l = 0; l < 4; l++)
        {
            if (distanciaToDoor[l][(int)titan.Pos.x][(int)titan.Pos.y] < min)
            {
                min = distanciaToDoor[l][(int)titan.Pos.x][(int)titan.Pos.y];
                agroIndex = l;
            }


        }
        bool Enter = false;
        for (int p = 0; p < DoorCells.Count; p++)
        {
            if (!Board.grid[DoorCells[p].x][DoorCells[p].y].obstacle)
            {
                distanciaToCenter = Board.Lee(8, 8, Board.grid[0].Count * Board.grid[0].Count, true);
                Enter = true;
                break;
            }
        }

        if (!Enter)
        {
            Board.distanciaToCenterAux = distanciaToDoor[agroIndex];
            distanciaToCenter = distanciaToDoor[agroIndex];

        }
        foreach (var coord in targets)
        {

            if (Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = Board.distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }


}