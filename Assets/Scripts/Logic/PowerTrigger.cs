

public class PowerTrigger
{
    private PowerUp powerUp;
    private PlayerManager player;
    GameManager gm = GameManager.gameManeger;
    public PowerTrigger(PowerUp powerUp, PlayerManager player)
    {
        this.powerUp = powerUp;
        this.player = player;
        Effect(powerUp);

    }
    public void Effect(PowerUp powerUp)
    {
        switch (powerUp.ePower)
        {
            case EPower.None: None(); break;
            case EPower.Sword: Sword(); break;
            case EPower.Fuel: Fuel(); break;
        }
    }
    void None()
    {

    }
    void Sword()
    {
        player.PowerUp(1, 2);
    }
    void Fuel()
    {
        player.SpeedUp(3, 0);
    }
}