using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displaySelectablesPlayers : MonoBehaviour
{
    GameManeger gm;
    public GameObject selectablePlayer;
    public PanelCombat pc;

    void Start()
    {
        gm = GameManeger.gameManeger;
        gm.SelectplayerCombat += CreateSelectables;


    }

    public void CreateSelectables()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < gm.nearPlayers.Count; i++)
        {
            Instantiate(selectablePlayer, Vector3.zero, Quaternion.identity, gameObject.transform);
            selectablePlayer.GetComponent<SelectionPlayer>().namePlayer.text = gm.nearPlayers[i].Name;
            selectablePlayer.GetComponent<SelectionPlayer>().index = i;
        }
        pc.anim.SetTrigger("select");
    }


}
