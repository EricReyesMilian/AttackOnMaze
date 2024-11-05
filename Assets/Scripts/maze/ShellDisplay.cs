using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShellDisplay : MonoBehaviour
{
    Image img_shell;

    private Shell shell;
    public Sprite default_sprite;

    public Color obstacule_color = Color.gray;
    public Color default_color = Color.white;
    public Color hover_color = Color.white;

    public Color reach_color;
    public Color reachHover_color;
    public Color combat_color;

    public int distToShell = -5;
    public TextMeshProUGUI num;
    public TextMeshProUGUI cordText;
    public bool hover;
    public Color player_color = Color.yellow;
    Color visited_color;
    public Vector2 coord;
    public bool obstacule;
    void Start()
    {

        img_shell = GetComponent<Image>();
        GameManeger.gameManeger.UpdateDisplay += AsignarCasillaCorrespondiente;

    }
    void Update()
    {
        if (shell != null)
        {
            obstacule = shell.obstacle;
            distToShell = GameManeger.gameManeger.distancia[(int)coord.x][(int)coord.y];
            num.text = distToShell + "";
            cordText.text = shell.coord + "";
            visited_color = GameManeger.gameManeger.players[GameManeger.gameManeger.turn].color;
            if (shell.reach && !shell.visitedOnMove && !hover && !shell.nearPlayer)
            {
                if (!GameManeger.gameManeger.runningBackwards)
                    img_shell.color = reach_color;
                else
                    img_shell.color = default_color;


            }
            else if (shell.reach && !shell.visitedOnMove && !hover && shell.nearPlayer)
            {
                if (!GameManeger.gameManeger.runningBackwards)
                    img_shell.color = combat_color;
                else
                    img_shell.color = default_color;



            }
            if (shell.obstacle)
            {
                img_shell.color = obstacule_color;
            }
            if (shell.hasAplayer)
            {
                img_shell.sprite = shell.player.sprite;
                img_shell.color = Color.white;

            }
            else
            {
                img_shell.sprite = default_sprite;
            }
            if (!(shell.hasAplayer || shell.obstacle || shell.reach || hover))
            {
                img_shell.color = default_color;
            }
            DrawWay();
        }
    }
    public void DrawWay()
    {
        if (shell.visitedOnMove)
        {
            if (!shell.hasAplayer)
            {
                img_shell.sprite = default_sprite;

                img_shell.color = visited_color;
            }
            else
            {
                img_shell.sprite = shell.player.sprite;
                img_shell.color = Color.white;

            }
        }
    }
    public void Click()
    {
        GameManeger.gameManeger.MoveplayerTo(shell.coord);
    }
    public void Hover()
    {
        //print("hover");
        if (!shell.reach)
        {
            img_shell.color = hover_color;

        }
        else
        {
            img_shell.color = reachHover_color;
        }
        hover = true;
        if (shell.hasAplayer)
        {
            for (int p = 0; p < GameManeger.gameManeger.players.Count; p++)
            {
                if (shell.player == GameManeger.gameManeger.players[p].play)
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
                    shell = GameManeger.gameManeger.matriz[i][j];
                    break;
                }
            }
        }

    }
}