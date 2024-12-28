using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class boxController : MonoBehaviour
{
    Animator anim;
    public Image image;
    public Image image2;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textPlayer;
    public player play;
    bool selected;
    // Start is lled before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        image.sprite = play.sprite;
        textName.text = play.Name;
        image2.color = play.color;

    }

    // Update is called once per frame
    void Update()
    {
        if (!selected)
            textPlayer.text = "Player " + MainMenuManeger.mm.PlayerIndex;
    }
    public void Hover()
    {
        if (MainMenuManeger.mm.selectables > 0)
            anim.SetBool("hover", true);
    }
    public void NoHover()
    {
        anim.SetBool("hover", false);
    }
    public void Select()
    {
        if (MainMenuManeger.mm.selectables > 0)
        {
            anim.SetTrigger("select");
            MainMenuManeger.mm.PlayerRemain--;
            selected = true;
            MainMenuManeger.mm.playersList[MainMenuManeger.mm.PlayerIndex - 1].Add(play);

        }

    }
}
