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
        if (MainMenuManeger.mm.PlayerIndex > 0)
        {
            if (MainMenuManeger.mm.playersList[MainMenuManeger.mm.PlayerIndex - 1].Count > 0)
            {
                Color newColor;
                ColorUtility.TryParseHtmlString("#8B956D", out newColor);
                text.color = newColor;

            }

        }
    }
    public void Click1()
    {
        if (MainMenuManeger.mm.playersList[MainMenuManeger.mm.PlayerIndex - 1].Count > 0)
        {
            if (MainMenuManeger.mm.PlayerIndex < MainMenuManeger.mm.PlayerCount)
            {
                MainMenuManeger.mm.PlayerIndex++;

            }
            else
            {
                //load scene
                MainMenuManeger.mm.LoadScene(2);

            }
            if (MainMenuManeger.mm.PlayerIndex == MainMenuManeger.mm.PlayerCount)
            {
                text.text = "Start Game >";
            }

        }
    }

}
