using SimpleJSON;
using UnityEngine;

public abstract class Function : MonoBehaviour
{
    /// <summary>
    /// </summary>
    /// <param name="jsonObject"></param>
    public virtual void OnCreate( JSONObject jsonObject )
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
}
