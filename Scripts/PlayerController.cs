using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float maxHealth;
	public float terminalSpeed;
	public float terminalVelocity;
	public float deathFallSpeed;
	public float maxFuel;
	public float fuelConsumptionRate;
	public float thrust;
	public float jumpForce;
	public float damageCooldown;
	public float gunCooldown;
	public AudioSource footStep1;
	public AudioSource footStep2;
	public GameObject FireBall;

	Vector3 movement;
	Animator anim;
	bool jumpable = true;
	bool dying;
	bool gunActive;
	float health;
	float fuel;
	float speed;
	float damageTimeStamp;
	float gunTimeStamp;
	GameObject gunTip;
	GameObject fuelGage;
	GameObject[] hearts;

	void Start () 
	{
		anim = GetComponent<Animator>();
		gunTip = GameObject.FindGameObjectWithTag("GunTip");
		fuelGage = GameObject.FindGameObjectWithTag("FuelGage");
		hearts = GameObject.FindGameObjectsWithTag("Health");
		health = maxHealth;
	}

	void FixedUpdate () 
	{
		if(rigidbody.velocity.y < deathFallSpeed && !dying)
			StartCoroutine(Death ());

		float h = Input.GetAxisRaw ("Horizontal");
		bool j = Input.GetButton ("Jump");
		bool shoot = Input.GetButton ("Fire1");

		if(dying)
		{
			h = 0;
			j = false;
			shoot = false;
		}

		if(Physics.Raycast(transform.position + new Vector3(0f,.2f,0f), transform.TransformDirection(Vector3.down), .2f) && 
		   rigidbody.velocity.y < .1f && rigidbody.velocity.y > -.1f)
		{
			jumpable = true;
		}
		else
		{
			jumpable = false;
		}

		if(gunActive && shoot && gunTimeStamp < Time.time && jumpable)
		{
			h = 0;
			shoot = false;
			gunTimeStamp = Time.time + gunCooldown;

			StartCoroutine(Shoot());
		}

		Move(h);

		if(!jumpable && j && fuel > 0f && rigidbody.velocity.y < terminalVelocity)
		{
			UseJetPack();
		}

		if(j && jumpable && rigidbody.velocity.y < .1f && rigidbody.velocity.y > -.1f)
			Jump(jumpForce);

		anim.SetFloat("verticalSpeed", rigidbody.velocity.y);
	}

	void Move(float h)
	{
		if (Mathf.Abs(speed) < terminalSpeed)
		{
			speed += h ;
		}
		if(h==0)
		{
			speed /=2f;
		}

		movement.Set (speed, 0f, 0f);
		movement = movement * Time.deltaTime;
		rigidbody.MovePosition (transform.position + movement);

		if(speed > 0)
			transform.rotation = Quaternion.Euler(0,90,0);
		else if(speed < 0)
			transform.rotation = Quaternion.Euler(0,270,0);

		anim.SetFloat("speed", Mathf.Abs (speed)/terminalSpeed);
	}

	public void Jump(float jumpForce)
	{
		rigidbody.velocity = new Vector3(0f, jumpForce, 0f);

		jumpable = false;
	}

	IEnumerator Shoot()
	{
		anim.SetTrigger("shoot");
		yield return new WaitForSeconds(.4f);
		Instantiate(FireBall, gunTip.transform.position, gunTip.transform.rotation);
	}

	void UseJetPack()
	{
		fuel -= fuelConsumptionRate;

		fuelGage.GetComponent<Slider>().value = fuel;
		rigidbody.AddForce(Vector3.up * thrust);

		foreach(ParticleSystem thruster in GetComponentsInChildren<ParticleSystem>())
		{
			if(thruster.gameObject.name == "ThrusterParticles")
				GetComponentInChildren<ParticleSystem>().Play();
		}

		if(fuel <= 0f)
		{
			foreach(Image sliderImage in fuelGage.GetComponentsInChildren<Image>())
			{
				sliderImage.enabled = false;
			}
		}
	}

	public void StepSound(int i)
	{
		if(i==1)
			footStep1.Play ();
		if(i==2)
			footStep2.Play ();
	}

	public void TakeDamage(string type, float amount)
	{
		if(damageTimeStamp < Time.time)
		{
			anim.SetTrigger(type);
			health -= amount;
			if(health < 3f)
				hearts[2].SetActive(false);
			if(health < 2f)
				hearts[1].SetActive(false);
			if(health < 1f)
				hearts[0].SetActive(false);

			damageTimeStamp = damageCooldown + Time.time;

			if (health <= 0f)
			{
				StartCoroutine(Death());
			}
		}
	}

	public void GainHealth(float amount)
	{
		health += amount;

		if(health > maxHealth)
			health = maxHealth;

		if(health == 3f)
			hearts[2].SetActive(true);
		if(health == 2f)
			hearts[1].SetActive(true);
		if(health == 1f)
			hearts[0].SetActive(true);
	}

	public void GainFuel(float amount)
	{
		fuel += amount;

		if (fuel > maxFuel)
			fuel = maxFuel;

		fuelGage.GetComponent<Slider>().value = fuel;

		if(fuel > 0f)
		{
			foreach(Image sliderImage in fuelGage.GetComponentsInChildren<Image>())
			{
				sliderImage.enabled = true;
			}
		}
	}

	public void PickUpGun()
	{
		gunActive = true;
		GameObject.FindGameObjectWithTag("GunTip").GetComponentInParent<Renderer>().enabled = true;
	}

	IEnumerator Death()
	{
		dying = true;
		float deathTimeStamp = Time.time + 1f;
		particleSystem.Play();
		while(deathTimeStamp > Time.time)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f,0.1f,0.1f), 2f * Time.deltaTime);
			yield return new WaitForEndOfFrame(); 
		}
		GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
		GameObject.FindGameObjectWithTag("GunTip").GetComponentInParent<Renderer>().enabled = false;
		yield return new WaitForSeconds(1f);
		GameObject.Destroy(gameObject);
		GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDAnimationManager>().GameOver();
	}

	void OnCollisionEnter(Collision collision)
	{
		if((collision.gameObject.name == "Bytes" || collision.gameObject.name == "Bytes(Clone)")&& 
		   collision.transform.position.y + collision.gameObject.GetComponent<SphereCollider>().radius < transform.position.y)
		{
			if(Input.GetButton ("Jump"))
			{
				Jump(collision.gameObject.GetComponent<byteController>().bouncePower + jumpForce);
			}
			else
			{
				Jump(collision.gameObject.GetComponent<byteController>().bouncePower);
			}
			collision.gameObject.GetComponent<byteController>().TakeDamage();
		}

		if((collision.gameObject.name == "FlyingBytes" || collision.gameObject.name == "FlyingBytes(Clone)")&& 
		   collision.transform.position.y + collision.gameObject.GetComponent<SphereCollider>().radius < transform.position.y)
		{
			if(Input.GetButton ("Jump"))
			{
				Jump(collision.gameObject.GetComponent<flyingByteController>().bouncePower + jumpForce);
			}
			else
			{
				Jump(collision.gameObject.GetComponent<flyingByteController>().bouncePower);
			}
			collision.gameObject.GetComponent<flyingByteController>().TakeDamage();
		}
	}
}
