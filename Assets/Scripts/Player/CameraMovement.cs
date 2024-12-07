using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;         // Reference to the player's Transform
    public Vector3 offset = new Vector3(0f, 7f, -12f); // Offset from the player
    public float followSpeed = 5f;   // Speed at which the camera follows the player
    public float rotateSpeed = 5f;   // Speed for rotating the camera to match player

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned to the CameraFollow script!");
            return;
        }

        // Desired position based on player position and offset
        Vector3 targetPosition = player.position + offset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}