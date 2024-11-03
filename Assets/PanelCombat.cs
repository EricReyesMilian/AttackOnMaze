using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCombat : MonoBehaviour
{
    public PlayerManeger player1;
    public PlayerManeger player2;

    public Image img1;
    public Image img2;
    // Start is called before the first frame update
    void Start()
    {

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
        }
        else
        {
            img1.color = Color.clear;
            img2.color = Color.clear;

        }
    }
}
