using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    protected bool destructible = true;

    [SerializeField]
    protected bool destroyOnDeath = true;

    [SerializeField]
    protected float maxHealth = 100f;
    public float MaxHealth { get { return maxHealth; } }

    [SerializeField]
    protected float health = 100f;
    public float Health { get { return health; } }

    [SerializeField] protected float resistance;
    public float Resistance { get { return resistance; } }

    [SerializeField] protected float regeneration;
    public float Regeneration { get { return regeneration; } }

    [SerializeField] protected float regenerationInterval = 5;
    float regenerationTimer = 0;


    [Space]
    [SerializeField] GameObject hitEffect;

    public UnityEvent OnHurt;
    public UnityEvent OnDeath;
    protected bool isAlive = true;

    private void OnValidate()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }

        if (maxHealth < 0)
        {
            maxHealth = 0;
        }
    }

    public void Hurt(float damage, float penetration = 0, Vector3 hitPosition = new Vector3())
    {
        if (isAlive == false)
        {
            return;
        }

        float calcResistance = (resistance - penetration) / 100; //10 resistance = 10% damage reduction.
        if (calcResistance < 0)
        {
            calcResistance = 0;
        }

        damage *= (1 - calcResistance);
        damage = Mathf.Max(1, damage);

        if (destructible)
        {
            if (hitEffect != null)
            {
                hitEffect.transform.position = hitPosition;
            }
            OnHurt.Invoke();

            if (health - damage > 0)
            {
                health -= damage;
            }
            else
            {
                health = 0;
                Die();
            }
        }
    }

    public void Heal(float value)
    {
        if (health + value < maxHealth)
        {
            health += value;
        }
        else
        {
            health = maxHealth;
        }
    }

    public void AddMaxHealth(float value)
    {
        maxHealth = Mathf.Max(1, maxHealth + value);
    }

    public void AddRegeneration(float value)
    {
        regeneration = Mathf.Max(0, regeneration + value);
    }

    public void AddResistance(float value)
    {
        resistance = Mathf.Max(0, resistance + (value * Mathf.Max(0, (1 - (resistance / 100)))));
    }

    private void Update()
    {
        UpdateRegeneration();
    }

    private void UpdateRegeneration()
    {
        regenerationTimer += Time.deltaTime;

        if (regenerationTimer >= regenerationInterval)
        {
            Heal(regeneration);
            regenerationTimer = 0;
        }
    }

    protected virtual void Die()
    {
        OnDeath.Invoke();

        isAlive = false;

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }
}
