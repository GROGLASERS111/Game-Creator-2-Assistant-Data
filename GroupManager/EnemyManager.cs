using System.Collections.Generic;
using UnityEngine;
using GroupManager;

public class EnemyManager : MonoBehaviour
{
    public float detectionRadius = 10f;
    private List<EnemyCharacter> enemies = new List<EnemyCharacter>();
    private EnemyCharacter currentAttacker = null;

    private void Awake()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyCharacter enemyCharacter = enemy.GetComponent<EnemyCharacter>();
            if (enemyCharacter != null)
            {
                enemies.Add(enemyCharacter);
            }
        }

        ChooseNewAttacker();
    }

    private void Update()
    {
        if (currentAttacker != null && (currentAttacker.IsDead || !currentAttacker.IsAttacking))
        {
            ChooseNewAttacker();
        }

        List<EnemyCharacter> enemiesToRemove = new List<EnemyCharacter>();

        foreach (EnemyCharacter enemy in enemies)
        {
            if (enemy.IsDead)
            {
                enemiesToRemove.Add(enemy);
                continue;
            }

            if (enemy != currentAttacker)
            {
                enemy.SetMoving(true);
                enemy.SetAttacking(false);
            }
        }

        foreach (EnemyCharacter deadEnemy in enemiesToRemove)
        {
            enemies.Remove(deadEnemy);
        }
    }

    private void ChooseNewAttacker()
    {
        if (currentAttacker != null)
        {
            currentAttacker.SetAttacking(false);
        }

        if (enemies.Count == 1 && !enemies[0].IsDead)
        {
            currentAttacker = enemies[0];
            currentAttacker.SetAttacking(true);
            currentAttacker.SetMoving(false);
            return;
        }

        EnemyCharacter newAttacker = null;
        float shortestDistance = detectionRadius;

        foreach (EnemyCharacter enemy in enemies)
        {
            if (enemy == currentAttacker || enemy.IsDead)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= shortestDistance)
            {
                newAttacker = enemy;
                shortestDistance = distance;
            }
        }

        if (newAttacker != null && newAttacker != currentAttacker)
        {
            newAttacker.SetAttacking(true);
            newAttacker.SetMoving(false);
            currentAttacker = newAttacker;
        }
        else if (newAttacker == null && enemies.Count > 1)
        {
            foreach (EnemyCharacter enemy in enemies)
            {
                if (enemy != currentAttacker && !enemy.IsDead)
                {
                    newAttacker = enemy;
                    break;
                }
            }

            if (newAttacker != null)
            {
                newAttacker.SetAttacking(true);
                newAttacker.SetMoving(false);
                currentAttacker = newAttacker;
            }
        }
    }
}
