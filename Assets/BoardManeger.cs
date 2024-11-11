using System.Collections.Generic;
public class BoardManeger
{
    public static List<List<int>> Lee(List<List<Cell>> tablero, int filaInical, int columnaInicial, int speed)
    {
        int size = tablero[0].Count;
        List<List<int>> distancias = new List<List<int>>();
        IniciarDistancias(ref distancias, tablero[0].Count);
        //Marcar casilla inicial
        distancias[filaInical][columnaInicial] = 0;
        //           N   S  E  W
        int[] df = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, 1, -1 };

        bool huboCambio;

        do
        {
            huboCambio = false;
            //Para cada posible celda del tablero
            for (int f = 0; f < size; f++)
            {
                for (int c = 0; c < size; c++)
                {
                    //saltarse las celdas no marcadas
                    if (distancias[f][c] == -1 || distancias[f][c] == speed) continue;
                    //saltarse las invalidas
                    if (tablero[f][c].obstacle) continue;
                    //inspeccionar celdas vecinas
                    for (int d = 0; d < df.Length; d++)
                    {
                        int vf = f + df[d];
                        int vc = c + dc[d];
                        //determinar si es un vecino valido
                        if (PosicionValida(size, vf, vc) && !tablero[vf][vc].hasAplayer
                        && !tablero[vf][vc].obstacle && distancias[vf][vc] == -1)
                        {
                            distancias[vf][vc] = distancias[f][c] + 1;
                            huboCambio = true;
                        }
                    }
                }
            }
        } while (huboCambio);
        return distancias;
    }

    public static List<List<int>> ReachPointInSubMatriz(List<List<Cell>> tablero, int playerPosF, int playerPosC)
    {
        List<List<int>> distancias = new List<List<int>>();
        int n = tablero[0].Count;
        IniciarDistancias(ref distancias, tablero[0].Count);
        if (playerPosC < (n) / 2 && playerPosF <= (n) / 2)
        {

            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle)
                        distancias[i][j] = 0;

                }
            }

        }
        else if (playerPosC >= (n) / 2 && playerPosF > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle)
                        distancias[i][j] = 0;


                }
            }

        }
        else if (playerPosC < (n) / 2 && playerPosF > (n) / 2)
        {
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = (n) / 2; j < n; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle)
                        distancias[i][j] = 0;


                }
            }

        }
        else
        {
            for (int i = (n) / 2; i < n; i++)
            {
                for (int j = 0; j < n / 2; j++)
                {
                    if (!tablero[i][j].hasAplayer && !tablero[i][j].obstacle)
                        distancias[i][j] = 0;


                }
            }

        }
        return distancias;

    }

    public static void IniciarDistancias(ref List<List<int>> distancias, int n)
    {
        for (int f = 0; f < n; f++)
        {
            distancias.Add(new List<int>());
            for (int c = 0; c < n; c++)
            {
                distancias[f].Add(-1);
            }
        }
    }
    private static bool PosicionValida(int n, int f, int c)
    {
        return f >= 0 && f < n && c >= 0 && c < n;
    }
    public static void ColorReachCell(List<List<Cell>> matriz, List<List<int>> distancias)
    {
        for (int i = 0; i < distancias[0].Count; i++)
            for (int j = 0; j < distancias[0].Count; j++)
            {
                if (distancias[i][j] != -1)
                {
                    matriz[i][j].reach = true;

                }
            }


    }

}