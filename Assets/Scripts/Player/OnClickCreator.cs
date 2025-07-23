using System;
using System.Collections;
using UnityEngine;

public class OnClickCreator : MonoBehaviour
{
    Camera cam;

    [SerializeField]
    LayerMask BuildableZone;

    [SerializeField]
    PointsKeeper pointsKeeper;

    [SerializeField]
    PlayerOptionSettings toCreateOnClick;
    public PlayerOptionSettings ToCreateOnClick {

        get => toCreateOnClick;

        set
        {
            toCreateOnClick = value;
        }
    }

    GameObject preview;
    public GameObject Preview { get => preview; set { preview = value; } }

    bool isInCoroutine;

    [SerializeField]
    float alpha;

    [SerializeField]
    PlayerInputEventsHandler PlayerInput;

    private IClickable currentHovering;
    private RaycastHit currentHit;

    public static event Action<string> OnError;

    [SerializeField]
    bool hasDebugPointsGeneration = false;


    public static Action<int> OnSpendPoints;


    private void OnEnable()
    {
        PlayerInput.OnRightClick += RightClickLogic;
        PlayerInput.OnLeftClick += LeftClickLogic;
    }

    private void OnDisable()
    {
        PlayerInput.OnRightClick -= RightClickLogic;
        PlayerInput.OnLeftClick -= LeftClickLogic;
    }

    private void Awake()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && !isInCoroutine)
        {
            currentHit = hit;

            if (hit.collider.gameObject.TryGetComponent<IClickable>(out IClickable clickable))
            {
                RepresentNextAt(transform.up * -100);

                if (currentHovering != null && currentHovering != clickable) 
                {
                    currentHovering.Unhover();
                }

                currentHovering = clickable;

                currentHovering.Hover(pointsKeeper.DecreaseCheck(clickable.Cost) ? UnityEngine.Color.green : UnityEngine.Color.red);
            }
            else if ((BuildableZone & (1 << hit.collider.gameObject.layer)) != 0)
            {
                RepresentNextAt(currentHit.point);

                ChangePreviewColor(pointsKeeper.DecreaseCheck(toCreateOnClick.Cost) ? UnityEngine.Color.white : UnityEngine.Color.red);

                if (currentHovering != null)
                {
                    currentHovering.Unhover();
                    currentHovering = null;
                }
            }
            else
            {
                RepresentNextAt(transform.up * -100); //Exile
                if (currentHovering != null)
                {
                    currentHovering.Unhover();
                    currentHovering = null;
                }
            }

        }

        //Debug Option
        if (!pointsKeeper.DecreaseCheck(500) && hasDebugPointsGeneration)
            EmergencyPointsGeneration();
    }




    void RightClickLogic()
    {
        if (currentHit.collider == null)
            return;

        if (Time.timeScale == 0)
            return;

        GameObject clickedObject = currentHit.collider.gameObject;

        if (clickedObject.TryGetComponent<IClickable>(out IClickable clickable))
        {
            StartCoroutine(MakeInvisibleForTime(preview.GetComponentInChildren<Renderer>(), 0.3f));
            clickable.RightClick();
        }
    }

    void LeftClickLogic()
    {
        if (currentHit.collider == null)
            return;

        if (Time.timeScale == 0) 
            return;

        GameObject clickedObject = currentHit.collider.gameObject;


        if (clickedObject.TryGetComponent<IClickable>(out IClickable click))
        {
            if (pointsKeeper.DecreaseCheck(click.Cost))
            {
                StartCoroutine(MakeInvisibleForTime(preview.GetComponentInChildren<Renderer>(), 0.1f));
                clickedObject.GetComponent<IClickable>().LeftClick();

                //OnSpendPoints?.Invoke(click.Cost);
                pointsKeeper.Decrease(click.Cost);
            }
            else
            {
                OnError("Not enough Points for action");
            }
        }
        else if ((BuildableZone & (1 << currentHit.collider.gameObject.layer)) != 0)
        {
            if (pointsKeeper.DecreaseCheck(toCreateOnClick.Cost))
            {
                OnSpendPoints?.Invoke(toCreateOnClick.Cost);
                pointsKeeper.Decrease(toCreateOnClick.Cost);

                StartCoroutine(MakeInvisibleForTime(preview.GetComponentInChildren<Renderer>(), 0.1f));
                GameObject.Instantiate(toCreateOnClick.RealObject, currentHit.point, toCreateOnClick.PreviewObject.transform.rotation);
            }
            else
            {
                OnError("Not enough Points to build this :(");
            }
        }

    }

    void ChangePreviewColor(Color newColor)
    {
        if (preview == null) return;

        if (preview.GetComponentInChildren<Renderer>().material.color != newColor)
            preview.GetComponentInChildren<Renderer>().material.color = ChangeColorWithoutAlpha(preview.GetComponentInChildren<Renderer>(), newColor);
    }

    private void EmergencyPointsGeneration(int min = 500)
    {
        if (pointsKeeper.Points > min) return;
        
        pointsKeeper.Add(1);
        
    }

    private void RepresentNextAt(Vector3 targetPosition)
    {
        if (preview!=null && preview.name != toCreateOnClick.PreviewObject.name)
        {
            Destroy(preview);
            preview = Instantiate(toCreateOnClick.PreviewObject, toCreateOnClick.PreviewObject.transform.position, toCreateOnClick.PreviewObject.transform.rotation);
        }


        if (preview == null)
        {
            preview = Instantiate(toCreateOnClick.PreviewObject, toCreateOnClick.PreviewObject.transform.position, toCreateOnClick.PreviewObject.transform.rotation);
        }
        else
        {
            preview.transform.position = targetPosition;
        }
    }

    IEnumerator ColorTextTime(Renderer renderer, float timeDuration, UnityEngine.Color color)
    {
        isInCoroutine = true;

        UnityEngine.Color rememberColor = renderer.material.color;

        renderer.material.color = ChangeColorWithoutAlpha(renderer, color);

        while (timeDuration > 0)
        {
            timeDuration-= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        renderer.material.color = ChangeColorWithoutAlpha(renderer, rememberColor);

        isInCoroutine = false;
    }

    IEnumerator MakeInvisibleForTime(Renderer renderer, float timeDuration)
    {
        isInCoroutine = true;

        renderer.enabled = false;

        while (timeDuration > 0)
        {
            timeDuration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (renderer != null)
            renderer.enabled = true;

        isInCoroutine = false;
    }

    private Color ChangeColorWithoutAlpha(Renderer renderer, Color newColor)
    {
        float currentAlpha = renderer.material.color.a;

        return new Color(newColor.r, newColor.g, newColor.b, currentAlpha);
    }
}
