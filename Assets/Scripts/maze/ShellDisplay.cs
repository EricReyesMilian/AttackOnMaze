using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CellDisplay : MonoBehaviour
{
    Image img_cell;

    private Cell cell;
    public Sprite default_sprite;

    public Color obstacule_color = Color.gray;
    public Color default_color = Color.white;
    public Color hover_color = Color.white;

    public Color reach_color;
    public Color reachHover_color;
    public Color combat_color;

    public int distToCell = -5;
    public TextMeshProUGUI num;
    public TextMeshProUGUI cordText;
    public bool hover;
    public Color player_color = Color.yellow;
    Color visited_color;
    public Vector2 coord;
    public bool hasAplayer;
    public bool obstacule;
    public bool reach;
    public List<PlayerManeger> NearPlayers = new List<PlayerManeger>();

    void Start()
    {

        img_cell = GetComponent<Image>();
        GameManeger.gameManeger.UpdateDisplay += AsignarCasillaCorrespondiente;
        GameManeger.gameManeger.DrawWay += DrawWay;

    }
    void Update()
    {
        if (cell != null)
        {
            hasAplayer = cell.hasAplayer;
            reach = cell.reach;
            obstacule = cell.obstacle;
            NearPlayers = cell.NearPlayers;
            distToCell = GameManeger.gameManeger.distancia[(int)coord.x][(int)coord.y];
            num.text = distToCell + "";
            cordText.text = cell.coord + "";
            visited_color = GameManeger.gameManeger.players[GameManeger.gameManeger.turn].color;
            if (GameManeger.gameManeger.isInCombat && !cell.hasAplayer && !cell.obstacle)
            {
                img_cell.color = reach_color;
            }
            else
           if (cell.reach && !cell.visitedOnMove && !hover && !cell.nearPlayer)
            {
                if (!GameManeger.gameManeger.runningBackwards)
                    img_cell.color = reach_color;
                else
                    img_cell.color = default_color;


            }
            else if (cell.reach && !cell.visitedOnMove && !hover && cell.nearPlayer)
            {
                if (!GameManeger.gameManeger.runningBackwards)
                    img_cell.color = combat_color;
                else
                    img_cell.color = default_color;



            }
            if (cell.obstacle)
            {
                img_cell.color = obstacule_color;
            }
            if (cell.hasAplayer)
            {
                img_cell.sprite = cell.player.sprite;
                img_cell.color = Color.white;

            }
            else
            {
                img_cell.sprite = default_sprite;
            }
            if (!(cell.hasAplayer || cell.obstacle || cell.reach || hover))
            {
                img_cell.color = default_color;
            }
            DrawWay();

        }
    }
    public void DrawWay()
    {
        if (cell.visitedOnMove)
        {
            if (!cell.hasAplayer)
            {
                img_cell.sprite = default_sprite;

                img_cell.color = visited_color;
            }
            else
            {
                img_cell.sprite = cell.player.sprite;
                img_cell.color = Color.white;

            }
        }
    }
    public void Click()
    {
        if (!GameManeger.gameManeger.isInCombat)
        {
            GameManeger.gameManeger.MoveplayerTo(cell.coord, GameManeger.gameManeger.turn);

        }
        else
        {
            if (GameManeger.gameManeger.lastWinner1)
            {
                GameManeger.gameManeger.MoveplayerTo(cell.coord, GameManeger.gameManeger.NextTurnIndex(GameManeger.gameManeger.turn));

            }
            else
            {
                GameManeger.gameManeger.MoveplayerTo(cell.coord, GameManeger.gameManeger.turn);

            }




        }
    }
    public void Hover()
    {
        //print("hover");
        if (!cell.reach)
        {
            img_cell.color = hover_color;

        }
        else
        {
            img_cell.color = reachHover_color;
        }
        hover = true;
        if (cell.hasAplayer)
        {
            for (int p = 0; p < GameManeger.gameManeger.players.Count; p++)
            {
                if (cell.player == GameManeger.gameManeger.players[p].play)
                {
                    displayPlayerTurnInfo.statdisplay.UpdateStats(p);

                }
            }

        }
        else
        {
            displayPlayerTurnInfo.statdisplay.UpdateStats(GameManeger.gameManeger.turn);

        }

    }
    public void No_Hover()
    {
        hover = false;

    }
    void AsignarCasillaCorrespondiente()
    {
        for (int i = 0; i < GameManeger.gameManeger.n; i++)
        {
            for (int j = 0; j < GameManeger.gameManeger.n; j++)
            {
                if (GameManeger.gameManeger.matriz[i][j].coord == coord)
                {
                    cell = GameManeger.gameManeger.matriz[i][j];
                    break;
                }
            }
        }

    }
}