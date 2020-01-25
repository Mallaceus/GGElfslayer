using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[SerializeField] public float detectionRaidus = 10;
	[SerializeField] public float speed = 3;
	[SerializeField] public float attackRange = 2;
	[SerializeField] public float damage = 10;
	[SerializeField] public float chargeTime = 3;

	[HideInInspector] public GameObject player;
	[HideInInspector] public Animator animator;

	EnemyState state;

    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponentInChildren<Animator>();
		SwitchState(new Idle());
    }

    void Update()
    {
		state.OnUpdate();
    }

	public void SwitchState(EnemyState newState)
	{
		state = newState;
		state.OnStart(this);
	}
}

public interface EnemyState
{
	void OnStart(EnemyAI enemyAI);
	void OnUpdate();
}

class Idle : EnemyState
{
	EnemyAI enemyAI;

	public void OnStart(EnemyAI enemyAI)
	{
		this.enemyAI = enemyAI;
	}

	public void OnUpdate()
	{
		if (Vector2.Distance(enemyAI.player.transform.position, enemyAI.transform.position) < enemyAI.detectionRaidus)
		{
			enemyAI.SwitchState(new Tracking());
		}
	}
}

class Tracking : EnemyState
{
	EnemyAI enemyAI;

	public void OnStart(EnemyAI enemyAI)
	{
		this.enemyAI = enemyAI;
	}

	public void OnUpdate()
	{
		enemyAI.transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Sign(enemyAI.transform.position.x - enemyAI.player.transform.position.x) == 1 ? 0 : 180, 0));
		enemyAI.transform.position -= enemyAI.transform.right * enemyAI.speed * Time.deltaTime;

		float distance = Vector2.Distance(enemyAI.player.transform.position, enemyAI.transform.position);

		if (distance < enemyAI.attackRange)
		{
			enemyAI.SwitchState(new Attacking());
		}
		else if (distance > enemyAI.detectionRaidus)
		{
			enemyAI.SwitchState(new Idle());
		}
	}
}

class Attacking : EnemyState
{
	EnemyAI enemyAI;

	float elapsedTime;

	public void OnStart(EnemyAI enemyAI)
	{
		this.enemyAI = enemyAI;
	}

	public void OnUpdate()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > enemyAI.chargeTime)
		{
			if(enemyAI.animator != null)
				enemyAI.animator.SetTrigger("Attack");

			enemyAI.player.GetComponent<Destructible>().Hurt(enemyAI.damage);

			float distance = Vector2.Distance(enemyAI.player.transform.position, enemyAI.transform.position);
			if (distance > enemyAI.attackRange)
				enemyAI.SwitchState(new Tracking());
			else
				elapsedTime = 0;
		}
	}
}