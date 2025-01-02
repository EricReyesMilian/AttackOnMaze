using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ErenSkill", menuName = "Skills/Eren")]
public class ErenSkill : Skill
{


    public override void Active(PlayerManager player)
    {
        player.titanForm = true;
        player.SwapImg(2);
        AudioManager.speaker.Play(Resources.Load<AudioClip>(player.play.Name));

        player.PowerUp(player.power);
        player.SpeedUp(2);

    }
    public override void Pasive(PlayerManager player)
    {
    }
    public override void PasiveOnWalk(PlayerManager player) { }

    public override void Desactive(PlayerManager player)
    {
        player.titanForm = false;
        player.PowerUp(-player.power / 2);
        player.SpeedUp(-2);

        player.SwapImg(1);

    }
}
