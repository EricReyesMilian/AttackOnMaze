using UnityEngine;

public class NextButtonUI : SkillButtonUI
{

    public override void SetColor()
    {
        if (GameManager.gameManeger.NextEnable)
            img.color = active;
        else
            img.color = disable;//
    }

}