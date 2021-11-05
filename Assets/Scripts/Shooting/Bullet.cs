using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Material bulletMaterial;
    [SerializeField] Rigidbody2D rgb;
    [SerializeField] GameObject bulletDeathParticle;

    [Header("Bullet Values")]
    [Tooltip("If true, bullet is dead")]
    [SerializeField] bool isGone;
    [SerializeField] Color bulletColor;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletLifetime;
    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;

    [SerializeField] bool isEnemyBullet;
    [SerializeField] Enemy parentEnemy;
    [SerializeField] float lifeStealAmount;

    private void Awake()
    {
        bulletMaterial.SetColor("Color_C13AA74B", bulletColor);

        if(isEnemyBullet) // Ignore collision with enemy bullet and enemy layers
            Physics2D.IgnoreLayerCollision(11,12, true);
        else // Ignore collision with player bullet and player layers
            Physics2D.IgnoreLayerCollision(10, 8, true);

        // Ignore collision between other bullets
        Physics2D.IgnoreLayerCollision(11, 11, true);
        Physics2D.IgnoreLayerCollision(10, 10, true);
        Physics2D.IgnoreLayerCollision(11, 10, true);
        Physics2D.IgnoreLayerCollision(10, 11, true);
    }

    public void ShootBullet(float damage, float lifetime, float speed, Vector2 direction, Color bColor, float sizeMultiplier, float lifeSteal, Enemy parent = default(Enemy))
    {
        if (isEnemyBullet && parent != null)
            parentEnemy = parent;

        bulletDamage = damage;
        bulletLifetime = lifetime;
        bulletSpeed = speed;
        bulletColor = bColor;
        bulletMaterial.SetColor("Color_C13AA74B", bulletColor);
        bulletDirection = direction;
        this.transform.localScale = transform.localScale * sizeMultiplier;

        if (lifeSteal > 0)
            lifeStealAmount = lifeSteal;

        var lookDir = direction - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + 90f));
       // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), 10f * Time.deltaTime);
        rgb.velocity = Vector3.forward * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bulletLifetime -= Time.deltaTime;
        rgb.velocity = bulletDirection * bulletSpeed;

        if (!isGone && bulletLifetime < 0f)
            DestroyBullet();
    }

    /*IEnumerator DestroyBullet()
    {
        isGone = true;
        Instantiate(bulletDeathParticle, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }*/

    private void DestroyBullet()
    {
        isGone = true;
        Instantiate(bulletDeathParticle, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyBullet)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(bulletDamage);
                parentEnemy.healEnemy(lifeStealAmount * bulletDamage);
            }
            if(!player.isDead && !isGone)
                DestroyBullet();
        }
        else
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
                PlayerController.instance.healPlayer(lifeStealAmount * bulletDamage);
            }
            if(!isGone)
                DestroyBullet();
        }


    }
}
