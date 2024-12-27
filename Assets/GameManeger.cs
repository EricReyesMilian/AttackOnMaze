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

    public List<List<Cell>> matriz = new List<List<Cell>>();
    public List<trap> trapList;
    public List<PowerUp> powerList;

    public List<List<int>> distancia = new List<List<int>>();
    public List<List<int>> distanciaToCenter = new List<List<int>>();
    // public Cell genericCell;
    //turno
    public int turn = -1;

    //Lista de jugadores
    [HideInInspector]
    public List<PlayerManeger> players = new List<PlayerManeger>();
    // sustituir por la cantidad de jugadores implementados
    public List<player> player_Scriptable = new List<player>();


    public bool playingCorrutine;
    public bool runningBackwards;



    public List<Vector2> wayToPoint;
    public GameObject playerContainer;

    bool skillWasActivatedThisTurn = false;
    public delegate void Accion();
    public event Accion UpdateDisplay;

    public delegate void Draw();
    public event Draw DrawWay;


    public delegate void Combate();
    public event Combate StartCombate;

    public delegate void startDisplay(int n);
    public event startDisplay StartDisplay;

    public delegate void SelectPlayerCombat();
    public event SelectPlayerCombat SelectplayerCombat;

    public delegate void ChangeTurnOrder();
    public event ChangeTurnOrder ChangeTurnOrd;



    public CreateBoard createBoard;



    public delegate void Stats(int i);
    public event Stats UpdateStats;

    bool[,] walls;
    public bool relocate;
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

    public bool SkillEnable = false;
    public bool ErenSkill;
    public bool ReinerSkill;
    public bool ArminSkill;
    public bool LeviSkill;
    public PlayerManeger player2;
    public List<(int x, int y)> predefinedEmptyCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 9), (8, 6), (8, 10), (10, 8), (10, 6), (6, 10), (10, 10) };
    public List<(int x, int y)> predefinedObstacleCells = new List<(int, int)> { (6, 9), (6, 7), (7, 10), (7, 6), (9, 6), (10, 7), (9, 10), (10, 9) };
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
        createBoard.InitBoard(n);

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




        //loaded

        FindPlayers();
        BoardManeger.IniciarDistancias(ref distancia, n);
        BoardManeger.IniciarDistancias(ref distanciaToCenter, n);
        //maze 
        MazeGenerator maze = new MazeGenerator(n, 7, 8, matriz, Algorithm.Prim, predefinedEmptyCells, predefinedObstacleCells);
        predefinedEmptyCells.Add((7, 8));
        BoardManeger boardManeger = new BoardManeger(matriz);
        matriz[6][8].obstacle = true;
        matriz[6][8].destroyableObs = true;

        matriz[8][6].obstacle = true;
        matriz[8][6].destroyableObs = true;

        matriz[10][8].obstacle = true;
        matriz[10][8].destroyableObs = true;

        matriz[8][10].obstacle = true;
        matriz[8][10].destroyableObs = true;

        boardManeger.AddTraps();
        boardManeger.AddPowerUps();
        //maze 

        distanciaToCenter = BoardManeger.Lee(matriz, n / 2, n / 2, n * n);

        ReachPointInMatriz();
        UpdateStats(turn);

        turn = -1;
        NextTurn();

    }

    // Update is called once per frame
    void Update()
    {


        #region DebugInputs
        if (Input.GetKeyDown(KeyCode.D))//romper pared
        {


            matriz[6][8].TakeDamage(5);
        }
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
        //quitar este metodo pal carajo
        // optimization:
        // ----- actualizar el tablero antes y despues de mover un ugador optimizando la cantidd=ad de operaciones
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
            if (ReinerSkill && players[turn].play.name == "Reiner" && players[turn].currentSpeed > 0)
            {
                PlayerSkills.ReinerRun(players[turn], matriz, distancia, predefinedEmptyCells);

                UpdateDisplay();

            }
            ColorBattleZone();

        }
    }
    //verifica si es posible y pasa el turno
    public void NextTurn()
    {
        if (!playingCorrutine && !isInCombat)
        {

            if (round > 1)
            {
                players[turn].DownCooldown();

            }
            if (skillWasActivatedThisTurn)
            {
                players[turn].ResetCooldown();
                print("skill1");
                skillWasActivatedThisTurn = false;
            }
            turn = NextTurnIndex(turn);

            if (round > 1)
            {
                if (players[turn].play.name == "Reiner" && ReinerSkill)
                {
                    players[turn].CtransformTime++;
                    if (players[turn].CtransformTime > players[turn].TransformTime)
                    {
                        ReinerSkill = false;
                        PlayerSkills.ReinerSkillOff(players[turn]);
                        players[turn].CtransformTime = 0;
                        players[turn].ResetCooldown();
                    }
                }
                else
                if (players[turn].play.name == "Eren" && ErenSkill)
                {
                    players[turn].CtransformTime++;
                    if (players[turn].CtransformTime > players[turn].TransformTime)
                    {
                        ErenSkill = false;
                        PlayerSkills.ErenSkillOff(players[turn]);
                        players[turn].CtransformTime = 0;
                        players[turn].ResetCooldown();
                    }
                }
                else
                               if (players[turn].play.name == "Armin" && ArminSkill)
                {
                    players[turn].CtransformTime++;
                    if (players[turn].CtransformTime > players[turn].TransformTime)
                    {
                        ArminSkill = false;
                        PlayerSkills.ArminSkillOff(players[turn]);
                        players[turn].CtransformTime = 0;
                        players[turn].ResetCooldown();
                    }
                }
            }

            ChangeTurnOrd();
            SetDist();
            InitReachCell();
            players[turn].ResetSpeed();

            ReachPointInMatriz();

            if (turn == 0)
            {
                round++;
            }
            if (players[turn].currentCooldown == 0 && !players[turn].play.isTitan)
            {
                SkillEnable = true;
            }
            else
            {
                SkillEnable = false;

            }
            //move titan
            if (players[turn].play.isTitan)
            {
                MoveTitan();

            }
            UpdateStats(turn);

        }

    }

    public void Skill()
    {
        if (SkillEnable)
        {
            skillWasActivatedThisTurn = true;
            //SetFalseSkills();
            SkillEnable = false;
            player sCplayer = players[turn].play;

            switch (sCplayer.Name)
            {
                case "Eren":
                    ErenSkill = true;
                    PlayerSkills.ErenSkillEf(players[turn]);
                    ReachPointInMatriz();
                    UpdateStats(turn);
                    UpdateDisplay();

                    break;
                case "Reiner":
                    ReinerSkill = true;
                    PlayerSkills.ReinerSkillEf(players[turn], matriz, distancia, predefinedEmptyCells);
                    ReachPointInMatriz();
                    UpdateStats(turn);
                    UpdateDisplay();

                    break;
                case "Armin":
                    ArminSkill = true;
                    PlayerSkills.ArminSkillEf(players[turn], matriz, predefinedObstacleCells);
                    ReachPointInMatriz();
                    UpdateStats(turn);
                    UpdateDisplay();

                    break;
                case "Levi":
                    LeviSkill = true;
                    PlayerSkills.LeviSkillEf(players[turn], matriz, distancia);

                    UpdateDisplay();
                    break;
            }

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
                if (titan.power >= matriz[coord.x][coord.y].NearPlayers[0].power
                && !matriz[coord.x][coord.y].NearPlayers[0].play.isTitan)
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
                matriz[i][j].special = false;
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
                    bool aux = LeviSkill;
                    if (LeviSkill)
                    {
                        if ((int)players[turn].Pos.x == (int)target.x)
                        {
                            foreach (var play in players)
                            {
                                if (play.Pos.x == players[turn].Pos.x
                                    && play.Pos.y < target.y && play.Pos.y != players[turn].Pos.y)
                                {
                                    play.PowerUp(-players[turn].power / 4);
                                }
                            }
                        }
                        if ((int)players[turn].Pos.y == (int)target.y)
                        {
                            foreach (var play in players)
                            {
                                if (play.Pos.y == players[turn].Pos.y
                                    && play.Pos.x < target.y && play.Pos.x != players[turn].Pos.x)
                                {
                                    play.PowerUp(-players[turn].power / 4);
                                }
                            }
                        }
                        LeviSkill = false;
                    }

                    players[turn].lastMove.Add(((int)players[turn].Pos.x, (int)players[turn].Pos.y));
                    StartCoroutine(MovePlayerCorutine(target, index));

                    StartCoroutine(ActivateInteraction(target, index, aux));



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

    IEnumerator ActivateInteraction(Vector2 target, int index, bool levi)
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
            if (matriz[(int)target.x][(int)target.y].nearPlayer && !levi)
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
            StartCoroutine(ActivateInteraction(target, index, levi));

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
        PlayerManeger player1 = players[turn];
        PlayerManeger player2 = matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer];

        combatScene = new Combat(player1, player2);
        StartCombate();


        //combat maneger
        lastWinner1 = combatScene.Player1IsWinner();

        player1.PowerUp(combatScene.Reward(lastWinner1, 1));
        player2.PowerUp(combatScene.Reward(lastWinner1, 2));

        this.player2 = player2;
        if (lastWinner1)//si gana el jugador poseedor del turno (player1)
        {

            distancia = BoardManeger.ReachPointInSubMatriz(matriz, (int)player2.Pos.x, (int)player2.Pos.y);

            if (!player1.play.isTitan)
            {
                print(player2.nameC);


                if (players.IndexOf(player1) > players.IndexOf(player2))
                {
                    turn--;
                    if (turn < 0)
                    {
                        turn = players.Count - 1;
                    }
                }
                ListHelper.MoveElement(players, player2, players.IndexOf(player1));

                ChangeTurnOrd();

            }
            BoardManeger.ColorReachCell(matriz, distancia);
            UpdateDisplay();
            ChangeTurnOrd();


            looserTitan = true;

            if (player1.play.isTitan)//si gano un titan
            {
                looserTitan = false;
                List<Cell> posiblesMoves = new List<Cell>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (matriz[i][j].reach && !predefinedEmptyCells.Contains((i, j)))
                        {
                            posiblesMoves.Add(matriz[i][j]);
                        }
                    }
                }
                isInCombat = true;
                ListHelper.MoveElement(players, player2, NextTurnIndex(players.IndexOf(player1)));
                ChangeTurnOrd();
                MoveplayerTo(posiblesMoves[Random.Range(0, posiblesMoves.Count)].coord, players.IndexOf(player2));

            }
            else
            {
                isInCombat = true;

            }



        }
        else//si pierde el jugador poseedor del turno (player1)
        {

            distancia = BoardManeger.ReachPointInSubMatriz(matriz, (int)player1.Pos.x, (int)player1.Pos.y);
            BoardManeger.ColorReachCell(matriz, distancia);
            UpdateDisplay();

            looserTitan = false;

            if (player2.play.isTitan)//si gano un titan
            {
                looserTitan = true;

                List<Cell> posiblesMoves = new List<Cell>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (matriz[i][j].reach && !predefinedEmptyCells.Contains((i, j)))
                        {
                            posiblesMoves.Add(matriz[i][j]);
                        }
                    }
                }
                isInCombat = true;
                MoveplayerTo(posiblesMoves[Random.Range(0, posiblesMoves.Count)].coord, players.IndexOf(player1));
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
        if (target != players[turn].Pos && distancia[(int)target.x][(int)target.y] != 0)
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
