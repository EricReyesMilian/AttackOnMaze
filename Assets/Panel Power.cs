public class PanelPowerUp : PanelEvent
{
    public PowerUp powerUp;

    void Update()
    {
        if (powerUp)
        {
            img1.sprite = powerUp.sprite;
            nameEvent.text = powerUp.namepowerUp;
            Description.text = powerUp.description;

        }
    }
}
