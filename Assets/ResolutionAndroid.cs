using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    public float targetAspect = 16.0f / 9.0f;

    void Start()
    {
        AdjustAspectRatio();
    }

    void AdjustAspectRatio()
    {
        // Obtener la relación de aspecto actual de la pantalla
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        // Si la relación de aspecto actual es mayor que la deseada, agregar barras negras arriba y abajo
        if (scaleHeight < 1.0f)
        {
            Rect rect = Camera.main.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            Camera.main.rect = rect;
        }
        else // Si la relación de aspecto actual es menor que la deseada, agregar barras negras a los lados
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = Camera.main.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            Camera.main.rect = rect;
        }
    }
}
