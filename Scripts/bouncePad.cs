using UnityEngine;
using System.Collections;

public class bouncePad : MonoBehaviour 
{
	public float bouncePower;

	PlayerController playerController;
	GameObject player;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
	}
	
	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject == player)
		{

			if(Input.GetButton ("Jump"))
			{
				playerController.Jump(bouncePower + playerController.jumpForce);
			}
			else
			{
				playerController.Jump(bouncePower);
			}
		}
	}
}
