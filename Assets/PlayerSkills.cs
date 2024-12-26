using System.Collections;
using System.Collections.Generic;

static class PlayerSkills
{
    public static void LeviSkillEf(PlayerManeger Levi, List<List<Cell>> matriz, List<List<int>> distancia)
    {
        int n = matriz[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        if ((int)Levi.Pos.x + 2 <= n && !matriz[(int)Levi.Pos.x + 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x + 2; i < n; i++)
            {

                if (matriz[i][(int)Levi.Pos.y].obstacle)
                {

                    reachSkillCells.Add((i - 1, (int)Levi.Pos.y));
                    break;
                }

            }

        }
        if ((int)Levi.Pos.x - 2 >= 0 && !matriz[(int)Levi.Pos.x - 1][(int)Levi.Pos.y].obstacle)
        {
            for (int i = (int)Levi.Pos.x - 2; i >= 0; i--)
            {
                if (matriz[i][(int)Levi.Pos.y].obstacle)
                {
                    reachSkillCells.Add((i + 1, (int)Levi.Pos.y));

                    break;
                }

            }


        }

        if ((int)Levi.Pos.y + 2 <= n && !matriz[(int)Levi.Pos.x][(int)Levi.Pos.y + 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y + 2; i < n; i++)
            {
                if (matriz[(int)Levi.Pos.x][i].obstacle)
                {
                    reachSkillCells.Add(((int)Levi.Pos.x, i - 1));

                    break;
                }

            }

        }
        if ((int)Levi.Pos.y - 2 >= 0 && !matriz[(int)Levi.Pos.x][(int)Levi.Pos.y - 1].obstacle)
        {
            for (int i = (int)Levi.Pos.y - 2; i >= 0; i--)
            {
                if (matriz[(int)Levi.Pos.x][i].obstacle)
                {

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
            matriz[coord.i][coord.j].special = true;

            distancia[coord.i][coord.j] = 0;

        }

        BoardManeger.ColorReachCell(matriz, distancia);


    }

    public static void ErenSkillEf(PlayerManeger Eren)
    {
        Eren.SwapImg(2);
        Eren.PowerUp(Eren.power, Eren.TransformTime + 1);
        Eren.SpeedUp(2, Eren.TransformTime + 1);
    }
    public static void ErenSkillOff(PlayerManeger Eren)
    {
        Eren.SwapImg(1);

    }

    public static void ReinerSkillEf(PlayerManeger Reiner, List<List<Cell>> matriz, List<List<int>> distancia, List<(int x, int y)> predefinedEmptyCells)
    {
        ReinerRun(Reiner, matriz, distancia, predefinedEmptyCells);
        Reiner.SwapImg(2);
        Reiner.PowerUp(5, Reiner.TransformTime);
    }
    public static void ReinerSkillOff(PlayerManeger Reiner)
    {
        Reiner.SwapImg(1);

    }
    public static void ReinerRun(PlayerManeger Reiner, List<List<Cell>> matriz, List<List<int>> distancia, List<(int x, int y)> predefinedEmptyCells)
    {
        int n = matriz[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        {
            bool obs = false;
            for (int i = (int)Reiner.Pos.x + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (matriz[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!matriz[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains((i, (int)Reiner.Pos.y)) && !matriz[i][(int)Reiner.Pos.y].hasAplayer)
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
                    if (matriz[i][(int)Reiner.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!matriz[i][(int)Reiner.Pos.y].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains((i, (int)Reiner.Pos.y)) && !matriz[i][(int)Reiner.Pos.y].hasAplayer)

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
                    if (matriz[(int)Reiner.Pos.x][i].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!matriz[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains(((int)Reiner.Pos.x, i)) && !matriz[i][(int)Reiner.Pos.y].hasAplayer)

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
                    if (matriz[(int)Reiner.Pos.x][i].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!matriz[(int)Reiner.Pos.x][i].obstacle)
                    {
                        if (!predefinedEmptyCells.Contains(((int)Reiner.Pos.x, i)) && !matriz[i][(int)Reiner.Pos.y].hasAplayer)

                            reachSkillCells.Add(((int)Reiner.Pos.x, i));
                        break;
                    }
                }


            }

        }

        foreach (var coord in reachSkillCells)
        {

            matriz[coord.i][coord.j].special = true;

            distancia[coord.i][coord.j] = 0;

        }

        BoardManeger.ColorReachCell(matriz, distancia);


    }
    public static void ArminSkillEf(PlayerManeger Armin, List<List<Cell>> matriz, List<(int x, int y)> predefinedObstacleCells)
    {
        ArminExplotion(Armin, matriz, predefinedObstacleCells);
        Armin.SwapImg(2);
        Armin.PowerUp(20, Armin.TransformTime);
        Armin.SpeedUpNormalize(2, Armin.TransformTime);
    }
    public static void ArminExplotion(PlayerManeger Armin, List<List<Cell>> matriz, List<(int x, int y)> predefinedObstacleCells)
    {
        for (int i = (int)Armin.Pos.x - 3; i < Armin.Pos.x + 3; i++)
        {
            for (int j = (int)Armin.Pos.y - 3; j < Armin.Pos.y + 3; j++)
            {
                if (i >= 0 && i < matriz.Count && j >= 0 && j < matriz.Count)
                {
                    if (matriz[i][j].obstacle && !matriz[i][j].destroyableObs && !predefinedObstacleCells.Contains((i, j)))
                    {
                        matriz[i][j].obstacle = false;
                    }

                }
            }
        }
    }
    public static void ArminSkillOff(PlayerManeger Armin)
    {
        Armin.SwapImg(1);

    }


}