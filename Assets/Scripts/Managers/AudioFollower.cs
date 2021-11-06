using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFollower : MonoBehaviour
{
    public float lifeTime;
    public float lifeTimeCounter;

    public bool isDone;
    public bool isLooping;
    void Update()
    {
        lifeTimeCounter -= Time.deltaTime;

        if (this.transform.parent != null)
        {
            this.transform.position = this.transform.parent.position;
        }

        if(lifeTimeCounter < 0)
        {
            //  Destroy(this.gameObject);
            isDone = true;
            this.GetComponent<AudioSource>().Stop();
            this.GetComponent<AudioSource>().enabled = false;
            Destroy(this.gameObject);
        }
    }
}
