public class PanelTrap : PanelEvent
{
    public trap trap;

    public void Update()
    {
        if (trap)
        {
            img1.sprite = trap.sprite;
            nameEvent.text = trap.nameTrap;
            Description.text = trap.description;

        }
    }

}