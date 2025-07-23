using System.Collections;
using UnityEngine;

public class TimeSelfDestructor : MonoBehaviour
{
    [SerializeField]
    int timeTillDestroy;
    public float TimeTillDestroy {get => timeTillDestroy; }


    [SerializeField]
    ParticleSystem positiveEffectParticleSystem;


    Renderer apperanceRenderer;

    private void OnEnable()
    {
        HighScoreManager.OnNewMilestoneInHighscore += AddTime;
    }

    private void OnDisable()
    {
        HighScoreManager.OnNewMilestoneInHighscore -= AddTime;
    }

    private void Start()
    {
        StartCoroutine("CountDown");

        positiveEffectParticleSystem.Play();

        apperanceRenderer = GetComponentInChildren<Renderer>();
    }

    IEnumerator CountDown()
    {
        while(timeTillDestroy > 0f)
        {
            timeTillDestroy--;
            yield return new WaitForSeconds(1f);
        }

        Object.Destroy(this.gameObject);
    }

    public void AddTime(int add)
    {
        positiveEffectParticleSystem.Play();

        timeTillDestroy += add;
    }

    public void DestroyNow()
    {
        timeTillDestroy = 0;
    }

    public void ChangeAppearence(Color newColor)
    {
        apperanceRenderer.material.color = newColor;
    }

    public void DechangeAppearence()
    {
        if (apperanceRenderer == null) return;

        apperanceRenderer.material.color = Color.white;
    }   

}
