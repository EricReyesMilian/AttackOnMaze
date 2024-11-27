using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Event
{
    Trap,
    PowerUp,
    Victory
}
public class GameManeger : MonoBehaviour
{
    public static GameManeger gameManeger;

    public int n;
    public int round = 1;
    [HideInInspector]
    public bool cellLoaded;
    public GameObject cellsContainer;

    public CreateBoard createBoard;



    public List<List<Cell>> matriz = new List<List<Cell>>();
    public List<trap> trapList;
    public List<PowerUp> powerList;

    public List<List<int>> distancia = new List<List<int>>();
    public List<List<int>> distanciaToCenter = new List<List<int>>();
    public Cell genericCell;
    //turno
    public int turn = 0;

    //Lista de jugadores
    [HideInInspector]
    public List<PlayerManeger> players = new List<PlayerManeger>();
    public List<player> player_Scriptable = new List<player>();


    public bool playingCorrutine;
    public bool runningBackwards;



    public List<Vector2> wayToPoint;
    public GameObject playerContainer;


    public delegate void Accion();
    public event Accion UpdateDisplay;

    public delegate void Draw();
    public event Draw DrawWay;


    public delegate void Combate();
    public event Combate StartCombate;

    public delegate void SelectPlayerCombat();
    public event SelectPlayerCombat SelectplayerCombat;

    public delegate void ChangeTurnOrder();
    public event ChangeTurnOrder ChangeTurnOrd;







    public delegate void Stats(int i);
    public event Stats UpdateStats;

    bool[,] walls;
    public bool relocate;
    public int loserPlayer;
    public bool canPassTurn;

    //combat
    public Combat combatScene;
    public bool lastWinner1;
    public bool looserTitan;
    public bool isInCombat;
    public Vector2 combatZoneCoord;
    public List<player> nearPlayers;

    public PanelTrap panelTrap;
    public PanelPowerUp panelPowerUp;



