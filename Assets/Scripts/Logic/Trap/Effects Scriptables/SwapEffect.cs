using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwapEffect", menuName = "AtackOnMaze/Trap/Effect/Swap")]
public class SwapTrap : TrapEffect
{
    [SerializeField]
    public int amount = 1;
    public int duration = 2;
    public override void Active(PlayerManager player)
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("swamp"));

        player.SpeedUpNormalize(amount, duration);
    }
}