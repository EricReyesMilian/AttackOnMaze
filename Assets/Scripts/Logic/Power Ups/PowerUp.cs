using UnityEngine;

public enum EPower
{
    None,
    Sword,
    Fuel,
    Serum,
    Key,
}
[CreateAssetMenu(menuName = "AtackOnMaze/PowerUp")]
public class PowerUp : ScriptableObject
{
    public Sprite sprite;
    public string namepowerUp;
    public string description;
    public EPower ePower;
    public int rarity = 10;
    public PowerEffect powerEffect;
}
