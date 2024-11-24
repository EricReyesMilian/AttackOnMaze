using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CellDisplay : MonoBehaviour
{
    GameManeger gm;
    Image img_cell;

    public Cell cell;
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
        gm = GameManeger.gameManeger;

        img_cell = GetComponent<Image>();
        gm.UpdateDisplay += AsignarCasillaCorrespondiente;
        gm.DrawWay += DrawWay;

    }
    void Update()
    {
        if (cell != null)
        {
            hasAplayer = cell.hasAplayer;
            reach = cell.reach;
            obstacule = cell.obstacle;
            NearPlayers = cell.NearPlayers;
            distToCell = gm.distancia[(int)coord.x][(int)coord.y];
            num.text = distToCell + "";
            cordText.text = cell.coord + "";
            visited_color = gm.players[gm.turn].color;
            if (cell.powerUp)
            {
                img_cell.color = Color.blue;

            }
            // else
            // if (cell.trap && cell.enableTrap)
            // {
            //     //uncomment to debbug traps
            //     // img_cell.color = Color.red;

            // }
            else
            if (gm.isInCombat && !cell.hasAplayer && !cell.obstacle)
            {
                img_cell.color = reach_color;
            }
            else
            if (cell.reach && !cell.visitedOnMove && !hover && !cell.nearPlayer)
            {
                if (!gm.runningBackwards)
                    img_cell.color = reach_color;
                else
                    img_cell.color = default_color;


            }
            else if (cell.reach && !cell.visitedOnMove && !hover && cell.nearPlayer)
            {
                if (!gm.runningBackwards)
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
            if (!(cell.hasAplayer || cell.obstacle || cell.reach || hover /*|| cell.trap*/ || cell.powerUp))
            {
                img_cell.color = default_color;
            }
            DrawWay();
            if (cell.trapActivated)
            {
                //  print("activated" + cell.trapType);
            }
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
        // BoardManeger.AddTrapOn(gm.trapList[1],(int)coord.x,(int)coord.y,gm.matriz);
        if (!gm.isInCombat)
        {
            gm.MoveplayerTo(cell.coord, gm.turn);

        }
        else
        {
            if (gm.lastWinner1)
            {
                gm.MoveplayerTo(cell.coord, gm.NextTurnIndex(gm.turn));

            }
            else
            {
                gm.MoveplayerTo(cell.coord, gm.turn);

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
            for (int p = 0; p < gm.players.Count; p++)
            {
                if (cell.player == gm.players[p].play)
                {
                    displayPlayerTurnInfo.statdisplay.UpdateStats(p);

                }
            }

        }
        else
        {
            displayPlayerTurnInfo.statdisplay.UpdateStats(gm.turn);

        }
        if (cell.trap)
        {
            print(cell.trapType);
        }

    }
    public void No_Hover()
    {
        hover = false;

    }
    void AsignarCasillaCorrespondiente()
    {

        cell = gm.matriz[(int)coord.x][(int)coord.y];


    }
}