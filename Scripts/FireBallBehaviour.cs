using UnityEngine;
using System.Collections;

public class FireBallBehaviour : MonoBehaviour 
{
	public float speed;
	public float rotationSpeed;

	GameObject player;
	GameObject[] enemies;
	Transform fireBallAura;
	// Use this for initialization
	void Awake () 
	{
		fireBallAura = GetComponentInChildren<Transform>();
		player = GameObject.FindGameObjectWithTag("Player");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		rigidbody.velocity = transform.forward * speed;

	}
	
	// Update is called once per frame
	void Update () 
	{
		fireBallAura.Rotate(new Vector3(45f,90f,60f) * rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other)
	{
		foreach(GameObject enemy in enemies)
		{
			if(other.gameObject == enemy)
			{
				if(enemy.GetComponent<byteController>() != null)
					enemy.GetComponent<byteController>().TakeDamage();

				if(enemy.GetComponent<copperController>() != null)
					enemy.GetComponent<copperController>().TakeDamage();
			}
		}
		if(other.gameObject != player)
			GameObject.Destroy(gameObject);
	}
}
