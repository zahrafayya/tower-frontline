using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin;
    private Camera mainCamera;
    private float minXPosition = -34.4f;
    private float maxXPosition = 34.4f;

    private void Start()
    {
        // Get the reference to the main camera
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for mouse button down event
        if (Input.GetMouseButtonDown(0))
        {
            // Store the initial mouse position
            dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        // Check for mouse button held down event
        if (Input.GetMouseButton(0))
        {
            // Calculate the difference between the current mouse position and the initial mouse position
            Vector3 currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = currentMousePosition - dragOrigin;

            // Reset Y-axis movement to zero
            difference.y = 0f;

            // Move the camera based on the difference
            mainCamera.transform.position -= difference;

            // Limit camera's X position
            Vector3 cameraPosition = mainCamera.transform.position;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, minXPosition, maxXPosition);
            mainCamera.transform.position = cameraPosition;
        }
    }
}