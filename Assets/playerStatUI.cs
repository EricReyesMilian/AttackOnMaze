using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerStatUI : MonoBehaviour
{
    public TextMeshProUGUI namePlayer;
    public string player;

    void Update()
    {
        if (player != null)
        {
            namePlayer.text = player;

        }

    }
}
