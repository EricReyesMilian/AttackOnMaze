using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyEffect", menuName = "AtackOnMaze/PowerUp/Effect/Key")]
public class KeyPower : PowerEffect
{
    public override void Active(PlayerManager player)
    {
        player.TakeKey();

    }
}
