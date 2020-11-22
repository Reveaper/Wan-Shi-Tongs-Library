using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public enum EndGameState
{
    WhiteOut, WhiteIn
}

public class EndGameEasterEgg : MonoBehaviour
{
    public List<GameObject> ObjectsTurnOn;

    public List<GameObject> ObjectsTurnOff;

    private LayerMask _playerMask;

    public GameObject EasterEggCanvas;

    public Image UILighten;
    private bool _hasTriggered;

    public GameObject Sun;
    public Material SkyBoxMaterial;

    public PostProcessProfile OutsideProfile;

    private float _waitTimer;

    private EndGameState _state = EndGameState.WhiteIn;

    private bool _hasStuffBeenDeleted;

    public GameObject Terrain;

    public AudioReverbFilter AudioEffects;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }


    private void Update()
    {
        if(_hasTriggered)
        {
            if(_state == EndGameState.WhiteIn)
            {
                if (UILighten.color.a < 1)
                    UILighten.color += new Color(0, 0, 0, 0.01f);

                if (UILighten.color.a >= 1)
                {
                    if(!_hasStuffBeenDeleted)
                    {
                        foreach (var obj in ObjectsTurnOff)
                            Destroy(obj.gameObject);
                        ObjectsTurnOff.Clear();
                        _hasStuffBeenDeleted = true;
                        EnableOutsideStuff();
                    }

                    _waitTimer += Time.deltaTime;

                    if (_waitTimer >= 3f)
                        _state = EndGameState.WhiteOut;
                }
            }

            if(_state == EndGameState.WhiteOut)
            {
                if (UILighten.color.a > 0)
                    UILighten.color -= new Color(0, 0, 0, 0.01f);

                if (UILighten.color.a <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void EnableOutsideStuff()
    {
        Sun.SetActive(true);
        RenderSettings.fog = false;
        RenderSettings.skybox = SkyBoxMaterial;
        DynamicGI.UpdateEnvironment();
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile = OutsideProfile;
        Terrain.SetActive(true);
        AudioEffects.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            _hasTriggered = true;
            EasterEggCanvas.SetActive(true);
        }
    }
}
