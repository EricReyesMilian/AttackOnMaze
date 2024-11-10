using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displaySelectablesPlayers : MonoBehaviour
{
    public GameObject selectablePlayer;
    public PanelCombat pc;

    void Start()
    {
        GameManeger.gameManeger.SelectplayerCombat += CreateSelectables;


    }

    public void CreateSelectables()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameManeger.gameManeger.nearPlayers.Count; i++)
        {
            Instantiate(selectablePlayer, Vector3.zero, Quaternion.identity, gameObject.transform);
            selectablePlayer.GetComponent<SelectionPlayer>().namePlayer.text = GameManeger.gameManeger.nearPlayers[i].Name;
            selectablePlayer.GetComponent<SelectionPlayer>().index = i;
        }
        pc.anim.SetTrigger("select");
    }


}
