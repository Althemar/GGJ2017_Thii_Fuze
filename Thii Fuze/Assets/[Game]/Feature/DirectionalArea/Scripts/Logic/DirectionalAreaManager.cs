using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAreaManager : MonoBehaviour {

    [SerializeField]
    private DirectionalAreaController[] controllerList;

    private void Start()
    {
        controllerList = GetComponentsInChildren<DirectionalAreaController>();
        StartCoroutine(WaveManager());
        InitControllers();
    }

    void InitControllers()
    {
        float direction = 1;
        foreach(var controller in controllerList)
        {
            controller.SetTargetDirection(direction);
            direction = -direction;
        }
    }


    public IEnumerator WaveManager()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 3.0f));
            StartCoroutine(Wave(controllerList, Random.Range(2f, 8f)));
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
