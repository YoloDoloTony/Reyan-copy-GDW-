using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCam : MonoBehaviour
{
    [SerializeField] GameObject _playerObj;

    Transform _tranform;

    Movement _player;

    // Update is called once per frame
    void Start()
    {
        _tranform = GetComponent<Transform>();

        _player = _playerObj.GetComponent<Movement>();
    }

    private void Update()
    {

        if (_playerObj.transform.position.x - _tranform.position.x > 3)
        {
            _tranform.position += transform.right * Time.deltaTime * _player.GetSpeed();
        }
        if (_playerObj.transform.position.x - _tranform.position.x < -3)
        {
            _tranform.position -= transform.right * Time.deltaTime * _player.GetSpeed();
        }
        if (_playerObj.transform.position.y - _tranform.position.y > 3)
        {
            _tranform.position += transform.up * Time.deltaTime * _player.GetSpeed();
        }
        if (_playerObj.transform.position.y - _tranform.position.y < -3)
        {
            _tranform.position -= transform.up * Time.deltaTime * _player.GetSpeed();
        }


    }
}
