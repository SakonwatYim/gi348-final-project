using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Weapon initialWeapon;
    [SerializeField] private Transform weaponPos;

    private InputAction actions;
    private PlayerMovement playerMovement;
    private Weapon currentWeapon;

    private void Awake()
    {
        actions = new InputAction();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        InputSystem.actions.FindAction("Shoot").performed += ctx => ShootWeapon();
        CreateWeapon(initialWeapon);
    }

    private void Update()
    {
        if (playerMovement.MoveDirection != Vector2.zero)
        {
            RotateWeapon(playerMovement.MoveDirection);
        }
    }

    private void CreateWeapon(Weapon weaponPrefab)
    {
        currentWeapon = Instantiate(weaponPrefab,weaponPos.position,
            Quaternion.identity, weaponPos);
    }

    private void ShootWeapon()
    {
        if (currentWeapon == null)
        {
            return;
        }

        currentWeapon.UseWeapon();
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

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    { 
        actions.Disable();
    }
}
