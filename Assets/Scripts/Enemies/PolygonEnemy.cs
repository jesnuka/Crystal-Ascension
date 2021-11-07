using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonEnemy : Enemy
{
    [Header("Polygon")]
    [SerializeField] GameObject spinnerObject;
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
       // float spinAmount = spinnerObject.transform.rotation.z + 5f;
       // spinnerObject.transform.rotation = Quaternion.RotateTowards(spinnerObject.transform.rotation, Quaternion.Euler(0, 0, spinAmount), speedMultiplier * Time.deltaTime);
        
        if (isMoving)
        {
            if (Vector2.Distance(transform.position, moveTarget) <= 1f)
            {
                isMoving = false;
             //   Debug.Log("Stop moving!");
            }

            // Debug.Log("Move to target");
            // Debug.Log(moveTarget);

            //transform.position = Vector2.MoveTowards(transform.position, moveTarget, speedMultiplier * Time.deltaTime);
            //  Vector3 sinPos = transform.up * Mathf.Sin(Time.deltaTime * 20f) * 2f;
            //  sinPos.z = 0f;
            //  transform.position = Vector2.MoveTowards(transform.position + (sinPos), moveTarget, speedMultiplier * Time.deltaTime);
          //  Vector3 sinPos = transform.up * Mathf.Sin(Time.deltaTime * Random.Range(15f, 30f)) * Random.Range(2f, 4f);
            Vector3 sinPos = transform.up * Mathf.Sin(Time.deltaTime * 50f) * 0.5f;
            // Vector3 tanPos = transform.up * Mathf.Tan(Time.deltaTime * 15f) * 4f;
            sinPos.z = 0f;
            transform.position = Vector2.MoveTowards(transform.position + (sinPos), moveTarget, speedMultiplier * Time.deltaTime);
            //transform.position = Vector2.MoveTowards(transform.position, moveTarget, speedMultiplier * Time.deltaTime);
            // spinnerObject.transform.Rotate(Vector2.up * Time.deltaTime, Space.Self);
            spinnerObject.transform.Rotate(new Vector3(0,0,1) * Time.deltaTime * speedMultiplier * 4f, Space.Self);
          //  spinnerObject.transform.RotateAround(spinnerObject.transform.position, spinnerObject.transform.up, 45f * Time.deltaTime);
        }
        else
        {
            // moveTarget = new Vector2(Random.Range(0, Camera.main.pixelWidth), Random.Range(0, Camera.main.pixelHeight));
            moveTarget = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
            //moveTarget = PlayerController.instance.transform.position;
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
        // TODO: Instead of all 5 at once, keep increasing "angle" and shoot in a circle, spinning
        float bulletAmountInt = Mathf.Floor(bulletAmount);
        if (bulletAmountInt < 1f)
            bulletAmountInt = 1f;

        for (int i = 0; i < bulletAmountInt; i++)
        {

            float spreadAngle = Mathf.Abs(bulletSpread);

            SoundManager.instance.PlaySoundOnce("polygonShoot", Vector3.zero, this.gameObject, true);
            GameObject bullet = Instantiate(bulletObject, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().ShootBullet(bulletDamage, bulletLifetime, bulletSpeed, direction, bulletColor, bulletSizeMultiplier, lifeStealAmountTrue, spreadAngle, this);
        }
    }

}
