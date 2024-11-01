using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShellDisplay : MonoBehaviour
{
    Image img_shell;

    public Shell shell;
    public Sprite default_sprite;

    public Color obstacule_color = Color.gray;
    public Color default_color = Color.white;
    public Color hover_color = Color.white;

    public Color reach_color;
    public Color reachHover_color;

    public int distToShell = -5;
    public TextMeshProUGUI num;
    public TextMeshProUGUI cordText;
    public bool hover;
    public Color player_color = Color.yellow;
    Color visited_color;

    void Start()
    {
        img_shell = GetComponent<Image>();
        shell.obstacule = true;

    }
    void Update()
    {

        num.text = distToShell + "";
        cordText.text = shell.coord + "";
        visited_color = GameManeger.gameManeger.players[GameManeger.gameManeger.turn].color;
        if (shell.reach && !shell.visitedOnMove && !hover)
        {
            if (!GameManeger.gameManeger.runningBackwards)
                img_shell.color = reach_color;
            else
                img_shell.color = default_color;


        }
        if (shell.obstacule)
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
        if (!(shell.hasAplayer || shell.obstacule || shell.reach || hover))
        {
            img_shell.color = default_color;
        }
    }
    public void DrawWay()
    {
        if (shell.visitedOnMove)
        {
            img_shell.color = visited_color;
        }
    }
    public void Click()
    {
        print("click!");
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



    }
    public void No_Hover()
    {
        hover = false;

    }
}