using UnityEngine;
using System.Collections.Generic;

public class TitanAI
{
    List<List<Cell>> grid;
    List<List<int>> distancia;
    List<List<int>> distanciaToCenter;


    public TitanAI(List<List<Cell>> grid, List<List<int>> distancia, List<List<int>> distanciaToCenter)
    {
        this.grid = grid;
        this.distancia = distancia;
        this.distanciaToCenter = distanciaToCenter;
    }
    public int GetMove(PlayerManager titan, List<(int x, int y)> targets)
    {
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

            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
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
            if (grid[coord.x][coord.y].nearPlayer)
            {
                return targets.IndexOf(coord);
            }
            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
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
            if (grid[coord.x][coord.y].nearPlayer && titan.power >= grid[coord.x][coord.y].player.power)
            {
                return targets.IndexOf(coord);
            }
            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
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
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (grid[coord.x][coord.y].nearPlayer)
            {
                if (titan.power >= grid[coord.x][coord.y].NearPlayers[0].power
                && !grid[coord.x][coord.y].NearPlayers[0].play.isTitan)
                {
                    huboCambio = true;
                    return targets.IndexOf(coord);

                }
            }
            if (distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y];
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