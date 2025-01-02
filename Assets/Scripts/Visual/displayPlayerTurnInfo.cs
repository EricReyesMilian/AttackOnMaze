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
    public TextMeshProUGUI transf;
    public TextMeshProUGUI player;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI power;
    public TextMeshProUGUI skill_Cooldown;
    public TextMeshProUGUI roundCount;
    public TextMeshProUGUI info;
    public TextMeshProUGUI infoMatch;
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
        if (!gm.players[i].haveKey)
        {
            infoMatch.text = "you need the key to become the Savior";

        }
        else
        if (gm.players[i].power < gm.WinPower)
        {
            infoMatch.text = "you need  " + (gm.WinPower - gm.players[i].power) + " units of power to become the Savior";
        }
        else
        {
            infoMatch.text = "you are the Savior; the city doors are open to you.";

        }
        info.text = gm.players[i].play.infoSkill;
        roundCount.text = "round: " + gm.round;
        portrait.sprite = gm.players[i].img;
        name_c.text = "" + gm.players[i].nameC;
        KeyImg.SetActive(gm.players[i].haveKey);
        power.text = "power: " + gm.players[i].power;
        skill_Cooldown.text = "cooldown: " + gm.players[i].currentCooldown;
        if (!gm.players[i].play.isTitan)
        {
            player.text = "Player " + (gm.players[i].team + 1);

        }
        else
        {
            player.text = " ";

        }
        if (gm.players[i].CtransformTime > 0)
            transf.text = "Transform: " + (gm.players[i].TransformTime - gm.players[i].CtransformTime);
        else
            transf.text = " ";

        if (i == gm.turn)
            speed.text = "speed: " + gm.players[i].speed + "(" + gm.players[i].currentSpeed + ")";
        else
            speed.text = "speed: " + gm.players[i].speed + "(" + gm.players[i].speed + ")";


    }
}
