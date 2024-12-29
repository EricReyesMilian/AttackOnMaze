using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualBoardController : MonoBehaviour
{

    public GameObject cellPref;
    GridLayoutGroup grid;
    public float height = 600;
    [Range(0, 0.99f)]
    public float padding;

    void Start()
    {
        GameManager.gameManeger.displayGrid += InitBoard;
    }
    public void InitBoard()
    {
        GridLayoutConfig(17);
        IAsigCells(17);

    }
    void Update()
    {
        if (gameObject.transform.childCount > 0)
        {
            if (gameObject.transform.GetChild(0).GetComponent<CellDisplay>().coord == new Vector2(GameManager.gameManeger.n - 1, GameManager.gameManeger.n - 1))
            {
                Transform firstChild = gameObject.transform.GetChild(0);
                firstChild.SetSiblingIndex(gameObject.transform.childCount - 1);

            }
        }


    }

    public void GridLayoutConfig(int n)
    {
        grid = GetComponent<GridLayoutGroup>();

        grid.constraintCount = n;
        grid.cellSize = new Vector2((height / n) * padding, (height / n) * padding);
        grid.spacing = new Vector2((height / n) * (1 - padding), (height / n) * (1 - padding));

    }
    public void InstantiateCells(int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Instantiate(cellPref, Vector3.zero, Quaternion.identity, gameObject.transform);
                cellPref.GetComponent<CellDisplay>().coord = new Vector2(i, j);
            }
        }

    }
    public void IAsigCells(int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                gameObject.transform.GetChild(i + j * (n)).GetComponent<CellDisplay>().coord = new Vector2(i, j);
            }
        }

    }

    public void DestroyCells()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }


}
