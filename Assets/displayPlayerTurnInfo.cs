using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayPlayerTurnInfo : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI name_c;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI power;
    public TextMeshProUGUI skill_Cooldown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < GameManeger.gameManeger.players.Count; i++)
        {
            if (GameManeger.gameManeger.players[i].isPlayerTurn)
            {
                portrait.sprite = GameManeger.gameManeger.players[i].img;
                name_c.text = "" + GameManeger.gameManeger.players[i].nameC;
                speed.text = "speed: " + GameManeger.gameManeger.players[i].speed;
                power.text = "power: " + GameManeger.gameManeger.players[i].power;
                skill_Cooldown.text = "cooldown: " + GameManeger.gameManeger.players[i].Pos;
            }
        }



    }
}
