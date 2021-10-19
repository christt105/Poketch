using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GridWeeper
{
    private int m_width;
    private int m_height;
    private float m_cellSize;
    private float m_gridLayoutCellSize;
    private float m_pokeSize;
    private int m_fontSize;
    private Transform m_parentRootWeeper;
    private Transform m_parentBgColumn;
    private Transform m_parentBgRow;
    private Font m_font;

    private VoltorbWeeper.WeepBlock[] m_ArrayWeeper;
    private VoltorbWeeper m_voltorbWeeperScript;
    public GameObject[] m_ArrayGameObjects;
    public GameObject[] m_ArrayGridBgColumn;
    public GameObject[] m_ArrayGridBgRow;

    private Material m_materialColor;

    private Sprite m_spriteBlock;
    private Sprite m_spriteVoltorb;
    private Sprite m_spriteDiglett;

    public GridWeeper(VoltorbWeeper.GridInfo gridInfo, VoltorbWeeper.WeepBlock[] ArrayVoltorbWeeper, Transform parentRootWeeper, Font font, Sprite spriteBlock, Sprite spriteVoltorb, Sprite spriteDiglett, Material materialColor, VoltorbWeeper voltorbWeeperScript)
    {
        this.m_width = gridInfo.size;
        this.m_height = gridInfo.size;
        this.m_cellSize = gridInfo.cellSize;
        this.m_gridLayoutCellSize = gridInfo.gridLayoutCellSize;
        this.m_pokeSize = gridInfo.pokeSize;
        this.m_fontSize = gridInfo.fontSize;
        this.m_parentRootWeeper = parentRootWeeper;
        this.m_parentBgColumn = gridInfo.parentBgColumn;
        this.m_parentBgRow = gridInfo.parentBgRow;
        this.m_font = font;
        this.m_spriteBlock = spriteBlock;
        this.m_spriteVoltorb = spriteVoltorb;
        this.m_spriteDiglett = spriteDiglett;
        this.m_materialColor = materialColor;
        this.m_ArrayWeeper = ArrayVoltorbWeeper;
        this.m_ArrayGameObjects = new GameObject[ArrayVoltorbWeeper.Length];
        this.m_voltorbWeeperScript = voltorbWeeperScript;
        this.m_ArrayGridBgColumn = new GameObject[m_height+1];
        this.m_ArrayGridBgRow = new GameObject[m_width+1];

        CreateWorldGrid();
    }

    public void CreateWorldGrid()
    {
        GridLayoutGroup gridLayout = m_parentRootWeeper.GetComponent<GridLayoutGroup>();
        gridLayout.constraintCount = m_height;
        gridLayout.cellSize = new Vector2(m_gridLayoutCellSize, m_gridLayoutCellSize);

        m_parentBgColumn = GameObject.Instantiate(m_parentBgColumn, m_parentRootWeeper.parent);
        m_parentBgColumn.SetSiblingIndex(1);
        m_parentBgColumn.GetComponent<GridLayoutGroup>().spacing = new Vector2(m_gridLayoutCellSize - m_parentBgColumn.GetComponent<GridLayoutGroup>().cellSize.x, 0);

        m_parentBgRow = GameObject.Instantiate(m_parentBgRow, m_parentRootWeeper.parent);
        m_parentBgRow.SetSiblingIndex(1);
        m_parentBgRow.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, m_gridLayoutCellSize - m_parentBgRow.GetComponent<GridLayoutGroup>().cellSize.y);

        for (int i = 0; i < m_ArrayWeeper.Length; i++)
        {
            CreateWorldText(m_ArrayWeeper[i].num.ToString(), out GameObject gameObject, m_font, m_fontSize, Color.grey, TextAnchor.MiddleCenter);
            m_ArrayGameObjects[i] = gameObject;
            m_ArrayGameObjects[i].transform.SetParent(m_parentRootWeeper);

            // Create Background
            if (i <= m_height) 
            {
                GameObject gridBgColumn = new GameObject("BackgroundImageColumn", typeof(Image));
                gridBgColumn.transform.SetParent(m_parentBgColumn);
                gridBgColumn.transform.localScale = new Vector3(1, 1, 1);
                gridBgColumn.GetComponent<Image>().material = m_materialColor;
                gridBgColumn.GetComponent<Image>().color = new Vector4(0.4745098f, 0.4745098f, 0.4745098f, 1);
                m_ArrayGridBgColumn[i] = gridBgColumn;

                GameObject gridBgRow = new GameObject("BackgroundImageRow", typeof(Image));
                gridBgRow.transform.SetParent(m_parentBgRow);
                gridBgRow.transform.localScale = new Vector3(1, 1, 1);
                gridBgRow.GetComponent<Image>().material = m_materialColor;
                gridBgRow.GetComponent<Image>().color = new Vector4(0.4745098f, 0.4745098f, 0.4745098f, 1);
                m_ArrayGridBgRow[i] = gridBgRow;

            }
        }
    }

    private void CreateWorldText(string text, out GameObject gameObject, Font font, int fontSize, Color color, TextAnchor textAnchor)
    {
        int weep_num = int.Parse(text);


        // Check if the Weep is Voltorb or Empty (TODO: Change magic numbers)
        if (weep_num == 9 || weep_num == 0)
        {
            gameObject = new GameObject("WeeperText", typeof(Image));
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize , m_cellSize);

            Image weepImageUI = gameObject.GetComponent<Image>();
            if(weep_num == 9)
                weepImageUI.sprite = m_spriteVoltorb;

            weepImageUI.material = m_materialColor;
        }
        else
        {
            gameObject = new GameObject("WeeperText", typeof(Text));
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize, m_cellSize);

            Text textUI = gameObject.GetComponent<Text>();
            textUI.text = text;
            textUI.fontSize = fontSize;
            textUI.color = color;
            textUI.font = font;
            textUI.alignment = textAnchor;
            textUI.material = m_materialColor;
        }

        // Create Child Button
        GameObject childObject = new GameObject("Button", typeof(Image), typeof(Button));
        childObject.transform.SetParent(gameObject.transform);
        childObject.transform.localPosition = Vector3.zero;
        childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize, m_cellSize);
        
        Image imageUI = childObject.GetComponent<Image>();
        imageUI.sprite = m_spriteBlock;
        imageUI.material = m_materialColor;

        // --- DEBUG ---
        // imageUI.color = new Vector4(0,0,0,0.2f);

        Button buttonUI = childObject.GetComponent<Button>();
        buttonUI.transition = Selectable.Transition.SpriteSwap;

        // Create Child Diglett
        GameObject childChildObject = new GameObject("Diglett", typeof(Image));
        childChildObject.transform.SetParent(childObject.transform);
        childChildObject.transform.localPosition = Vector3.zero;
        childChildObject.GetComponent<RectTransform>().sizeDelta = new Vector2(m_cellSize * m_pokeSize, m_cellSize * m_pokeSize);

        childChildObject.GetComponent<Image>().sprite = m_spriteDiglett;
        childChildObject.GetComponent<Image>().material = m_materialColor;
        childChildObject.SetActive(false);     

        GameObject go = gameObject;
        buttonUI.onClick.AddListener(() => m_voltorbWeeperScript.OnClick(go.transform.GetSiblingIndex()));
    }


    public void DeleteGrid()
    {
        for (int i = 0; i < m_ArrayGameObjects.Length; i++)
        {
            GameObject.Destroy(m_ArrayGameObjects[i].gameObject);

            if(i <= m_height)
            {
                GameObject.Destroy(m_ArrayGridBgColumn[i].gameObject);
                GameObject.Destroy(m_ArrayGridBgRow[i].gameObject);
            }
        }
        GameObject.Destroy(m_parentBgColumn.gameObject);
        GameObject.Destroy(m_parentBgRow.gameObject);

        System.Array.Clear(m_ArrayWeeper, 0, m_ArrayWeeper.Length);
    }
}
