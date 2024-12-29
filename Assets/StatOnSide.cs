using UnityEngine;

public class StatOnSide : MonoBehaviour
{
    GameManeger gm;

    public GameObject playerStatPrefb;
    public GameObject playerManegerPrefbWrapper;

    void Start()
    {
        gm = GameManeger.gameManeger;
        gm.ChangeTurnOrd += ChangeOrder;
        for (int j = 0; j < gm.player_Scriptable.Count + MainMenuManeger.mm.playersList.Count; j++)
        {
            Instantiate(playerStatPrefb, Vector3.zero, Quaternion.identity, playerManegerPrefbWrapper.transform);
        }
        for (int i = 0; i < MainMenuManeger.mm.playersList.Count; i++)
        {


        }


    }

    void ChangeOrder()
    {

        for (int i = 0; i < gm.players.Count; i++)
        {
            for (int j = 0; j < gm.players.Count; j++)
            {
                if (transform.GetChild(i).GetComponent<PlayerManeger>().play.Name == gm.players[j].play.Name)
                {
                    transform.GetChild(i).SetSiblingIndex(j);

                }
            }

        }
        int newIndex = 0;
        for (int i = gm.turn; i < gm.players.Count; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }
        int a = newIndex;
        for (int i = a; i < gm.turn; i++)
        {


            transform.GetChild(i).SetSiblingIndex(newIndex);

            newIndex++;



        }


    }
}