
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
        powerUp.powerEffect.Active(player);
    }

}