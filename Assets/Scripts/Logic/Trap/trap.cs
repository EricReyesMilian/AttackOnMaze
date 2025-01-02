using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AtackOnMaze/trap")]
public class trap : ScriptableObject
{
    public string nameTrap;
    public Sprite sprite;
    public string description;
    public int range = 1;
    public int rarity = 5;
    public bool activationTrap;
    public TrapEffect trapEffect;


}