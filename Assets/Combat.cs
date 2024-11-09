
using System;

using System.Collections.Generic;

public class Combat
{
    PlayerManeger player1;
    PlayerManeger player2;
    public Combat(PlayerManeger player1, PlayerManeger player2)
    {
        this.player1 = player1;
        this.player2 = player2;
    }

    public bool Player1IsWinner()
    {
        double r = new Random().NextDouble();
        return (float)(player1.power) / (player1.power + player2.power) >= r;

    }

    public int Reward(bool player1Win, int index)
    {
        if (index == 1)
        {
            if (player1Win)
            {
                if (player1.power >= player2.power)
                {
                    return (player2.power / 2) % 2 == 0 ? player2.power / 2 : (player2.power / 2) + 1;//recompensa si es mas fuerte
                }
                else
                {
                    return (player2.power / 4) % 4 == 0 ? player2.power / 4 : (player2.power / 4) + 1;//recompensa si es mas debil

                }
            }
            else
            {
                if (player1.power >= player2.power)
                {
                    return -((player1.power / 4) % 2 == 0 ? player1.power / 4 : (player1.power / 4) + 1); ;//penalizacion si es mas fuerte
                }
                else
                {
                    return -((player1.power / 2) % 2 == 0 ? player1.power / 2 : (player1.power / 2) + 1);//penalizacion si es mas debil
                }
            }

        }
        else if (index == 2)
        {
            if (player1Win)
            {
                if (player2.power >= player1.power)
                {
                    return -((player2.power / 4) % 2 == 0 ? player2.power / 4 : (player2.power / 4) + 1);//penalizacion si es mas fuerte
                }
                else
                {
                    return -((player2.power / 2) % 2 == 0 ? player2.power / 2 : (player2.power / 2) + 1);//penalizacion si es mas debil
                }
            }
            else
            {
                if (player2.power >= player1.power)
                {
                    return (player1.power / 2) % 2 == 0 ? player1.power / 2 : (player1.power / 2) + 1;//recompensa si es mas fuerte
                }
                else
                {
                    return (player1.power / 4) % 4 == 0 ? player1.power / 4 : (player1.power / 4) + 1;//recompensa si es mas debil

                }


            }

        }
        else
        {
            return -1;
        }
    }


}