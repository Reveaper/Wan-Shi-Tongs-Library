using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FootStepHandler : MonoBehaviour
{
    public List<AudioSource> FootStepSoundsConcrete;
    public List<AudioSource> FootStepSoundsSand;
    public List<AudioSource> FootStepSoundsWater;

    private bool _isInWater;

    private LayerMask _concreteMask;
    private LayerMask _sandMask;
    private LayerMask _waterMask;
    private LayerMask _propMask;
    private LayerMask _bookCaseMask;

    private void Start()
    {
        _concreteMask = LayerMask.NameToLayer("Concrete");
        _sandMask = LayerMask.NameToLayer("Sand");
        _waterMask = LayerMask.NameToLayer("Water");
        _propMask = LayerMask.NameToLayer("Prop");
        _bookCaseMask = LayerMask.NameToLayer("BookCase");
    }

    private void OnTriggerEnter(Collider other)
    {
        var footHitObjectMask = other.gameObject.layer;

        if (footHitObjectMask == _waterMask)
            _isInWater = true;


        if(_isInWater)
        {
            if (footHitObjectMask == _sandMask)
                PlayRandomSound(ref FootStepSoundsWater, 0.13f, 0.2f);
        }
        else
        {
            if (footHitObjectMask == _concreteMask)
                PlayRandomSound(ref FootStepSoundsConcrete, 0.22f, 0.34f);
            else if (footHitObjectMask == _propMask)
                PlayRandomSound(ref FootStepSoundsConcrete, 0.22f, 0.34f);
            else if (footHitObjectMask == _bookCaseMask)
                PlayRandomSound(ref FootStepSoundsConcrete, 0.22f, 0.34f);
            else if (footHitObjectMask == _sandMask)
                PlayRandomSound(ref FootStepSoundsSand, 0.13f, 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == _waterMask)
            _isInWater = false;
    }

    private void PlayRandomSound(ref List<AudioSource> soundList, float minVolume, float maxVolume)
    {
        if(soundList.Count > 0)
        {
            int randomSoundIndex = UnityEngine.Random.Range(0, soundList.Count);
            soundList[randomSoundIndex].volume = UnityEngine.Random.Range(minVolume, maxVolume);
            soundList[randomSoundIndex].PlayDelayed(0.01f);
        }
    }


}
