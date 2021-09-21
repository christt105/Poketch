using UnityEngine;

public class CoinFlipper : Function
{
    [SerializeField]
    private CoinFlip m_Coin;

    #region Function

    public override void OnChange()
    {
        m_Coin.Reset();
    }

    #endregion
}
