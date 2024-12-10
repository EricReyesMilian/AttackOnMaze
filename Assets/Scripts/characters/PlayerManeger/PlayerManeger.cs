using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    public player play;
    public int index;
    public Vector2 positionOnBoard { get; private set; }
    public string nameC { get; private set; }
    public int speed { get; private set; }
    public int currentSpeed { get; private set; }
    public int cooldown { get; private set; }
    public int currentCooldown { get; private set; }
    public int power { get; private set; }
    public Sprite img { get; private set; }
    public bool isPlayerTurn { get; private set; }
    public int TransformTime;
    public int CtransformTime;
    public Color color { get; private set; }
    public Vector2 Pos;

    List<(int x, int y)> powerUpTimer = new List<(int x, int y)>();
    List<(int x, int y)> speedUpTimer = new List<(int x, int y)>();

    public List<(int x, int y)> lastMove = new List<(int x, int y)>();


    // Update is called once per frame
    void Update()
    {
        if (index == GameManeger.gameManeger.turn)
        {
            isPlayerTurn = true;
            for (int i = 0; i < powerUpTimer.Count; i++)
            {
                if (powerUpTimer[i].y == GameManeger.gameManeger.round)
                {
                    PowerUp(-powerUpTimer[i].x);
                    powerUpTimer.RemoveAt(i);
                }
            }
            for (int i = 0; i < speedUpTimer.Count; i++)
            {
                if (speedUpTimer[i].y == GameManeger.gameManeger.round)
                {
                    SpeedUp(-speedUpTimer[i].x);
                    speedUpTimer.RemoveAt(i);
                }
            }
        }
        else
        {
            isPlayerTurn = false;
        }


    }
    public void InitStats()
    {
        nameC = play.Name;
        speed = play.speed;
        currentSpeed = speed;
        cooldown = play.cooldown;
        power = play.power;
        img = play.sprite;
        color = play.color;
        TransformTime = play.TransformTime;
        currentCooldown = cooldown;

    }
    public void SwapImg()
    {
        if (img == play.sprite)
            img = play.sprite2;
        else
            img = play.sprite;

    }
    public void PowerUp(int amount)
    {
        power += amount;
        power = Floor(power);
    }
    public void PowerUp(int amount, int dur)
    {
        PowerUp(amount);
        powerUpTimer.Add((amount, GameManeger.gameManeger.round + dur));
    }
    public void PowerDivide(float n)
    {
        power = (int)Mathf.Abs(power / n);
        power = Floor(power);
    }
    public void SpeedUp(int amount)
    {
        speed += amount;
        if (amount > 0)
        {
            currentSpeed += amount;
        }
        speed = Floor(speed);
    }
    public void SpeedUp(int amount, int dur)
    {
        SpeedUp(amount);
        speedUpTimer.Add((amount, GameManeger.gameManeger.round + dur));

    }
    public void DownCurrentSpeed(int amount)
    {
        currentSpeed -= amount;
        currentSpeed = Floor(currentSpeed, 0);

    }
    public void ResetCooldown()
    {
        currentCooldown = cooldown;
    }
    public void DownCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= 1;

        }
    }
    public void ResetSpeed()
    {
        currentSpeed = speed;
    }
    int Floor(int x)
    {
        if (x < 1)
        {
            return x = 1;
        }
        else
        {
            return x;
        }
    }
    int Floor(int x, int b)
    {
        if (x < b)
        {
            return x = b;
        }
        else
        {
            return x;
        }
    }
}

