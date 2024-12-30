using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayPlayerTurnInfo : MonoBehaviour
{
    GameManager gm;
    public static displayPlayerTurnInfo statdisplay;
    public Image portrait;
    public TextMeshProUGUI name_c;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI power;
    public TextMeshProUGUI skill_Cooldown;

    public GameObject KeyImg;
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
        gm = GameManager.gameManeger;
        gm.UpdateStats += UpdateStats;
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void UpdateStats(int i)
    {

        portrait.sprite = gm.players[i].img;
        name_c.text = "" + gm.players[i].nameC;
        KeyImg.SetActive(gm.players[i].haveKey);
        power.text = "power: " + gm.players[i].power;
        skill_Cooldown.text = "cooldown: " + gm.players[i].currentCooldown;

        if (i == gm.turn)
            speed.text = "speed: " + gm.players[i].speed + "(" + gm.players[i].currentSpeed + ")";
        else
            speed.text = "speed: " + gm.players[i].speed + "(" + gm.players[i].speed + ")";


    }
}
