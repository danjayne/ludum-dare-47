using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    private float m_TimeSinceJump;
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    public int MaxJumpCount = 2;
    private int _jumpCount;

    [Header("Crouching")]
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    [Header("Movement")]
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public float FootstepSoundDelay = .6f;
    float timeSinceLastFootstep;

    [Header("Dash")]
    public float _DashMultiplier = 5f;
    public UnityEvent OnDashEvent;


    [Header("Max Velocity")]
    public float MaxVelocity;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private float _sqrMaxVelocity;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep; /* Allows trigger stays to be calculated accurately. */

        _sqrMaxVelocity = MaxVelocity * MaxVelocity;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnDashEvent == null)
            OnDashEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void Start()
    {
        _jumpCount = MaxJumpCount;
    }

    private void FixedUpdate()
    {
        ClampVelocity();

        m_TimeSinceJump += Time.deltaTime;

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded && m_TimeSinceJump > .1)
                {
                    _jumpCount = MaxJumpCount;
                    OnLandEvent.Invoke();
                }
            }
        }

        timeSinceLastFootstep += Time.deltaTime;

        if (Math.Abs(m_Rigidbody2D.velocity.x) > 0.01 && m_Grounded && timeSinceLastFootstep > FootstepSoundDelay)
        {
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Footsteps, 2f);
            timeSinceLastFootstep = 0f;
        }
    }

    private void ClampVelocity()
    {
        var v = m_Rigidbody2D.velocity;
        // Clamp the velocity, if necessary
        // Use sqrMagnitude instead of magnitude for performance reasons.
        if (v.sqrMagnitude > _sqrMaxVelocity)
        { // Equivalent to: rigidbody.velocity.magnitude > maxVelocity, but faster.
          // Vector3.normalized returns this vector with a magnitude 
          // of 1. This ensures that we're not messing with the 
          // direction of the vector, only its magnitude.
            m_Rigidbody2D.velocity = v.normalized * MaxVelocity;
        }
    }

    public void Move(float move, bool crouch, bool jump, bool dash)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (jump && _jumpCount > 0)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_TimeSinceJump = 0f;
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Jump);

            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            m_Rigidbody2D.AddForce(new Vector2(0f, _jumpCount == MaxJumpCount ?  m_JumpForce : m_JumpForce * .7f));
            _jumpCount--;
        }

        if (dash)
        {
            //m_Grounded = false;
            var dashForce = m_JumpForce * _DashMultiplier;
            AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.Dash);
            m_Rigidbody2D.AddForce(new Vector2(m_FacingRight ? dashForce : -dashForce, 0f));
            OnDashEvent.Invoke();
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}