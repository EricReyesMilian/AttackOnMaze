using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManeger : MonoBehaviour
{
    public List<List<player>> playersList = new List<List<player>>();
    public static MainMenuManeger mm;
    public int PlayerCount;
    public int PlayerIndex;
    public int PlayerRemain;
    public int selectables;
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
        PlayerRemain = 5;
    }
    void Start()
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            playersList.Add(new List<player>());
        }
    }
    void Update()
    {
        selectables = PlayerRemain - (PlayerCount - PlayerIndex);

    }
    public void LoadScene(int s)
    {
        SceneManager.LoadScene(s);
    }
}
