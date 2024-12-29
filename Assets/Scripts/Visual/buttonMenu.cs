using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class buttonMenu : MonoBehaviour
{
    protected TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public virtual void Hover()
    {
        text.fontStyle = FontStyles.Underline;
        Color newColor;
        ColorUtility.TryParseHtmlString("#8B956D", out newColor);
        text.color = newColor;
    }
    public void NoHover()
    {
        text.fontStyle = FontStyles.Normal;
        text.color = Color.white;

    }
    public void Click()
    {
        MainMenuManeger.mm.anim.SetTrigger("next");
        for (int i = 0; i < MainMenuManeger.mm.PlayerCount; i++)
        {
            MainMenuManeger.mm.playersList.Add(new List<player>());
        }
    }



}
