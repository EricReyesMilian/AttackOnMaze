using System;
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
    public int round = 0;
    public int WinPower = 30;
    public GameObject cellsContainer;
    public GameObject playerContainer;
    public PlayerManager winner;
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
    public bool HasMoved = false;
    bool skillWasActivatedThisTurn = false;
    public Vector2 combatZoneCoord;
    // Listas
    public List<trap> trapList;
    public List<PowerUp> powerList;
    public PowerUp TheKey;
    [HideInInspector]
    public List<PlayerManager> players = new List<PlayerManager>();
    public List<player> player_Scriptable = new List<player>();
    public List<Vector2> wayToPoint;
    public List<player> nearPlayers;

    // Delegados y eventos
    public Action UpdateDisplay;
    public Action DrawWay;
    public Action StartCombate;
    public Action<int> StartDisplay;
    public Action<trap> openPanelTrap;
    public Action<PowerUp> openPanelPowerUp;
    public Action SelectplayerCombat;
    public Action createPlayerBoxInstances;
    public Action ChangeTurnOrd;
    public Action displayGrid;
    public Action<int> UpdateStats;

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
        //  Board.grid.Clear();
        Board.AddCellsToMatriz(n);

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
        if (Input.GetKeyDown(KeyCode.R))//remover cooldown
        {
            for (int i = 0; i < player_Scriptable.Count; i++)
            {
                players[i].RemoveCooldown();
            }

        }
        if (Input.GetKeyDown(KeyCode.D))//romper pared
        {
            Board.grid[6][8].TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoMenu();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            print(players[turn].TitanIQ);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            players[turn].IntUp(1);
        }
        if (Input.GetKeyDown(KeyCode.T))//fuerza el avance de un turno
        {
            NextTurn();
        }
        if (Input.GetKeyDown(KeyCode.Space))//regenera el laberinto
        {
            SceneManager.LoadScene(2);

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            players[turn].PowerUp(WinPower);
        }
        #endregion
        NextEnable = !players[turn].isTitan && !playingCorrutine && !isInCombat && HasMoved;
        if (players[turn].currentCooldown == 0 && !players[turn].isTitan)
        {
            SkillEnable = true;
        }
        else
        {
            SkillEnable = false;
        }
        if (turn >= 0)
            players[turn].UpdateDebuff();

    }
    void LoadPlayers()
    {
        for (int i = 0; i < MainMenuManeger.mm.playersList.Count; i++)
        {
            for (int j = 0; j < MainMenuManeger.mm.playersList[i].Count; j++)
            {
                MainMenuManeger.mm.playersList[i][j].team = i;
                player_Scriptable.Add(MainMenuManeger.mm.playersList[i][j]);

            }

        }
    }
    void InitializePlayers()
    {
        for (int i = 0; i < player_Scriptable.Count; i++)
        {
            players.Add(new PlayerManager(player_Scriptable[i]));
            players[i].index = i;
            players[i].team = player_Scriptable[i].team;
            players[i].InitStats();
            players[i].Pos = new Vector2(Board.startCells[i].x, Board.startCells[i].y);
            Board.PlacePlayerIn(Board.startCells[i].x, Board.startCells[i].y, i);
            print(players[i].nameC);

        }

    }

    void InitializeBoard()
    {
        Board.IniciarDistancias(ref Board.distancia, n);
        Board.IniciarDistancias(ref Board.distanciaToCenter, n);
        MazeGenerator maze = new MazeGenerator(n, 7, 8, Board.grid, Algorithm.Prim);
        Board.predefinedEmptyCells.Add((7, 8));
        Board.AssignDestroyableWalls();
        Board.AddTraps();
        Board.AddPowerUps();
        Board.AddKey();
        Board.distanciaToCenter = Board.Lee(n / 2, n / 2, n * n);

    }
    void InitializeGame()
    {
        ReachPointInMatriz();
        UpdateStats(turn);
        turn = -1;
        createPlayerBoxInstances();
        NextTurn();
    }
    public void ReachPointInMatriz()
    {
        if (!isInCombat)
        {
            int speed = players[turn].currentSpeed;
            Board.InitReachCell();

            Board.distancia = Board.Lee((int)players[turn].Pos.x, (int)players[turn].Pos.y, speed, false);


            Board.ColorReachCell();
            if (players[turn].titanForm && players[turn].currentSpeed > 0)
                players[turn].play.skill.PasiveOnWalk(players[turn]);
            Board.SetBattleZone(players);
            UpdateDisplay();


        }
    }
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void NextTurnBut()
    {
        if (NextEnable && HasMoved)
        {
            AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));
            NextTurn();
        }

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
        print("NextTurn");
        if (round > 1 && !players[turn].titanForm)
        {
            players[turn].DownCooldown();
        }
        if (skillWasActivatedThisTurn)
        {
            players[turn].ResetCooldown();
            skillWasActivatedThisTurn = false;
        }

        if (turn >= 0)
        {
            if (players[turn].play.isTitan && !players[turn].isTitan)
            {
                players[turn].isTitan = true;


            }
            if (players[turn].sick)
            {
                players[turn].sickTime -= 1;
                if (players[turn].sickTime < 0)
                {
                    players[turn].sickTime = 2;
                    players[turn].sick = false;
                    players[turn].isTitan = false;
                }
            }

        }
        turn = NextTurnIndex(turn);
        if (players[turn].haveKey)
        {
            players[turn].PowerUp(5);


        }
        CheckSavior();

        if (round > 1)
        {
            if (players[turn].titanForm)
            {
                players[turn].CtransformTime++;
                if (players[turn].CtransformTime > players[turn].TransformTime)
                {
                    players[turn].CtransformTime = 0;
                    players[turn].ResetCooldown();
                    players[turn].play.skill.Desactive(players[turn]);
                    if (players[turn].titanForm)
                        players[turn].play.skill.Pasive(players[turn]);

                }

            }
        }

        ChangeTurnOrd();
        Board.SetDist();
        Board.InitReachCell();
        players[turn].ResetSpeed();
        ReachPointInMatriz();

        if (turn == 0)
        {
            round++;
            if (round % players.Count == 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].IntUp(1);

                }

                Board.AddPowerUps();

            }

        }
        if (players[turn].currentCooldown == 0 && !players[turn].isTitan)
        {
            SkillEnable = true;
        }
        else
        {
            SkillEnable = false;
        }
        //move titan
        if (players[turn].isTitan)
        {
            MoveTitan();
        }
        if (PlayerCanMove())
            HasMoved = false;
        UpdateStats(turn);
    }
    public void CheckSavior()
    {
        if (Savior())
        {
            Board.OpenDoorsToSavior();
        }
        else
        {
            Board.CloseDoors();

        }
    }
    public bool Savior()
    {
        return players[turn].power >= WinPower && players[turn].haveKey;
    }
    public void Skill()
    {
        if (SkillEnable && !players[turn].titanForm && !isInCombat)
        {
            AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

            skillWasActivatedThisTurn = true;
            SkillEnable = false;

            players[turn].ResetCooldown();
            if (players[turn].play.Name != "Levi")
                ReachPointInMatriz();
            players[turn].play.skill.Active(players[turn]);
            if (players[turn].play.Name != "Levi" && players[turn].play.Name != "Falco")
                ReachPointInMatriz();
            UpdateStats(turn);
            UpdateDisplay();
            UpdateStats(turn);

        }
    }
    bool PlayerCanMove()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (Board.grid[i][j].reach)
                    return true;

            }
        }
        return false;
    }
    private void MoveTitan()
    {
        List<(int x, int y)> targets = new List<(int x, int y)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (Board.grid[i][j].reach && !targets.Contains(((int)Board.grid[i][j].coord.x, (int)Board.grid[i][j].coord.y)))
                {
                    targets.Add(((int)Board.grid[i][j].coord.x, (int)Board.grid[i][j].coord.y));
                }
            }
        }
        print(players[turn].nameC);

        TitanAI titanAI = new TitanAI();
        int destination = titanAI.GetMove(players[turn], targets);
        if (destination != -1 && destination < targets.Count)
        {
            MoveplayerTo(new Vector2(targets[destination].x, targets[destination].y), turn);

        }
        else if (destination == -1)
            Invoke("NextTurn", 0.5f);

    }
    bool Win(PlayerManager player)
    {
        (int x, int y) a = ((int)player.Pos.x, (int)player.Pos.y);
        return Board.predefinedCenterCells.Contains(a);
    }

    public void MoveplayerTo(Vector2 target, int index)
    {
        if (index == turn && !isInCombat && Board.ReachPoint(target) && !playingCorrutine)
        {
            AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

            players[turn].lastMove.Add(((int)players[turn].Pos.x, (int)players[turn].Pos.y));
            Board.UnPlacePlayer((int)players[turn].Pos.x, (int)players[turn].Pos.y);
            StartCoroutine(MovePlayerCorutine(target, index));
            //
            UpdateStats(turn);
            if (!players[turn].isTitan)
            {
                ReachPointInMatriz();
            }
            HasMoved = true;
        }
        else if (isInCombat && !playingCorrutine && Board.ReachPoint(target))
        {

            Board.UnPlacePlayer((int)players[index].Pos.x, (int)players[index].Pos.y);
            players[index].Pos = target;
            Board.PlacePlayerIn((int)target.x, (int)target.y, index);
            Board.ClearMatrizNearPlayers();
            isInCombat = false;

            if (players[turn].isTitan)
            {
                Board.InitReachCell();
                Board.SetDist();
                ReachPointInMatriz();
                NextTurn();
            }
            else
            {
                Board.InitReachCell();
                Board.SetDist();
                ReachPointInMatriz();
            }

            UpdateStats(turn);
        }
    }
    public void ActivateTrap(trap trap, Vector2 target, PlayerManager player)
    {
        Board.grid[(int)target.x][(int)target.y].trapActivated = true;
        TrapTriggerEffect trapManeger = new TrapTriggerEffect(trap, player);
        openPanelTrap(trap);
        if (trap.activationTrap)
        {
            foreach (var trapChild in Board.grid[(int)target.x][(int)target.y].childsTrap)
            {
                print("pop");
                Board.grid[trapChild.x][trapChild.y].enableTrap = false;
            }
        }
        ReachPointInMatriz();

    }
    public void ActivatePower(PowerUp powerUp, PlayerManager player)
    {
        PowerTrigger PowerManeger = new PowerTrigger(powerUp, player);
        openPanelPowerUp(powerUp);

        panelPowerUp.powerUp = powerUp;
        panelPowerUp.OpenPanel();
        ReachPointInMatriz();


    }
    IEnumerator MovePlayerCorutine(Vector2 target, int? index)
    {
        playingCorrutine = true;

        Board.grid[(int)players[index ?? turn].Pos.x][(int)players[index ?? turn].Pos.y].hasAplayer = false;

        if (index == turn && !isInCombat)
        {
            players[index ?? turn].DownCurrentSpeed(Board.distancia[(int)target.x][(int)target.y]);
            wayToPoint.Clear();
            Board.SavePath(target, players[turn].Pos);
            wayToPoint.Reverse();
            for (int w = 0; w < wayToPoint.Count; w++)//avanza el player hasta la casilla
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == wayToPoint[w].x && j == wayToPoint[w].y)
                        {
                            Board.grid[i][j].visitedOnMove = true;
                            Board.UnPlacePlayer((int)players[index ?? turn].Pos.x, (int)players[index ?? turn].Pos.y);
                            players[index ?? turn].Pos = new Vector2(wayToPoint[w].x, wayToPoint[w].y);
                            Board.PlacePlayerIn((int)wayToPoint[w].x, (int)wayToPoint[w].y, index ?? turn);


                            UpdateDisplay();
                            if (w > 0)
                                yield return new WaitForSeconds(0.25f); // Espera 0.5 segundos

                        }
                    }
                }
            }
            if (Win(players[index ?? turn]))//check winner
            {

                winner = players[index ?? turn];
                SceneManager.LoadScene(3);
            }
            runningBackwards = true;
            for (int w = 0; w < wayToPoint.Count; w++)//elimina el camino recorrido
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == wayToPoint[w].x && j == wayToPoint[w].y)
                        {
                            Board.grid[i][j].visitedOnMove = false;

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
                    Board.grid[i][j].visitedOnMove = false;
                    UpdateDisplay();
                }
            }
            wayToPoint.Clear();
            playingCorrutine = false;
            Board.InitReachCell();
            Board.SetDist();
            ReachPointInMatriz();
            ActivateInteraction(target, turn);


        }
        playingCorrutine = false;
        if (players[turn].isTitan && !isInCombat)
        {

            NextTurn();
        }
        StopCoroutine(MovePlayerCorutine(target, index));

    }
    void ActivateInteraction(Vector2 target, int index)
    {
        if (Board.isPowerUp(target) && !players[index].play.isTitan)
        {
            Board.grid[(int)target.x][(int)target.y].powerUp = false;
            ActivatePower(Board.grid[(int)target.x][(int)target.y].powerUpType, players[index]);
        }
        if (Board.isTrap(target) && !players[index].play.isTitan)
        {
            ActivateTrap(Board.grid[(int)target.x][(int)target.y].trapType, target, players[index]);

        }
        if (Board.grid[(int)target.x][(int)target.y].nearPlayer)
        {
            bool notTitan = false;
            if (players[turn].isTitan)
            {
                for (int i = 0; i < Board.grid[(int)target.x][(int)target.y].NearPlayers.Count; i++)
                {
                    if (!Board.grid[(int)target.x][(int)target.y].NearPlayers[i].isTitan)
                    {
                        notTitan = true;
                    }
                }
                if (notTitan)
                {
                    Board.InitReachCell();
                    SelectPlayer(target, players[index].isTitan);

                }
            }
            else
            {
                Board.InitReachCell();
                SelectPlayer(target, players[index].isTitan);

            }

        }
        else if (!players[index].isTitan)
        {
            Board.InitReachCell();
            Board.SetDist();

            ReachPointInMatriz();

        }

    }

    private void SelectPlayer(Vector2 target, bool titan)
    {
        nearPlayers.Clear();
        combatZoneCoord = target;
        for (int k = 0; k < Board.grid[(int)target.x][(int)target.y].NearPlayers.Count; k++)
        {
            if (titan)
            {
                if (!Board.grid[(int)target.x][(int)target.y].NearPlayers[k].isTitan)
                {
                    nearPlayers.Add(Board.grid[(int)target.x][(int)target.y].NearPlayers[k].play);

                }
            }
            else
            {
                nearPlayers.Add(Board.grid[(int)target.x][(int)target.y].NearPlayers[k].play);

            }
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
            if (nearPlayers.Count > 0)
                Combat(combatZoneCoord, indexFightForTitan);
            else
                NextTurn();

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
        PlayerManager player2 = Board.grid[(int)target.x][(int)target.y].NearPlayers[indexNearPlayer];

        combatScene = new Combat(player1, player2);
        StartCombate();
        AudioManager.speaker.Play(Resources.Load<AudioClip>("fight"));

        //combat maneger
        lastWinner1 = combatScene.Player1IsWinner();

        player1.PowerUp(combatScene.Reward(lastWinner1, 1));
        player2.PowerUp(combatScene.Reward(lastWinner1, 2));

        this.player2 = player2;
        if (lastWinner1)//si gana el jugador poseedor del turno (player1)
        {

            Board.distancia = Board.ReachPointInSubMatriz((int)player2.Pos.x, (int)player2.Pos.y);
            int at = players.IndexOf(player1);
            int de = players.IndexOf(player2);

            if (!player1.isTitan)
            {
                players.RemoveAt(de);
                int newDe = at < de ? at + 1 : at;
                players.Insert(newDe, player2);
                turn = players.IndexOf(player1);
                turn--;
                if (turn < 0)
                    turn = players.Count - 1;
                ChangeTurnOrd();

            }
            else
            {
                players.RemoveAt(de);
                int newDe = at < de ? at + 1 : at;
                players.Insert(newDe, player2);
                turn = players.IndexOf(player2);
                turn--;
                if (turn < 0)
                    turn = players.Count - 1;

                ChangeTurnOrd();
            }
            Board.ColorReachCell();
            UpdateDisplay();
            ChangeTurnOrd();

            looserTitan = true;

            if (player1.isTitan)//si gano un titan
            {
                looserTitan = false;
                List<Cell> posiblesMoves = new List<Cell>();
                Board.distancia = Board.ReachPointInSubMatriz((int)player2.Pos.x, (int)player2.Pos.y);
                Board.ColorReachCell();
                UpdateDisplay();

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Board.grid[i][j].reach && !Board.predefinedCenterCells.Contains((i, j)))
                        {
                            posiblesMoves.Add(Board.grid[i][j]);
                        }
                    }
                }
                isInCombat = true;
                if (player2.haveKey)
                {
                    player2.LoseKey();
                    Board.DropKeyIn((int)player2.Pos.x, (int)player2.Pos.y);
                }

                MoveplayerTo(posiblesMoves[UnityEngine.Random.Range(0, posiblesMoves.Count)].coord, players.IndexOf(player2));

            }
            else
            {
                if (player2.haveKey)
                {
                    player2.LoseKey();
                    player1.TakeKey();
                }
                isInCombat = true;

            }
        }
        else//si pierde el jugador poseedor del turno (player1)
        {
            Board.distancia = Board.ReachPointInSubMatriz((int)player2.Pos.x, (int)player2.Pos.y);
            Board.ColorReachCell();
            UpdateDisplay();
            ChangeTurnOrd();

            UpdateDisplay();

            looserTitan = false;

            if (player2.isTitan)//si gano un titan
            {
                looserTitan = false;
                List<Cell> posiblesMoves = new List<Cell>();
                Board.distancia = Board.ReachPointInSubMatriz((int)player2.Pos.x, (int)player2.Pos.y);
                Board.ColorReachCell();
                UpdateDisplay();

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (Board.grid[i][j].reach && !Board.predefinedCenterCells.Contains((i, j)))
                        {
                            posiblesMoves.Add(Board.grid[i][j]);
                        }
                    }
                }
                isInCombat = true;
                if (player1.haveKey)
                {
                    player1.LoseKey();
                    Board.DropKeyIn((int)player1.Pos.x, (int)player1.Pos.y);
                }

                MoveplayerTo(posiblesMoves[UnityEngine.Random.Range(0, posiblesMoves.Count)].coord, players.IndexOf(player1));

            }
            else
            {
                if (player2.play.isTitan)
                {
                    if (player1.haveKey)
                    {
                        player1.LoseKey();
                        Board.DropKeyIn((int)player1.Pos.x, (int)player1.Pos.y);
                    }
                }
                else
                if (player1.haveKey)
                {
                    player1.LoseKey();
                    player2.TakeKey();
                }
                isInCombat = true;

            }

            UpdateDisplay();

        }
    }
}
