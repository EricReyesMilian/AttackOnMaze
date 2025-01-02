using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordEffect", menuName = "AtackOnMaze/PowerUp/Effect/Sword")]
public class SwordPower : PowerEffect
{
    [SerializeField]
    int powerUp = 5;

    public override void Active(PlayerManager player)
    {
        player.PowerUp(powerUp);
    }
}
