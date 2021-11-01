using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmission : MonoBehaviour
{
    [Header("Lifetime Related")]
    [SerializeField] float lifeTimeCurrent;
    [SerializeField] ParticleSystem ps;

    //ParticleSystem.MainModule newMain;

    private void Awake()
    {
        lifeTimeCurrent = ps.duration;

        if (ps == null)
            ps = this.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        lifeTimeCurrent -= Time.deltaTime;

        if (lifeTimeCurrent < 0)
            Destroy(this.gameObject);
    }
}
