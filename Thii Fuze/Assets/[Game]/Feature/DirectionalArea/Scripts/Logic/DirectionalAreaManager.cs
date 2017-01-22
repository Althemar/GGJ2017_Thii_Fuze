using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAreaManager : MonoBehaviour {

    [SerializeField]
    private DirectionalAreaController[] controllerList;

    private void Start()
    {
        controllerList = GetComponentsInChildren<DirectionalAreaController>();
        StartCoroutine(InitControllers());
        StartCoroutine(WaveManager());

        DeadZone.OnPlayerEnterDeadZone += EndControllers;
        Bomb.OnBombExplosion += EndControllers;
    }

    private void OnDestroy()
    {
        DeadZone.OnPlayerEnterDeadZone -= EndControllers;
        Bomb.OnBombExplosion -= EndControllers;
    }

    IEnumerator InitControllers()
    {
        float direction = Mathf.Lerp(-1, 1, Mathf.Round(Random.value));
        foreach(var controller in controllerList)
        {
            yield return new WaitForSeconds(0.05f);
            controller.SetTargetDirection(direction);
            direction = -direction;
        }
    }

    void EndControllers()
    {
        foreach (var controller in controllerList)
        {
            controller.SetTargetDirection(0);
            controller.active = false;
        }
    }


    public IEnumerator WaveManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 2.0f));
            StartCoroutine(Wave(controllerList, Random.Range(4f, 8f)));
        }
    }

    public IEnumerator Wave(DirectionalAreaController[] controllerList, float speed)
    {
        int count = controllerList.Length;
        int index = count - 1;
        float progress = 0;
        bool hasReachedEnd = false;
        while (!hasReachedEnd)
        {
            yield return new WaitForFixedUpdate();
            progress += speed * Time.fixedDeltaTime;
            while(progress > 1 && index >= 0)
            {
                --progress;
                controllerList[index].SetTargetDirection(-controllerList[index].GetTargetDirection());
                --index;
            }

        }

        yield return null;
    }

}
