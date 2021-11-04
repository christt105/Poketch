using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDay : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Image m_Image;

    [SerializeField]
    private NumberController m_NumberController;

    private Calendar m_Calendar;

    public NumberController NumberController => m_NumberController;

    private void Start()
    {
        m_Calendar = GetComponentInParent < Calendar >();
    }

    public void OnPointerDown( PointerEventData eventData )
    {
        Select();
        m_Calendar.Save(transform.GetSiblingIndex(), m_Image.enabled);
    }

    public void Select()
    {
        m_Image.enabled = !m_Image.enabled;
    }

    public void Hide()
    {
        GetComponent < Image >().enabled = false;
        m_NumberController.gameObject.SetActive( false );
        m_Image.enabled = false;
    }

    public void SetSpecial()
    {
        m_NumberController.GetComponent < RectTransform >().localScale = Vector3.one * 1.1f;
    }

    public void SetNumbersColor( Color color )
    {
        foreach ( Image image in m_NumberController.GetComponentsInChildren < Image >() )
        {
            image.color = color;
        }
    }

    public void Unhide()
    {
        GetComponent < Image >().enabled = true;
        m_Image.enabled = false;
        m_NumberController.gameObject.SetActive( true );
    }
}
