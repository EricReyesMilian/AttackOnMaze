using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArminSkill", menuName = "Skills/Armin")]
public class ArminSkill : Skill
{

    [SerializeField]
    int range = 2;
    public override void Active(PlayerManager player)
    {
        player.titanForm = true;
        player.SwapImg(2);
        player.PowerUp(20);
        player.SpeedUpNormalize(2, player.TransformTime);
        OnActive(player);

    }
    public override void PasiveOnWalk(PlayerManager player) { }

    public void OnActive(PlayerManager player)
    {
        for (int i = (int)player.Pos.x - range; i < player.Pos.x + range; i++)
        {
            for (int j = (int)player.Pos.y - range; j < player.Pos.y + range; j++)
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
    public override void Pasive(PlayerManager player)
    {
    }

    public override void Desactive(PlayerManager player)
    {
        player.titanForm = false;
        player.PowerUp(-20);

        player.SwapImg(1);
    }
}
