using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    public Color active;
    public Color disable;
    protected Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        SetColor();

    }
    public virtual void SetColor()
    {
        if (GameManager.gameManeger.SkillEnable && GameManager.gameManeger.players[GameManager.gameManeger.turn].CtransformTime == 0)
            img.color = active;
        else
            img.color = disable;
    }
}
