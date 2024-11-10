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

        GameManeger.gameManeger.Combat(GameManeger.gameManeger.combatZoneCoord, index);
    }
}
