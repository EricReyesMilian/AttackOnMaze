using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikesEffect", menuName = "AtackOnMaze/Trap/Effect/Spikes")]
public class SpikesTrap : TrapEffect
{
    [SerializeField]
    public int amount = 1;
    public override void Active(PlayerManager player)
    {
        player.PowerUp(-amount);
    }
}