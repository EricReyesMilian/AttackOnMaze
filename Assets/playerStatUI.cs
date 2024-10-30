using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerStatUI : MonoBehaviour
{
    public TextMeshProUGUI namePlayer;
    public int index;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        namePlayer.text = GameManeger.gameManeger.players[index].nameC;

    }
}
