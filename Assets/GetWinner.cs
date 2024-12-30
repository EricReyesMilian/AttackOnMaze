using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GetWinner : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = GameManager.gameManeger.winner;
    }
    void Update()
    {
        text.text = GameManager.gameManeger.winner;

    }

}
