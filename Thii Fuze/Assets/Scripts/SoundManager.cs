using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour 
{
	public PlayerController player;

	public FMODUnity.StudioEventEmitter fmodEmiter_Alarm;
	public FMODUnity.StudioEventEmitter fmodEmiter_Fuse;
	public FMODUnity.StudioEventEmitter fmodEmiter_Boom;
	public FMODUnity.StudioEventEmitter fmodEmiter_Clear;
    public FMODUnity.StudioEventEmitter fmodEmiter_Electocuted;

    public FMODUnity.StudioEventEmitter fmodEmiter_Ambience;

	public void Start()
	{
		Bomb.OnPlayerInitiateBomb += PlayTrigger;
		Bomb.OnBombExplosion += PlayBoom;
		WinZone.OnPlayerEnterWinZone += PlayClear;
        SceneManager.sceneLoaded += OnSceneWasLoaded;
        SceneManager.sceneUnloaded += OnSceneIsUnloading;
        DeadZone.OnPlayerEnterDeadZone += PlayElectrocuted;
        
        fmodEmiter_Ambience.SetParameter("Synch", 90);
    }

	public void Update()
	{
		if(player != null) fmodEmiter_Ambience.SetParameter("Synch", player.discordAngle);
	}

	public void OnDestroy()
	{
		Bomb.OnPlayerInitiateBomb -= PlayTrigger;
		Bomb.OnBombExplosion -= PlayBoom;
		WinZone.OnPlayerEnterWinZone -= PlayClear;
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
        SceneManager.sceneUnloaded -= OnSceneIsUnloading;
        DeadZone.OnPlayerEnterDeadZone -= PlayElectrocuted;
    }

    private const int PlaySceneIndex = 2;
    public void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == PlaySceneIndex)
        {
            OnPlaySceneWasLoaded();
        }
    }

    public void OnSceneIsUnloading(Scene scene)
    {

        if (scene.buildIndex == PlaySceneIndex)
        {
            OnPlayIsUnloadeding();
        }
    }

    public void OnPlaySceneWasLoaded()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPlayIsUnloadeding()
    {
        player = null;
        fmodEmiter_Ambience.SetParameter("Bomb", 0);
        fmodEmiter_Alarm.Stop();
        fmodEmiter_Fuse.Stop();
    }

    public void PlayTrigger()
	{
		fmodEmiter_Alarm.Play();
		fmodEmiter_Fuse.Play();
		fmodEmiter_Ambience.SetParameter("Bomb", 1);
	}

	public void PlayBoom()
	{
		fmodEmiter_Boom.Play();
        fmodEmiter_Fuse.Stop();
    }

    public void PlayClear()
	{
		fmodEmiter_Clear.Play();
    }

    public void PlayElectrocuted()
    {
        fmodEmiter_Electocuted.Play();
    }

}
