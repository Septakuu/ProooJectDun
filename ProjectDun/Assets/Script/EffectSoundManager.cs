using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
    public static EffectSoundManager Instance { get; private set; }
    [SerializeField] AudioSource sfxAudio;
    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioClip levelUp;
    [SerializeField] AudioClip skeletonDeath;
    [SerializeField] AudioClip swingWeapon;
    [SerializeField] AudioClip correctSound;
	private void Awake()
	{
        Instance = this;
	}

	public void PlayLevelUp()
	{
        sfxAudio.PlayOneShot(levelUp);
	}
    public void SkeletonDeath()
	{
        sfxAudio.PlayOneShot(skeletonDeath);
	}
    public void SwingWeapon()
	{
        sfxAudio.PlayOneShot(swingWeapon);
	}
    public void CorrectInteract()
	{
        sfxAudio.PlayOneShot(correctSound);
	}
}
