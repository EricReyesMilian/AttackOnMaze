using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FalconSkill", menuName = "Skills/Falcon")]
public class FalconSkill : Skill
{


    public override void Active(PlayerManager player)
    {

        Board.distancia = Board.ReachPointInMap();
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
