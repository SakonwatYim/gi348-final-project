using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Config")]
    [SerializeField] private PlayerCreation[] players;

    [Header("UI")]
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private Image playerIcon;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private TextMeshProUGUI playerHealthMaxStat;
    [SerializeField] private TextMeshProUGUI playerArmorMaxStat;
    [SerializeField] private TextMeshProUGUI playerEnergyMaxStat;
    [SerializeField] private TextMeshProUGUI playerCriticalMaxStat;
    [SerializeField] private TextMeshProUGUI coinsTMP;
    [SerializeField] private TextMeshProUGUI playerUpgradeCostTMP;
    [SerializeField] private TextMeshProUGUI playerUnlockCostTMP;

    [Header("Bars")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image amrorBar;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image criticalBar;

    [Header("Button")]
    [SerializeField] private GameObject unlockButton;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject selectButton;

    private SelectablePlayer currentPlayer;
    private void Start()
    {
        CreatePlayer();
    }


    private void Update()
    {
        coinsTMP.text = CoinManager.Instance.Coins.ToString();
    }

    private void CreatePlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            PlayerMovement player = Instantiate(players[i].player, players[i].CreationPos.position,
                Quaternion.identity, players[i].CreationPos);
            player.enabled = false;
        }
    }

    public void ClickPlayer(SelectablePlayer selectablePlayer)
    {
        currentPlayer = selectablePlayer;
        VerifiyPlayer();
        ShowPlayerStats();
    }

    public void SelectPlayer()
    {
        if (currentPlayer.Config.Unlocked)
        {
            currentPlayer.GetComponent<PlayerMovement>().enabled = true;
            currentPlayer.Config.ResetPlayerStats();
            ClosePlayerPanel();
        }

    }

    public void UpgradePlayer()
    {
        if (CoinManager.Instance.Coins >= currentPlayer.Config.UngradeCost)
        {
            CoinManager.Instance.RemoveCoins(currentPlayer.Config.UngradeCost);
            UpgradePlayerStat();
            ShowPlayerStats();

        }
    }

    private void UpgradePlayerStat()
    {
        PlayerConfig config = currentPlayer.Config;
        config.Level++;
        config.MaxHealth++;
        config.MaxArmor++;
        config.MaxEnergy += 10f;
        config.CriticalChance += 2f;
        config.CriticalDamage += 5f;

        int upgrade = config.UngradeCost;
        config.UngradeCost = upgrade + (upgrade * (config.UdgradeMultiplier / 100));
    }

    private void ShowPlayerStats()
    {
        playerPanel.SetActive(true);
        playerIcon.sprite = currentPlayer.Config.Icon;
        playerName.text = currentPlayer.Config.Name;
        playerLevel.text = $"Level {currentPlayer.Config.Level}";
        playerHealthMaxStat.text = currentPlayer.Config.MaxHealth.ToString();
        playerArmorMaxStat.text = currentPlayer.Config.MaxArmor.ToString();
        playerEnergyMaxStat.text = currentPlayer.Config.MaxEnergy.ToString();
        playerCriticalMaxStat.text = currentPlayer.Config.CriticalChance.ToString();

        playerUnlockCostTMP.text = currentPlayer.Config.UnlockCost.ToString();
        playerUpgradeCostTMP.text = currentPlayer.Config.UngradeCost.ToString();
    }

    private void VerifiyPlayer()
    {
        if (currentPlayer.Config.Unlocked == false)
        {
            upgradeButton.SetActive(false);
            selectButton.SetActive(false);
            unlockButton.SetActive(true);
        }
        else
        {
            upgradeButton.SetActive(true);
            selectButton.SetActive(true);
            unlockButton.SetActive(false);
        }
    }

    public void ClosePlayerPanel()
    {
        playerPanel.SetActive(false);
    }


    [Serializable]
    public class PlayerCreation
    {
        public PlayerMovement player;
        public Transform CreationPos;

    }
}
