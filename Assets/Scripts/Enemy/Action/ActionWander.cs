using UnityEngine;

public class ActionWander : FSMAction
{
    [Header("Config")]
    [SerializeField] private bool useDebug;
    [SerializeField] private bool useRamdomMovement;
    [SerializeField ] private bool useTileMovement;

    [SerializeField] private float wanderSpeed;
    [SerializeField] private Vector2 moveRange;
    [SerializeField] private float minDistanceCheck = 0.5f;

    [Header("obstacles")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float rangeDetection;

    private EnemyBrain enemy;   
    private Vector3 movePosition;
    private Vector3 moveDirection;

    private void Awake()
    {
        enemy = GetComponent<EnemyBrain>();
    }

    private void Start()
    {
        GetNewMovePosition();  

    }

    public override void Act()
    {
        moveDirection = (movePosition - transform.position).normalized;
        transform.Translate(moveDirection * (wanderSpeed * Time.deltaTime));
        if (CanGetNewPosition())
        {
            GetNewMovePosition();
        }
    }

    private void GetNewMovePosition()
    {
        if (useRamdomMovement)
        {
            movePosition = transform.position + GetRamdomDirection();
        }
        if (useTileMovement)
        {
            movePosition = GetTilePos();
        }
    }

    private bool CanGetNewPosition()
    {
        if (Vector3.Distance(transform.position, movePosition) < minDistanceCheck)
        {
            return true;
        }

        Collider2D collider = Physics2D.OverlapCircle(transform.position,
            rangeDetection, obstacleMask);
        if (collider != null)
        {
            Vector3 oppesiteDirection = -moveDirection;
            transform.position += oppesiteDirection * 0.1f;
            return true;
        }
        return false;
    }

    private Vector3 GetTilePos()
    {
        return enemy.RoomParent.GetAvailableTilePos();
    }

    private Vector3 GetRamdomDirection()
    {
        float randomX = Random.Range(-moveRange.x, moveRange.x);
        float randomY = Random.Range(-moveRange.y, moveRange.y);
        return new Vector3(randomX, randomY);
    }

    private void OnDrawGizmos()
    {
        if (useDebug == false) return;
        if (useRamdomMovement)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, moveRange * 2f);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, movePosition);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeDetection);
    }
}
