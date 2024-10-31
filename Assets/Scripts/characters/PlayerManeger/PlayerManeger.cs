using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    public player play;
    public Vector2 positionOnBoard;
    public string nameC;
    public int speed;
    public int cooldown;
    public int power;
    public Sprite img;
    public bool isPlayerTurn;
    public Color color;
    public Vector2 Pos;
    // Start is called before the first frame update
    //
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitStats()
    {
        nameC = play.Name;
        speed = play.speed;
        cooldown = play.cooldown;
        power = play.power;
        img = play.sprite;
        color = play.color;
    }
}
