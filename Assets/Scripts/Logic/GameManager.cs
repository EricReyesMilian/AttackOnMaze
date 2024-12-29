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
public class GameManager : MonoBehaviour
{
    public static GameManager gameManeger;

    // Configuración del juego
    public int n;
    public int round = 1;
    public GameObject cellsContainer;
    public GameObject playerContainer;

    // Estado del juego
    [HideInInspector]
    public bool cellLoaded;
    public int turn = -1;
    public bool playingCorrutine;
    public bool runningBackwards;
    public bool lastWinner1;
    public bool looserTitan;
    public bool isInCombat;
    public bool NextEnable = true;
    public bool SkillEnable = false;
    public bool ZekeSkill;
    public bool ErenSkill;
    public bool ReinerSkill;
    public bool ArminSkill;
    public bool LeviSkill;
    public bool HasMoved = false;
    bool skillWasActivatedThisTurn = false;
    public Vector2 combatZoneCoord;
    // Listas
    public List<List<Cell>> matriz = new List<List<Cell>>();
    public List<trap> trapList;
    public List<PowerUp> powerList;
    public List<List<int>> distancia = new List<List<int>>();
    public List<List<int>> distanciaToCenter = new List<List<int>>();
    [HideInInspector]
    public List<PlayerManager> players = new List<PlayerManager>();
    public List<player> player_Scriptable = new List<player>();
    public List<Vector2> wayToPoint;
    public List<player> nearPlayers;
    public List<(int x, int y)> predefinedEmptyCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15), (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 9), (8, 6), (8, 10), (10, 8), (10, 6), (6, 10), (10, 10) };
    public List<(int x, int y)> predefinedCenterCells = new List<(int, int)> { (8, 7), (8, 8), (8, 9), (9, 7), (9, 8), (9, 9), (7, 7), (7, 8), (7, 9) };
    public List<(int x, int y)> predefinedObstacleCells = new List<(int, int)> { (6, 9), (6, 7), (7, 10), (7, 6), (9, 6), (10, 7), (9, 10), (10, 9) };
    public List<(int x, int y)> startCells = new List<(int, int)> { (1, 1), (15, 1), (1, 15), (15, 15), (7, 1), (15, 8), (1, 7), (8, 15) };

    // Delegados y eventos
    public delegate void Accion();
    public event Accion UpdateDisplay;

    public delegate void Draw();
    public event Draw DrawWay;

    public delegate void Combate();
    public event Combate StartCombate;

    public delegate void startDisplay(int n);
    public event startDisplay StartDisplay;

    public delegate void OpenPanelTrap(trap trap);
    public event OpenPanelTrap openPanelTrap;

    public delegate void OpenPanelPowerUp(PowerUp powerUp);
    public event OpenPanelPowerUp openPanelPowerUp;

    public delegate void SelectPlayerCombat();
    public event SelectPlayerCombat SelectplayerCombat;

    public delegate void CreatePlayerBoxInstances();
    public event CreatePlayerBoxInstances createPlayerBoxInstances;

    public delegate void ChangeTurnOrder();
    public event ChangeTurnOrder ChangeTurnOrd;

    public delegate void DisplayGrid();
    public event DisplayGrid displayGrid;

    public delegate void Stats(int i);
    public event Stats UpdateStats;

    // Otros
    public Combat combatScene;
    public PlayerManager player2;
    public PanelTrap panelTrap;
    public PanelPowerUp panelPowerUp;
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
    void Start()
    {
        displayGrid();
        LoadPlayers();
        InitializePlayers();
        InitializeBoard();
        InitializeGame();
    }
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
            SceneManager.LoadScene(2);

        }
        #endregion
        NextEnable = !players[turn].play.isTitan && !playingCorrutine && !isInCombat && HasMoved;
    }
    void LoadPlayers()
    {
        for (int i = 0; i < MainMenuManeger.mm.playersList.Count; i++)
        {
            for (int j = 0; j < MainMenuManeger.mm.playersList[i].Count; j++)
            {
                player_Scriptable.Add(MainMenuManeger.mm.playersList[i][j]);
            }

        }
    }
    void InitializePlayers()
    {
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            players.Add(new PlayerManager());
            players[i].index = i;
            players[i].play = player_Scriptable[i];
            players[i].InitStats();
            players[i].Pos = new Vector2(startCells[i].x, startCells[i].y);
            PlacePlayerIn(startCells[i].x, startCells[i].y, i);
            print(players[i].nameC);

        }

    }
    void PlacePlayerIn(int i, int j, int index)
    {
        matriz[i][j].hasAplayer = true;
        matriz[i][j].player = players[index].play;

    }
    void UnPlacePlayer()
    {
        matriz[(int)players[turn].Pos.x][(int)players[turn].Pos.y].hasAplayer = false;
        matriz[(int)players[turn].Pos.x][(int)players[turn].Pos.y].player = null;

    }
    void InitializeBoard()
    {
        Board.IniciarDistancias(ref distancia, n);
        Board.IniciarDistancias(ref distanciaToCenter, n);
        MazeGenerator maze = new MazeGenerator(n, 7, 8, matriz, Algorithm.Prim, predefinedEmptyCells, predefinedObstacleCells);
        predefinedEmptyCells.Add((7, 8));
        Board board = new Board(matriz);
        AssignDestroyableWalls();
        board.AddTraps();
        board.AddPowerUps();
        distanciaToCenter = Board.Lee(matriz, n / 2, n / 2, n * n);

    }
    void InitializeGame()
    {

        ReachPointInMatriz();
        UpdateStats(turn);
        turn = -1;
        createPlayerBoxInstances();
        NextTurn();
    }
    void SetBattleZone()
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
    void AssignDestroyableWalls()
    {
        matriz[6][8].obstacle = true;
        matriz[6][8].destroyableObs = true;

        matriz[8][6].obstacle = true;
        matriz[8][6].destroyableObs = true;

        matriz[10][8].obstacle = true;
        matriz[10][8].destroyableObs = true;

        matriz[8][10].obstacle = true;
        matriz[8][10].destroyableObs = true;
    }
    public void ReachPointInMatriz()
    {
        if (!isInCombat)
        {
            int speed = players[turn].currentSpeed;
            InitReachCell();
            distancia = Board.Lee(matriz, (int)players[turn].Pos.x, (int)players[turn].Pos.y, speed);
            Board.ColorReachCell(matriz, distancia);
            if (ReinerSkill && players[turn].play.name == "Reiner" && players[turn].currentSpeed > 0)
            {
                PlayerSkills.ReinerRun(players[turn], matriz, distancia, predefinedEmptyCells);

                UpdateDisplay();

            }
            SetBattleZone();

        }
    }
    public void NextTurnBut()
    {
        if (NextEnable && HasMoved)
            NextTurn();
    }
    public int NextTurnIndex(int turn)
    {
        if (turn + 1 > players.Count - 1)
            return 0;
        else
            return turn + 1;

    }
    public void NextTurn()
    {
        HasMoved = false;
        if (round > 1 && !players[turn].titanForm)
        {
            players[turn].DownCooldown();
        }
        if (skillWasActivatedThisTurn)
        {
            players[turn].ResetCooldown();
            skillWasActivatedThisTurn = false;
        }
        turn = NextTurnIndex(turn);

        if (round > 1)
        {
            if (players[turn].titanForm)
            {
                players[turn].CtransformTime++;
                if (players[turn].CtransformTime > players[turn].TransformTime)
                {
                    players[turn].CtransformTime = 0;
                    players[turn].ResetCooldown();

                    switch (players[turn].play.name)
                    {
                        case "Reiner":
                            ReinerSkill = false;
                            PlayerSkills.ReinerSkillOff(players[turn]); break;
                        case "Eren":
                            ErenSkill = false;
                            PlayerSkills.ErenSkillOff(players[turn]); break;
                        case "Armin":
                            ArminSkill = false;
                            PlayerSkills.ArminSkillOff(players[turn]); break;
                        case "Zeke":
                            ZekeSkill = false;
                            PlayerSkills.ZekeSkillOff(players[turn]); break;

                    }
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
    public void Skill()
    {
        if (SkillEnable)
        {
            skillWasActivatedThisTurn = true;
            SkillEnable = false;
            player sCplayer = players[turn].play;

            switch (sCplayer.Name)
            {
                case "Eren":
                    HandleErenSkill(); break;
                case "Reiner":
                    HandleReinerSkill(); break;
                case "Zeke":
                    HandleZekeSkill(); break;
                case "Armin":
                    HandleArminSkill(); break;
                case "Levi":
                    HandleLeviSkill(); break;
            }
        }
    }
    void HandleErenSkill()
    {
        ErenSkill = true;
        PlayerSkills.ErenSkillEf(players[turn]);
        ReachPointInMatriz();
        UpdateStats(turn);
        UpdateDisplay();
    }
    void HandleReinerSkill()
    {
        ReinerSkill = true;
        PlayerSkills.ReinerSkillEf(players[turn], matriz, distancia, predefinedEmptyCells);
        ReachPointInMatriz();
        UpdateStats(turn);
        UpdateDisplay();
    }
    void HandleArminSkill()
    {
        ArminSkill = true;
        PlayerSkills.ArminSkillEf(players[turn], matriz, predefinedObstacleCells);
        ReachPointInMatriz();
        UpdateStats(turn);
        UpdateDisplay();
    }
    void HandleZekeSkill()
    {
        ZekeSkill = true;
        PlayerSkills.ZekeSkillEf(players[turn], matriz, distancia);
    }
    void HandleLeviSkill()
    {
        LeviSkill = true;
        PlayerSkills.LeviSkillEf(players[turn], matriz, distancia);
        UpdateDisplay();
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
        TitanAI titanAI = new TitanAI(matriz, distancia, distanciaToCenter);

        int destination = titanAI.GetMove(players[turn], targets);
        MoveplayerTo(new Vector2(targets[destination].x, targets[destination].y), turn);


    }
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
    public void MoveplayerTo(Vector2 target, int index)
    {
        if (index == turn && !isInCombat && ReachPoint(target) && !playingCorrutine)
        {
            bool aux = LeviSkill;
            if (LeviSkill)
            {
                ApplyDamageOnLeviMove(target);
                LeviSkill = false;
            }

            players[turn].lastMove.Add(((int)players[turn].Pos.x, (int)players[turn].Pos.y));
            UnPlacePlayer();
            StartCoroutine(MovePlayerCorutine(target, index));
            StartCoroutine(ActivateInteraction(target, index, aux));
            //
            UpdateStats(turn);
            if (!players[turn].play.isTitan)
            {
                ReachPointInMatriz();
            }
            HasMoved = true;
        }
        else if (isInCombat && !playingCorrutine && ReachPoint(target))
        {

            players[index].Pos = target;
            PlacePlayerIn((int)target.x, (int)target.y, index);
            UnPlacePlayer();
            ClearMatrizNearPlayers();
            isInCombat = false;
            PlacePlayerIn((int)target.x, (int)target.y, index);

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

            UpdateStats(turn);
        }

    }
    void ApplyDamageOnLeviMove(Vector2 target)
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

    }
    bool isTrap(Vector2 target)
    {
        return matriz[(int)target.x][(int)target.y].trap && matriz[(int)target.x][(int)target.y].enableTrap;
    }
    bool isPowerUp(Vector2 target)
    {
        return matriz[(int)target.x][(int)target.y].powerUp;
    }
    public void ActivateTrap(trap trap, Vector2 target, PlayerManager player)
    {
        matriz[(int)target.x][(int)target.y].trapActivated = true;
        TrapTriggerEffect trapManeger = new TrapTriggerEffect(trap, player);
        openPanelTrap(trap);
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
    public void ActivatePower(PowerUp powerUp, Vector2 target, PlayerManager player)
    {
        matriz[(int)target.x][(int)target.y].powerUp = false;
        PowerTrigger PowerManeger = new PowerTrigger(powerUp, player);
        openPanelPowerUp(powerUp);

        panelPowerUp.powerUp = powerUp;
        panelPowerUp.OpenPanel();
        ReachPointInMatriz();


    }
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
                            UnPlacePlayer();
                            players[index ?? turn].Pos = new Vector2(wayToPoint[w].x, wayToPoint[w].y);
                            PlacePlayerIn((int)wayToPoint[w].x, (int)wayToPoint[w].y, index ?? turn);


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
            wayToPoint.Clear();
            playingCorrutine = false;
            InitReachCell();
            SetDist();
            ReachPointInMatriz();

        }
        playingCorrutine = false;
        if (players[turn].play.isTitan && !matriz[(int)target.x][(int)target.y].nearPlayer)
        {

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
    public void Combat(Vector2 target, int indexNearPlayer)
    {
        PlayerManager player1 = players[turn];
        PlayerManager player2 = matriz[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer];

        combatScene = new Combat(player1, player2);
        StartCombate();


        //combat maneger
        lastWinner1 = combatScene.Player1IsWinner();

        player1.PowerUp(combatScene.Reward(lastWinner1, 1));
        player2.PowerUp(combatScene.Reward(lastWinner1, 2));

        this.player2 = player2;
        if (lastWinner1)//si gana el jugador poseedor del turno (player1)
        {

            distancia = Board.ReachPointInSubMatriz(matriz, predefinedCenterCells, (int)player2.Pos.x, (int)player2.Pos.y);
            int at = players.IndexOf(player1);
            int de = players.IndexOf(player2);

            if (!player1.play.isTitan)
            {
                //MoveElementInList();
                players.RemoveAt(de);
                int newDe = at < de ? at + 1 : at;
                players.Insert(newDe, player2);
                turn = players.IndexOf(player1);

                ChangeTurnOrd();

            }
            Board.ColorReachCell(matriz, distancia);
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

                players.RemoveAt(de);
                int newDe = at < de ? at + 1 : at;
                players.Insert(newDe, player2);
                turn = players.IndexOf(player1);
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

            distancia = Board.ReachPointInSubMatriz(matriz, predefinedCenterCells, (int)player1.Pos.x, (int)player1.Pos.y);
            Board.ColorReachCell(matriz, distancia);
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
    bool ReachPoint(Vector2 target)
    {
        return matriz[(int)target.x][(int)target.y].reach;

    }
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
