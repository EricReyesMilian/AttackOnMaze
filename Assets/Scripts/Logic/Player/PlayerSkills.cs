using System.Collections;
using System.Collections.Generic;

static class PlayerSkills
{
    public static void ZekeSkillEf(PlayerManager Zeke, List<List<Cell>> grid, List<List<int>> distancia)
    {
        Zeke.titanForm = true;
        Zeke.SwapImg(2);
        Zeke.PowerUp(5);
    }
    public static void ZekeSkillOff(PlayerManager Zeke)
    {

        Zeke.titanForm = false;
        Zeke.PowerUp(-5);

        Zeke.SwapImg(1);

    }
    public static void LeviSkillEf(PlayerManager Levi, List<List<Cell>> grid, List<List<int>> distancia)
    {
        int n = grid[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        if ((int)Levi.Pos.x + 2 <= n && !grid[(int)Levi.Pos.x + 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x + 2; i < n; i++)
            {

                if (grid[i][(int)Levi.Pos.y].obstacle)
                {
                    if (!grid[i - 1][(int)Levi.Pos.y].hasAplayer)
                        reachSkillCells.Add((i - 1, (int)Levi.Pos.y));
                    break;
                }

            }

        }
        if ((int)Levi.Pos.x - 2 >= 0 && !grid[(int)Levi.Pos.x - 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x - 2; i >= 0; i--)
            {
                if (grid[i][(int)Levi.Pos.y].obstacle)
                {
                    if (!grid[i + 1][(int)Levi.Pos.y].hasAplayer)
                        reachSkillCells.Add((i + 1, (int)Levi.Pos.y));

                    break;
                }

            }


        }

        if ((int)Levi.Pos.y + 2 <= n && !grid[(int)Levi.Pos.x][(int)Levi.Pos.y + 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y + 2; i < n; i++)
            {
                if (grid[(int)Levi.Pos.x][i].obstacle)
                {
                    if (!grid[(int)Levi.Pos.x][i - 1].hasAplayer)

                        reachSkillCells.Add(((int)Levi.Pos.x, i - 1));

                    break;
                }

            }

        }
        if ((int)Levi.Pos.y - 2 >= 0 && !grid[(int)Levi.Pos.x][(int)Levi.Pos.y - 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y - 2; i >= 0; i--)
            {
                if (grid[(int)Levi.Pos.x][i].obstacle)
                {
                    if (!grid[(int)Levi.Pos.x][i + 1].hasAplayer)

                        reachSkillCells.Add(((int)Levi.Pos.x, i + 1));

                    break;
                }

            }

        }
        if (reachSkillCells.Count > 0)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    distancia[i][j] = -1;

                }
            }

        }
        foreach (var coord in reachSkillCells)
        {
            grid[coord.i][coord.j].special = true;

            distancia[coord.i][coord.j] = 0;

        }

        Board.ColorReachCell(grid, distancia);


    }

    public static void ErenSkillEf(PlayerManager Eren)
    {
        Eren.titanForm = true;
        Eren.SwapImg(2);
        Eren.PowerUp(Eren.power);
        Eren.SpeedUp(2);
    }
    public static void ErenSkillOff(PlayerManager Eren)
    {
        Eren.titanForm = false;
        Eren.PowerUp(-Eren.power / 2);
        Eren.SpeedUp(-2);

        Eren.SwapImg(1);

    }

    public static void ReinerSkillEf(PlayerManager Reiner, List<List<Cell>> grid, List<List<int>> distancia, List<(int x, int y)> predefinedEmptyCells)
    {
        Reiner.titanForm = true;
        ReinerRun(Reiner, grid, distancia, predefinedEmptyCells);
        Reiner.SwapImg(2);
        Reiner.PowerUp(5);
    }
    public static void ReinerSkillOff(PlayerManager Reiner)
    {
        Reiner.titanForm = false;
        Reiner.PowerUp(-5);

        Reiner.SwapImg(1);

    }
    public static void ReinerRun(PlayerManager Reiner, List<List<Cell>> grid, List<List<int>> distancia, List<(int x, int y)> predefinedEmptyCells)
    {
        int n = grid[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        {
            bool obs = false;
            for (int i = (int)Reiner.Pos.x + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains((i, (int)Reiner.Pos.y)) && !grid[i][(int)Reiner.Pos.y].hasAplayer)

                            reachSkillCells.Add((i, (int)Reiner.Pos.y));
                        break;
                    }
                }

            }

        }
        {
            bool obs = false;

            for (int i = (int)Reiner.Pos.x - 1; i > 0; i--)
            {
                if (!obs)
                {
                    if (grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains((i, (int)Reiner.Pos.y)) && !grid[i][(int)Reiner.Pos.y].hasAplayer)
                            reachSkillCells.Add((i, (int)Reiner.Pos.y));
                        break;
                    }
                }
            }
        }
        {
            bool obs = false;
            for (int i = (int)Reiner.Pos.y + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (grid[(int)Reiner.Pos.x][i].obstacle)
                        obs = true;

                }
                else
                {
                    if (!grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains(((int)Reiner.Pos.x, i)) && !grid[i][(int)Reiner.Pos.y].hasAplayer)
                            reachSkillCells.Add(((int)Reiner.Pos.x, i));
                        break;
                    }
                }

            }

        }
        {
            bool obs = false;

            for (int i = (int)Reiner.Pos.y - 1; i > 0; i--)
            {
                if (!obs)
                {
                    if (grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        obs = true;
                    }
                }
                else
                {
                    if (!grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains(((int)Reiner.Pos.x, i)) && !grid[i][(int)Reiner.Pos.y].hasAplayer)
                            reachSkillCells.Add(((int)Reiner.Pos.x, i));
                        break;
                    }
                }
            }
        }

        foreach (var coord in reachSkillCells)
        {

            grid[coord.i][coord.j].special = true;

            distancia[coord.i][coord.j] = 0;

        }
        Board.ColorReachCell(grid, distancia);
    }
    public static void ArminSkillEf(PlayerManager Armin, List<List<Cell>> grid, List<(int x, int y)> predefinedObstacleCells)
    {
        Armin.titanForm = true;
        ArminExplotion(Armin, grid, predefinedObstacleCells);
        Armin.SwapImg(2);
        Armin.PowerUp(20);
        Armin.SpeedUpNormalize(2, Armin.TransformTime);
    }
    public static void ArminExplotion(PlayerManager Armin, List<List<Cell>> grid, List<(int x, int y)> predefinedObstacleCells)
    {
        int range = 2;
        for (int i = (int)Armin.Pos.x - range; i < Armin.Pos.x + range; i++)
        {
            for (int j = (int)Armin.Pos.y - range; j < Armin.Pos.y + range; j++)
            {
                if (i >= 0 && i < grid.Count && j >= 0 && j < grid.Count)
                {
                    if (grid[i][j].obstacle && !grid[i][j].destroyableObs && !predefinedObstacleCells.Contains((i, j)))
                    {
                        grid[i][j].obstacle = false;
                    }

                }
            }
        }
    }
    public static void ArminSkillOff(PlayerManager Armin)
    {
        Armin.titanForm = false;
        Armin.PowerUp(-20);

        Armin.SwapImg(1);

    }


}