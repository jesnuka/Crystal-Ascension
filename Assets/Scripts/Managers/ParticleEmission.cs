using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmission : MonoBehaviour
{
    [Header("Lifetime Related")]
    [SerializeField] public float lifeTime;
    [SerializeField] public int particleAmount;
    [SerializeField] ParticleSystem ps;
    [SerializeField] bool isPlayers;
    [SerializeField] public bool isOn;

    //ParticleSystem.MainModule newMain;

    private void Start()
    {
        if (!isPlayers)
            isOn = true;

        lifeTime = ps.duration;

        if (ps == null)
            ps = this.GetComponent<ParticleSystem>();

        var em = ps.emission;
        em.SetBursts
            (
            new ParticleSystem.Burst[]
                {
                    new ParticleSystem.Burst (0f, particleAmount)
                }
            );

        Debug.Log("First, particleAmount is" + particleAmount);
       // em.burstCount = particleAmount;
    }

    private void Update()
    {
        if(isOn)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0)
                Destroy(this.gameObject);
        }
    }
    public void PlayParticleBurst()
    {
        ps.Play();
        isOn = true;
    }
}
