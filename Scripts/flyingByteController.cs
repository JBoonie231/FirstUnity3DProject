using UnityEngine;
using System.Collections;

public class flyingByteController : MonoBehaviour 
{
	public float speed;
	public float bouncePower;
	public float deathFallSpeed;

	Animation anim;
	GameObject player;
	PlayerController playerController;
	bool right = true;
	bool dying;
	
	void Start () 
	{
		if(transform.eulerAngles.y == 270f)
			right = false;

		anim = GetComponent<Animation>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
	}
	
	void Update()
	{
		if(transform.eulerAngles.y < 0)
			right = false;

		if(rigidbody.velocity.y < deathFallSpeed && !dying)
			StartCoroutine(Death ());
		
		if (!anim.isPlaying)
			anim.Play ("walk");
	}
	
	void FixedUpdate () 
	{
		if(anim.IsPlaying("walk") && !dying)
		{
			if(right)
				rigidbody.velocity = new Vector3(speed, rigidbody.velocity.y, 0f);

			else
				rigidbody.velocity = new Vector3(-speed, rigidbody.velocity.y, 0f);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player && !dying)
			BitePlayer ();
		else
		{
			TakeDamage();
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == player && !anim.IsPlaying("byte") && !dying)
			BitePlayer ();
	}
	
	void BitePlayer()
	{
		anim.Play ("byte");
		playerController.TakeDamage("bit", 1f);
	}
	
	public void TakeDamage()
	{
		StartCoroutine(Death());
	}
	
	IEnumerator Death()
	{
		dying = true;
		float deathTimeStamp = Time.time + 1f;
		if(!particleSystem.isPlaying)
			particleSystem.Play();
		while(deathTimeStamp > Time.time)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f,0.1f,0.1f), 2f * Time.deltaTime);
			yield return new WaitForEndOfFrame(); 
		}
		GameObject.Destroy(gameObject);
	}
}
