using UnityEngine;

public class CarEntity : MonoBehaviour
{
    Boid boid;
    public Boid Boid => boid;
    
    public MeshRenderer AppearenceRenderer;

    [SerializeField]
    EStates currentState;

    public EStates CurrentState
    {
        get => currentState;

        set
        {
            currentState = value;
        }
    }

    public Color DefaultColor;

    public float CongestionSpeed;

    private void Awake()
    {
        boid = GetComponent<Boid>();

        DefaultColor = AppearenceRenderer.materials[0].color;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.transform.position + transform.forward * -1);
    }

}
