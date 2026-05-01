using UnityEngine;

public class CoinBonus : BonusBase
{
    [SerializeField] private int coinsToAdd;

    protected override void GetBonus()
    {
        CoinManager.Instance.AddCoins(coinsToAdd);

        // play get-coin SFX
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGetCoin();
        }
    }
}
