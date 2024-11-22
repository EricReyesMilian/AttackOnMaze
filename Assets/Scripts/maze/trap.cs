using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AtackOnMaze/trap")]
public class trap : ScriptableObject
{
    public string nameTrap;
    public Sprite sprite;
    public Trap type;
    public string description;
    public int range = 1;

    public bool activationTrap;



}