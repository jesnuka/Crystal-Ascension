using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Material bulletMaterial;
    [SerializeField] Rigidbody2D rgb;

    [Header("Bullet Values")]
    [SerializeField] Color bulletColor;
    [SerializeField] float bulletDamage;
    [SerializeField] float bulletLifetime;
    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;

    private void Awake()
    {
        bulletMaterial.SetColor("Color_C13AA74B", bulletColor);
    }

    public void ShootBullet(float damage, float lifetime, float speed, Vector2 direction, Color bColor)
    {
        bulletDamage = damage;
        bulletLifetime = lifetime;
        bulletSpeed = speed;
        bulletColor = bColor;
        bulletMaterial.SetColor("Color_C13AA74B", bulletColor);
        bulletDirection = direction;

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

        if (bulletLifetime < 0f)
            DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroyBullet();
        // TODO: Check enemy collision here first
    }
}
