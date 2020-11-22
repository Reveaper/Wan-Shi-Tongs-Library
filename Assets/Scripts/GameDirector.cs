using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    public float MaxGameTime;

    private float _currentGameTime;

    public GameObject Terrain;

    bool _hasStartedShaking;

    public Image TimeSlider;

    public List<Image> UITimeSlider;

    public bool IsInConversation;

    private bool _hasGameEnded;

    public List<GameObject> EndGameParticles;

    private float _shakeTimer;

    public List<GameObject> EndGameImmediateTurnOff;

    public GameObject UIConversation;
    public Text UIConversationMessage;

    public AudioSource EQSound;

    private void Update()
    {
        if(!_hasGameEnded)
        {
            if (!IsInConversation)
                _currentGameTime += Time.deltaTime;

            TimeSlider.fillAmount = 1 - (_currentGameTime / MaxGameTime);

            if (_currentGameTime >= MaxGameTime)
            {
                Terrain.transform.position += Vector3.up * Time.deltaTime;
                _shakeTimer += Time.deltaTime;

                if(_shakeTimer >= 10f && UIConversation != null)
                    Destroy(UIConversation.gameObject);

                if (_shakeTimer >= 45f && EQSound != null)
                {
                    EQSound.Stop();
                    Destroy(EQSound.gameObject);
                }


                if (!_hasStartedShaking)
                {
                    vThirdPersonCamera camera = Camera.main.gameObject.GetComponent<vThirdPersonCamera>();
                    camera.BeginShake(45f);
                    foreach (var particle in EndGameParticles)
                        particle.SetActive(true);

                    UIConversation.SetActive(true);
                    UIConversationMessage.text = "Time's up. Enough loitering around!";

                    foreach (var obj in EndGameImmediateTurnOff)
                    {
                        obj.SetActive(false);
                        Destroy(obj.gameObject);
                    }

                    EQSound.gameObject.SetActive(true);

                    _hasStartedShaking = true;
                }
            }
        }
    }

    public void EndGame()
    {
        foreach (var UIelement in UITimeSlider)
            UIelement.gameObject.SetActive(false);

        _hasGameEnded = true;
    }
}
