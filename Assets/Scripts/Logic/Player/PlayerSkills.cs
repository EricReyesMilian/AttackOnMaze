using System.Collections;
using System.Collections.Generic;

static class PlayerSkills
{
    public static void ZekeSkillEf(PlayerManager Zeke)
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
    public static void LeviSkillEf(PlayerManager Levi)
    {
        int n = Board.grid[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        if ((int)Levi.Pos.x + 2 <= n && !Board.grid[(int)Levi.Pos.x + 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x + 2; i < n; i++)
            {

                if (Board.grid[i][(int)Levi.Pos.y].obstacle)
                {
                    if (!Board.grid[i - 1][(int)Levi.Pos.y].hasAplayer)
                        reachSkillCells.Add((i - 1, (int)Levi.Pos.y));
                    break;
                }

            }

        }
        if ((int)Levi.Pos.x - 2 >= 0 && !Board.grid[(int)Levi.Pos.x - 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x - 2; i >= 0; i--)
            {
                if (Board.grid[i][(int)Levi.Pos.y].obstacle)
                {
                    if (!Board.grid[i + 1][(int)Levi.Pos.y].hasAplayer)
                        reachSkillCells.Add((i + 1, (int)Levi.Pos.y));

                    break;
                }

            }


        }

        if ((int)Levi.Pos.y + 2 <= n && !Board.grid[(int)Levi.Pos.x][(int)Levi.Pos.y + 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y + 2; i < n; i++)
            {
                if (Board.grid[(int)Levi.Pos.x][i].obstacle)
                {
                    if (!Board.grid[(int)Levi.Pos.x][i - 1].hasAplayer)

                        reachSkillCells.Add(((int)Levi.Pos.x, i - 1));

                    break;
                }

            }

        }
        if ((int)Levi.Pos.y - 2 >= 0 && !Board.grid[(int)Levi.Pos.x][(int)Levi.Pos.y - 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y - 2; i >= 0; i--)
            {
                if (Board.grid[(int)Levi.Pos.x][i].obstacle)
                {
                    if (!Board.grid[(int)Levi.Pos.x][i + 1].hasAplayer)

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
                    Board.distancia[i][j] = -1;

                }
            }

        }
        foreach (var coord in reachSkillCells)
        {
            Board.grid[coord.i][coord.j].special = true;

            Board.distancia[coord.i][coord.j] = 0;

        }

        Board.ColorReachCell();


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

    public static void ReinerSkillEf(PlayerManager Reiner)
    {
        Reiner.titanForm = true;
        ReinerRun(Reiner);
        Reiner.SwapImg(2);
        Reiner.PowerUp(5);
    }
    public static void ReinerSkillOff(PlayerManager Reiner)
    {
        Reiner.titanForm = false;
        Reiner.PowerUp(-5);

        Reiner.SwapImg(1);

    }
    public static void ReinerRun(PlayerManager Reiner)
    {
        int n = Board.grid.Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        {
            bool obs = false;
            for (int i = (int)Reiner.Pos.x + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (Board.grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!Board.grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains((i, (int)Reiner.Pos.y)) && !Board.grid[i][(int)Reiner.Pos.y].hasAplayer)

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
                    if (Board.grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!Board.grid[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains((i, (int)Reiner.Pos.y)) && !Board.grid[i][(int)Reiner.Pos.y].hasAplayer)
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
                    if (Board.grid[(int)Reiner.Pos.x][i].obstacle)
                        obs = true;

                }
                else
                {
                    if (!Board.grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains(((int)Reiner.Pos.x, i)) && !Board.grid[(int)Reiner.Pos.x][i].hasAplayer)
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
                    if (Board.grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        obs = true;
                    }
                }
                else
                {
                    if (!Board.grid[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains(((int)Reiner.Pos.x, i)) && !Board.grid[(int)Reiner.Pos.x][i].hasAplayer)
                            reachSkillCells.Add(((int)Reiner.Pos.x, i));
                        break;
                    }
                }
            }
        }

        foreach (var coord in reachSkillCells)
        {

            Board.grid[coord.i][coord.j].special = true;

            Board.distancia[coord.i][coord.j] = 0;

        }
        Board.ColorReachCell();
    }
    public static void ArminSkillEf(PlayerManager Armin)
    {
        Armin.titanForm = true;
        ArminExplotion(Armin);
        Armin.SwapImg(2);
        Armin.PowerUp(20);
        Armin.SpeedUpNormalize(2, Armin.TransformTime);
    }
    public static void ArminExplotion(PlayerManager Armin)
    {
        int range = 2;
        for (int i = (int)Armin.Pos.x - range; i < Armin.Pos.x + range; i++)
        {
            for (int j = (int)Armin.Pos.y - range; j < Armin.Pos.y + range; j++)
            {
                if (i >= 0 && i < Board.grid.Count && j >= 0 && j < Board.grid.Count)
                {
                    if (Board.grid[i][j].obstacle && !Board.grid[i][j].destroyableObs && !Board.predefinedObstacleCells.Contains((i, j)))
                    {
                        Board.grid[i][j].obstacle = false;
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