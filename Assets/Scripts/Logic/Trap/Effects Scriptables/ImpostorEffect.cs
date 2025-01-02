using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImpostorEffect", menuName = "AtackOnMaze/Trap/Effect/ImpostorTrap")]
public class ImpostorTrap : TrapEffect
{
    [SerializeField]

    public override void Active(PlayerManager player)
    {
        player.PowerDivide(2);
    }
}