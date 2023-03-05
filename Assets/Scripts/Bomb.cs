using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private Rigidbody rb;
    public bool armed;
    public float rotationSpeed;

    private bool exploded;
    private bool released;

    private Quaternion droppedRot;

    private float dropTime;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb != null && released != true && armed == true)
        {
            released = true;
            droppedRot = transform.rotation;
        }
        if (released)
        {
            FaceDir();
            dropTime += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!armed || exploded == true) return;
        exploded = true;
        Explode(collision);
    }

    public virtual void FaceDir()
    {
        Vector3 dir = transform.position - transform.position + transform.forward;
        Vector3 curDir = transform.rotation.eulerAngles.normalized;
        Vector3 toDir = ((dir + curDir) / rotationSpeed) * rotationSpeed;
        transform.LookAt(transform.position + toDir);
    }

    public virtual void Explode(Collision collision)
    {

    }

}
