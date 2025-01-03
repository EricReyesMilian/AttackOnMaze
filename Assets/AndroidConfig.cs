using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidConfig : MonoBehaviour
{
    public CanvasScaler cs;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        Config();
#endif
    }

    // Update is called once per frame
    void Config()
    {
        cs.referenceResolution = new Vector2(1080, 1920);
    }
}
