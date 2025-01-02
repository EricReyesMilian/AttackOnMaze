using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NullSkill", menuName = "Skills/Null")]
public class NullSkill : Skill
{


    public override void Active(PlayerManager player)
    {
    }
    public override void Pasive(PlayerManager player)
    {
    }
    public override void PasiveOnWalk(PlayerManager player) { }

    public override void Desactive(PlayerManager player)
    {

    }
}
