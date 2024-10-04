using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public float G = 6.67430e-11f;
    GameObject[] celestialBodies;


    private void Start()
    {
        celestialBodies = GameObject.FindGameObjectsWithTag("CelestialBody");
        InitializeOrbitalVelocity();

        // Pin the Sun to the center of the universe
        Rigidbody SunRigidBody = GameObject.Find("Sun").GetComponent<Rigidbody>();
        SunRigidBody.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        foreach (var a in celestialBodies)
        {
            float m1 = a.GetComponent<Rigidbody>().mass;
            foreach (var b in celestialBodies)
            {
                if (a == b)
                    continue;

                float m2 = b.GetComponent<Rigidbody>().mass;
                float r = Vector3.Distance(a.transform.position, b.transform.position);

                a.GetComponent<Rigidbody>().AddForce(
                    (b.transform.position - a.transform.position).normalized * (G * (m1 * m2) / (r * r))
                );
            }
        }
    }

    void InitializeOrbitalVelocity()
    {
        foreach (var a in celestialBodies)
        {
            Vector3 velocity = Vector3.zero;
            foreach (var b in celestialBodies)
            {
                if (a == b)
                    continue;

                float m2 = b.GetComponent<Rigidbody>().mass;
                float r = Vector3.Distance(a.transform.position, b.transform.position);

                a.transform.LookAt(b.transform);

                velocity += a.transform.right * Mathf.Sqrt(G * m2 / r);
            }

            a.GetComponent<Rigidbody>().velocity += velocity;
        }
    }
}
