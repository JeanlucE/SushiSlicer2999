using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public static PlayerController main;

    public float CameraDistance;
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

    private PlayerAnimationController animationControl;

    // Use this for initialization
    void Start()
    {
        main = this;
        animationControl = GetComponent<PlayerAnimationController>();
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


                    //Only rotate player if we have movementinput
                    if (moveInput.SqrMagnitude() > 0.3f * 0.3f)
                    {
                        lookDirection = moveInput;
                        animationControl.SetRunning(true);
                    }
                    else
                    {
                        animationControl.SetRunning(false);
                    }
                    
                    if (Input.GetButtonDown("Controller1A"))
                    {
                        swordState = SwordState.Unsheathed;
                        sliceStartTime = Time.time;
                        sliceDirection = lookDirection.normalized;
                        sliceStartPosition = transform.position;
                        sliceTargetPosition = transform.position + (Vector3)(SliceDistance * sliceDirection);

                        List<EnemyData> ingredientsSliced = new List<EnemyData>(); //sliced this frame
                        List<SliceInfo> sliceInfos = animationControl.Slice(sliceStartPosition, sliceTargetPosition);//slice objects
                        foreach (SliceInfo s in sliceInfos)
                        {
                            EnemyScript e = s.gameObject.GetComponent<EnemyScript>();
                            if (e != null)
                            {
                                ingredientsSliced.Add(e.type);
                                ComboList.Instance.AddIngredient(e.type);
                            }
                        }
                        lookDirection = sliceDirection;

                        Debug.DrawRay(transform.position, (Vector3)(SliceDistance * sliceDirection), Color.red, 2);
                    }

                    Debug.DrawRay(transform.position, (Vector3)(SliceDistance * moveInput));

                    //rotate player to face forward
                    float angletoRotate = -Vector2.Angle(Vector2.up, lookDirection);
                    if (Mathf.Sign(lookDirection.x) == -1) { angletoRotate *= -1; }

                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, angletoRotate, Time.deltaTime * TurnSpeed));
                }
                break;
            case SwordState.Unsheathed:
                {
                    float progress = (Time.time - sliceStartTime) / SliceTime;

                    progress = SlicePositionCurve.Evaluate(progress);

                    transform.position = Vector3.Lerp(sliceStartPosition, sliceTargetPosition, progress);

                    if (Time.time > sliceStartTime + SliceTime)
                    {
                        animationControl.SetSlice(false);
                        swordState = SwordState.Sheathed;
                    }
                }
                break;
        }
    }

    private Vector2 cameraTarget;
    public void LateUpdate()
    {
        
    }

    private enum SwordState { Sheathed, Unsheathed }
}
