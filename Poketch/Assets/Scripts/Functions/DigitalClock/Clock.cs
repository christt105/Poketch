using System;
using UnityEngine;

public class Clock : Function
{
    [SerializeField]
    private NumberController m_Hours;

    [SerializeField]
    private NumberController m_Minutes;

    private Vector2Int m_LastTime = Vector2Int.one * -1;

    public override void OnChange()
    {
        CancelInvoke();
        InvokeRepeating( nameof( UpdateClock ), 0f, 2f );
    }

    public override void OnExit()
    {
        CancelInvoke();
    }

    #region Private

    private void UpdateClock()
    {
        int hours = DateTime.Now.Hour;
        int minutes = DateTime.Now.Minute;

        if ( m_LastTime.x != hours )
        {
            m_Hours.SetNumber( hours );
            m_LastTime.x = hours;
        }

        if ( m_LastTime.y != minutes )
        {
            m_Minutes.SetNumber( minutes );
            m_LastTime.y = minutes;
        }
    }

    #endregion
}
