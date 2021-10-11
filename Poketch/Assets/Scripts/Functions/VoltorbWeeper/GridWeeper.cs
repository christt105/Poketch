using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridWeeper
{
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private Font m_font;
    private int m_textSize;
    private Transform m_parent;

    private int[,] m_ArrayGrid;
    private List<Text> m_ArrayText = new List<Text>();
    private List<Button> m_ArrayButton = new List<Button>();
    private List<Image> m_ArrayImage = new List<Image>();

    private Material m_materialColor;

    private Sprite m_spriteBlock;

    public GridWeeper(int width, int height, float cellSize, Font font, int textSize, Transform parent, Sprite spriteBlock, Material materialColor, VoltorbWeeper.Weeper[] ArrayVoltorbWeeper)
    {
        this.m_width = width;
        this.m_height = height;
        this.m_cellSize = cellSize;
        this.m_textSize = textSize;
        this.m_parent = parent;
        this.m_font = font;
        this.m_spriteBlock = spriteBlock;
        this.m_materialColor = materialColor;

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
                CreateWorldText(m_ArrayGrid[i, j].ToString(), null, GetWorldPosition(i, j) + new Vector3(m_cellSize, m_cellSize) * 0.5f, out Button buttonUI, out Image imageUI, out Text textUI, m_font, m_textSize, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 1);

                m_ArrayText.Add(textUI);
                m_ArrayImage.Add(imageUI);
                m_ArrayButton.Add(buttonUI);

                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
            Debug.DrawLine(GetWorldPosition(0, m_height), GetWorldPosition(m_width, m_height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(m_width, 0), GetWorldPosition(m_width, m_height), Color.white, 100f);

        }

        for (int i = 0; i < m_ArrayText.Count; i++)
        {
            m_ArrayText[i].gameObject.transform.SetParent(m_parent);
        }


        //Todo: Put img Voltorbs instead of mines
        //Todo: Put empty block instead of 0
        //Todo: Put blocks in fornt of ALL
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x, y) * m_cellSize) + m_parent.position;
    }

    private void CreateWorldText(string text, Transform parent, Vector3 localPosition, out Button buttonUI, out Image imageUI, out Text textUI, Font font, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAligment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("WeeperText", typeof(Text));
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPosition;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize, m_cellSize);

        textUI = gameObject.GetComponent<Text>();
        textUI.text = text;
        textUI.fontSize = fontSize;
        textUI.color = color;
        textUI.font = font;
        textUI.alignment = textAnchor;

        // Create Child Immage
        GameObject childObject = new GameObject("Button", typeof(Button), typeof(Image));
        childObject.transform.SetParent(gameObject.transform);
        childObject.transform.localPosition = Vector3.zero;
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize, m_cellSize);

        imageUI = childObject.GetComponent<Image>();
        imageUI.sprite = m_spriteBlock;
        imageUI.material = m_materialColor;

        buttonUI = childObject.GetComponent<Button>();
        buttonUI.transition = Selectable.Transition.SpriteSwap;
        SpriteState spriteState = buttonUI.spriteState;

        spriteState.pressedSprite = m_spriteBlock;

        buttonUI.spriteState = spriteState;

    }

    public void GetMousePosition(Vector3 mousePosition)
    {
        Gizmos.DrawSphere(mousePosition, 5);
    }

    public void OnDrawGizmosSelected(Vector3 mousePosition)
    {
        Gizmos.DrawSphere(mousePosition, 5);

    }

   /* public void SetValue(int x, int y, int value)
    {
        if (m_parent.position.x + x >= m_parent.position.x && m_parent.position.y + y >= m_parent.position.y && m_parent.position.x + x < m_parent.position.x + m_width && m_parent.position.y + y < m_parent.position.y + m_height)
            Debug.DrawLine(Vector3.zero, new Vector3(m_parent.position.x + x, m_parent.position.y + y, 0));
        //m_ArrayGrid[x, y] = value; 
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
    }*/

    public void DeleteGrid()
    {
        for (int i = 0; i < m_ArrayText.Count; i++)
            GameObject.Destroy(m_ArrayText[i].gameObject);

        System.Array.Clear(m_ArrayGrid, 0, m_ArrayGrid.Length);

        m_ArrayText.Clear();
        m_ArrayImage.Clear();
        m_ArrayButton.Clear();
    }
}
