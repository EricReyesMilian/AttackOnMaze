using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    public player play;
    public bool titanForm;
    public int index;
    public int team;
    public bool sick;
    public int sickTime = 2;
    public Sprite spriteTitan;
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
    public bool isTitan;
    List<(int x, int y)> powerUpTimer = new List<(int x, int y)>();
    List<(int x, int y)> speedUpTimer = new List<(int x, int y)>();
    List<(int x, int y)> cooldownUpTimer = new List<(int x, int y)>();

    public int TitanIQ = 0;
    public List<(int x, int y)> lastMove = new List<(int x, int y)>();

    public bool haveKey = false;

    public PlayerManager(player play)
    {
        this.play = play;
        spriteTitan = Resources.Load<Sprite>("norm");
    }
    // Update is called once per frame
    public void UpdateDebuff()
    {

        for (int i = 0; i < powerUpTimer.Count; i++)
        {
            if (powerUpTimer[i].y == GameManager.gameManeger.round)
            {
                PowerUp(-powerUpTimer[i].x);
                powerUpTimer.RemoveAt(i);
            }
        }
        for (int i = 0; i < speedUpTimer.Count; i++)
        {
            if (speedUpTimer[i].y == GameManager.gameManeger.round)
            {
                SpeedUp(-speedUpTimer[i].x);
                speedUpTimer.RemoveAt(i);
            }
        }
        for (int i = 0; i < cooldownUpTimer.Count; i++)
        {
            if (cooldownUpTimer[i].y == GameManager.gameManeger.round)
            {
                ResetCooldown();

                cooldownUpTimer.RemoveAt(i);
            }
        }




    }
    public void InitStats()
    {
        TitanIQ = play.TitanIQ;
        nameC = play.Name;
        speed = play.speed;
        currentSpeed = speed;
        cooldown = play.cooldown;
        isTitan = play.isTitan;
        power = play.power;
        img = play.sprite;
        color = play.color;
        TransformTime = play.TransformTime;
        currentCooldown = cooldown;

    }
    public void SwapImg(int i)
    {
        if (i == 1)
        {
            img = play.sprite;

        }
        else
        {
            img = play.sprite2;

        }

    }
    public void IntUp(int amount)
    {
        if (amount < 0)
        {
            TitanIQ += amount;
            if (TitanIQ < 0)
            {
                TitanIQ = 0;
            }
        }
        else
        {
            if (TitanIQ < 4)
            {
                TitanIQ += amount;
            }

        }
    }
    public void PowerUp(int amount)
    {
        power += amount;
        power = Floor(power);
        GameManager.gameManeger.CheckSavior();
    }
    public void PowerUp(int amount, int dur)
    {
        PowerUp(amount);
        powerUpTimer.Add((amount, GameManager.gameManeger.round + dur));
        GameManager.gameManeger.CheckSavior();

    }
    public void PowerDivide(float n)
    {
        power = (int)Mathf.Abs(power / n);
        power = Floor(power);
        GameManager.gameManeger.CheckSavior();

    }
    public void SpeedUp(int amount)
    {
        speed += amount;
        if (amount > 0)
        {
            currentSpeed += amount;
        }
        speed = Floor(speed);
        if (currentSpeed > speed)
        {
            ResetSpeed();
        }
    }
    public void SpeedUp(int amount, int dur)
    {
        SpeedUp(amount);
        speedUpTimer.Add((amount, GameManager.gameManeger.round + dur));
        if (currentSpeed > speed)
        {
            ResetSpeed();
        }
    }
    public void CSpeedUp(int amount)
    {
        currentSpeed += amount;

    }
    public void SpeedUpNormalize(int norm, int dur)
    {
        speedUpTimer.Add((-(speed - 1), GameManager.gameManeger.round + dur));
        speed = norm;
        if (currentSpeed > speed)
        {
            ResetSpeed();
        }
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
    public void RemoveCooldown()
    {
        currentCooldown = 0;
    }
    public void ResetCooldown(int dur)
    {
        cooldownUpTimer.Add((1, GameManager.gameManeger.round + dur));

    }
    public void TakeKey()
    {
        haveKey = true;
    }
    public void LoseKey()
    {
        haveKey = false;
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

