using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapEffect : ScriptableObject
{
    public abstract void Active(PlayerManager player);
}
