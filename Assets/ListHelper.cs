using System;
using System.Collections.Generic;

public class ListHelper
{
    public static void MoveElement<T>(List<T> list, T elementToMove, int targetIndex)
    {
        if (!list.Contains(elementToMove))
        {
            throw new ArgumentException("El elemento debe estar presente en la lista.");
        }

        // Elimina el elemento que deseas mover
        list.Remove(elementToMove);

        // Inserta el elemento que deseas mover inmediatamente después del elemento de destino
        list.Insert(targetIndex, elementToMove);
    }
}
