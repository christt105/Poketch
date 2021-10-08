using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWeeper
{
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private int m_textSize;
    private Transform m_parent;

    private int[,] m_ArrayGrid;
    private List<TextMesh> m_ArrayTextMesh = new List<TextMesh>();

    public GridWeeper(int width, int height, float cellSize, int textSize, Transform parent, VoltorbWeeper.Weeper[] ArrayVoltorbWeeper)
    {
        this.m_width = width;
        this.m_height = height;
        this.m_cellSize = cellSize;
        this.m_textSize = textSize;
        this.m_parent = parent;

        m_ArrayGrid = new int[width, height];

        int i = 0;
        for (int y = m_ArrayGrid.GetLength(0) - 1; y >= 0 ; y--)
        {
            for (int x = 0; x < m_ArrayGrid.GetLength(1); x++)
            {
                m_ArrayGrid[x, y] = ArrayVoltorbWeeper[i].m_num;
                i++;
            }
        }

        CreateWorldGrid(ArrayVoltorbWeeper);
    }

    public void CreateWorldGrid(VoltorbWeeper.Weeper[] ArrayVoltorbWeeper)
    {
        for (int i = 0; i < m_ArrayGrid.GetLength(0); i++)
        {
            for (int j = 0; j < m_ArrayGrid.GetLength(1); j++)
            {
                m_ArrayTextMesh.Add(CreateWorldText(m_ArrayGrid[i, j].ToString(), null, GetWorldPosition(i, j) + new Vector3(m_cellSize, m_cellSize) * 0.5f, m_textSize, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0));

                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
            Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);

        }

        for (int i = 0; i < m_ArrayTextMesh.Count; i++)
            m_ArrayTextMesh[i].gameObject.transform.SetParent(m_parent);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * m_cellSize;
    }

    private TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition,int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAligment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAligment;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }

    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < m_width && y < m_height)
            m_ArrayGrid[x, y] = value;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / m_cellSize);
        y = Mathf.FloorToInt(worldPosition.y / m_cellSize);
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void DeleteGrid()
    {
        for (int i = 0; i < m_ArrayTextMesh.Count; i++)
            GameObject.Destroy(m_ArrayTextMesh[i].gameObject);
    }
}
