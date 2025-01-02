using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SerumEffect", menuName = "AtackOnMaze/PowerUp/Effect/Serum")]
public class SerumPower : PowerEffect
{
    public override void Active(PlayerManager player)
    {
        player.RemoveCooldown();

    }
}
