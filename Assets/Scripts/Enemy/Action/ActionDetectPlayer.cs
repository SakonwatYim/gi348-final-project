using UnityEngine;

public class ActionDetectPlayer : FSMAction
{
    [Header("Config")]
    [SerializeField] private float rangeDetection = 5f;
    [SerializeField] private LayerMask playerMask;

    private EnemyBrain enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBrain>();
    }

    public override void Act()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 
            rangeDetection, playerMask);
        if (collider == null)
        {
            enemy.Player = null;
            return;
        }
            enemy.Player = collider.transform;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, rangeDetection);
    }
}
