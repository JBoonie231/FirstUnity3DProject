using UnityEngine;
using System.Collections;

public class fuelCellPickUpBehaviour : MonoBehaviour 
{
	public float fuelAmount;

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
		foreach(Transform childTransform in GetComponent<Transform>())
		{
			if(childTransform.gameObject.name == "fullCellMesh")
			{
				childTransform.Rotate(Vector3.up * .5f);
				break;
			}
		}

		transform.position = new Vector3(transform.position.x,originalPosition + (Mathf.Sin(Time.time) * 0.1f),transform.position.z);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{
			playerController.GainFuel(fuelAmount);
			GameObject.Destroy(gameObject);
		}
	}
}
