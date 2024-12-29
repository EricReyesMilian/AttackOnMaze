using UnityEngine;

[CreateAssetMenu(menuName = "AtackOnMaze/PowerUp")]
public class PowerUp : ScriptableObject
{
    public Sprite sprite;
    public string namepowerUp;
    public string description;
    public EPower ePower;
}
public enum EPower
{
    None,
    Sword,
    Fuel,
}