using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TriggerSpiritWorld : MonoBehaviour
{
    private LayerMask _playerMask;

    public PostProcessProfile DefaultProfile;
    public PostProcessProfile SpiritProfile;

    private float _currentSpiritWorldTimer;
    [SerializeField]
    private float _spiritWorldTime = 5f;

    private bool _isInSpiritWorld;

    public Image TimeSlider;

    public GameObject SpiritList;

    public GameObject Player;
    public Material DefaultPlayerMaterial;
    public Material SpiritPlayerMaterial1;
    public Material SpiritPlayerMaterial2;
    public List<GameObject> PlayerSpiritParticles;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if(_isInSpiritWorld)
        {
            _currentSpiritWorldTimer += Time.deltaTime;
            TimeSlider.fillAmount = 1 - (_currentSpiritWorldTimer / _spiritWorldTime);

            if (_currentSpiritWorldTimer >= _spiritWorldTime)
            {
                EnterNormalWorld();
                _currentSpiritWorldTimer = 0;
            }
        }
    }

    private void EnterSpiritWorld()
    {
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile = SpiritProfile;
        _isInSpiritWorld = true;
        _currentSpiritWorldTimer = 0;

        SpiritList.SetActive(true);

        var meshRenderer = Player.GetComponent<SkinnedMeshRenderer>();
        Material[] mats = new Material[7];
        mats[0] = SpiritPlayerMaterial2;
        mats[1] = SpiritPlayerMaterial1;
        mats[2] = SpiritPlayerMaterial1;
        mats[3] = SpiritPlayerMaterial2;
        mats[4] = SpiritPlayerMaterial1;
        mats[5] = SpiritPlayerMaterial1;
        mats[6] = SpiritPlayerMaterial1;
        meshRenderer.materials = mats;

        foreach (var particle in PlayerSpiritParticles)
            particle.SetActive(true);
    }

    public void EnterNormalWorld()
    {
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        _currentSpiritWorldTimer = 0;
        volume.profile = DefaultProfile;
        _isInSpiritWorld = false;
        SpiritList.SetActive(false);
        foreach (var particle in PlayerSpiritParticles)
            particle.SetActive(false);
        var meshRenderer = Player.GetComponent<SkinnedMeshRenderer>();
        Material[] mats = new Material[7];
        mats[0] = DefaultPlayerMaterial;
        mats[1] = DefaultPlayerMaterial;
        mats[2] = DefaultPlayerMaterial;
        mats[3] = DefaultPlayerMaterial;
        mats[4] = DefaultPlayerMaterial;
        mats[5] = DefaultPlayerMaterial;
        mats[6] = DefaultPlayerMaterial;
        meshRenderer.materials = mats;
        TimeSlider.fillAmount = 0;

    }

    public void SpiritCollected(float extraSpiritWorldTime)
    {
        _currentSpiritWorldTimer -= extraSpiritWorldTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();

            if(player.SpiritsCaptured < 5)
                EnterSpiritWorld();
        }
    }
}
