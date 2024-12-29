using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManeger : MonoBehaviour
{
    public List<List<player>> playersList = new List<List<player>>();
    public static MainMenuManeger mm;
    public int PlayerCount = 2;
    public int PlayerIndex;
    public int PlayerRemain;
    public int selectables;
    public Animator anim;
    void Awake()
    {
        if (mm)
        {
            Destroy(this.gameObject);
        }
        else
        {
            mm = this;
        }
        PlayerIndex = 1;
        PlayerRemain = 5;//cantidad de personajes jugables
    }
    void Start()
    {

    }
    void Update()
    {
        selectables = PlayerRemain - (PlayerCount - PlayerIndex);

    }
    public void LoadScene(int s)
    {
        SceneManager.LoadScene(s);
    }
    public void Plus()
    {

        PlayerCount++;
        if (PlayerCount > PlayerRemain)
        {
            PlayerCount = 2;
        }
    }
    public void Less()
    {

        PlayerCount--;
        if (PlayerCount < 2)
        {
            PlayerCount = PlayerRemain;
        }
    }
}
