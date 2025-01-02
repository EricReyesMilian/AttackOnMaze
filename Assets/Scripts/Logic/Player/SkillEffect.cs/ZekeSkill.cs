using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZekeSkill", menuName = "Skills/Zeke")]
public class ZekeSkill : Skill
{
    [SerializeField]
    int powerUp = 5;

    public override void Active(PlayerManager player)
    {
        player.titanForm = true;
        player.SwapImg(2);
        AudioManager.speaker.Play(Resources.Load<AudioClip>(player.play.Name));

        player.PowerUp(powerUp);
        GameManager.gameManeger.ZekeSkill = true;

    }
    public override void Pasive(PlayerManager player)
    {
    }
    public override void PasiveOnWalk(PlayerManager player) { }

    public override void Desactive(PlayerManager player)
    {
        player.titanForm = false;
        player.PowerUp(-powerUp);
        GameManager.gameManeger.ZekeSkill = false;

        player.SwapImg(1);

    }
}
