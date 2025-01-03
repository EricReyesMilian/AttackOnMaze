using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GetWinner : MonoBehaviour
{
    TextMeshProUGUI text;
    PlayerManager winner;
    public AudioSource a;
    public AudioSource b;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        winner = GameManager.gameManeger.winner;
        if (winner.play.isTitan)
        {
            text.text = "Titans Won       humanity will remember";
            b.Play();
        }
        else
        {
            text.text = "Player " + (winner.team + 1) + " Won";
            a.Play();
        }
    }

    public void Inicio()
    {
        Invoke("loadscene", 3f);
    }
    void loadscene()
    {
        SceneManager.LoadScene(0);

    }
}
