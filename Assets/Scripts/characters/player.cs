using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AtackOnMaze/player")]
public class player : ScriptableObject
{
    public string Name;
    public int speed;
    public int cooldown;
    public int power;
    public Color color;
    public Sprite sprite;

    public bool isTitan;
}