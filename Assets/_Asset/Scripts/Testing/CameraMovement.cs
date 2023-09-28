using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float _moveSpeed = 3f;

    public float _offsetX = 0f;
    public float _offsetY = 0f;
    public float _rotationSensitivity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // A and D keys
        float verticalInput = Input.GetAxis("Vertical"); // W and S keys
        float upwardInput = 0f;

        if (Input.GetKey(KeyCode.Space))
            upwardInput += 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            upwardInput -= 1f;

        // Combine the input into a single vector
        Vector3 movement = new Vector3(horizontalInput, upwardInput, verticalInput);

        // Convert local coordinates to world coordinates based on the camera's orientation
        Vector3 worldMovement = transform.TransformDirection(movement);

        // Multiply by speed and time
        worldMovement *= _moveSpeed * Time.deltaTime;

        // Move the camera
        transform.position += worldMovement;


        _offsetY += Input.GetAxis("Mouse X") * _rotationSensitivity;
        _offsetX += Input.GetAxis("Mouse Y") * -1f * _rotationSensitivity;
        transform.localEulerAngles = new Vector3(_offsetX, _offsetY, 0f);
    }
}
