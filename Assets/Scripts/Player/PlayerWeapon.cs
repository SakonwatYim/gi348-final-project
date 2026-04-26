using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Config")]
    //[SerializeField] private Weapon initialWeapon; สร้างตอนเริ่ม
    [SerializeField] private Transform weaponPos;

    private InputAction actions;
    private PlayerEnergy playerEnergy;
    private PlayerMovement playerMovement;
    private Weapon currentWeapon;

    private Coroutine weaponCooutine;
    private ItemText weaponNameText;

    private int weaponIndex;
    private Weapon[] equippedWeapons = new Weapon[2];

    private void Awake()
    {
        actions = new InputAction();
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
        if (playerMovement.MoveDirection != Vector2.zero)
        {
            RotateWeapon(playerMovement.MoveDirection);
        }
    }

    private void CreateWeapon(Weapon weaponPrefab)
    {
        currentWeapon = Instantiate(weaponPrefab,weaponPos.position,
            Quaternion.identity, weaponPos);
        equippedWeapons[weaponIndex] = currentWeapon;
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

    private void RotateWeapon(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (direction.x > 0f) //หันขวา
        {
            weaponPos.localScale = Vector3.one;
            currentWeapon.transform.localScale = Vector3.one;
        }
        else //หันซ้าย
        {
            weaponPos.localScale = new Vector3(-1,1,1);
            currentWeapon.transform.localScale =  new Vector3(-1,-1,1);
        }

        currentWeapon.transform.eulerAngles = new Vector3(0,0,angle);

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
