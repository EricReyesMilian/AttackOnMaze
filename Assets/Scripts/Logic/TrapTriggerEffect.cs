
public enum Trap
{
    none,
    spikes,
    impostor,
    cold,
    swap,
}
public class TrapTriggerEffect
{
    private trap trap;
    private PlayerManager player;
    GameManager gm = GameManager.gameManeger;
    public TrapTriggerEffect(trap trap, PlayerManager player)
    {
        this.trap = trap;
        this.player = player;
        Effect(trap);

    }
    public void Effect(trap trap)
    {
        switch (trap.type)
        {
            case Trap.none: None(); break;
            case Trap.spikes: Spikes(); break;
            case Trap.impostor: Impostor(); break;
            case Trap.cold: Cold(); break;
            case Trap.swap: Swap(); break;
        }
    }
    void None()
    {

    }
    void Swap()
    {
        player.SpeedUpNormalize(1, 2);
    }
    void Cold()
    {
        player.ResetCooldown();
    }
    void Spikes()
    {
        player.PowerUp(-1);
    }
    void Impostor()
    {
        player.PowerDivide(2);
    }
}