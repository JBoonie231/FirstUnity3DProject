using UnityEngine;
using System.Collections;

public class gunPickUpBehaviour : MonoBehaviour 
{
	PlayerController playerController;
	GameObject player;
	float originalPosition;
	
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
		originalPosition = transform.position.y;
	}
	
	void Update()
	{
		transform.Rotate(Vector3.up * .5f);
		transform.position = new Vector3(transform.position.x,originalPosition + (Mathf.Sin(Time.time) * 0.1f),transform.position.z);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			playerController.PickUpGun();
			GameObject.Destroy(gameObject);
		}
	}
}
