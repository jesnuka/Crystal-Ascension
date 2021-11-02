using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeShooterEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        // TODO: Complete this. Results in 3D like flowing bullet patterns towards the middle of the screen
        Vector3 shootDir;
        shootDir = Input.mousePosition;
        shootDir.z = 0f;
        shootDir = Camera.main.ScreenToWorldPoint(shootDir);
        shootDir = shootDir - transform.position;

        //Vector3 cursorPos = Input.mousePosition;
        //cursorPos.z = 10f;
        // shootCooldown -= Time.deltaTime;
        //   movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0))
        {
            // Vector2 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //  Vector2 direction = cursor - new Vector2(transform.position.x, transform.position.y);
            // direction.Normalize();
            shootDir.Normalize();
            // Debug.Log("Shoot dir: " + direction);

            //  if (shootCooldown <= 0f)
            {
                //       Shoot(shootDir);
                //      shootCooldown = shootCooldownMax;
            }

        }
    }
    }
