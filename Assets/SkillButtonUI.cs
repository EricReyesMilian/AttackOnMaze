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
        if (GameManeger.gameManeger.NextEnable)
                img.color = active;
            else
                img.color = disable;//
    }
}
