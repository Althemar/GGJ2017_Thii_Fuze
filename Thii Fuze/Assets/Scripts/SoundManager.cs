using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	public PlayerController player;

	public FMODUnity.StudioEventEmitter fmodEmiter_Alarm;
	public FMODUnity.StudioEventEmitter fmodEmiter_Fuse;
	public FMODUnity.StudioEventEmitter fmodEmiter_Boom;
	public FMODUnity.StudioEventEmitter fmodEmiter_Clear;

	public FMODUnity.StudioEventEmitter fmodEmiter_Ambience;

	public void Start()
	{
		Bomb.OnPlayerInitiateBomb += PlayTrigger;
		Bomb.OnBombExplosion += PlayBoom;
		WinZone.OnPlayerEnterWinZone += PlayClear;
	}

	public void Update()
	{
		fmodEmiter_Ambience.SetParameter("Synch", player.discordAngle);

	}

	public void OnDestroy()
	{
		Bomb.OnPlayerInitiateBomb -= PlayTrigger;
		Bomb.OnBombExplosion -= PlayBoom;
		WinZone.OnPlayerEnterWinZone -= PlayClear;
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
	}

	public void PlayClear()
	{
		fmodEmiter_Clear.Play();
	}
}
