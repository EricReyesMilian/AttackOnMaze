using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColdTrapEffect", menuName = "AtackOnMaze/Trap/Effect/ColdTrap")]
public class ColdTrap : TrapEffect
{
    [SerializeField]

    public override void Active(PlayerManager player)
    {
        player.ResetCooldown();
    }
}