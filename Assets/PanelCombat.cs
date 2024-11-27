using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCombat : MonoBehaviour
{
    GameManeger gm;
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
    public Animator anim;
    public GameObject Panel;
    bool fin;
    public displayPlayerTurnInfo displayPlayerTurnInfo;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManeger.gameManeger;
        anim = GetComponent<Animator>();
        // gameObject.SetActive(false);
        gm.StartCombate += OpenPanel;

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
                power2.text = "power:" + gm.combatScene.GetPower2().ToString();
                power1.text = "power:" + gm.combatScene.GetPower1().ToString();
                int per1 = Mathf.CeilToInt((float)gm.combatScene.GetPower2() / (gm.combatScene.GetPower1() + gm.combatScene.GetPower2()) * 100);

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
        anim.SetBool("player1", gm.lastWinner1);//bool igual al ganador//error
    }
    public void ClosePanel()
    {
        fin = false;
        displayPlayerTurnInfo.UpdateStats(gm.turn);
        Panel.SetActive(false);

        //  gameObject.SetActive(false);

    }
    public void OpenPanel()
    {
        anim.SetTrigger("combat");
        Panel.SetActive(true);
        player1 = gm.combatScene.player1;
        player2 = gm.combatScene.player2;

    }
}
