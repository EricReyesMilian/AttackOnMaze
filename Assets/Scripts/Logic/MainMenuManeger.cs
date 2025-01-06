using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManeger : MonoBehaviour
{
    public List<List<player>> playersList = new List<List<player>>();
    public List<player> titanList = new List<player>();

    public static MainMenuManeger mm;
    public GameObject panelInfo;
    public int PlayerCount = 1;
    public int PlayerIndex = 1;
    public int PlayerRemain;
    public int selectables;
    public Animator anim;
    public GameObject audioO;
    public GameObject slider;
    public Slider progressBar;
    bool activeScene = false;
    public int TopSelection;
    public bool checkAmount;
    void Awake()
    {
        if (mm)
        {
            Destroy(this.gameObject);
        }
        else
        {
            mm = this;
        }
        PlayerIndex = 1;
        PlayerRemain = 6;//cantidad de personajes jugables
        selectables = PlayerRemain;
    }
    void Start()
    {

    }
    public void AddTitans()
    {

        playersList.Add(titanList);

    }
    public void PanelInfo(bool active)
    {
        if (!activeScene)
        {
            if (active)
                AudioManager.speaker.Play(Resources.Load<AudioClip>("fight"));

            panelInfo.SetActive(active);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadScene(int s)
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        SceneManager.LoadSceneAsync(s);
    }

    public void LoadSceneAsync()
    {
        if (!activeScene)
        {
            slider.SetActive(true);
            StartCoroutine(ILoadSceneAsync());
            activeScene = true;
        }
    }

    IEnumerator ILoadSceneAsync()
    {
        // Mostrar la pantalla de carga

        // Iniciar la carga de la escena de manera asíncrona
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        // Desactivar la activación automática de la escena
        operation.allowSceneActivation = false;

        // Actualizar la barra de progreso
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 8f);
            progressBar.value = progress;
            // Activar la escena cuando esté completamente cargada
            if (operation.progress >= 0.9f)
            {

                progressBar.value = 0.125f;
                while (progressBar.value <= 0.99f)
                {
                    progressBar.value += (Time.deltaTime / 2);
                    yield return null;
                    // yield return new WaitForSeconds(0.12f);
                }
                progressBar.value = 1;
                Destroy(audioO);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    public void Plus()
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        PlayerCount++;

        if (PlayerCount > PlayerRemain)
        {
            PlayerCount = 1;
        }
        selectables = PlayerRemain / PlayerCount;
    }
    public void Less()
    {
        AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

        PlayerCount--;
        if (PlayerCount < 1)
        {
            PlayerCount = PlayerRemain;
        }
        selectables = PlayerRemain / PlayerCount;

    }
}
