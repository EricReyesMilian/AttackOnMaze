using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FuelEffect", menuName = "AtackOnMaze/PowerUp/Effect/Fuel")]
public class FuelPower : PowerEffect
{
    [SerializeField]
    int amountSpeed = 3;

    public override void Active(PlayerManager player)
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("fuel"));

        player.CSpeedUp(amountSpeed);
    }
}
