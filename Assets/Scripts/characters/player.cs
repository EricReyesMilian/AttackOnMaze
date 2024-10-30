using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AtackOnMaze/player")]
public class player : ScriptableObject
{
    public string Name;
    public int speed;
    public int cooldown;
    public int power;

    //las siguientes caracteristicas son para el apartado visual en 2d
    //recomendado eliminar para mas generalidad
    public Sprite sprite;
    //investigar delegados
}