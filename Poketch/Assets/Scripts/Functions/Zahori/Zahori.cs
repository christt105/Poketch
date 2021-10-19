using SimpleJSON;
using UnityEngine;

public class Zahori : Function
{
    [SerializeField]
    private GameObject circle;

    private Canvas canvas;

    public override void OnCreate(JSONNode jsonObject)
    {
        canvas = FindObjectOfType<Canvas>();
    }
    public override void OnChange()
    {
        circle.SetActive(false);
    }

    private void OnEnable()
    {
        Signals.onScreenTouched += ScreenTouched;
    }

    private void OnDisable()
    {
        Signals.onScreenTouched -= ScreenTouched;
    }

    private void ScreenTouched()
    {
        if (!circle.activeInHierarchy)
            ActivateCircle();
    }

    private void ActivateCircle()
    {
        circle.SetActive(true);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        circle.transform.position = canvas.transform.TransformPoint(pos);
    }
}
