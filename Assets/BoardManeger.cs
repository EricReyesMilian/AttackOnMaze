using System.Collections.Generic;
public class BoardManeger
{
    public static List<List<int>> Lee(List<List<Shell>> tablero, int filaInical, int columnaInicial, int speed)
    {
        int size = tablero[0].Count;
        List<List<int>> distancias = new List<List<int>>();
        IniciarDistancias(distancias, size);
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
    private static void IniciarDistancias(List<List<int>> distancias, int size)
    {
        for (int f = 0; f < size; f++)
        {
            distancias.Add(new List<int>());
            for (int c = 0; c < size; c++)
            {
                distancias[f].Add(-1);
            }
        }
    }
    private static bool PosicionValida(int n, int f, int c)
    {
        return f >= 0 && f < n && c >= 0 && c < n;
    }

}