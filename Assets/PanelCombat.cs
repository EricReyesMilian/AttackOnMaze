using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCombat : MonoBehaviour
{
    public PlayerManeger player1;
    public PlayerManeger player2;

    public Image img1;
    public Image img2;

    public TextMeshProUGUI power1;
    public TextMeshProUGUI power2;
    public TextMeshProUGUI percent2;
    public TextMeshProUGUI percent1;
    public TextMeshProUGUI Next;

    public TextMeshProUGUI fight;
    Animator anim;
    bool fin;
    public displayPlayerTurnInfo displayPlayerTurnInfo;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // gameObject.SetActive(false);
        GameManeger.gameManeger.StartCombate += OpenPanel;

    }

    // Update is called once per frame
    void Update()
    {
        if (player1 && player2)
        {
            img1.sprite = player1.img;
            img2.sprite = player2.img;
            img1.color = Color.white;
            img2.color = Color.white;
            if (!fin)
            {
                power1.text = "power:" + GameManeger.gameManeger.power1Before.ToString();
                power2.text = "power:" + GameManeger.gameManeger.power2Before.ToString();
                int per1 = Mathf.CeilToInt((float)GameManeger.gameManeger.power2Before / (GameManeger.gameManeger.power1Before + GameManeger.gameManeger.power2Before) * 100);

                int per2 = 100 - per1;
                percent1.text = per1 + "%";
                percent2.text = per2 + "%";

                Next.text = "Fight!!!";

            }
            else
            {
                power1.text = "power:" + player1.power.ToString();
                power2.text = "power:" + player2.power.ToString();
                Next.text = "Next";

            }

        }
        else
        {
            img1.color = Color.clear;
            img2.color = Color.clear;

        }
    }
    public void Hover()
    {
        fight.fontStyle = FontStyles.Underline;

    }
    public void NoHover()
    {
        fight.fontStyle = FontStyles.Normal;

    }
    public void Click()
    {
        anim.SetTrigger("Fight");

        fin = true;
        anim.SetBool("player1", GameManeger.gameManeger.player1Win);//bool igual al ganador
    }
    public void ClosePanel()
    {
        fin = false;
        displayPlayerTurnInfo.UpdateStats(GameManeger.gameManeger.turn);
        //  gameObject.SetActive(false);

    }
    public void OpenPanel()
    {
        anim.SetTrigger("combat");

    }
}
