using System.Collections;
using Unity.XR.GoogleVr;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speed;
    
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float transperency = 0.3f;

    public Vector2 MoveDirection => moveDirection;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2D;
    private InputAction action;
    
    private Vector2 moveDirection;
    private float currentSpeed;
    private bool usingDash;
    

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        action = InputSystem.actions.FindAction("Move");
        
    }
    
    void Start()
    {
        currentSpeed = speed;
        InputSystem.actions.FindAction("Dash").performed += context => Dash();
    }

    // Update is called once per frame
    void Update()
    {
        CaptureInput();
        RoratePlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb2D.MovePosition(rb2D.position + moveDirection * (currentSpeed * Time.deltaTime));
        
    }

    private void Dash()
    {
        if (usingDash)
        {
            return; 
        }
        
        usingDash = true;
        StartCoroutine(routine:IEDash());
    }

    private IEnumerator IEDash()
    {
        currentSpeed = dashSpeed;
        ModifySpriteRendrer(transperency);
        yield return new WaitForSeconds(dashTime);
        currentSpeed = speed;
        ModifySpriteRendrer(alpha:1f);
        usingDash = false;
    }

    private void RoratePlayer()
    {
        if (moveDirection.x >= 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x <= 0f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void ModifySpriteRendrer(float alpha) 
    {
        Color color = spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, alpha);
        spriteRenderer.color = color;
    }

    public void FaceRightDirection()
    {
        spriteRenderer.flipX = false;
    }

    private void CaptureInput()
    {
        moveDirection = action.ReadValue<Vector2>().normalized;
    }
    
    

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
         action.Disable();
    }
}
