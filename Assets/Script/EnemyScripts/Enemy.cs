using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 100;
    public GameObject moneyObject;
    [SerializeField]
    public ParticleSystem particles;

    private ParticleSystem instantiatedParticles;

    private void Update()
    {
        IsDead();
    }

    private void IsDead()
    {
        if (hp <= 0)
        {
            AddMoney();
            Destroy(moneyObject.gameObject);
            Destroy(instantiatedParticles.gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        TriggerParticles();
        if (hp <= 0f)
        {
            IsDead();
        }
    }

    private void AddMoney()
    {
        moneyObject = Instantiate(moneyObject, transform.position, Quaternion.identity);
        Money moneyScript = moneyObject.GetComponent<Money>();
        moneyScript.moneyData.amount = moneyScript.AmountProbability();
        PlayerCurrency.Instance.AddCurrency(moneyScript.moneyData.amount);
    }

    public void TriggerParticles()
    {
        if (particles != null)
        {
            instantiatedParticles = Instantiate(particles, transform.position, Quaternion.identity);
            instantiatedParticles.gameObject.SetActive(true);
            instantiatedParticles.Play();

            StartCoroutine(DestroyParticles(instantiatedParticles, 0.4f));
        }
    }

    private IEnumerator DestroyParticles(ParticleSystem instantiatedParticles, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(instantiatedParticles.gameObject);
    }
}
