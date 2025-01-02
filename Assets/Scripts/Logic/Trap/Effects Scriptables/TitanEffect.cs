using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitanEffect", menuName = "AtackOnMaze/Trap/Effect/Titan")]
public class TitanTrap : TrapEffect
{
    [SerializeField]
    public override void Active(PlayerManager player)
    {
        player.isTitan = true;
        player.sick = true;
    }
}