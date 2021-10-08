using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;


public class VoltorbWeeper : Function
{
    enum WeeperType
    {
        EMPTY,
        ONE,
        TWO,
        THREE,
        FOUR,
        VOLTORB
    }

    public struct Weeper
    {
        public bool m_is_voltorb;
        public int m_num;
    }

    [SerializeField]
    private int m_max_voltorbs;

    [SerializeField]
    private Vector2Int m_sizeGrid = new Vector2Int();

    [SerializeField] [Range(0f, 200f)]
    private float cellSize;
    
    [SerializeField] [Range(0, 200)]
    private int textSize;   

    private Weeper[] m_ArrayWeeper;

    private GridWeeper m_grid;

    [SerializeField]
    private Transform m_RootWeeper;

    #region Override Functions

    public override void OnCreate(JSONNode jsonObject)
    {

    }

    public override void OnChange()
    {
        if (m_max_voltorbs <= m_sizeGrid.x * m_sizeGrid.y)
        {
            m_ArrayWeeper = AddVoltorbNeighbors(MergeVoltorbWeeper(CreateVoltorbs()));
            
            m_grid = new GridWeeper(m_sizeGrid.x, m_sizeGrid.y, cellSize, textSize, m_RootWeeper, m_ArrayWeeper);
        }
        else
            Debug.LogError("--- Error: You are using more Vortorbs than existent spots ---");
    }

    public override void OnExit()
    {
        DeleteArrayVoltorbWeeper();
    }

    #endregion


    private void Update()
    {

        if(Input.GetMouseButton(0))
        {
            m_grid.SetValue(GetMouseWorldPosition(), 56);
        }

        if (Input.GetKeyDown("space"))
        {
            DeleteArrayVoltorbWeeper();

            if (m_max_voltorbs <= m_sizeGrid.x * m_sizeGrid.y)
            {
                m_ArrayWeeper = AddVoltorbNeighbors(MergeVoltorbWeeper(CreateVoltorbs()));

                m_grid = new GridWeeper(m_sizeGrid.x, m_sizeGrid.y, cellSize, textSize, m_RootWeeper, m_ArrayWeeper);
            }
            else
                Debug.LogError("--- Error: You are using more Vortorbs than existent spots ---");
        }
    }

    private int[] CreateVoltorbs()
    {
        int[] new_ArrayVoltorb = new int[m_max_voltorbs];

        for (int i = 0; i < m_max_voltorbs; i++)
            new_ArrayVoltorb[i] = UnityEngine.Random.Range(0, m_sizeGrid.x * m_sizeGrid.y);

        return CheckForDuplicatedVoltorbs(new_ArrayVoltorb);
    }

    private int[] CheckForDuplicatedVoltorbs(int[] check_ArrayVoltorb)
    {
        int[] safe_ArrayVoltorb = new int[m_max_voltorbs];

        for (int i = 0; i < m_max_voltorbs; i++)
        {
            for (int j = 0; j < safe_ArrayVoltorb.Length; j++)
            {
                if (check_ArrayVoltorb[i] == safe_ArrayVoltorb[j] && !(i == j))
                {
                    Debug.Log("Duplicated: " + check_ArrayVoltorb[i] + " " + check_ArrayVoltorb[j]);

                    check_ArrayVoltorb[i]  = UnityEngine.Random.Range(0, m_sizeGrid.x * m_sizeGrid.y);
                    j = -1;    
                }
            }
            // Array without duplicated Voltorb
            safe_ArrayVoltorb[i] = check_ArrayVoltorb[i];
        }

        return safe_ArrayVoltorb;
    }

    private Weeper[] MergeVoltorbWeeper(int[] ArrayVoltorbPosition)
    {
        Weeper [] ArrayWeeperType = new Weeper[m_sizeGrid.x * m_sizeGrid.y];

        for (int i = 0; i < ArrayVoltorbPosition.Length; i++)
        {
            ArrayWeeperType[ArrayVoltorbPosition[i]].m_is_voltorb = true;
            ArrayWeeperType[ArrayVoltorbPosition[i]].m_num = 9;
        }

        return ArrayWeeperType;
    }

    private Weeper[] CastArrayToWeep(int[] ArrayWeeperInt)
    {
        Weeper[] ArrayWeeper = new Weeper[m_sizeGrid.x * m_sizeGrid.y];
        
        for (int i = 0; i < ArrayWeeperInt.Length; i++)
        {
            ArrayWeeper[i].m_num = ArrayWeeperInt[i];
            
        }

        return ArrayWeeper;
    }

    private Weeper[] AddVoltorbNeighbors(Weeper[] ArrayWeeper)
    {
        for (int i = 0; i < ArrayWeeper.Length; i++)
        {
            if(ArrayWeeper[i].m_is_voltorb)
            {
                int check_right_column = (i + 1) % m_sizeGrid.x;
                int check_left_column = (i) % m_sizeGrid.x;

                bool right_neighbor = false;
                bool left_neighbor = false;
                bool up_neighbor = false;
                bool down_neighbor = false;

                if (check_right_column != 0 || i == 0)
                    right_neighbor = true;
                if (check_left_column != 0)
                    left_neighbor = true;
                if (i >= m_sizeGrid.x)
                    up_neighbor = true;
                if (i < ArrayWeeper.Length - m_sizeGrid.x)
                    down_neighbor = true;

                if (right_neighbor && !ArrayWeeper[i + 1].m_is_voltorb)
                    ArrayWeeper[i + 1].m_num++;
                if (right_neighbor && up_neighbor && !ArrayWeeper[i - m_sizeGrid.x + 1].m_is_voltorb)
                    ArrayWeeper[i - m_sizeGrid.x + 1].m_num++;
                if (right_neighbor && down_neighbor && !ArrayWeeper[i + m_sizeGrid.x + 1].m_is_voltorb)
                    ArrayWeeper[i + m_sizeGrid.x + 1].m_num++;

                if (left_neighbor && !ArrayWeeper[i - 1].m_is_voltorb)
                    ArrayWeeper[i - 1].m_num++;
                if (left_neighbor && up_neighbor && !ArrayWeeper[i - m_sizeGrid.x - 1].m_is_voltorb)
                    ArrayWeeper[i - m_sizeGrid.x - 1].m_num++;
                if (left_neighbor && down_neighbor && !ArrayWeeper[i + m_sizeGrid.x - 1].m_is_voltorb)
                    ArrayWeeper[i + m_sizeGrid.x - 1].m_num++;

                if (up_neighbor && !ArrayWeeper[i - m_sizeGrid.x].m_is_voltorb)
                    ArrayWeeper[i - m_sizeGrid.x].m_num++;
                if (down_neighbor && !ArrayWeeper[i + m_sizeGrid.x].m_is_voltorb)
                    ArrayWeeper[i + m_sizeGrid.x].m_num++;
            }
        }
        
        return ArrayWeeper;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }


    private void DeleteArrayVoltorbWeeper()
    {
        Array.Clear(m_ArrayWeeper, 0, m_ArrayWeeper.Length);
        m_grid.DeleteGrid();
    }
}
