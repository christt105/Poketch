using SimpleJSON;
using UnityEngine;

public class Zahori : Function
{
    [SerializeField]
    private GameObject circle;
    [SerializeField]
    private GameObject spot;

    private Canvas canvas;

    public override void OnCreate(JSONNode jsonObject)
    {
        canvas = FindObjectOfType<Canvas>();
    }
    public override void OnChange()
    {
        circle.SetActive(false);
        spot.SetActive(false);
    }

    private void OnEnable()
    {
        Signals.onScreenTouched += ScreenTouched;
        Signals.onCircleExpanded += SpawnSpot;
    }

    private void OnDisable()
    {
        Signals.onScreenTouched -= ScreenTouched;
        Signals.onCircleExpanded -= SpawnSpot;
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

        SoundManager.Instance.PlaySFX(SoundManager.SFX.Zahori);
    }

    private void SpawnSpot()
    {
        if (Random.Range(0, 6) == 0)
            spot.SetActive(true);
    }
}
