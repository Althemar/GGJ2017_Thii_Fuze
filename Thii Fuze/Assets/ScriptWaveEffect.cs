using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScriptWaveEffect : MonoBehaviour {

    public Material fx;

    public Transform target;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fx == null)
            return;
        fx.SetPass(0);
        fx.SetTexture("_MainTex", source);
        Graphics.Blit(destination, fx);
    }

    public void Start()
    {
        if (!Application.isPlaying)
            return;

        DeadZone.OnPlayerEnterDeadZone += ElectricBoom;
        Bomb.OnBombExplosion += BombBoom;

        //StartCoroutine(Shaker());
    }

    private void OnDestroy()
    {
        DeadZone.OnPlayerEnterDeadZone -= ElectricBoom;
        Bomb.OnBombExplosion -= BombBoom;
    }

    public void ElectricBoom()
    {
        StartCoroutine(Shake(0.75f, 0.33f));
    }
    public void BombBoom()
    {
        StartCoroutine(Shake(1.25f, 0.66f));
    }

    IEnumerator Shaker()
    {
        while (true)
        {
            StartCoroutine(Shake(0.33f, 0.33f));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Shake(0.33f, 0.20f));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Shake(0.33f, 0.20f));
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Shake(float duration, float strengh)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + duration)
        {
            yield return new WaitForEndOfFrame();
            float param = (Time.realtimeSinceStartup - startTime) / duration;
            float slider = Mathf.Sin(param * Mathf.PI * 2);
            fx.SetFloat("_EffectStrengh", slider >= 0 ? Mathf.Lerp(0f, 0.5f * strengh, slider) : Mathf.Lerp(0f, -0.075f * strengh, - slider) * (1 -param) * (1 - param));
            //fx.SetVector("_PosDevSpace", GetTargetUv());
        }
        fx.SetFloat("_EffectStrengh", 0f);
    }

    //public Vector2 GetTargetUv()
    //{
    //    Vector2 uv = Camera.main.WorldToScreenPoint(target.position);
    //    uv = new Vector2(uv.x / Screen.width, uv.y / Screen.height);
    //    Debug.Log(uv);
    //    return uv;
    //}

}
