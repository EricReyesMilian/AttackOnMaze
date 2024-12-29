public class PanelPowerUp : PanelEvent
{
    public PowerUp powerUp;

    void Start()
    {
        GameManager.gameManeger.openPanelPowerUp += UpdatePowerUp;
    }
    void Update()
    {
        if (powerUp)
        {
            img1.sprite = powerUp.sprite;
            nameEvent.text = powerUp.namepowerUp;
            Description.text = powerUp.description;

        }
    }

    public void UpdatePowerUp(PowerUp powerUp)
    {
        this.powerUp = powerUp;
        OpenPanel();
    }
}
