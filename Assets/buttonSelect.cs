using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class buttonSelect : buttonMenu
{

    // Update is called once per frame
    public override void Hover()
    {
        Color newColor;
        ColorUtility.TryParseHtmlString("#8B956D", out newColor);
        text.color = newColor;
    }
    public void Click()
    {
        if (MainMenuManeger.mm.PlayerIndex < MainMenuManeger.mm.PlayerCount)
        {
            MainMenuManeger.mm.PlayerIndex++;

        }
        else
        {
            //load scene

        }
        if (MainMenuManeger.mm.PlayerIndex == MainMenuManeger.mm.PlayerCount)
        {
            text.text = "Start Game >";
        }
    }

}
