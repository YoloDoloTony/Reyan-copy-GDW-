using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    [SerializeField] float horizontalMoveRange;
    [SerializeField] float verticalMoveRange;

    //Level max and min boundaries
    [SerializeField] float levelMinX;
    [SerializeField] float levelMaxX;
    [SerializeField] float levelMinY;
    [SerializeField] float levelMaxY;

    //Shake Variables
    Vector3 initialPosition;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private float dampingSpeed = 1.0f;

    void Update()
    {
        initialPosition = transform.localPosition;

        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeMagnitude = magnitude;
        shakeDuration = duration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Horizontal Camera Movement
        //Right
        if (player.position.x > transform.position.x + horizontalMoveRange && transform.position.x < levelMaxX)
        {
            transform.position = new Vector3(player.position.x - horizontalMoveRange, transform.position.y, transform.position.z);
        }
        //Left
        if (player.position.x < transform.position.x - horizontalMoveRange && transform.position.x > levelMinX)
        {
            transform.position = new Vector3(player.position.x + horizontalMoveRange, transform.position.y, transform.position.z);
        }

        //Vertical Camera Movement
        //Up
        if (player.position.y > transform.position.y + verticalMoveRange && transform.position.y < levelMaxY)
        {
            transform.position = new Vector3(transform.position.x, player.position.y - verticalMoveRange, transform.position.z);
        }
        //Down
        if (player.position.y < transform.position.y - verticalMoveRange && transform.position.y > levelMinY)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + verticalMoveRange, transform.position.z);
        }
    }
}