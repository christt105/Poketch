using SimpleJSON;
using UnityEngine;

public abstract class Function : MonoBehaviour
{
    [SerializeField]
    private bool m_UseAuxButton = false;

    public bool UseAuxButton => m_UseAuxButton;

    /// <summary>
    /// </summary>
    /// <param name="jsonObject"></param>
    public virtual void OnCreate( JSONNode jsonObject )
    {
    }

    /// <summary>
    /// </summary>
    public virtual void OnChange()
    {
    }

    /// <summary>
    /// </summary>
    public virtual void OnExit()
    {
    }

    /// <summary>
    /// </summary>
    public virtual void OnAuxButton()
    {
    }
}
