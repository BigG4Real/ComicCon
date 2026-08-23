using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Focus player")]
    [SerializeField] Camera playerCamera;
    [SerializeField] float cameraSize = 6f;
    [SerializeField] float followForcePosition;
    [SerializeField] float followForceAmount;
    [SerializeField] float followForceSmooth;
    [SerializeField] MovementController movement;
    [SerializeField] Vector3 velocity = new Vector3(0,0,0);

    [Header("Focus Point")]
    [SerializeField] List<Transform> focusPoints;
    Vector2 pointToFollow;
    void Update()
    {
        playerCamera.orthographicSize = cameraSize;
        if (focusPoints.Count > 0)
        {
            FocusPoint();
            MoveCamera();
            return;
        }
        MoveCamera();
        FollowPlayer();
        return;
    }

    void MoveCamera()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(pointToFollow.x, pointToFollow.y, -10),
            ref velocity,
            followForceSmooth,
            Time.deltaTime
            * followForceAmount
        );
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void FollowPlayer()
    {
        pointToFollow = new Vector2(
            movement.transform.position.x + Math.Clamp(movement.rb.linearVelocity.x, -followForcePosition, followForcePosition),
            movement.transform.position.y + Math.Clamp(movement.rb.linearVelocity.y, -followForcePosition, followForcePosition)
            );
    }

    void FocusPoint()
    {
        pointToFollow.x = 0;
        pointToFollow.y = 0;
        for (int i = 0; i < focusPoints.Count; i++)
        {
            pointToFollow.x += focusPoints[i].position.x;
            pointToFollow.y += focusPoints[i].position.y;
        }
        pointToFollow.x /= focusPoints.Count;
        pointToFollow.y /= focusPoints.Count;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointToFollow, 0.1f);
    }
}
