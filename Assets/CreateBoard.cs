using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour
{

    public GameObject cellPref;
    GridLayoutGroup grid;
    int n;
    public float height = 600;
    [Range(0, 0.99f)]
    public float padding;

    //maze
    public int obstaclesAmount;
    GameManeger gm;

    int dir;
    [Range(0, 5)]
    public int complexity;

    bool ready;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManeger.gameManeger;
        n = GameManeger.gameManeger.n;
        grid = GetComponent<GridLayoutGroup>();
        DestroyCells();
        GridLayoutConfig(n);
        InstantiateCells(n);
    }

    void Update()
    {
        if (ready)
        {
            if (gameObject.transform.childCount > 0)
            {
                Transform firstChild = gameObject.transform.GetChild(0);
                firstChild.SetSiblingIndex(gameObject.transform.childCount - 1);
            }
            ready = false;
        }
    }
    void GridLayoutConfig(int n)
    {
        grid.constraintCount = n;
        grid.cellSize = new Vector2((height / n) * padding, (height / n) * padding);
        grid.spacing = new Vector2((height / n) * (1 - padding), (height / n) * (1 - padding));

    }
    void InstantiateCells(int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Instantiate(cellPref, Vector3.zero, Quaternion.identity, gameObject.transform);
                cellPref.GetComponent<CellDisplay>().coord = new Vector2(i, j);
            }
        }
        ready = true;

        gm.cellLoaded = true;
    }

    void DestroyCells()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }


}
