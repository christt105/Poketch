using UnityEngine;

public class Signals
{
    public delegate void OnPaint(Color colorToPaint);
    public static event OnPaint onPaint;

    public delegate void OnResetTexture();
    public static event OnResetTexture onResetTexture;

    public delegate void OnInitializeValues(Texture2D texture2D, Vector2Int textureRect, Color[] pixelColors);
    public static event OnInitializeValues onInitializeValues;

    public delegate void OnScreenTouched();
    public static event OnScreenTouched onScreenTouched;

    public static void SignalOnInitializeValues(Texture2D texture2D, Vector2Int textureRect, Color[] pixelColors)
    {
        onInitializeValues?.Invoke(texture2D, textureRect, pixelColors);
    }
    public static void SignalOnResetTexture()
    {
        onResetTexture?.Invoke();
    }
    public static void SignalOnPaint(Color colorToPaint)
    {
        onPaint?.Invoke(colorToPaint);
    }

    public static void SignalOnScreenTouched()
    {
        onScreenTouched?.Invoke();
    }
}

