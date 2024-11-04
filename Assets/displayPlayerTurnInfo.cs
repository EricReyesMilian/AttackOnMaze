using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayPlayerTurnInfo : MonoBehaviour
{
    public static displayPlayerTurnInfo statdisplay;
    public Image portrait;
    public TextMeshProUGUI name_c;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI power;
    public TextMeshProUGUI skill_Cooldown;


    void Awake()
    {
        if (statdisplay)
        {
            Destroy(this.gameObject);
        }
        else
        {
            statdisplay = this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManeger.gameManeger.UpdateStats += UpdateStats;
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void UpdateStats(int i)
    {

        portrait.sprite = GameManeger.gameManeger.players[i].img;
        name_c.text = "" + GameManeger.gameManeger.players[i].nameC;
        power.text = "power: " + GameManeger.gameManeger.players[i].power;
        skill_Cooldown.text = "cooldown: " + GameManeger.gameManeger.players[i].cooldown;

        if (i == GameManeger.gameManeger.turn)
            speed.text = "speed: " + GameManeger.gameManeger.players[i].speed + "(" + GameManeger.gameManeger.currentSpeed + ")";
        else
            speed.text = "speed: " + GameManeger.gameManeger.players[i].speed + "(" + GameManeger.gameManeger.players[i].speed + ")";


    }
}
