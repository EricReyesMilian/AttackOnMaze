using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Colorbackground : MonoBehaviour
{
    Image image;
    public TextMeshProUGUI text;//
    public Color color1;
    public Color color2;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.text == "Titans Won       humanity will remember")
        {
            image.color = color2;

        }
        else
        {
            image.color = color1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
