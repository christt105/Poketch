using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class VoltorbWeeper : Function
{
    enum GameMode
    {
        VOLTORB,
        DIGLETT
    }
    private GameMode m_gameMode = GameMode.VOLTORB;

    public enum DifficultMode
    {
        EASY,
        NORMAL,
        DIFFICULT
    }
    private DifficultMode m_diff = DifficultMode.DIFFICULT;
    enum Buttons
    {
        VOLTORB,
        DIGLETT
    }

    [Serializable]
    public struct WeepBlock
    {
        public int num;
        public bool is_voltorb;
        public bool is_diglett;
        public bool is_zero_checked;
        public bool is_pressed;
    }

    [Serializable]
    public struct WeepButton
    {
        public Transform button;
        public Sprite sprite_button;
        public Sprite sprite_pressed;
        public bool m_is_selected;
        [Range(0, 50)]
        public float offset_pushed;
    }

    [Serializable]
    public struct GridInfo
    {
        public DifficultMode mode;
        public int max_voltorbs;
        public int size;
        public int cellSize;
        [Range(0f, 1f)]
        public float pokeSize;
        public int fontSize;

        [Header("--- GRIDLAYOUT ---")]
        public Transform parentBgColumn;
        public Transform parentBgRow;
        public int gridLayoutCellSize;
        public int GetArea() { return size * size; }
    }

    [Header("--- GRID ---")]
    [SerializeField]
    private GridInfo[] m_gridInfo;

    [SerializeField]
    private Font m_font;

    [SerializeField]
    private Material m_materialColor;

    [SerializeField]
    private Transform m_parentRootWeeper;

    [Header("--- SPRITES ---")]
    [SerializeField] 
    private Sprite m_spriteVoltorb;

    [SerializeField]
    private Sprite  m_spriteBlock;    
    
    [SerializeField]
    private Sprite  m_spriteDiglett;

    [Header("--- ANIMATIONS ---")]
    [SerializeField]
    private Animator m_anim_voltorbFound;

    [Header("--- ARRAYS ---")]
    [SerializeField]
    private WeepButton[] m_ArrayUIButtons;

    [SerializeField]
    private WeepBlock[] m_ArrayWeeper;

    [Header("--- OTHER ---")]
    [SerializeField]
    private float m_maxTimePressedDiglett;

    [SerializeField]
    private NumberController m_numController;

    [SerializeField]
    private Transform m_mainMenu;

    [SerializeField]
    private Transform m_winloseMenu;

    [SerializeField]
    private Text m_winloseText;

    [SerializeField]
    private Transform m_winloseDiglett;

    [SerializeField]
    private Transform m_winloseVoltorb;

    [SerializeField]
    private Text m_winLoseTime;

    private float m_timer;
    private float m_timerButtonPressed;
    private int m_remainingVoltorb;
    private bool m_enableTimer;
    private bool m_firstClick;
    private bool m_clickPressed;

    private GridWeeper m_grid;

    #region Override Functions
    public override void OnCreate(JSONNode jsonObject)
    {

    }

    public override void OnChange()
    {
        ShowMenu(true);
    }

    public override void OnExit()
    {
        DeleteVoltorbWeeper();
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            ResetVoltorbWeeper();
        
        if(m_enableTimer)
            m_timer += Time.deltaTime;
        
        if(m_clickPressed)
            m_timerButtonPressed += Time.deltaTime;
    }

    public void OnPointerDownButton()
    {
        m_clickPressed = true;

        Debug.Log("Pressed");
    }

    public void OnPointerUpButton(int index)
    {
        m_clickPressed = false;
        OnClick(index);

        Debug.Log(m_timerButtonPressed);
        Debug.Log("UN Pressed----------");
        m_timerButtonPressed = 0;
    }
    private void OnClick(int index)
    {
        if (m_timerButtonPressed >= m_maxTimePressedDiglett)
            DiglettLogic(index);
        else
        {
            switch (m_gameMode)
            {
                case GameMode.VOLTORB:
                    VoltorbLogic(index);
                    break;

                case GameMode.DIGLETT:
                    DiglettLogic(index);
                    break;

                default:
                    break;
            }
        }
       
    }

    public void OnClickStart(int difficulty)
    {
        m_diff = (DifficultMode)difficulty;
        CreateVoltorbWeeper();
        ShowMenu(false);

        m_remainingVoltorb = m_gridInfo[(int)m_diff].max_voltorbs;
        m_numController.SetNumber(m_remainingVoltorb);
        m_timer = 0;
        m_enableTimer = true;

        // Game Mode
        ChangeStateVoltButton((int)Buttons.VOLTORB, true);
        ChangeStateVoltButton((int)Buttons.DIGLETT, false);
        m_gameMode = GameMode.VOLTORB;

    }
    public void OnClickChangeClickMode()
    {
        if (m_gameMode == GameMode.VOLTORB)
        {
            ChangeStateVoltButton((int)Buttons.VOLTORB, false);
            ChangeStateVoltButton((int)Buttons.DIGLETT, true);

            m_gameMode = GameMode.DIGLETT;
        }
        else if (m_gameMode == GameMode.DIGLETT)
        {
            ChangeStateVoltButton((int)Buttons.DIGLETT, false);
            ChangeStateVoltButton((int)Buttons.VOLTORB, true);

            m_gameMode = GameMode.VOLTORB;
        }
    }

    private void CreateVoltorbWeeper()
    {
        if (m_gridInfo[(int)m_diff].max_voltorbs <= m_gridInfo[(int)m_diff].size * m_gridInfo[(int)m_diff].size)
        {
            m_firstClick = true;
            m_ArrayWeeper = AddVoltorbNeighbors(MergeVoltorbWeeper(CreateVoltorbs()));
            VoltorbWeeper voltorbWeeperScript = gameObject.GetComponent<VoltorbWeeper>();
            m_grid = new GridWeeper(m_gridInfo[(int)m_diff], m_ArrayWeeper, m_parentRootWeeper, m_font, m_spriteBlock, m_spriteVoltorb, m_spriteDiglett, m_materialColor, voltorbWeeperScript);
        }
        else
            Debug.LogError("--- Error: You are using more Vortorbs than cells ---");
    }

    private int[] CreateVoltorbs() 
    {
        int[] new_ArrayVoltorb = new int[m_gridInfo[(int)m_diff].max_voltorbs];

        for (int i = 0; i < m_gridInfo[(int)m_diff].max_voltorbs; i++)
            new_ArrayVoltorb[i] = UnityEngine.Random.Range(0, m_gridInfo[(int)m_diff].GetArea());

        return CheckForDuplicatedVoltorbs(new_ArrayVoltorb);
    }

    private int[] CheckForDuplicatedVoltorbs(int[] check_ArrayVoltorb)
    {
        int[] safe_ArrayVoltorb = new int[m_gridInfo[(int)m_diff].max_voltorbs];

        for (int i = 0; i < m_gridInfo[(int)m_diff].max_voltorbs; i++)
        {
            for (int j = 0; j < safe_ArrayVoltorb.Length; j++)
            {
                if (check_ArrayVoltorb[i] == safe_ArrayVoltorb[j] && !(i == j))
                {
                    Debug.Log("--- CheckForDuplicatedVoltorbs() Found ---");

                    check_ArrayVoltorb[i]  = UnityEngine.Random.Range(0, m_gridInfo[(int)m_diff].GetArea());
                    j = -1;    
                }
            }
            // Array without duplicated Voltorb
            safe_ArrayVoltorb[i] = check_ArrayVoltorb[i];
        }

        return safe_ArrayVoltorb;
    }

    private WeepBlock[] MergeVoltorbWeeper(int[] ArrayVoltorbPosition)
    {
        WeepBlock [] ArrayWeeperType = new WeepBlock[m_gridInfo[(int)m_diff].GetArea()];

        for (int i = 0; i < ArrayVoltorbPosition.Length; i++)
        {
            ArrayWeeperType[ArrayVoltorbPosition[i]].is_voltorb = true;
            ArrayWeeperType[ArrayVoltorbPosition[i]].num = 9;
        }

        return ArrayWeeperType;
    }

    private WeepBlock[] AddVoltorbNeighbors(WeepBlock[] ArrayWeeper)
    {
        for (int i = 0; i < ArrayWeeper.Length; i++)
        {
            if(ArrayWeeper[i].is_voltorb)
            {

                int sizeGrid = m_gridInfo[(int)m_diff].size;

                int check_right_column = (i + 1) % sizeGrid;
                int check_left_column = (i) % sizeGrid;

                bool right_neighbor = false;
                bool left_neighbor = false;
                bool up_neighbor = false;
                bool down_neighbor = false;

                if (check_right_column != 0 || i == 0)
                    right_neighbor = true;
                if (check_left_column != 0)
                    left_neighbor = true;
                if (i >= sizeGrid)
                    up_neighbor = true;
                if (i < ArrayWeeper.Length - sizeGrid)
                    down_neighbor = true;

                if (right_neighbor && !ArrayWeeper[i + 1].is_voltorb)
                    ArrayWeeper[i + 1].num++;
                if (right_neighbor && up_neighbor && !ArrayWeeper[i - sizeGrid + 1].is_voltorb)
                    ArrayWeeper[i - sizeGrid + 1].num++;
                if (right_neighbor && down_neighbor && !ArrayWeeper[i + sizeGrid + 1].is_voltorb)
                    ArrayWeeper[i + sizeGrid + 1].num++;

                if (left_neighbor && !ArrayWeeper[i - 1].is_voltorb)
                    ArrayWeeper[i - 1].num++;
                if (left_neighbor && up_neighbor && !ArrayWeeper[i - sizeGrid - 1].is_voltorb)
                    ArrayWeeper[i - sizeGrid - 1].num++;
                if (left_neighbor && down_neighbor && !ArrayWeeper[i + sizeGrid - 1].is_voltorb)
                    ArrayWeeper[i + sizeGrid - 1].num++;

                if (up_neighbor && !ArrayWeeper[i - sizeGrid].is_voltorb)
                    ArrayWeeper[i - sizeGrid].num++;
                if (down_neighbor && !ArrayWeeper[i + sizeGrid].is_voltorb)
                    ArrayWeeper[i + sizeGrid].num++;
            }
        }
        
        return ArrayWeeper;
    }

    private void VoltorbLogic(int index)
    {
        if (m_firstClick && m_ArrayWeeper[index].num != 0)
        {
            // Restart :)
            ResetVoltorbWeeper();
            OnClick(index);
            Debug.Log("--- ResetVoltorbWeeper() ---");
        }
        else if (m_ArrayWeeper[index].num == 0)
        {
            if (m_firstClick)
            {
                m_firstClick = false;
            }

            
            DisableZeroNeighbors(index, true, index => { return m_ArrayWeeper[index].num == 0; });
            DisableBlock(index, false);
            DisableZeroNeighbors(index, false, index => true );
        }
        else if (m_ArrayWeeper[index].is_diglett)
        {
            return;
        }
        else if (m_ArrayWeeper[index].is_voltorb)
        {
            for (int i = 0; i < m_ArrayWeeper.Length; i++)
            {
                if (m_ArrayWeeper[i].is_voltorb)
                {
                    m_grid.m_ArrayGameObjects[i].GetComponent<RectTransform>().localScale = new Vector2(m_gridInfo[(int)m_diff].pokeSize, m_gridInfo[(int)m_diff].pokeSize);
                    DisableBlock(i, false);
                }
            }
            m_grid.m_ArrayGameObjects[index].AddComponent<Animator>();
            Animator anim = m_grid.m_ArrayGameObjects[index].GetComponent<Animator>();
            anim.runtimeAnimatorController = m_anim_voltorbFound.runtimeAnimatorController;

            m_enableTimer = false;
            Invoke("LoseCondition", 2);
            Invoke("DeleteVoltorbWeeper", 2f);
        }
        else
            DisableBlock(index, false);

        if (IsArrayWeeperPressed())
        {
            m_enableTimer = false;
            WinCondition();
            DeleteVoltorbWeeper();
        }
    }
    private void DiglettLogic(int index)
    {
        if (!m_ArrayWeeper[index].is_diglett)
        {
            m_ArrayWeeper[index].is_diglett = true;
            m_grid.m_ArrayGameObjects[index].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            ChangeRemainingVoltorbNumber(true);
        }
        else
        {
            m_ArrayWeeper[index].is_diglett = false;
            m_grid.m_ArrayGameObjects[index].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            ChangeRemainingVoltorbNumber(false);
        }
    }

    private void ChangeStateVoltButton(int position, bool value)
    {
        Image sprite = m_ArrayUIButtons[position].button.GetChild(0).GetComponent<Image>();

        if (!m_ArrayUIButtons[position].m_is_selected && value)
        {
            m_ArrayUIButtons[position].button.GetChild(0).position = new Vector3(m_ArrayUIButtons[position].button.GetChild(0).position.x, m_ArrayUIButtons[position].button.GetChild(0).position.y - m_ArrayUIButtons[position].offset_pushed, m_ArrayUIButtons[position].button.GetChild(0).position.z);
            m_ArrayUIButtons[position].button.GetComponent<Image>().sprite = m_ArrayUIButtons[position].sprite_pressed;
            m_ArrayUIButtons[position].button.GetComponent<Button>().enabled = false;

            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 0.7f);
        }
        else if (m_ArrayUIButtons[position].m_is_selected && !value)
        {
            m_ArrayUIButtons[position].button.GetChild(0).position = new Vector3(m_ArrayUIButtons[position].button.GetChild(0).position.x, m_ArrayUIButtons[position].button.GetChild(0).position.y + m_ArrayUIButtons[position].offset_pushed, m_ArrayUIButtons[position].button.GetChild(0).position.z);
            m_ArrayUIButtons[position].button.GetComponent<Image>().sprite = m_ArrayUIButtons[position].sprite_button;
            m_ArrayUIButtons[position].button.GetComponent<Button>().enabled = true;

            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }

        m_ArrayUIButtons[position].m_is_selected = value;
    }

    private void DisableZeroNeighbors(int index, bool value, Predicate<int> predicate)
    {
        int sizeGrid = m_gridInfo[(int)m_diff].size;

        int check_right_column = (index + 1) % sizeGrid;
        int check_left_column = (index) % sizeGrid;

        bool right_neighbor = false;
        bool left_neighbor = false;
        bool up_neighbor = false;
        bool down_neighbor = false;

        if (check_right_column != 0 || index == 0)
            right_neighbor = true;
        if (check_left_column != 0)
            left_neighbor = true;
        if (index >= sizeGrid)
            up_neighbor = true;
        if (index < m_ArrayWeeper.Length - sizeGrid)
            down_neighbor = true;

        if(right_neighbor && predicate(index+1))
            DisableBlock(index + 1, value);

        if(right_neighbor && up_neighbor  && predicate(index - sizeGrid + 1))
            DisableBlock(index - sizeGrid + 1, true);        
        
        if(right_neighbor && down_neighbor && predicate(index + sizeGrid + 1))
            DisableBlock(index + sizeGrid + 1, true);

        if (left_neighbor && predicate(index - 1))
            DisableBlock(index - 1, true);
        if (left_neighbor && up_neighbor && predicate(index - sizeGrid - 1))
            DisableBlock(index - sizeGrid - 1, true);
        if (left_neighbor && down_neighbor && predicate(index + sizeGrid - 1))
            DisableBlock(index + sizeGrid - 1, true);

        if (up_neighbor && predicate(index - sizeGrid))
            DisableBlock(index - sizeGrid, true);
        if (down_neighbor && predicate(index + sizeGrid))
            DisableBlock(index + sizeGrid, true);
        
    }

    private void DisableBlock(int index, bool check_neighbor)
    {
        if (check_neighbor && !m_ArrayWeeper[index].is_zero_checked)
        {
            m_ArrayWeeper[index].is_zero_checked = true;
            m_grid.m_ArrayGameObjects[index].transform.GetChild(0).gameObject.SetActive(false);
            OnClick(index);
        }
        else
        {
            m_grid.m_ArrayGameObjects[index].transform.GetChild(0).gameObject.SetActive(false);

            if (m_ArrayWeeper[index].num == 0)
                m_ArrayWeeper[index].is_zero_checked = true;
        }
        m_ArrayWeeper[index].is_pressed = true;
    }

    private void ChangeRemainingVoltorbNumber(bool add_diglett)
    {
        if (add_diglett)
            m_remainingVoltorb--;
        else
            m_remainingVoltorb++;

        m_numController.SetNumber(m_remainingVoltorb);

    }

    private bool IsArrayWeeperPressed()
    {
        bool ret = true;

        for (int i = 0; i < m_ArrayWeeper.Length; i++)
            if (!m_ArrayWeeper[i].is_voltorb && !m_ArrayWeeper[i].is_pressed)
                ret = false;

        return ret;
    }

    private void WinCondition()
    {
        m_winloseMenu.gameObject.SetActive(true);
        m_winloseText.gameObject.SetActive(true);
        m_winloseDiglett.gameObject.SetActive(true);
        m_winloseText.GetComponent<Text>().text = "YOU WIN";
        for (int i = 0; i < m_ArrayUIButtons.Length; i++)
            m_ArrayUIButtons[i].button.gameObject.SetActive(false);

        m_winLoseTime.text = "Time: " + Mathf.FloorToInt(m_timer).ToString() + "s";
    }

    private void LoseCondition()
    {
        m_winloseMenu.gameObject.SetActive(true);
        m_winloseText.gameObject.SetActive(true);

        m_winloseVoltorb.gameObject.SetActive(true);
        m_winloseText.GetComponent<Text>().text = "GAME OVER";
        for (int i = 0; i < m_ArrayUIButtons.Length; i++)
            m_ArrayUIButtons[i].button.gameObject.SetActive(false);

        m_winLoseTime.text = "Time: " + Mathf.FloorToInt(m_timer).ToString() + "s";
    }

    public void ResetButton()
    {
        ShowMenu(true);
    }

    private void ResetVoltorbWeeper()
    {
        DeleteVoltorbWeeper();
        CreateVoltorbWeeper();
    }

    private void ShowMenu(bool value)
    {
        m_mainMenu.gameObject.SetActive(value);

        for (int i = 0; i < m_ArrayUIButtons.Length; i++)
            m_ArrayUIButtons[i].button.gameObject.SetActive(!value);

        m_winloseMenu.gameObject.SetActive(false);
        m_winloseDiglett.gameObject.SetActive(false);
        m_winloseVoltorb.gameObject.SetActive(false);
    }


    private void DeleteVoltorbWeeper()
    {
        Array.Clear(m_ArrayWeeper, 0, m_ArrayWeeper.Length);
        if(m_grid != null)
        {
            m_grid.DeleteGrid();
            m_grid = null;
        }
    }
}
