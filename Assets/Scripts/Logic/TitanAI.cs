using UnityEngine;
using System.Collections.Generic;

public class TitanAI
{
    List<List<List<int>>> distanciaToDoor = new List<List<List<int>>>();

    public List<(int x, int y)> predefinedAgroCells = new List<(int, int)> { (8, 5), (5, 8), (11, 8), (8, 11) };
    public List<(int x, int y)> predefinedCenterCells = new List<(int, int)> { (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 8), (7, 9) };

    public List<(int x, int y)> DoorCells = new List<(int, int)> { (8, 6), (6, 8), (10, 8), (8, 10) };



    public TitanAI()
    {

        InitDistToDoors();
    }
    void InitDistToDoors()
    {
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
            distanciaToDoor[l] = Board.Lee(Board.grid, predefinedAgroCells[l].x, predefinedAgroCells[l].y, Board.grid.Count * Board.grid.Count);

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
                Board.grid[DoorCells[i].x][DoorCells[i].y].TakeDamage(1);
                return -1;
            }
        }

        switch (titan.play.TitanIQ)
        {
            case 0:
                return IQTonto(titan, targets);
            case 1:
                return IQ1(titan, targets);
            case 2:
                return IQ2(titan, targets);
            case 3:
                return IQ3(titan, targets);
            case 4:
                return IQ4(titan, targets);
            default:
                return Random.Range(0, targets.Count);


        }
    }
    int IQTonto(PlayerManager titan, List<(int x, int y)> targets)
    {
        return Random.Range(0, targets.Count);

    }
    int IQ1(PlayerManager titan, List<(int x, int y)> targets)
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
    int IQ2(PlayerManager titan, List<(int x, int y)> targets)
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (Board.grid[coord.x][coord.y].nearPlayer)
            {
                return targets.IndexOf(coord);
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
    int IQ3(PlayerManager titan, List<(int x, int y)> targets)
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (Board.grid[coord.x][coord.y].nearPlayer)
            {
                if (titan.power >= Board.grid[coord.x][coord.y].NearPlayers[0].power
                && !Board.grid[coord.x][coord.y].NearPlayers[0].play.isTitan)
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
    int IQ4(PlayerManager titan, List<(int x, int y)> targets)
    {
        int max = int.MinValue;
        int min = int.MaxValue;
        int i = 1;
        bool huboCambio = false;
        List<List<int>> distanciaToCenter;
        int agroIndex = 0;
        for (int l = 0; l < 4; l++)
        {
            if (CalcularDistanciaManhattan(titan.Pos, new Vector2(predefinedAgroCells[l].x, predefinedAgroCells[l].y)) < min)
            {
                min = CalcularDistanciaManhattan(titan.Pos, new Vector2(predefinedAgroCells[l].x, predefinedAgroCells[l].y));
                agroIndex = l;
            }


        }
        distanciaToCenter = distanciaToDoor[agroIndex];
        foreach (var coord in targets)
        {
            if (Board.grid[coord.x][coord.y].nearPlayer)
            {
                if (titan.power >= Board.grid[coord.x][coord.y].NearPlayers[0].power
                && !Board.grid[coord.x][coord.y].NearPlayers[0].play.isTitan)
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
        return i;

    }
    public int CalcularDistanciaManhattan(Vector2 punto1, Vector2 punto2)
    {
        int distancia = Mathf.Abs((int)punto1.x - (int)punto2.x) + Mathf.Abs((int)punto1.y - (int)punto2.y);
        return distancia;
    }

}