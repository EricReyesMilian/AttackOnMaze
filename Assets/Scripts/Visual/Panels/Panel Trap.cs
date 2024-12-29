public class PanelTrap : PanelEvent
{
    public trap trap;
    void Start()
    {
        GameManager.gameManeger.openPanelTrap += UpdateTrap;
    }
    public void Update()
    {
        if (trap)
        {
            img1.sprite = trap.sprite;
            nameEvent.text = trap.nameTrap;
            Description.text = trap.description;

        }
    }
    public void UpdateTrap(trap trap)
    {
        this.trap = trap;
        OpenPanel();
    }

}