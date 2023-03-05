using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBoy : Bomb
{

    public AudioClip explodeClose;
    public AudioClip explodeFar;
    public float closeToFarDistance;
    public float farDistance;
    public AnimationCurve volume;
    public float volumeMultiplier;

    public float craterSize;
    public float craterDepth;

    public override void Explode(Collision collision)
    {
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (dist < closeToFarDistance)
        {
            AudioSource.PlayClipAtPoint(explodeClose, transform.position);
        }
        else if (dist < farDistance && dist > closeToFarDistance)
        {
            AudioSource.PlayClipAtPoint(explodeFar, transform.position);
        }

        Debug.Log(collision.transform.name);

        Crater.Create(transform.position, craterSize, collision.transform, 0.025f, craterDepth);

        Destroy(gameObject);

    }
}
