using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SelectionPlayer : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI namePlayer;



    public void SetPlayerCombat()
    {

        GameManager.gameManeger.Combat(GameManager.gameManeger.combatZoneCoord, index);
    }
}
