using System;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private List<Boid> neighbours;

    public event EventHandler<Boid> OnRemoveBoid;

    void RemoveBoid(object sender, Boid boid)
    {
        if (!neighbours.Contains(boid)) return;

        neighbours.Remove(boid);
    }

    Vector3 currentVelocity, desiredVelocity;

    [SerializeField]
    float speed;

    public float Speed { get => speed; set { speed = value; } }

    [SerializeField]
    Transform target;
    public Transform Target { get => target;  set {target = value;}}

    [SerializeField]
    BoidSettings boidSettings;

    public BoidSettings BoidSettings { get; set; }

    void Start()
    {
        neighbours = new List<Boid>();
    }

    void Update()
    {
        desiredVelocity += (target.position - transform.position).normalized * boidSettings.Target ;

        desiredVelocity += Alignment() * boidSettings.Alignment;
        desiredVelocity += Cohesion() * boidSettings.Cohesion;
        desiredVelocity += Seperation() * boidSettings.Seperation;

        desiredVelocity.Normalize();
        desiredVelocity *= speed;

        Vector3 difference = desiredVelocity - currentVelocity;
        currentVelocity += difference * Time.deltaTime;

        currentVelocity = Vector3.ClampMagnitude(currentVelocity, speed);

        currentVelocity.y = 0f;

        transform.position += currentVelocity * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward,currentVelocity,0.1f);

        desiredVelocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + currentVelocity);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position+transform.forward, transform.position + transform.forward*-1);
        Gizmos.DrawLine(transform.position + transform.right, transform.position + transform.right*-1);

    }

    void OnDestroy()
    {
        OnRemoveBoid?.Invoke(this, this);
    }

    private void OnDisable()
    {
        OnRemoveBoid?.Invoke(this, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid boid = other.GetComponent<Boid>();

        if (boid != null)
        {
            neighbours.Add(boid);
            boid.OnRemoveBoid += RemoveBoid;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid boid = other.GetComponent<Boid>();

        if (boid != null)
        {
            neighbours.Remove(boid);
            boid.OnRemoveBoid -= RemoveBoid;
        }
    }

    Vector3 Alignment()
    {
        if (neighbours.Count == 0) return Vector3.zero;

        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            alignment += neighbours[i].currentVelocity;
        }

        alignment /= neighbours.Count;

        return alignment;
    }

    Vector3 Cohesion()
    {
        if (neighbours.Count == 0) return Vector3.zero;

        Vector3 center = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            center += neighbours[i].transform.position;
        }

        center /= neighbours.Count;

        return center - transform.position;
    }

    Vector3 Seperation()
    {
        if (neighbours.Count == 0) return Vector3.zero;

        Vector3 direction = Vector3.zero;
        Vector3 difference;

        for (int i = 0; i < neighbours.Count; i++)
        {
            difference = neighbours[i].transform.position - transform.position;
            direction += difference / difference.sqrMagnitude;
        }

        return -direction;
    }

}
