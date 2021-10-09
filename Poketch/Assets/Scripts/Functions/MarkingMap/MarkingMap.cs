using SimpleJSON;
using UnityEngine;

public class MarkingMap : Function
{
    [SerializeField]
    private Transform m_MarksTransform;

    private Mark[] m_MarksArray;

    public override void OnCreate( JSONNode jsonObject )
    {
        int i = 0;
        m_MarksArray = m_MarksTransform.GetComponentsInChildren < Mark >();

        foreach ( Mark mark in m_MarksArray )
        {
            mark.Set( jsonObject?[i++], this );
        }
    }

    public void Save()
    {
        JSONArray json = new JSONArray();

        foreach ( Mark mark in m_MarksArray )
        {
            JSONObject jsonMark = new JSONObject();
            Vector2 position = mark.GetPosition();

            jsonMark["x"] = position.x;
            jsonMark["y"] = position.y;

            json.Add( jsonMark );
        }

        FunctionController.Instance.SaveFunctionInfo( GetType().Name, json );
    }
}
