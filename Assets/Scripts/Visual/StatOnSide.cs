using UnityEngine;

public class StatOnSide : MonoBehaviour
{

    public GameObject playerStatPrefb;
    public GameObject playerManegerPrefbWrapper;

    void Start()
    {
        GameManager.gameManeger.ChangeTurnOrd += ChangeOrder;
        GameManager.gameManeger.createPlayerBoxInstances += CreateUiPlayerBoxes;



    }
    public void CreateUiPlayerBoxes()
    {
        for (int j = 0; j < GameManager.gameManeger.player_Scriptable.Count; j++)
        {
            Instantiate(playerStatPrefb, Vector3.zero, Quaternion.identity, playerManegerPrefbWrapper.transform);
        }
    }
    void ChangeOrder()
    {

        for (int i = 0; i < GameManager.gameManeger.players.Count; i++)
        {
            transform.GetChild(i).GetComponent<playerStatUI>().player = GameManager.gameManeger.players[i].play.Name;

        }
        int newIndex = 0;
        for (int i = GameManager.gameManeger.turn; i < GameManager.gameManeger.players.Count; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }
        int a = newIndex;
        for (int i = a; i < GameManager.gameManeger.turn; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }


    }
}