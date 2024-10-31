using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBoard : MonoBehaviour
{
    public GameObject shellPref;
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
    // Start is called before the first frame update
    void Start()
    {
        n = GameManeger.gameManeger.n;
        grid = GetComponent<GridLayoutGroup>();
        DestroyShells();
        GridLayoutConfig(n);
        InstantiateShells(n);
        gm = GameManeger.gameManeger;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GridLayoutConfig(int n)
    {
        grid.constraintCount = n;
        grid.cellSize = new Vector2((height / n) * padding, (height / n) * padding);
        grid.spacing = new Vector2((height / n) * (1 - padding), (height / n) * (1 - padding));

    }
    void InstantiateShells(int n)
    {
        for (int i = 0; i < n * n; i++)
        {
            Instantiate(shellPref, Vector3.zero, Quaternion.identity, gameObject.transform);
        }
        GameManeger.gameManeger.shellLoaded = true;
    }
    void DestroyShells()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }
    public void DrawMaze(int obstaclesAmount, int i, int j)
    {
        Vector2 cord = new Vector2(i, j);
        Queue<Vector2> queue = new Queue<Vector2>();
        bool[,] visited = new bool[n, n];
        queue.Enqueue(cord);
        int a = 0;
        while (obstaclesAmount > 0 && queue.Count > 0)
        {
            Vector2 aux = queue.Dequeue();

            a++;

            gm.BreakObstaculeIn(aux);


            obstaclesAmount--;

            if (aux.x > 0)
            {
                if (dir != 0)
                {
                    int r = Random.Range(0, complexity);
                    if (r == 0 || a == 1)
                    {
                        if (gm.ObstaculeIn(aux + Vector2.left) && !gm.isVisited(visited, aux + Vector2.left))
                        {
                            queue.Enqueue(aux + Vector2.left);
                            gm.VisitShell(visited, aux + Vector2.left);
                            dir = 0;


                        }

                    }
                    gm.VisitShell(visited, aux + Vector2.left);

                }
                else if (gm.ObstaculeIn(aux + Vector2.left) && !gm.isVisited(visited, aux + Vector2.left))
                {
                    queue.Enqueue(aux + Vector2.left);
                    gm.VisitShell(visited, aux + Vector2.left);
                    dir = 0;


                }
            }
            if (aux.y > 0)
            {
                if (dir != 1 || a == 1)
                {
                    int r = Random.Range(0, complexity);
                    if (r == 0 || a == 1)
                    {
                        if (gm.ObstaculeIn(aux + Vector2.down) && !gm.isVisited(visited, aux + Vector2.down))
                        {
                            queue.Enqueue(aux + Vector2.down);
                            gm.VisitShell(visited, aux + Vector2.down);
                            dir = 1;

                        }

                    }
                    gm.VisitShell(visited, aux + Vector2.down);

                }
                else if (gm.ObstaculeIn(aux + Vector2.down) && !gm.isVisited(visited, aux + Vector2.down))
                {
                    queue.Enqueue(aux + Vector2.down);
                    gm.VisitShell(visited, aux + Vector2.down);
                    dir = 1;

                }

            }
            if (aux.y < n - 1)
            {
                if (dir != 2)
                {
                    int r = Random.Range(0, complexity);
                    if (r == 0 || a == 1)
                    {
                        if (gm.ObstaculeIn(aux + Vector2.up) && !gm.isVisited(visited, aux + Vector2.up))
                        {
                            queue.Enqueue(aux + Vector2.up);
                            gm.VisitShell(visited, aux + Vector2.up);
                            dir = 2;


                        }
                    }
                    gm.VisitShell(visited, aux + Vector2.up);

                }
                else
                if (gm.ObstaculeIn(aux + Vector2.up) && !gm.isVisited(visited, aux + Vector2.up))
                {
                    queue.Enqueue(aux + Vector2.up);
                    gm.VisitShell(visited, aux + Vector2.up);
                    dir = 2;


                }
            }

            if (aux.x < n - 1)
            {
                if (dir != 3)
                {
                    int r = Random.Range(0, complexity);
                    if (r == 0 || a == 1)
                    {
                        if (gm.ObstaculeIn(aux + Vector2.right) && !gm.isVisited(visited, aux + Vector2.right))
                        {
                            queue.Enqueue(aux + Vector2.right);
                            gm.VisitShell(visited, aux + Vector2.right);
                            dir = 3;


                        }
                    }
                    gm.VisitShell(visited, aux + Vector2.right);

                }
                else
                if (gm.ObstaculeIn(aux + Vector2.right) && !gm.isVisited(visited, aux + Vector2.right))
                {
                    queue.Enqueue(aux + Vector2.right);
                    gm.VisitShell(visited, aux + Vector2.right);
                    dir = 3;


                }
            }


        }

    }
}
