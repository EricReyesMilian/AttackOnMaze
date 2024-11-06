using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public static GameManeger gameManeger;

    public int n;
    [HideInInspector]
    public bool shellLoaded;
    public GameObject shellsContainer;

    public CreateBoard createBoard;


    public List<List<Shell>> matriz = new List<List<Shell>>();


    public List<List<int>> distancia = new List<List<int>>();
    public Shell genericShell;
    //turno
    public int turn = 0;

    //Lista de jugadores
    [HideInInspector]
    public List<PlayerManeger> players = new List<PlayerManeger>();


    public bool playingCorrutine;
    public bool runningBackwards;
    public List<player> player_Scriptable = new List<player>();

    public int shellLimit;
    int shellsAmount = 0;


    public int currentSpeed;

    public List<Vector2> wayToPoint;
    public GameObject playerContainer;

    public GameObject CombatPanel;

    public delegate void Accion();
    public event Accion UpdateDisplay;

    public delegate void Draw();
    public event Draw DrawWay;

    public bool player1Win;
    public int power1Before;
    public int power2Before;

    public delegate void Combate();
    public event Combate StartCombate;

    public delegate void Stats(int i);
    public event Stats UpdateStats;

    bool[,] walls;
    public bool relocate;
    public int loserPlayer;
    public bool combat;
    public bool canPassTurn;
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
        walls = new bool[n, n];
        //este metodo debe ser sustituido por los jugadores seleccionados
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            players.Add(playerContainer.transform.GetChild(i).GetComponent<PlayerManeger>());
            players[i].index = i;
            players[i].play = player_Scriptable[i];
            players[i].InitStats();

            print(players[i].nameC);
        }

        //sets
        players[0].Pos = new Vector2(8, 7);
        players[1].Pos = new Vector2(9, 8);
        players[2].Pos = new Vector2(9, 9);

        currentSpeed = players[turn].speed;//asigna la velocidad de el personaje del turno actual
        CreateDist();//crea la matriz de distancia



    }

    // Update is called once per frame
    void Update()
    {

        if (shellLoaded)
        {
            AddShellsToMatriz();
            MatrizInit();
            FindPlayers();


            //maze
            MazeGenerator maze = new MazeGenerator(n, 7, 8, matriz, Algorithm.Prim);

            //maze

            UpdateDisplay();

            ReachPointInMatriz();
            UpdateStats(turn);

            shellLoaded = false;
        }
        #region DebugInputs
        if (Input.GetKeyDown(KeyCode.T))//fuerza el avance de un turno
        {

            NextTurn();
        }
        if (Input.GetKeyDown(KeyCode.Space))//regenera el laberinto
        {
            // MatrizInit();
            // FindPlayersOnMaze();
            // PositioningShells();
            // MazeGenerator maze = new MazeGenerator(n, 7, 8, matriz, Algorithm.Dfs);


            // UpdateDisplay();

            // ReachPointInMatriz();

            //ReachPointInMatriz();

        }
        #endregion




    }

    //asigna el jugador a la casilla correspondiente
    public void FindPlayers()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].hasAplayer = false;
                matriz[i][j].player = null;

            }
        }
        for (int p = 0; p < players.Count; p++)
        {
            matriz[(int)players[p].Pos.x][(int)players[p].Pos.y].hasAplayer = true;
            matriz[(int)players[p].Pos.x][(int)players[p].Pos.y].player = players[p].play;

        }

    }

    public void ColorBattleZone()
    {
        for (int p = 0; p < players.Count; p++)
        {
            if (p != turn)
            {
                int i = (int)((players[p].Pos.x));
                int j = (int)((players[p].Pos.y));

                if (i > 0)
                {
                    if (matriz[i - 1][j].reach)
                    {
                        matriz[i - 1][j].nearPlayer = true;
                        matriz[i - 1][j].NearPlayers.Add(players[p]);
                    }
                }
                if (j > 0)
                {
                    if (matriz[i][j - 1].reach)
                    {
                        matriz[i][j - 1].nearPlayer = true;
                        matriz[i][j - 1].NearPlayers.Add(players[p]);
                    }



                }
                if (i < n - 1)
                {
                    if (matriz[i + 1][j].reach)
                    {
                        matriz[i + 1][j].nearPlayer = true;
                        matriz[i + 1][j].NearPlayers.Add(players[p]);
                    }

                }

                if (j < n - 1)
                {
                    if (matriz[i][j + 1].reach)
                    {
                        matriz[i][j + 1].nearPlayer = true;
                        matriz[i][j + 1].NearPlayers.Add(players[p]);
                    }

                }
            }
        }
        UpdateDisplay();
    }
    public void ReachPointInSubMatriz(Vector2 pos)
    {


        if (pos.y < (n) / 2 && pos.x <= (n) / 2)
        {

            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    ColorReachShell(new Vector2(i, j));

                }
            }

        }
        else if (pos.y >= (n) / 2 && pos.x > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    ColorReachShell(new Vector2(i, j));

                }
            }

        }
        else if (pos.y < (n) / 2 && pos.x > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    ColorReachShell(new Vector2(i, j));

                }
            }

        }
        else
        {

            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    ColorReachShell(new Vector2(i, j));

                }
            }


        }
    }
    public void ReachPointInMatriz()
    {
        if (!combat)
        {

            int speed = currentSpeed;
            InitReachShell();
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
            ColorBattleZone();

        }
    }
    private void MatrizInit()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].obstacle = true;
                matriz[i][j].NearPlayers.Clear();
                UpdateDisplay();
            }
        }
        shellsAmount = 0;

    }

    public bool isVisited(bool[,] visited, Vector2 cord)
    {
        return visited[int.Parse(cord.x.ToString()), int.Parse(cord.y.ToString())];
    }
    public void VisitShell(bool[,] visited, Vector2 cord)
    {
        visited[int.Parse(cord.x.ToString()), int.Parse(cord.y.ToString())] = true;

    }
    public void NextTurn()
    {
        if (!playingCorrutine && !combat)
        {
            turn += 1;
            if (turn > players.Count - 1)
                turn = 0;

            SetDist();
            InitReachShell();
            currentSpeed = players[turn].speed;
            ReachPointInMatriz();
            UpdateStats(turn);
        }

    }
    public bool ObstaculeIn(Vector2 cord)
    {
        return matriz[(int)cord.x][(int)cord.y].obstacle;
    }
    bool PlayerIn(Vector2 cord)
    {
        return matriz[(int)cord.x][(int)cord.y].hasAplayer;
    }
    void CreateObstaculeIn(Vector2 cord)
    {
        matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].obstacle = true;
    }
    public void BreakObstaculeIn(bool[,] walls)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            for (int j = 0; j < walls.Length; j++)
            {
                if (!walls[i, j])
                {
                    matriz[i][j].obstacle = false;
                    shellsAmount++;

                }


            }
        }
    }
    public void ColorReachShell(Vector2 cord)
    {
        try
        {
            if (!PlayerIn(cord))
            {
                matriz[(int)cord.x][(int)cord.y].reach = true;

            }

        }
        catch
        {
            print(cord);
        }

    }
    public void InitReachShell()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].reach = false;
                matriz[i][j].nearPlayer = false;


            }
        }

    }
    public void UnColorReachShell(Vector2 cord)
    {
        matriz[int.Parse(cord.x.ToString())][int.Parse(cord.y.ToString())].reach = false;



    }
    public void MoveplayerTo(Vector2 target, int index)
    {
        if (!playingCorrutine)
        {
            if (index == turn && !combat)
            {
                if (ReachPoint(target))
                {
                    StartCoroutine(MovePlayerCorutine(target, index));
                }

            }
            else if (combat)
            {
                if (ReachPoint(target))
                {

                    players[index].Pos = target;
                    FindPlayers();
                    ClearMatrizNearPlayers();
                    combat = false;
                    FindPlayers();
                    InitReachShell();
                    SetDist();

                    ReachPointInMatriz();
                }

            }

        }

    }



    IEnumerator MovePlayerCorutine(Vector2 target, int? index)
    {
        playingCorrutine = true;


        matriz[(int)players[index ?? turn].Pos.x][(int)players[index ?? turn].Pos.y].hasAplayer = false;

        if (index == turn && !combat)
        {
            currentSpeed -= distancia[(int)target.x][(int)target.y];
            wayToPoint.Clear();
            SavePath(target);
            wayToPoint.Reverse();
            for (int w = 0; w < wayToPoint.Count; w++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == wayToPoint[w].x && j == wayToPoint[w].y)
                        {
                            matriz[i][j].visitedOnMove = true;
                            players[index ?? turn].Pos = new Vector2(wayToPoint[w].x, wayToPoint[w].y);
                            FindPlayers();
                            UpdateDisplay();
                            if (w > 0)
                                yield return new WaitForSeconds(0.25f); // Espera 0.5 segundos

                        }

                    }

                }

            }

            runningBackwards = true;
            for (int w = 0; w < wayToPoint.Count; w++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == wayToPoint[w].x && j == wayToPoint[w].y)
                        {
                            matriz[i][j].visitedOnMove = false;

                            DrawWay();
                            if (w < wayToPoint.Count - 1)
                                yield return new WaitForSeconds(0.1f); // Espera 0.1 segundos


                        }

                    }

                }

            }
            runningBackwards = false;


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matriz[i][j].visitedOnMove = false;
                    UpdateDisplay();



                }

            }

            //restar current velocity
            wayToPoint.Clear();

            playingCorrutine = false;
            if (matriz[(int)target.x][(int)target.y].nearPlayer)
            {
                InitReachShell();

                Combat(target);

            }
            else
            {
                InitReachShell();
                SetDist();

                ReachPointInMatriz();

            }

        }

        playingCorrutine = false;

        StopCoroutine(MovePlayerCorutine(target, index));

    }
    void ClearMatrizNearPlayers()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriz[i][j].NearPlayers.Clear();

            }
        }

    }
    private void Combat(Vector2 target)
    {
        print("Combat!");
        StartCombate();
        CombatPanel.GetComponent<PanelCombat>().player1 = players[turn];
        power1Before = players[turn].power;
        int i = 0;
        while (true)
        {
            if (matriz[(int)target.x][(int)target.y].NearPlayers[i] != players[turn])
            {
                CombatPanel.GetComponent<PanelCombat>().player2 = matriz[(int)target.x][(int)target.y].NearPlayers[i];
                power2Before = matriz[(int)target.x][(int)target.y].NearPlayers[i].power;

                break;

            }

            i++;
        }

        float ran = Random.Range(0, (float)1);
        print(ran);
        if (power1Before >= power2Before)//atacante mas fuerte
        {
            if ((float)(power1Before) / (power1Before + power2Before) >= ran)
            {
                //gana
                if (power2Before == 1)
                {
                    players[turn].power += 1;
                }
                else
                {
                    players[turn].power += power2Before / 2;

                }


                matriz[(int)target.x][(int)target.y].NearPlayers[i].power /= 2;
                player1Win = true;
            }
            else
            {

                //pierde menos
                if (power1Before == 1)
                {
                    matriz[(int)target.x][(int)target.y].NearPlayers[i].power += 1;
                }
                else
                {
                    if (power1Before < 4)
                    {
                        matriz[(int)target.x][(int)target.y].NearPlayers[i].power += 1;

                    }
                    else
                    {
                        matriz[(int)target.x][(int)target.y].NearPlayers[i].power += power1Before / 4;

                    }

                }
                player1Win = false;

            }

        }
        else
        {
            if ((float)(power1Before) / (power1Before + power2Before) >= ran)
            {

                //gana menos
                if (power2Before == 1)
                {
                    players[turn].power += 1;
                }
                else
                {
                    players[turn].power += power2Before / 4;

                }

                if (power2Before == 1)
                {
                    matriz[(int)target.x][(int)target.y].NearPlayers[i].power -= 1;

                }
                else
                {
                    matriz[(int)target.x][(int)target.y].NearPlayers[i].power -= power2Before / 4;

                }
                player1Win = true;

            }
            else
            {
                //pierde mas
                if (power1Before == 1)
                {
                    players[turn].power -= 1;

                }
                else
                {
                    players[turn].power -= power1Before / 2;

                }
                if (power1Before == 1)
                {
                    matriz[(int)target.x][(int)target.y].NearPlayers[i].power += 1;

                }
                else
                {
                    matriz[(int)target.x][(int)target.y].NearPlayers[i].power += power1Before / 2;

                }
                player1Win = false;


            }
        }

        if (player1Win)
        {
            //seleccionar casilla
            for (int p = 0; p < players.Count; p++)
            {
                if (matriz[(int)target.x][(int)target.y].NearPlayers[i] == players[p])
                {
                    loserPlayer = p;
                    ReachPointInSubMatriz(players[p].Pos);
                    UpdateDisplay();

                    break;
                }
            }
        }
        else
        {
            loserPlayer = turn;
            ReachPointInSubMatriz(players[turn].Pos);
            UpdateDisplay();

            //seleccionar casilla   
        }

        combat = true;
    }


    //metodo recursivo que luego de mover la ficha devuelve el camino mas corto
    void SavePath(Vector2 target)
    {
        wayToPoint.Add(target);
        if (target != players[turn].Pos)
        {
            bool taken = false;
            if (target.y + 1 < n && !taken)
            {
                if (distancia[(int)target.x][(int)target.y + 1] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x, target.y + 1));

                    taken = true;

                }
            }
            if (target.y - 1 >= 0 && !taken)
            {
                if (distancia[(int)target.x][(int)target.y - 1] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x, target.y - 1));

                    taken = true;

                }
            }
            if (target.x + 1 < n && !taken)
            {
                if (distancia[(int)target.x + 1][(int)target.y] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x + 1, target.y));

                    taken = true;

                }

            }
            if (target.x - 1 >= 0 && !taken)
            {
                if (distancia[(int)target.x - 1][(int)target.y] - distancia[(int)target.x][(int)target.y] == -1)
                {
                    SavePath(new Vector2(target.x - 1, target.y));
                    taken = true;

                }
            }

        }
    }
    //comprueba si la casilla puede ser accesible al caminar
    bool ReachPoint(Vector2 target)
    {
        return !(matriz[(int)target.x][(int)target.y].obstacle || matriz[(int)target.x][(int)target.y].hasAplayer) && matriz[(int)target.x][(int)target.y].reach;

    }
    //agrega las clases shells a la matriz
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
    //crea la matriz de distancia
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
    //asigna -1 a los valores de la matriz de distancia
    public void SetDist()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distancia[i][j] = -1;


            }
        }
    }

}
