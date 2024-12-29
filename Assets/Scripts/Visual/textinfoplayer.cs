using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textinfoplayer : MonoBehaviour
{
    TextMeshProUGUI text;
    public TextMeshProUGUI numPlayer;

    public TextMeshProUGUI textI;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Player " + MainMenuManeger.mm.PlayerIndex + " select";
        numPlayer.text = MainMenuManeger.mm.PlayerCount + "";
        textI.text = "you can select up to " + MainMenuManeger.mm.selectables + " characters.";
    }
}
