using UnityEngine;

public class CoinManager : PersistentSingleleton<CoinManager>
{
    [Header("Config")]
    [SerializeField] private int initialCoinsTest;
    public int Coins {  get; private set; }
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        Coins = PlayerPrefs.GetInt(COIN_KEY, initialCoinsTest);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetFloat(COIN_KEY, Coins);
        PlayerPrefs.Save();
    }
    public void RemoveCoins(int amount) 
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            PlayerPrefs.SetFloat(COIN_KEY, Coins);
            PlayerPrefs.Save();
        }
    }
}
