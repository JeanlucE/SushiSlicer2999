﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float TurnSpeed;
    [Range(0, 1)]
    public float UnsheathePoint;
    [Range(0,1)]
    public float SheathePoint;
    public float SliceTime;
    public AnimationCurve SlicePositionCurve;
    public float SliceDistance;
    

    private SwordState swordState = SwordState.Sheathed;
    private float sliceStartTime;
    private Vector2 sliceDirection;
    private Vector2 sliceStartPosition;
    private Vector2 sliceTargetPosition;

    private Vector2 lookDirection = Vector2.up;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch (swordState)
        {
            case SwordState.Sheathed:
                {
                    float xMoveInput = Input.GetAxis("Controller1LX");
                    float yMoveInput = Input.GetAxis("Controller1LY");
                    Vector2 moveInput = new Vector3(xMoveInput, yMoveInput).normalized;

                    float xSliceInput = Input.GetAxis("Controller1RX");
                    float ySliceInput = Input.GetAxis("Controller1RY");
                    Vector2 sliceInput = new Vector2(xSliceInput, ySliceInput);


                    transform.position += (Vector3) moveInput * MoveSpeed * Time.deltaTime;

                    //Debug.Log(sliceInput.magnitude);
                    if (sliceInput.SqrMagnitude() > UnsheathePoint * UnsheathePoint)
                    //if (Input.GetKeyDown(KeyCode.Joystick1Button0))
                    {
                        swordState = SwordState.Unsheathed;
                        sliceStartTime = Time.time;
                        sliceDirection = sliceInput.normalized;
                        sliceStartPosition = transform.position;
                        sliceTargetPosition = transform.position + (Vector3)(SliceDistance * sliceDirection);

                        SpriteSlicer.SliceAll(sliceStartPosition, sliceTargetPosition);

                        Debug.DrawRay(transform.position, (Vector3) (SliceDistance * sliceDirection),  Color.red, 2);
                    }

                    Debug.DrawRay(transform.position, (Vector3)(SliceDistance * moveInput));

                    /*
                    if (moveInput.SqrMagnitude() > 0.3f * 0.3f)
                        lookDirection = moveInput;

                    float angletoRotate = -Vector2.Angle(Vector2.up, lookDirection);
                    if (Mathf.Sign(lookDirection.x) == -1) { angletoRotate *= -1; }

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, angletoRotate, Time.deltaTime * TurnSpeed));

                    */
                }
                break;
            case SwordState.Unsheathed:
                {
                    float progress = (Time.time - sliceStartTime) / SliceTime;

                    progress = SlicePositionCurve.Evaluate(progress);

                    transform.position = Vector2.Lerp(sliceStartPosition, sliceTargetPosition, progress);

                    float xSliceInput = Input.GetAxis("Controller1RX");
                    float ySliceInput = Input.GetAxis("Controller1RY");
                    Vector2 sliceInput = new Vector2(xSliceInput, ySliceInput);

                    if (Time.time > sliceStartTime + SliceTime && sliceInput.SqrMagnitude() < SheathePoint * SheathePoint)
                    {
                        swordState = SwordState.Sheathed;
                    }
                }
                break;
        }
    }



    private enum SwordState { Sheathed, Unsheathed }
}
