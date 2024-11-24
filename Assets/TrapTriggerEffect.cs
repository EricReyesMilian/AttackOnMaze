

public class TrapTriggerEffect
{
    private trap trap;
    private PlayerManeger player;
    GameManeger gm = GameManeger.gameManeger;
    public TrapTriggerEffect(trap trap, PlayerManeger player)
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
        }
    }
    void None()
    {

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