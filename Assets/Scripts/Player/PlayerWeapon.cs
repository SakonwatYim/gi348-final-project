using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerWeapon : CharacterWeapon
{
    public static event Action<Weapon> OnWeaponUIUdateEvent;

    [Header("Player")]
    [SerializeField] private PlayerConfig config;
    private int weaponIndex;
    private Weapon[] equippedWeapons = new Weapon[2];

    private InputAction actions;
    private PlayerEnergy playerEnergy;
    private PlayerDetection detection;
    private PlayerMovement playerMovement;
   
    private Coroutine weaponCooutine;
    private ItemText weaponNameText;

    protected override void Awake()
    {
        base.Awake();
        actions = new InputAction();
        detection = GetComponentInChildren<PlayerDetection>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        InputSystem.actions.FindAction("Shoot").performed += ctx => ShootWeapon();
        //CreateWeapon(initialWeapon);
        InputSystem.actions.FindAction("ChangeWeapon").performed += ctx => ChangeWeapon();
    }

    private void Update()
    {
        if (currentWeapon == null) return;
        RotatePlayerWeapon();
    }

    private void CreateWeapon(Weapon weaponPrefab)
    {
        currentWeapon = Instantiate(weaponPrefab,weaponPos.position,
            Quaternion.identity, weaponPos);
        equippedWeapons[weaponIndex] = currentWeapon;
        equippedWeapons[weaponIndex].CharacterParent = this;
        ShowCurrentWeaponName();
        OnWeaponUIUdateEvent?.Invoke(currentWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (equippedWeapons[0] == null)
        {
            CreateWeapon(weapon);
            return;
        }
        if (equippedWeapons[1] == null)
        {
            weaponIndex++;
            equippedWeapons[0].gameObject.SetActive(false);
            CreateWeapon(weapon);
            return;
        }

        //Detroy current weapon
        currentWeapon.DestoryWeapon();
        equippedWeapons[weaponIndex] = null;

        //Create new weapon
        CreateWeapon(weapon);

    }

    private void ChangeWeapon()
    {
        if (equippedWeapons[1] == null) return;
        for (int i = 0; i < equippedWeapons.Length; i++) 
        {
            equippedWeapons [i].gameObject.SetActive(false);
        }

        weaponIndex = 1 - weaponIndex;
        currentWeapon = equippedWeapons[weaponIndex];
        currentWeapon.gameObject.SetActive(true);
        ResetWeaponForChange();
        ShowCurrentWeaponName();
        OnWeaponUIUdateEvent?.Invoke(currentWeapon);

        // play switch sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySwitchGun();
        }
    }

    private void RotatePlayerWeapon()
    {
        if (playerMovement.MoveDirection != Vector2.zero)
        {
            RotateWeapon(playerMovement.MoveDirection);
        }
        if (detection.EnemyTarget != null)
        {
            Vector3 dirToEnemy = detection.EnemyTarget.
                transform.position - transform.position;
            RotateWeapon(dirToEnemy);
        }
    }

    private void ShootWeapon()
    {
        if (currentWeapon == null)
        {
            return;
        }
        if (CanUseWeapon() == false)
        {
            return;
        }

        currentWeapon.UseWeapon();
        playerEnergy.UseEnergy(currentWeapon.ItemWeapon.RequiredEnergy);
    }

    public float GetDamgeUsingCriticalChance()
    {
        float damage = currentWeapon.ItemWeapon.Damage;
        float porc = Random.Range(0f, 100f);
        if (porc < config.CriticalChance)
        {
            damage = damage * config.CriticalDamage / 100f + damage;
            return damage;
        }

        return damage;
    }

    private void ShowCurrentWeaponName()
    {
        if (weaponCooutine != null)
        {
            StopCoroutine(weaponCooutine);
        }

        if (weaponNameText != null && weaponNameText.gameObject.activeInHierarchy)
        {
            Destroy(weaponNameText.gameObject);
        }

        weaponCooutine = StartCoroutine(IEShowName());
    }

    private IEnumerator IEShowName()
    {
        Vector3 texetPos = transform.position + Vector3.up;
        Color weaponNameColor = GameManager.Instance.
            GetWeaponNameColor(currentWeapon.ItemWeapon.Rarity);
        weaponNameText = ItemTextManager.Instance
            .ShowMessage(currentWeapon.ItemWeapon.ID, weaponNameColor,
            texetPos);
        weaponNameText.transform.SetParent(transform);
        yield return new WaitForSeconds(2f);
        Destroy(weaponNameText.gameObject);
    }

    private bool CanUseWeapon()
    {
        if (currentWeapon.ItemWeapon.WeaponType == WeaponType.Gun && playerEnergy.CanUseEnergy)
        {
            return true;
        }

        if (currentWeapon.ItemWeapon.WeaponType == WeaponType.Melee)
        {
            return true;
        }

        return false;
    }

    private void ResetWeaponForChange()
    {
        Transform weaponTranform = currentWeapon.transform;
        weaponTranform.rotation = Quaternion.identity;
        weaponTranform.localScale = Vector3.one;
        weaponPos.rotation = Quaternion.identity;
        weaponPos.localScale =Vector3.one;
        playerMovement.FaceRightDirection();
        
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    { 
        actions.Disable();
    }
}
