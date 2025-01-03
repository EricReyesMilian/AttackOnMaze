using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManeger : MonoBehaviour
{
    public List<List<player>> playersList = new List<List<player>>();
    public List<player> titanList = new List<player>();

    public static MainMenuManeger mm;

    public int PlayerCount = 1;
    public int PlayerIndex = 1;
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
    public void AddTitans()
    {
        playersList.Add(titanList);
    }
    void Update()
    {
        selectables = PlayerRemain - (PlayerCount - PlayerIndex);

    }
    public void LoadScene(int s)
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        SceneManager.LoadScene(s);
    }
    public void Plus()
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        PlayerCount++;
        if (PlayerCount > PlayerRemain)
        {
            PlayerCount = 1;
        }
    }
    public void Less()
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        PlayerCount--;
        if (PlayerCount < 1)
        {
            PlayerCount = PlayerRemain;
        }
    }
}
