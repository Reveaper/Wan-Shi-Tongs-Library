using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceTrigger : MonoBehaviour
{
    private PerformanceHandler _performanceHandler;
    LayerMask _playerMask;

    public List<FloorSet> EnableFloorSets;
    public List<FloorSet> DisableFloorSets;

    private bool _isGoingToDisable = false;
    private float _disableWaitTimer;
    private float _disableWaitTimerMax = 0.25f;


    private void Start()
    {
        GameObject performanceHandlerInScene = GameObject.FindGameObjectWithTag("PerformanceHandler");
        _performanceHandler = performanceHandlerInScene.GetComponent<PerformanceHandler>();

        _playerMask = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if(_isGoingToDisable)
        {
            _disableWaitTimer += Time.deltaTime;

            if (_disableWaitTimer >= _disableWaitTimerMax)
            {
                _isGoingToDisable = false;
                _disableWaitTimer = 0;

                foreach (FloorSet floorSet in DisableFloorSets)
                    _performanceHandler.ChangeFloorSetState(floorSet, false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _playerMask)
        {
            foreach(FloorSet floorSet in EnableFloorSets)
                _performanceHandler.ChangeFloorSetState(floorSet, true);

            _isGoingToDisable = true;
        }
    }
}
