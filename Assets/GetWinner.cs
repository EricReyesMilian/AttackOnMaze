using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GetWinner : MonoBehaviour
{
    TextMeshProUGUI text;
    public AudioSource a;
    public AudioSource b;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = GameManager.gameManeger.winner;
        if (text.text == "Titans Won       humanity will remember")
        {
            b.Play();
        }
        else
        {
            a.Play();
        }
    }
    void Update()
    {
        text.text = GameManager.gameManeger.winner;

    }

}
