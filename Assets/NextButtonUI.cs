using UnityEngine;

public class NextButtonUI : SkillButtonUI
{
   
     public override void SetColor()
    {
        if (GameManeger.gameManeger.NextEnable)
                img.color = active;
            else
                img.color = disable;//
    }
    
}