    public List<(int x, int y)> predefinedEmptyCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9) };
    public List<(int x, int y)> predefinedObstacleCells = new List<(int, int)> { (7, 7), (7, 9), (8, 6), (9, 6), (10, 6), (10, 7), (8, 10), (9, 10), (10, 10), (10, 8), (10, 9) };
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
        AddCellsToMatriz();

    }
    // Start is called before the first frame update
    void Start()
    {

        walls = new bool[n, n];
        //este metodo debe ser sustituido por los jugadores seleccionados
        //recordar luego de eso cambiar el orden de ejecucion de los script
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            players.Add(playerContainer.transform.GetChild(i).GetComponent<PlayerManeger>());
            players[i].index = i;
            players[i].play = player_Scriptable[i];
            players[i].InitStats();

            print(players[i].nameC);
        }

        //sets
        players[0].Pos = new Vector2(1, 1);
        players[1].Pos = new Vector2(15, 1);
        players[2].Pos = new Vector2(1, 15);
        players[3].Pos = new Vector2(15, 15);

        ///    players[3].Pos = new Vector2(0, 9);


    }

    // Update is called once per frame
    void Update()
    {

        if (cellLoaded)
        {


            FindPlayers();
            BoardManeger.IniciarDistancias(ref distancia, n);
            BoardManeger.IniciarDistancias(ref distanciaToCenter, n);
            //maze 
            MazeGenerator maze = new MazeGenerator(n, 7, 8, matriz, Algorithm.Prim, predefinedEmptyCells, predefinedObstacleCells);
            BoardManeger boardManeger = new BoardManeger(matriz);
            boardManeger.AddTraps();
            boardManeger.AddPowerUps();
            //maze 

            distanciaToCenter = BoardManeger.Lee(matriz, n / 2, n / 2, n * n);

            ReachPointInMatriz();
            UpdateStats(turn);
            UpdateDisplay();
            cellLoaded = false;
        }
        #region DebugInputs
        if (Input.GetKeyDown(KeyCode.T))//fuerza el avance de un turno
        {

            NextTurn();
        }
        if (Input.GetKeyDown(KeyCode.Space))//regenera el laberinto
        {
            SceneManager.LoadScene(0);

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
    //marca las casillas con jugadores cercanos y los enlista
    public void ColorBattleZone()
    {
        ClearMatrizNearPlayers();
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
    //marca todas las casillas alcanzables
    public void ReachPointInMatriz()
    {
        if (!isInCombat)
        {
            int speed = players[turn].currentSpeed;
            InitReachCell();
            distancia = BoardManeger.Lee(matriz, (int)players[turn].Pos.x, (int)players[turn].Pos.y, speed);
            BoardManeger.ColorReachCell(matriz, distancia);
            ColorBattleZone();

        }
    }
    //verifica si es posible y pasa el turno
    public void NextTurn()
    {
        if (!playingCorrutine && !isInCombat)
        {

            turn = NextTurnIndex(turn);
            ChangeTurnOrd();
            SetDist();
            InitReachCell();
            players[turn].ResetSpeed();

            ReachPointInMatriz();

            if (turn == 0)
            {
                round++;
            }
            //move titan
            if (players[turn].play.isTitan)
            {
                MoveTitan();

            }
            UpdateStats(turn);

        }

    }
    private void MoveTitan()
    {
        List<(int x, int y)> targets = new List<(int x, int y)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (matriz[i][j].reach)
                {
                    targets.Add(((int)matriz[i][j].coord.x, (int)matriz[i][j].coord.y));
                }
            }
        }
        int destination = TargetIACell(players[turn], targets);
        MoveplayerTo(new Vector2(targets[destination].x, targets[destination].y), turn);


    }
    int TargetIACell(PlayerManeger titan, List<(int x, int y)> targets)
    {
        switch (titan.play.TitanIQ)
        {
            case 0:
                return IQTonto(titan, targets);
            case 1:
                return IQ1(titan, targets);
            case 2:
                return IQ2(titan, targets);
            case 3:
                return IQ3(titan, targets);
            case 4:
                return IQ4(titan, targets);
            default:
                return Random.Range(0, targets.Count);


        }
    }
    int IQTonto(PlayerManeger titan, List<(int x, int y)> targets)
    {
        return Random.Range(0, targets.Count);

    }
    int IQ1(PlayerManeger titan, List<(int x, int y)> targets)
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {

            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    int IQ2(PlayerManeger titan, List<(int x, int y)> targets)
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (matriz[coord.x][coord.y].nearPlayer)
            {
                return targets.IndexOf(coord);
            }
            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    int IQ3(PlayerManeger titan, List<(int x, int y)> targets)
    {
        int max = -5;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (matriz[coord.x][coord.y].nearPlayer && titan.power >= matriz[coord.x][coord.y].player.power)
            {
                return targets.IndexOf(coord);
            }
            if (distancia[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    int IQ4(PlayerManeger titan, List<(int x, int y)> targets)
    {
        int max = int.MinValue;
        int i = 1;
        bool huboCambio = false;
        foreach (var coord in targets)
        {
            if (matriz[coord.x][coord.y].nearPlayer)
            {
                if (titan.power >= matriz[coord.x][coord.y].NearPlayers[0].power)
                {
                    huboCambio = true;
                    //SelectPlayer(new Vector2((int)coord.x, (int)coord.y), true);
                    return targets.IndexOf(coord);

                }
            }
            if (distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y] >= max)
            {
                if (!titan.lastMove.Contains(coord))
                {
                    i = targets.IndexOf(coord);
                    max = distancia[coord.x][coord.y] - distanciaToCenter[coord.x][coord.y];
                    huboCambio = true;
                }
            }

        }
        if (!huboCambio)
        {
            titan.lastMove.Clear();
        }
        return i;

    }
    //suma 1 al turno
    public int NextTurnIndex(int turn)
    {
        if (turn + 1 > players.Count - 1)
            return 0;
        else
            return turn + 1;

    }
    //desmarca todas las casillas alcanzables y con jugadores cercanos
    public void InitReachCell()
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
    //mueve a jugador
    public void MoveplayerTo(Vector2 target, int index)
    {
        if (!playingCorrutine)
        {
            if (index == turn && !isInCombat)
            {
                if (ReachPoint(target))
                {
                    players[turn].lastMove.Add(((int)players[turn].Pos.x, (int)players[turn].Pos.y));
                    StartCoroutine(MovePlayerCorutine(target, index));

                    StartCoroutine(ActivateInteraction(target, index));




                }
                UpdateStats(turn);

                if (players[turn].play.isTitan)
                {
                    // NextTurn();
                }
                else
                {
                    ReachPointInMatriz();

                }


            }
            else if (isInCombat)
            {
                if (ReachPoint(target))
                {

                    players[index].Pos = target;
                    FindPlayers();
                    ClearMatrizNearPlayers();
                    isInCombat = false;
                    FindPlayers();

                    if (players[turn].play.isTitan)
                    {
                        InitReachCell();
                        SetDist();

                        ReachPointInMatriz();

                        NextTurn();
                    }
                    else
                    {
                        InitReachCell();
                        SetDist();

                        ReachPointInMatriz();
                    }


                }
                UpdateStats(turn);


            }

        }


    }
    bool isTrap(Vector2 target)
    {
        return matriz[(int)target.x][(int)target.y].trap && matriz[(int)target.x][(int)target.y].enableTrap;
    }
    bool isPowerUp(Vector2 target)
    {
        return matriz[(int)target.x][(int)target.y].powerUp;
    }
    public void ActivateTrap(trap trap, Vector2 target, PlayerManeger player)
    {
        matriz[(int)target.x][(int)target.y].trapActivated = true;
        TrapTriggerEffect trapManeger = new TrapTriggerEffect(trap, player);
        panelTrap.trap = trap;
        panelTrap.OpenPanel();
        if (trap.activationTrap)
        {
            foreach (var trapChild in matriz[(int)target.x][(int)target.y].childsTrap)
            {
                print("pop");
                matriz[trapChild.x][trapChild.y].enableTrap = false;
            }
        }
        ReachPointInMatriz();

    }
    public void ActivatePower(PowerUp powerUp, Vector2 target, PlayerManeger player)
    {
        matriz[(int)target.x][(int)target.y].powerUp = false;
        PowerTrigger PowerManeger = new PowerTrigger(powerUp, player);
        panelPowerUp.powerUp = powerUp;
        panelPowerUp.OpenPanel();
        ReachPointInMatriz();


    }

    //corrutina para el movimiento del jugador
    IEnumerator MovePlayerCorutine(Vector2 target, int? index)
    {
        playingCorrutine = true;


        matriz[(int)players[index ?? turn].Pos.x][(int)players[index ?? turn].Pos.y].hasAplayer = false;

        if (index == turn && !isInCombat)
        {
            players[index ?? turn].DownCurrentSpeed(distancia[(int)target.x][(int)target.y]);
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
            // if (matriz[(int)target.x][(int)target.y].nearPlayer)
            // {
            //     InitReachCell();

            //     SelectPlayer(target);

            // }
            // else
            // {
            InitReachCell();
            SetDist();

            ReachPointInMatriz();

            // }

        }


        playingCorrutine = false;
        if (players[turn].play.isTitan && !matriz[(int)target.x][(int)target.y].nearPlayer)
        {
            print("endTitanTurn");
            NextTurn();
        }
        StopCoroutine(MovePlayerCorutine(target, index));

    }

    IEnumerator ActivateInteraction(Vector2 target, int index)
    {
        yield return new WaitForSeconds(0.25f); // Espera 0.5 segundos
        if (!playingCorrutine)
        {
            if (isPowerUp(target) && !players[index].play.isTitan)
            {
                ActivatePower(matriz[(int)target.x][(int)target.y].powerUpType, target, players[index]);
            }
            if (isTrap(target) && !players[index].play.isTitan)
            {
                ActivateTrap(matriz[(int)target.x][(int)target.y].trapType, target, players[index]);

            }
            if (matriz[(int)target.x][(int)target.y].nearPlayer)
            {
                InitReachCell();
                SelectPlayer(target, players[index].play.isTitan);

            }
            else if (!players[index].play.isTitan)
            {
                InitReachCell();
                SetDist();

                ReachPointInMatriz();

            }

            StopAllCoroutines();

        }
        else
        {
            StartCoroutine(ActivateInteraction(target, index));

        }

    }

    //inicializa la lista de jugadores cercanos
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
    //selecciona un jugador entre los mas cercanos para iniciar el combate
    private void SelectPlayer(Vector2 target, bool titan)
    {
        nearPlayers.Clear();
        combatZoneCoord = target;
        for (int k = 0; k < matriz[(int)target.x][(int)target.y].NearPlayers.Count; k++)
        {
            nearPlayers.Add(matriz[(int)target.x][(int)target.y].NearPlayers[k].play);

        }
        if (titan)
        {
            int powerMax = int.MaxValue;
            int indexFightForTitan = 0;
            for (int k = 0; k < nearPlayers.Count; k++)
            {
                if (nearPlayers[k].power <= powerMax)
                {
                    powerMax = nearPlayers[k].power;
                    indexFightForTitan = k;
                }

            }
            Combat(combatZoneCoord, indexFightForTitan);

        }
        else
        if (nearPlayers.Count > 1)
        {
            SelectplayerCombat();


        }
        else
        {
            Combat(combatZoneCoord, 0);
        }
    }
    //combate
    public void Combat(Vector2 target, int indexNearPlayer)
    {
        combatScene = new Combat(players[turn], matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer]);
        StartCombate();


        //combat maneger
        lastWinner1 = combatScene.Player1IsWinner();

        players[turn].PowerUp(combatScene.Reward(lastWinner1, 1));
        matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer].PowerUp(combatScene.Reward(lastWinner1, 2));
        //resultado de la victoria
        if (lastWinner1)
        {
            //seleccionar casilla

            for (int p = 0; p < players.Count; p++)
            {
                if (matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer] == players[p])
                {
                    loserPlayer = p;
                    distancia = BoardManeger.ReachPointInSubMatriz(matriz, (int)players[p].Pos.x, (int)players[p].Pos.y);
                    ListHelper.MoveElement(players, players[p], NextTurnIndex(turn));
                    ChangeTurnOrd();
                    BoardManeger.ColorReachCell(matriz, distancia);
                    UpdateDisplay();

                    break;
                }
            }

            looserTitan = true;

            if (players[turn].play.isTitan)
            {
                looserTitan = false;
                List<Cell> posiblesMoves = new List<Cell>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (matriz[i][j].reach)
                        {
                            posiblesMoves.Add(matriz[i][j]);
                        }
                    }
                }
                isInCombat = true;
                MoveplayerTo(posiblesMoves[Random.Range(0, posiblesMoves.Count)].coord, NextTurnIndex(turn));
                //NextTurn();
            }
            else
            {
                isInCombat = true;

            }



        }
        else
        {
            loserPlayer = turn;
            distancia = BoardManeger.ReachPointInSubMatriz(matriz, (int)players[turn].Pos.x, (int)players[turn].Pos.y);
            BoardManeger.ColorReachCell(matriz, distancia);
            looserTitan = false;

            if (matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer].play.isTitan)
            {
                looserTitan = true;

                List<Cell> posiblesMoves = new List<Cell>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (matriz[i][j].reach)
                        {
                            posiblesMoves.Add(matriz[i][j]);
                        }
                    }
                }
                isInCombat = true;
                MoveplayerTo(posiblesMoves[Random.Range(0, posiblesMoves.Count)].coord, turn);
                //NextTurn();
            }
            else
            {
                isInCombat = true;

            }

            UpdateDisplay();

            //seleccionar casilla   
        }


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
        return matriz[(int)target.x][(int)target.y].reach;

    }
    //agrega las clases cells a la matriz
    void AddCellsToMatriz()
    {
        for (int i = 0; i < n; i++)
        {
            matriz.Add(new List<Cell>());

            for (int j = 0; j < n; j++)
            {
                matriz[i].Add(new Cell());
                matriz[i][j].coord = new Vector2(i, j);

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
