using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static GameManeger gameManeger;

    public int n;
    public int playersCount;
    public bool shellLoaded;
    public GameObject shellsContainer;
    public List<List<Shell>> matriz = new List<List<Shell>>();
    public PlayerManeger playerTurn;

    //turno
    int turn = 0;
    //
    //Lista de jugadores
    [HideInInspector]
    public List<PlayerManeger> players = new List<PlayerManeger>();

    public GameObject playerStatPrefb;
    public GameObject playerManegerPrefbWrapper;

    public List<player> player_Scriptable = new List<player>();

    public int shellLimit;
    int shellsAmount = 0;
    public List<List<int>> distancia = new List<List<int>>();

    int dir = 3;

    public int currentSpeed;

    [Range(1, 5)]
    public int complexity;
    public List<Vector2> wayToPoint;

    private void Awake()
    {
        if (gameManeger)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManeger = this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //este metodo debe ser sustituido por los jugadores seleccionados
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            players.Add(new PlayerManeger());
            players[i].play = player_Scriptable[i];
            players[i].InitStats();

            print(players[i].nameC);
        }
        for (int i = 0; i < players.Count; i++)
        {
            Instantiate(playerStatPrefb, Vector3.zero, Quaternion.identity, playerManegerPrefbWrapper.transform);
            playerStatPrefb.GetComponent<playerStatUI>().index = i;
        }

        //sets
        players[0].Pos = new Vector2(8, 7);
        players[1].Pos = new Vector2(8, 8);
        currentSpeed = players[turn].speed;
        CreateDist();
        ReachPointInMatriz();
    }

    // Update is called once per frame
    void Update()
    {


        //asignar turno
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            if (turn == i)
            {
                players[i].isPlayerTurn = true;
                playerTurn = players[i];
            }
            else
            {
                players[i].isPlayerTurn = false;

            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            NextTurn();
        }

        if (shellLoaded)
        {
            AddShellsToMatriz();

            AddShellsToObjects();
            MatrizInit();

            DrawMaze(15 * 15, 8, 7);

            shellLoaded = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MatrizInit();
            DrawMaze(15 * 15, 8, 7);
            DrawMaze(15 * 15, 8, 6);
            DrawMaze(15 * 15, 7, 7);
            DrawMaze(15 * 15, 6, 7);


        }
        for (int p = 0; p < players.Count; p++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (ComparePlayerPos(i, j, p))
                    {
                        matriz[i][j].hasAplayer = true;
                        matriz[i][j].player = players[p].play;
                    }
                    {
                        matriz[i][j].hasAplayer = false;

                    }



                }
            }

        }


        PositioningShells();
        print(players[turn].Pos);
        ReachPointInMatriz();

        UpdateDisplayMatriz();
    }

    public void PositioningShells()
    {
        for (int p = 0; p < players.Count; p++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (players[p].Pos == new Vector2(i, j))
                    {
                        matriz[i][j].hasAplayer = true;
                        matriz[i][j].player = players[p].play;

                    }

                }
            }


        }



    }

    private void UpdateDisplayMatriz()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<Shell>().obstacule = matriz[i][j].obstacule;
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<Shell>().hasAplayer = matriz[i][j].hasAplayer;
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<Shell>().player = matriz[i][j].player;
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<Shell>().reach = matriz[i][j].reach;
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<ShellDisplay>().distToShell = distancia[i][j];

            }
        }
    }

    public void ReachPointInMatriz()
    {
        int speed = currentSpeed;
        Vector2 cord = new Vector2(players[turn].Pos.x, players[turn].Pos.y);//inicial
        Queue<Vector2> queue = new Queue<Vector2>();

        bool[,] visited = new bool[n, n];
        distancia[(int)cord.x][(int)cord.y] = 0;

        queue.Enqueue(cord);
        while (queue.Count > 0)
        {
            Vector2 aux = queue.Dequeue();
            ColorReachShell(aux);
            if (distancia[(int)aux.x][(int)aux.y] < speed)
            {

                if (aux.x > 0)
                {
                    if (!PlayerIn(aux + Vector2.left) && !ObstaculeIn(aux + Vector2.left)
                    && !isVisited(visited, aux + Vector2.left)
                    )
                    {
                        queue.Enqueue(aux + Vector2.left);
                        VisitShell(visited, aux + Vector2.left);
                        if (distancia[(int)aux.x][(int)aux.y] < speed)
                            distancia[(int)(aux.x + Vector2.left.x)][(int)(aux.y + Vector2.left.y)] = distancia[(int)(aux.x)][(int)(aux.y)] + 1;

                    }

                }
                if (aux.y > 0)
                {
                    if (!PlayerIn(aux + Vector2.down) && !ObstaculeIn(aux + Vector2.down) && !isVisited(visited, aux + Vector2.down))
                    {
                        queue.Enqueue(aux + Vector2.down);
                        VisitShell(visited, aux + Vector2.down);
                        if (distancia[(int)aux.x][(int)aux.y] < speed)
                            distancia[(int)(aux.x + Vector2.down.x)][(int)(aux.y + Vector2.down.y)] = distancia[(int)(aux.x)][(int)(aux.y)] + 1;

                    }


                }
                if (aux.y < n - 1)
                {
                    if (!PlayerIn(aux + Vector2.up) && !ObstaculeIn(aux + Vector2.up) && !isVisited(visited, aux + Vector2.up))
                    {
                        queue.Enqueue(aux + Vector2.up);
                        VisitShell(visited, aux + Vector2.up);
                        if (distancia[(int)aux.x][(int)aux.y] < speed)
                            distancia[(int)(aux.x + Vector2.up.x)][(int)(aux.y + Vector2.up.y)] = distancia[(int)(aux.x)][(int)(aux.y)] + 1;

                    }

                }

                if (aux.x < n - 1)
                {
                    if (!PlayerIn(aux + Vector2.right) && !ObstaculeIn(aux + Vector2.right) && !isVisited(visited, aux + Vector2.right))
                    {
                        queue.Enqueue(aux + Vector2.right);
                        VisitShell(visited, aux + Vector2.right);
                        if (distancia[(int)aux.x][(int)aux.y] < speed)
                            distancia[(int)(aux.x + Vector2.right.x)][(int)(aux.y + Vector2.right.y)] = distancia[(int)(aux.x)][(int)(aux.y)] + 1;




                    }


                }
            }




        }

    }

    private void MatrizInit()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].obstacule = true;
                UpdateDisplayMatriz();



            }
        }
        shellsAmount = 0;

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

            BreakObstaculeIn(aux);


            obstaclesAmount--;

            if (aux.x > 0)
            {
                if (dir != 0)
                {
                    int r = Random.Range(0, complexity);
                    if (r == 0 || a == 1)
                    {
                        if (ObstaculeIn(aux + Vector2.left) && !isVisited(visited, aux + Vector2.left))
                        {
                            queue.Enqueue(aux + Vector2.left);
                            VisitShell(visited, aux + Vector2.left);
                            dir = 0;


                        }

                    }
                    VisitShell(visited, aux + Vector2.left);

                }
                else if (ObstaculeIn(aux + Vector2.left) && !isVisited(visited, aux + Vector2.left))
                {
                    queue.Enqueue(aux + Vector2.left);
                    VisitShell(visited, aux + Vector2.left);
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
                        if (ObstaculeIn(aux + Vector2.down) && !isVisited(visited, aux + Vector2.down))
                        {
                            queue.Enqueue(aux + Vector2.down);
                            VisitShell(visited, aux + Vector2.down);
                            dir = 1;

                        }

                    }
                    VisitShell(visited, aux + Vector2.down);

                }
                else if (ObstaculeIn(aux + Vector2.down) && !isVisited(visited, aux + Vector2.down))
                {
                    queue.Enqueue(aux + Vector2.down);
                    VisitShell(visited, aux + Vector2.down);
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
                        if (ObstaculeIn(aux + Vector2.up) && !isVisited(visited, aux + Vector2.up))
                        {
                            queue.Enqueue(aux + Vector2.up);
                            VisitShell(visited, aux + Vector2.up);
                            dir = 2;


                        }
                    }
                    VisitShell(visited, aux + Vector2.up);

                }
                else
                if (ObstaculeIn(aux + Vector2.up) && !isVisited(visited, aux + Vector2.up))
                {
                    queue.Enqueue(aux + Vector2.up);
                    VisitShell(visited, aux + Vector2.up);
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
                        if (ObstaculeIn(aux + Vector2.right) && !isVisited(visited, aux + Vector2.right))
                        {
                            queue.Enqueue(aux + Vector2.right);
                            VisitShell(visited, aux + Vector2.right);
                            dir = 3;


                        }
                    }
                    VisitShell(visited, aux + Vector2.right);

                }
                else
                if (ObstaculeIn(aux + Vector2.right) && !isVisited(visited, aux + Vector2.right))
                {
                    queue.Enqueue(aux + Vector2.right);
                    VisitShell(visited, aux + Vector2.right);
                    dir = 3;


                }
            }


        }

    }
    bool ComparePlayerPos(int i, int j, int p)
    {
        return i == players[p].positionOnBoard.x && j == players[p].positionOnBoard.y;
    }
    bool isVisited(bool[,] visited, Vector2 cord)
    {
        return visited[int.Parse(cord.x.ToString()), int.Parse(cord.y.ToString())];

    }
    void VisitShell(bool[,] visited, Vector2 cord)
    {
        visited[int.Parse(cord.x.ToString()), int.Parse(cord.y.ToString())] = true;

    }
    void NextTurn()
    {
        turn += 1;
        if (turn > players.Count - 1)
        {
            turn = 0;
        }
        SetDist();

        InitReachShell();
        currentSpeed = players[turn].speed;
    }
    bool ObstaculeIn(Vector2 cord)
    {
        return matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].obstacule;


    }
    bool PlayerIn(Vector2 cord)
    {
        return matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].hasAplayer;

    }
    void CreateObstaculeIn(Vector2 cord)
    {

        matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].obstacule = true;
        UpdateDisplayMatriz();


    }
    public void BreakObstaculeIn(Vector2 cord)
    {
        //  print(cord);
        matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].obstacule = false;
        shellsAmount++;


    }
    public void ColorReachShell(Vector2 cord)
    {
        matriz[(int)cord.x][(int)cord.y].reach = true;

    }
    public void InitReachShell()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].reach = false;


            }
        }

    }
    public void UnColorReachShell(Vector2 cord)
    {
        matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].reach = false;



    }
    public void MoveplayerTo(Vector2 target)
    {
        print("point");
        if (ReachPoint(target))
        {
            currentSpeed -= distancia[(int)target.x][(int)target.y];
            wayToPoint.Clear();
            SavePath(target);
            wayToPoint.Reverse();
            matriz[(int)players[turn].Pos.x][(int)players[turn].Pos.y].hasAplayer = false;


            players[turn].Pos = target;
            //restar current velocity
            InitReachShell();
            ReachPointInMatriz();

        }
    }
    void SavePath(Vector2 target)
    {
        //
        wayToPoint.Add(target);
        if (target != players[turn].Pos)
        {
            bool taken = false;
            if (target.y + 1 < n - 1 && !taken)
            {
                if (distancia[(int)target.x][(int)target.y + 1] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x, target.y + 1));
                    taken = true;

                }
            }
            if (target.y - 1 > 0 && !taken)
            {
                if (distancia[(int)target.x][(int)target.y - 1] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x, target.y - 1));
                    taken = true;

                }
            }
            if (target.x + 1 < n - 1 && !taken)
            {
                if (distancia[(int)target.x + 1][(int)target.y] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x + 1, target.y));
                    taken = true;

                }

            }
            if (target.x - 1 > 0 && !taken)
            {
                if (distancia[(int)target.x - 1][(int)target.y] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x - 1, target.y));
                    taken = true;

                }
            }

        }
    }
    bool ReachPoint(Vector2 target)
    {
        return !(matriz[(int)target.x][(int)target.y].obstacule || matriz[(int)target.x][(int)target.y].hasAplayer) && matriz[(int)target.x][(int)target.y].reach;

    }
    void AddShellsToMatriz()
    {
        for (int i = 0; i < n; i++)
        {
            matriz.Add(new List<Shell>());

            for (int j = 0; j < n; j++)
            {
                matriz[i].Add(new Shell());
                matriz[i][j].coord = new Vector2(i, j);


            }
        }
    }
    void CreateDist()
    {
        for (int i = 0; i < n; i++)
        {
            distancia.Add(new List<int>());

            for (int j = 0; j < n; j++)
            {
                distancia[i].Add(-1);


            }
        }
    }
    void SetDist()
    {
        for (int i = 0; i < n; i++)
        {

            for (int j = 0; j < n; j++)
            {
                distancia[i][j] = -1;


            }
        }
    }
    void AddShellsToObjects()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<Shell>().coord = matriz[i][j].coord;
                //shellsContainer.transform.GetChild(i * (n) + j).gameObject.GetComponent<ShellDisplay>().shell = matriz[i][j];

            }
        }
    }
}
