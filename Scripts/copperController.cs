using UnityEngine;
using System.Collections;

public class copperController : MonoBehaviour 
{
	public float speed;
	public float health;
	public float deathFallSpeed;
	public float hitCooldown;
	
	Animation anim;
	GameObject player;
	PlayerController playerController;
	float hitTimeStamp;
	bool right = true;
	bool dying;
	bool attack;
	bool inRange;
	
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animation>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerController>();
	}
	
	void Update()
	{
		if(6.8f > Vector3.Distance(transform.position, player.transform.position))
		{
			inRange = true;

			if(5.5f > Vector3.Distance(transform.position, player.transform.position))
				attack = true;
			
			else
				attack = false;
		}
		else
			inRange = false;

		if(rigidbody.velocity.y < deathFallSpeed && !dying)
			StartCoroutine(Death ());
		
		if (!inRange && !anim.IsPlaying("idle") && !anim.IsPlaying("gothit"))
			anim.Play ("idle");

		else if(attack && !anim.IsPlaying("attackrun") && !anim.IsPlaying("gothit"))
			anim.Play ("attackrun");

		else if(inRange && !attack && !anim.IsPlaying("threaten") && !anim.IsPlaying("gothit"))
			anim.Play ("threaten");

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(player.transform.position.x > transform.position.x)
		{
			transform.eulerAngles = new Vector3(0f, 90f, 0f);
			right = true;
		}
		else if(player.transform.position.x < transform.position.x)
		{
			transform.eulerAngles = new Vector3(0f, -90f, 0f);
			right = false;
		}

		if (!attack || dying || anim.IsPlaying("gothit"))
		{
			rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f);
		}
		
		else
		{
			if(right)
				rigidbody.velocity = new Vector3(speed, rigidbody.velocity.y, 0f);
			else
				rigidbody.velocity = new Vector3(-speed, rigidbody.velocity.y, 0f);
		}
	}

	public void TakeDamage()
	{
		anim.Play ("gothit");
		health -= 1f;
		
		if (health <= 0f)
			StartCoroutine(Death());
	}

	void HitPlayer()
	{
		hitTimeStamp = hitCooldown + Time.time;

		playerController.TakeDamage("hit", 2f);
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

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject == player && !dying)
			HitPlayer ();
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.gameObject == player && hitTimeStamp < Time.time && !dying)
			HitPlayer ();
	}
}
