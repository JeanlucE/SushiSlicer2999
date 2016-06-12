using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    public static PlayerController main;

    public float CameraDistance;
    public float CameraSpeed;
    public float MoveSpeed;
    public float TurnSpeed;
    [Range(0, 1)]
    public float UnsheathePoint;
    [Range(0,1)]
    public float SheathePoint;
    public float SliceTime;
    public AnimationCurve SlicePositionCurve;
    public float SliceDistance;

    public List<AudioClip> SliceSounds = new List<AudioClip>();
    public List<AudioClip> SamuraiSounds = new List<AudioClip>();
    public AudioClip knockClip;

    private SwordState swordState = SwordState.Sheathed;
    private float sliceStartTime;
    private Vector2 sliceDirection;
    private Vector2 sliceStartPosition;
    private Vector2 sliceTargetPosition;

    private Vector2 lookDirection = Vector2.up;

    private PlayerAnimationController animationControl;

    public Rigidbody2D rb { get; private set; }

    public float recoverSpeed;

    private float knockTorpor;
    private bool knockedOut;

    private float slowdown = 1.0f;
    private int tentakels = 0;

    public bool IsKnockedOut
    {
        get { return knockedOut; }
    }

    void Awake()
    {
        main = this;
    }

    // Use this for initialization
    void Start()
    {
        animationControl = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knock(float torporIncrease, Vector2 knockback = new Vector2())
    {
        knockTorpor += torporIncrease;

        rb.velocity += knockback;

        if (knockClip)
            SoundEffectManager.Instance.CreateSoundEffect(knockClip);

        if (!knockedOut && knockTorpor > 100)
        {
            //print(knockTorpor);
            knockedOut = true;
            animationControl.Knockout();

            animationControl.SetSlice(false);
        }
    }

    IEnumerator AttachTentakel()
    {
        tentakels++;

        yield return new WaitForSeconds(Random.Range(0.8f, 2.0f));

        tentakels--;

        yield return null;
    }

    public void Slowdown(float factor)
    {
        StartCoroutine(AttachTentakel());
    }

    // Update is called once per frame
    void Update()
    {
        if (knockTorpor > 0)
        {
            knockTorpor -= recoverSpeed * Time.deltaTime;
        }

        animationControl.animator.speed = slowdown;

        if (knockedOut)
        {
            rb.AddForce(-3.5f * rb.velocity);
            if (rb.velocity.magnitude < 0.1f)
            {
                animationControl.SetRunning(false);
                slowdown = 1.0f;
            }
            else
            {
                slowdown = rb.velocity.magnitude / MoveSpeed;
            }

            if (knockTorpor < 20)
            {
                knockedOut = false;
                animationControl.RecoverKnockout();
                slowdown = 1.0f;
            }

            animationControl.UpdateKnockout(knockTorpor / 100.0f);
        }
        else
        {
            if (tentakels > 0)
            {
                slowdown = 1.0f / tentakels;
            }

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

                        //transform.position += (Vector3)moveInput * MoveSpeed * Time.deltaTime;
                        rb.velocity = (Vector3)moveInput * MoveSpeed * slowdown;

                        //Only rotate player if we have movementinput
                        if (moveInput.SqrMagnitude() > 0.3f * 0.3f)
                        {
                            lookDirection = moveInput.normalized;
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
                            //sliceStartPosition = transform.position;
                            //sliceTargetPosition = transform.position + (Vector3)(SliceDistance * sliceDirection);
                            sliceStartPosition = rb.position;
                            sliceTargetPosition = rb.position + (SliceDistance * sliceDirection);

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

                            //Do sound effect
                            int random = UnityEngine.Random.Range(0, SliceSounds.Count);
                            SoundEffectManager.Instance.CreateSoundEffect(SliceSounds[random]);

                            random = UnityEngine.Random.Range(0, SamuraiSounds.Count);
                            SoundEffectManager.Instance.CreateSoundEffect(SamuraiSounds[random]);

                            //Debug.DrawRay(transform.position, (Vector3)(SliceDistance * sliceDirection), Color.red, 2);
                        }

                        //Debug.DrawRay(transform.position, (Vector3)(SliceDistance * moveInput));
                    }
                    break;
                case SwordState.Unsheathed:
                    {
                        float progress = (Time.time - sliceStartTime) / SliceTime;

                        //transform.position = Vector3.Lerp(sliceStartPosition, sliceTargetPosition, progress);
                        rb.MovePosition(Vector2.Lerp(sliceStartPosition, sliceTargetPosition, progress));

                        if (Time.time > sliceStartTime + SliceTime)
                        {
                            animationControl.SetSlice(false);
                            swordState = SwordState.Sheathed;
                        }
                    }
                    break;
            }

            //rotate player to face forward
            float angletoRotate = -Vector2.Angle(Vector2.up, lookDirection);
            if (Mathf.Sign(lookDirection.x) == -1) { angletoRotate *= -1; }
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angletoRotate, Time.deltaTime * TurnSpeed));
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.LerpAngle(transform.eulerAngles.z, angletoRotate, Time.deltaTime * TurnSpeed));
        }

        //Move Camera
        cameraTarget = transform.position + (Vector3)lookDirection * CameraDistance;

        Vector3 currentPosition = Camera.main.transform.position;

        cameraTarget = new Vector3(cameraTarget.x, cameraTarget.y - 2, currentPosition.z);

        Camera.main.transform.position = Vector3.SmoothDamp(currentPosition, cameraTarget, ref cameraVelocity, CameraSpeed);
    }

    private Vector3 cameraTarget;
    private Vector3 cameraVelocity;

    private enum SwordState { Sheathed, Unsheathed }
}
