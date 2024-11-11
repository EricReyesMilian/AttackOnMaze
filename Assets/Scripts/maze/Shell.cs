using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool obstacle = true;
    public bool hasAplayer;
    public player player;
    public bool reach;
    public Vector2 coord;
    public bool visitedOnMove;
    public bool nearPlayer;
    public List<PlayerManeger> NearPlayers = new List<PlayerManeger>();




}
