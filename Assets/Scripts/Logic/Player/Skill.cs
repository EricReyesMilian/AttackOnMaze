using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public abstract void Active(PlayerManager player);
    public abstract void Pasive(PlayerManager player);
    public abstract void PasiveOnWalk(PlayerManager player);

    public abstract void Desactive(PlayerManager player);
}
