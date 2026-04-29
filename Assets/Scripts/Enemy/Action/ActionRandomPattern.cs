using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRandomPattern : FSMAction
{
    [Header("Config")]
    [SerializeField] private float projectileAmount;
    [SerializeField] private float timeBtwAttack;

    [Header("Speed")]
    [SerializeField] private float minRandomSpeed = 4f;
    [SerializeField] private float maxRandomSpeed = 8f;

    [Header("Projectile Spawn")]
    [SerializeField] private float minSpawntime = 0.1f;
    [SerializeField] private float maxSpawntime = 1.5f;

    private EnemyPattern enemyPattern;
    private float timer;

    private void Awake()
    {
        enemyPattern = GetComponent<EnemyPattern>();
    }

    private void Start()
    {
        timer = timeBtwAttack;
    }

    public override void Act()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(IERandomAttack());
            timer = timeBtwAttack;
        }
    }

    private IEnumerator IERandomAttack()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            //Random speed
            float speed = Random.Range(minRandomSpeed, maxRandomSpeed);

            //Random Direction
            Vector3 randomDir = Random.insideUnitCircle.normalized;

            //Projectile 
            Projectile projectile = enemyPattern.GetProjectile();
            projectile.Speed = speed;
            projectile.Direction = randomDir;
            yield return new WaitForSeconds(Random.Range(minSpawntime, maxSpawntime));

        }
    }
}
   

