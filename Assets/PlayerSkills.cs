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
        Eren.SwapImg();
        Eren.PowerUp(Eren.power);
        Eren.SpeedUp(2);
    }
    public static void ErenSkillOff(PlayerManeger Eren)
    {
        Eren.SwapImg();
        Eren.PowerDivide(2);
        Eren.SpeedUp(-2);
    }


}