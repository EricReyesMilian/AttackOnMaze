using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeviSkill", menuName = "Skills/Levi")]
public class LeviSkill : Skill
{
    [SerializeField]

    public override void Active(PlayerManager player)
    {
        int n = Board.grid[0].Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        if ((int)player.Pos.x + 2 <= n && !Board.grid[(int)player.Pos.x + 1][(int)player.Pos.y].obstacle)
        {
            for (int i = (int)player.Pos.x + 2; i < n; i++)
            {

                if (Board.grid[i][(int)player.Pos.y].obstacle)
                {
                    if (!Board.grid[i - 1][(int)player.Pos.y].hasAplayer)
                        reachSkillCells.Add((i - 1, (int)player.Pos.y));
                    break;
                }

            }

        }
        if ((int)player.Pos.x - 2 >= 0 && !Board.grid[(int)player.Pos.x - 1][(int)player.Pos.y].obstacle)
        {
            for (int i = (int)player.Pos.x - 2; i >= 0; i--)
            {
                if (Board.grid[i][(int)player.Pos.y].obstacle)
                {
                    if (!Board.grid[i + 1][(int)player.Pos.y].hasAplayer)
                        reachSkillCells.Add((i + 1, (int)player.Pos.y));

                    break;
                }

            }


        }

        if ((int)player.Pos.y + 2 <= n && !Board.grid[(int)player.Pos.x][(int)player.Pos.y + 1].obstacle)
        {
            for (int i = (int)player.Pos.y + 2; i < n; i++)
            {
                if (Board.grid[(int)player.Pos.x][i].obstacle)
                {
                    if (!Board.grid[(int)player.Pos.x][i - 1].hasAplayer)

                        reachSkillCells.Add(((int)player.Pos.x, i - 1));

                    break;
                }

            }

        }
        if ((int)player.Pos.y - 2 >= 0 && !Board.grid[(int)player.Pos.x][(int)player.Pos.y - 1].obstacle)
        {
            for (int i = (int)player.Pos.y - 2; i >= 0; i--)
            {
                if (Board.grid[(int)player.Pos.x][i].obstacle)
                {
                    if (!Board.grid[(int)player.Pos.x][i + 1].hasAplayer)

                        reachSkillCells.Add(((int)player.Pos.x, i + 1));

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
    public override void Pasive(PlayerManager player)
    {
    }
    public override void PasiveOnWalk(PlayerManager player) { }

    public override void Desactive(PlayerManager player)
    {


    }
}
