using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class buttonSelect : buttonMenu
{
    bool loaded = false;
    // Update is called once per frame
    public override void Hover()
    {
        if (MainMenuManeger.mm.PlayerIndex > 0)
        {
            if (MainMenuManeger.mm.playersList[MainMenuManeger.mm.PlayerIndex - 1].Count > 0)
            {
                AudioManager.speaker.Play(Resources.Load<AudioClip>("tick"));

                Color newColor;
                ColorUtility.TryParseHtmlString("#8B956D", out newColor);
                text.color = newColor;

            }

        }
    }
    public void Click1()
    {
        if (!MainMenuManeger.mm.checkAmount && MainMenuManeger.mm.playersList[0].Count > 0)
        {
            MainMenuManeger.mm.selectables = MainMenuManeger.mm.playersList[0].Count;
            MainMenuManeger.mm.TopSelection = MainMenuManeger.mm.selectables;
            MainMenuManeger.mm.checkAmount = true;

            if (MainMenuManeger.mm.PlayerIndex < MainMenuManeger.mm.PlayerCount)
            {
                MainMenuManeger.mm.PlayerIndex++;

            }
            else
            {
                //load scene
                if (!loaded)
                {
                    MainMenuManeger.mm.AddTitans();
                    for (int i = 0; i < MainMenuManeger.mm.PlayerCount; i++)
                    {

                        for (int j = 0; j < MainMenuManeger.mm.playersList[i].Count; j++)
                        {
                            Debug.Log((i + 1) + "" + MainMenuManeger.mm.playersList[i][j]);

                        }
                    }
                    MainMenuManeger.mm.LoadScene(2);

                    loaded = true;
                }

            }

        }
        else
        if (MainMenuManeger.mm.selectables <= 0)
        {
            if (!MainMenuManeger.mm.checkAmount)
            {
                MainMenuManeger.mm.selectables = MainMenuManeger.mm.playersList[0].Count;
                MainMenuManeger.mm.TopSelection = MainMenuManeger.mm.selectables;
                MainMenuManeger.mm.checkAmount = true;

            }
            else
            {
                MainMenuManeger.mm.selectables = MainMenuManeger.mm.TopSelection;
            }
            AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

            if (MainMenuManeger.mm.PlayerIndex < MainMenuManeger.mm.PlayerCount)
            {
                MainMenuManeger.mm.PlayerIndex++;

            }
            else
            {
                //load scene
                if (!loaded)
                {
                    MainMenuManeger.mm.AddTitans();
                    for (int i = 0; i < MainMenuManeger.mm.PlayerCount; i++)
                    {

                        for (int j = 0; j < MainMenuManeger.mm.playersList[i].Count; j++)
                        {
                            Debug.Log((i + 1) + "" + MainMenuManeger.mm.playersList[i][j]);

                        }
                    }
                    MainMenuManeger.mm.LoadScene(2);

                    loaded = true;
                }


            }
            if (MainMenuManeger.mm.PlayerIndex == MainMenuManeger.mm.PlayerCount)
            {
                text.text = "Start Game >";
            }

        }
    }

}
