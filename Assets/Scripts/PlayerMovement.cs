using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D rb2D;
    private InputAction action;
    private Vector2 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        action = new InputAction();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CaptureInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        rb2D.MovePosition(rb2D.position + moveDirection * (speed * Time.deltaTime));
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
