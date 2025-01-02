using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImpostorEffect", menuName = "AtackOnMaze/Trap/Effect/ImpostorTrap")]
public class ImpostorTrap : TrapEffect
{
    [SerializeField]

    public override void Active(PlayerManager player)
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("fear"));
        player.PowerUp(-(player.power / 2), 2);
    }
}