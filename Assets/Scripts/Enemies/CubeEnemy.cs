using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
    [Header("Movement")]
    [SerializeField] float moveTimer;
    [SerializeField] Vector2 moveTarget;
    [SerializeField] bool isMoving;
    public override void ChildUpdate()
    {
    }

    public override void EnemyDeathExtra()
    {
        // startDeathTimer = true;
        Destroy(this.gameObject);
    }

    public override void MoveEnemy()
    {
        if(isMoving)
        {
            if (Vector2.Distance(transform.position, moveTarget) <= 1f)
            {
                isMoving = false;
             //   Debug.Log("Stop moving!");
            }

           // Debug.Log("Move to target");
           // Debug.Log(moveTarget);
            transform.position = Vector2.MoveTowards(transform.position, moveTarget, speedMultiplier * Time.deltaTime);
        }
        else
        {
            // moveTarget = new Vector2(Random.Range(0, Camera.main.pixelWidth), Random.Range(0, Camera.main.pixelHeight));
            moveTarget = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
          //  Debug.Log("Move!");
            isMoving = true;
        }
    }

    public override void ShootingBehavior()
    {
        if (shootCooldown <= 0f)
        {
            Shoot(CalculateDirection());
            shootCooldown = shootCooldownMax;
        }
    }

    private Vector2 CalculateDirection()
    {
        Vector2 direction = (Vector2)player.transform.position - (Vector2)transform.position;
        direction.Normalize();
        return direction;
    }

    private void Shoot(Vector2 direction)
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletObject, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ShootBullet(bulletDamage, bulletLifetime, bulletSpeed, direction, bulletColor, bulletSizeMultiplier);
        }
    }

}
