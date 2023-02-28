using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{

    [Header("Engine")]
    public float thrust;
    public float rotationSpeed;

    [Header("Sounds")]
    public AudioClip LaunchClose;
    public AudioClip LaunchFar;
    public AudioClip Loop;

    [Header("Targeting")]
    public Vector3 target;
    public float activationRadius;

    [Header("Parachute")]
    public float parachuteArea;
    public float parachuteDragCoefficient;

    [Header("Physics")]
    public float mass;
    public float airDensity;
    public float gravity;

    [Header("Explosion")]
    public float ExplosionRadius;
    public float ExplosionForce;
    public GameObject ExplosionEffect;

    private float weight;
    private Rigidbody rb;

    private void Awake()
    {
        weight = mass * gravity;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collided(collision);
    }

    private void Update()
    {
        CheckIfInRadius();
    }

    private void FixedUpdate()
    {
        Phys();
    }

    public virtual void Phys()
    {

        Vector3 dir = target - transform.position;
        dir.Normalize();

        Vector3 rot = Vector3.Cross(dir, transform.forward);
        rb.angularVelocity = -rot * rotationSpeed;

        rb.velocity = transform.forward * thrust;
    }

    public virtual void CheckIfInRadius()
    {
        if (Vector3.Distance(target, transform.position) <= activationRadius) InRadius();
    }

    public virtual void Collided(Collision collision)
    {
        Debug.Log("Collided");
        Explode();
    }

    public virtual void Explode()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach (Collider c in colliders)
        {

            if(c.transform.tag == "Terrain")
            {
                float dist = Vector3.Distance(c.transform.position, transform.position);
                Crater.Create(transform.position, ExplosionRadius - dist, c.transform, 0.5f, 100);
            }



        }

        Destroy(gameObject);
    }

    public virtual void InRadius()
    {
        Explode();
    }

}
