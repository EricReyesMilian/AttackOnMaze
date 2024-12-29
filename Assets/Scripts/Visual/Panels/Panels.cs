using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PanelEvent : MonoBehaviour
{
    GameManager gm;
    public Image img1;
    public TextMeshProUGUI nameEvent;
    public TextMeshProUGUI Description;
    public Animator anim;
    public GameObject Panel;
    public TextMeshProUGUI butt;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gameManeger;
        anim = GetComponent<Animator>();

    }
    public void Hover()
    {
        butt.fontStyle = FontStyles.Underline;

    }
    public void NoHover()
    {
        butt.fontStyle = FontStyles.Normal;

    }
    public void Click()
    {
        anim.SetTrigger("quit");
        Panel.SetActive(false);


    }
    public void ClosePanel()
    {
        Panel.SetActive(false);

    }
    public void OpenPanel()
    {
        anim.SetTrigger("start");
        Panel.SetActive(true);

    }
}
