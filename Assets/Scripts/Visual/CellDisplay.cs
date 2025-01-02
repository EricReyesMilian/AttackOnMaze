using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CellDisplay : MonoBehaviour
{
    GameManager gm;
    Image img_cell;

    public Cell cell;
    public Sprite default_sprite;

    public Color obstacule_color = Color.gray;
    public Color centerCell_color;

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
    public List<PlayerManager> NearPlayers = new List<PlayerManager>();
    public TextMeshProUGUI endurence;

    void Awake()
    {
        gm = GameManager.gameManeger;
        gm.UpdateDisplay += AsignarCasillaCorrespondiente;
        gm.DrawWay += DrawWay;

    }
    void Start()
    {

        img_cell = GetComponent<Image>();

    }
    void Update()
    {
        if (cell != null)
        {
            if (Board.predefinedCenterCells.Contains(((int)cell.coord.x, (int)cell.coord.y)))
            {
                default_color = centerCell_color;
            }
            hasAplayer = cell.hasAplayer;
            reach = cell.reach;
            obstacule = cell.obstacle;
            NearPlayers = cell.NearPlayers;
            distToCell = Board.distancia[(int)coord.x][(int)coord.y]; //+ gm.distanciaToCenter[(int)coord.x][(int)coord.y];
            num.text = distToCell + "";
            cordText.text = cell.coord + "";
            visited_color = gm.players[gm.turn].color;
            if (cell.destroyableObs)
            {
                endurence.text = "" + cell.endurence;
            }
            else
            {
                endurence.text = "";
            }

            if (cell.special)
            {
                img_cell.color = gm.players[gm.turn].play.color;

            }
            else
            if (gm.isInCombat && !cell.hasAplayer && !cell.obstacle && !cell.powerUp)
            {
                img_cell.color = reach_color;
            }
            else
            if (cell.reach && !cell.visitedOnMove && !hover && !cell.nearPlayer)
            {
                if (!gm.runningBackwards && !gm.playingCorrutine)
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
                if (cell.player.titanForm)
                {
                    img_cell.sprite = cell.player.play.sprite2;

                }
                else
                {
                    img_cell.sprite = cell.player.play.sprite;

                }
                img_cell.color = Color.white;

            }
            else if (cell.powerUp)
            {
                img_cell.sprite = cell.powerUpType.sprite;
                if (!hover)
                {
                    img_cell.color = Color.white;
                    if (cell.reach && !gm.playingCorrutine)
                    {
                        img_cell.color = new Color(reach_color.r, reach_color.g, reach_color.b, reach_color.a * 1.8f);


                    }

                }

            }
            else
            {
                img_cell.sprite = default_sprite;
            }
            if (!(cell.hasAplayer || cell.obstacle || cell.reach || hover || cell.powerUp))
            {
                img_cell.color = default_color;
            }
            DrawWay();
            if (cell.player != null)
            {
                if (cell.player.sick)
                {
                    img_cell.sprite = cell.player.spriteTitan;

                }

            }
            if (cell.trap && cell.enableTrap && Input.GetKey(KeyCode.LeftShift))
            {
                //uncomment to debbug traps
                img_cell.color = Color.red;

            }
        }
    }
    public void DrawWay()
    {
        if (img_cell)
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
                    if (cell.player.titanForm)
                    {
                        img_cell.sprite = cell.player.play.sprite2;

                    }
                    else
                    {
                        img_cell.sprite = cell.player.play.sprite;

                    }
                    img_cell.color = Color.white;

                }
            }

        }
    }
    public void Click()
    {
        if (gm.ZekeSkill)
        {
            if (cell.player != null)
            {
                if (cell.player.isTitan)
                {

                    int at = 0;
                    int de = gm.players.IndexOf(cell.player);
                    for (int i = 0; i < gm.players.Count; i++)
                    {
                        if (gm.players[i].nameC == "Zeke")
                        {
                            at = i;
                            break;
                        }
                    }

                    //gm.turn = gm.players.IndexOf(de);
                    gm.players.RemoveAt(de);
                    int newDe = at < de ? at + 1 : at;
                    gm.players.Insert(newDe, cell.player);
                    cell.player.isTitan = false;
                    gm.turn = gm.players.IndexOf(gm.players[at]);
                    gm.turn--;
                    if (gm.turn < 0)
                    {
                        gm.turn = gm.players.Count;
                    }
                    for (int i = 0; i < gm.players.Count; i++)
                    {
                        if (gm.players[i].nameC == "Zeke")
                        {
                            gm.players[i].CtransformTime = gm.players[i].TransformTime;

                        }
                    }
                    gm.NextTurn();



                }

            }
        }
        if (!gm.isInCombat)
        {
            if (gm.ReinerSkill)
            {
                if (cell.special)
                {
                    gm.players[gm.turn].DownCurrentSpeed(1);
                }
            }

            gm.MoveplayerTo(cell.coord, gm.turn);

        }
        else
        {
            if (gm.lastWinner1)
            {
                AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

                gm.MoveplayerTo(cell.coord, gm.players.IndexOf(gm.player2));

            }
            else
            {
                AudioManager.speaker.Play(Resources.Load<AudioClip>("click"));

                gm.MoveplayerTo(cell.coord, gm.turn);

            }

        }

    }
    public void Hover()
    {
        if (cell.reach)
        {
            AudioManager.speaker.Play(Resources.Load<AudioClip>("tick"));

        }
        if (!cell.reach && !cell.powerUp)
        {
            img_cell.color = hover_color;

        }
        else
        {
            if (cell.reach)
            {
                img_cell.color = reachHover_color;

            }
        }
        hover = true;
        if (cell.hasAplayer)
        {

            if (cell.player.play && cell.player.isTitan && gm.ZekeSkill)
            {
                img_cell.color = hover_color;

            }

            for (int p = 0; p < gm.players.Count; p++)
            {
                if (cell.player == gm.players[p])
                {
                    displayPlayerTurnInfo.statdisplay.UpdateStats(p);

                }
            }

        }
        else
        {
            displayPlayerTurnInfo.statdisplay.UpdateStats(gm.turn);

        }


    }
    public void No_Hover()
    {
        hover = false;

    }
    void AsignarCasillaCorrespondiente()
    {

        cell = Board.grid[(int)coord.x][(int)coord.y];



    }
}