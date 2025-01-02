using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReinerSkill", menuName = "Skills/Reiner")]
public class ReinerSkill : Skill
{
    public override void Active(PlayerManager player)
    {
        player.titanForm = true;
        Pasive(player);
        AudioManager.speaker.Play(Resources.Load<AudioClip>(player.play.Name));

        player.SwapImg(2);
        player.PowerUp(5);
    }
    public override void PasiveOnWalk(PlayerManager player)
    {
        int n = Board.grid.Count;
        List<(int i, int j)> reachSkillCells = new List<(int i, int j)>();

        {
            bool obs = false;
            for (int i = (int)player.Pos.x + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (Board.grid[i][(int)player.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!Board.grid[i][(int)player.Pos.y].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains((i, (int)player.Pos.y)) && !Board.grid[i][(int)player.Pos.y].hasAplayer)

                            reachSkillCells.Add((i, (int)player.Pos.y));
                        break;
                    }
                }

            }

        }
        {
            bool obs = false;

            for (int i = (int)player.Pos.x - 1; i > 0; i--)
            {
                if (!obs)
                {
                    if (Board.grid[i][(int)player.Pos.y].obstacle)
                    {
                        obs = true;

                    }

                }
                else
                {
                    if (!Board.grid[i][(int)player.Pos.y].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains((i, (int)player.Pos.y)) && !Board.grid[i][(int)player.Pos.y].hasAplayer)
                            reachSkillCells.Add((i, (int)player.Pos.y));
                        break;
                    }
                }
            }
        }
        {
            bool obs = false;
            for (int i = (int)player.Pos.y + 1; i < n - 1; i++)
            {
                if (!obs)
                {
                    if (Board.grid[(int)player.Pos.x][i].obstacle)
                        obs = true;

                }
                else
                {
                    if (!Board.grid[(int)player.Pos.x][i].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains(((int)player.Pos.x, i)) && !Board.grid[(int)player.Pos.x][i].hasAplayer)
                            reachSkillCells.Add(((int)player.Pos.x, i));
                        break;
                    }
                }

            }

        }
        {
            bool obs = false;

            for (int i = (int)player.Pos.y - 1; i > 0; i--)
            {
                if (!obs)
                {
                    if (Board.grid[(int)player.Pos.x][i].obstacle)
                    {
                        obs = true;
                    }
                }
                else
                {
                    if (!Board.grid[(int)player.Pos.x][i].obstacle)
                    {
                        if (!Board.predefinedCenterCells.Contains(((int)player.Pos.x, i)) && !Board.grid[(int)player.Pos.x][i].hasAplayer)
                            reachSkillCells.Add(((int)player.Pos.x, i));
                        break;
                    }
                }
            }
        }

        foreach (var coord in reachSkillCells)
        {

            Board.grid[coord.i][coord.j].special = true;

            Board.distancia[coord.i][coord.j] = 1;

        }
        Board.ColorReachCell();
    }

    public override void Pasive(PlayerManager player)
    {

    }

    public override void Desactive(PlayerManager player)
    {
        player.titanForm = false;
        player.PowerUp(-5);

        player.SwapImg(1);


    }
}
