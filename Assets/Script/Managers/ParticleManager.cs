using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{ 
    public static ParticleManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public ParticleSystem SpawnParticles(ParticleSystem particles, Vector3 position, float destroyTime, bool shootP = false)
    {
        ParticleSystem currentParticles = Instantiate(particles, position, Quaternion.identity);
        if(shootP)
        {
            currentParticles.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            currentParticles.transform.localScale = new Vector3(1, 1, 1);
        }
        
        StartCoroutine(DestroyParticles(destroyTime, currentParticles));
        return currentParticles;
    }

    private IEnumerator DestroyParticles(float destroyTime, ParticleSystem particles)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(particles.gameObject);
    }
}