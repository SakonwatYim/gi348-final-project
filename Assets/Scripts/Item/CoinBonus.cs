using UnityEngine;

public class CoinBonus : BonusBase
{
    [SerializeField] private int coinsToAdd;

    protected override void GetBonus()
    {
        CoinManager.Instance.AddCoins(coinsToAdd);
    }
}
