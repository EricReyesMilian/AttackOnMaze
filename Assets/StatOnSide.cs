using UnityEngine;

public class StatOnSide : MonoBehaviour
{

    public GameObject playerStatPrefb;
    public GameObject playerManegerPrefbWrapper;

    void Start()
    {
        GameManeger.gameManeger.ChangeTurnOrd += ChangeOrder;
        GameManeger.gameManeger.createPlayerBoxInstances += CreateUiPlayerBoxes;



    }
    public void CreateUiPlayerBoxes()
    {
        for (int j = 0; j < GameManeger.gameManeger.player_Scriptable.Count; j++)
        {
            Instantiate(playerStatPrefb, Vector3.zero, Quaternion.identity, playerManegerPrefbWrapper.transform);
        }
    }
    void ChangeOrder()
    {

        for (int i = 0; i < GameManeger.gameManeger.players.Count; i++)
        {
            transform.GetChild(i).GetComponent<playerStatUI>().player = GameManeger.gameManeger.players[i].play.Name;

        }
        int newIndex = 0;
        for (int i = GameManeger.gameManeger.turn; i < GameManeger.gameManeger.players.Count; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }
        int a = newIndex;
        for (int i = a; i < GameManeger.gameManeger.turn; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }


    }
}