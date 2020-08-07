using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

	Health healthscript;
	BossMove bossmovescript;

	[SerializeField]
	int numberOfProjectiles;

	[SerializeField]
	GameObject projectile;

	Vector2 startPoint;

	float radius, moveSpeed;

	public float shootTimer;

	// Use this for initialization
	void Start()
	{
		healthscript = gameObject.GetComponent<Health>();
		bossmovescript = gameObject.GetComponent<BossMove>();

		radius = 5f;
		moveSpeed = 5f;

		numberOfProjectiles = 5;
	}

	// Update is called once per frame
	void Update()
	{
		shootTimer -= Time.deltaTime;
		if (shootTimer <= 0)
		{
			startPoint = transform.position;
			SpawnProjectiles(numberOfProjectiles);

			if (healthscript.currentLifeAmount == 4)
            {
				//reset shoot timer / projectiles
				shootTimer = Random.Range(0.5f, 2f);
				numberOfProjectiles = Random.Range(5, 9);
			}

			else if (healthscript.currentLifeAmount == 3)
			{
				//reset shoot timer / projectiles
				shootTimer = Random.Range(0.5f, 1f);
				numberOfProjectiles = Random.Range(6, 10);
			}

			else if (healthscript.currentLifeAmount == 2)
			{
				//reset shoot timer / projectiles
				shootTimer = 0.1f;
				numberOfProjectiles = 5;
			}

			else if (healthscript.currentLifeAmount == 1)
			{
				//reset shoot timer / projectiles
				shootTimer = 0f;
				numberOfProjectiles = 0;
			}

			else if (healthscript.currentLifeAmount == 0)
			{
				Destroy(gameObject);
			}
		}

	}

	void SpawnProjectiles(int numberOfProjectiles)
	{
		float angleStep = 360f / numberOfProjectiles;
		float angle = 0f;

		for (int i = 0; i <= numberOfProjectiles - 1; i++)
		{

			float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
			float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

			Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
			Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;

			var proj = Instantiate(projectile, startPoint, Quaternion.identity);
			proj.GetComponent<Rigidbody2D>().velocity =
				new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

			//Bullet Damage
			proj.GetComponent<BulletScript>().Damage = 5;

			//Shot from Boss
			proj.gameObject.GetComponent<BulletScript>().shotFromBoss = true;
			proj.gameObject.GetComponent<SpriteRenderer>().color = new Color(33, 239, 206, 255);

			angle += angleStep;
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Player")
		{
			collision.GetComponent<Health>().ChangeHealth(-5);
		}
	}

}