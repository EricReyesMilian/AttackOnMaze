using UnityEngine;

public class StatOnSide : MonoBehaviour
{
    public GameObject playerStatPrefb;
    public GameObject playerManegerPrefbWrapper;

    void Start()
    {
        for (int i = 0; i < GameManeger.gameManeger.player_Scriptable.Count; i++)
        {
            Instantiate(playerStatPrefb, Vector3.zero, Quaternion.identity, playerManegerPrefbWrapper.transform);
            playerStatPrefb.GetComponent<playerStatUI>().index = i;
        }

    }
}