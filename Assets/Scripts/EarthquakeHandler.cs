using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeHandler : MonoBehaviour
{
    public List<ParticleSystem> Particles;

    public List<Animation> AnimatedBookCases;

    public float ShakeTime = 2f;

    private LayerMask _playerMask;

    private bool _wasTriggered = false;

    private float _timer;

    private AudioSource _audio;

    private void Start()
    {
        _playerMask = LayerMask.NameToLayer("Player");
        _audio = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_wasTriggered)
        {
            _timer += Time.deltaTime;

            if(_timer >= 6f)
            {
                foreach (ParticleSystem particle in Particles)
                {
                    particle.gameObject.SetActive(true);
                    particle.Stop();
                    Destroy(particle.gameObject);
                }

                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _playerMask && !_wasTriggered)
        {
            _wasTriggered = true;

            foreach(ParticleSystem particle in Particles)
            {
                particle.gameObject.SetActive(true);
                particle.Play();
            }

            vThirdPersonCamera camera = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
            camera.BeginShake(ShakeTime);

            _audio.Play();

            foreach (Animation animation in AnimatedBookCases)
                animation.Play();
        }
    }
}
