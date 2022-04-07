using UnityEngine;

public class GravHinge : MonoBehaviour
{
    private float _rotationSpeed = 5f;

    private Quaternion _targetRot;

    private void Awake()
    {
        _targetRot = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, _rotationSpeed * Time.deltaTime);
    }

    public void TargetRotation(Quaternion targetRotation)
    {
        _targetRot = targetRotation;
    }
}