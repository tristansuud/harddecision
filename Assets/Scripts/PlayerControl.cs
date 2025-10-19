using PlayerEvents;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Logger LoggerInstance;
    public static GameObject player;

    public float moveSpeed = 5f;
    public float aimMoveSpeed = 3.5f;
    public float rotationSpeed = 25f;
    public float aimSpeed = 25f;
    public float gravity = -9.81f;

    [SerializeField] private Color glowColor = Color.green;
    [SerializeField] private float glowIntensity = 1.5f;

    public KeyCode InteractButton = KeyCode.E;
    public KeyCode ReloadButton = KeyCode.R;
    public KeyCode WalkButton = KeyCode.LeftShift;

    [SerializeField] private float interactionDuration = 1.5f; // how long the player stays in Interacting
    

    private CharacterController controller;
    private Animator animator;
    

    public float equipTime = 0.5f;       // seconds to take gun out
    public float unequipTime = 0.5f;     // seconds to put gun away
    public float recoilDelay = 0.3f;     // delay between shots
    public int maxAmmo = 6;

    private enum PlayerState
    {
        Idle,
        Equipping,
        Aiming,
        Shooting,
        Unequipping,
        NoControl,
        NoGun,
        Interacting
    }
    private PlayerState playerState = PlayerState.Idle;

    public enum InteractionType
    {
        None,
        Door,
        GroundLevel,
        WaistLevel,
        HighLevel
    }
    
    private float stateTimer = 0f;
    
    //  Delegates
    /*
    public delegate void OnInteractKeyPress();
    public delegate void OnAimEnter();
    public delegate void OnAimExit();
    public delegate void OnAimStay();
    public delegate void OnShoot();
    
    public static OnAimEnter onAimEnter;
    public static OnAimExit onAimExit;
    public static OnAimStay onAimStay;
    public static OnShoot onShoot;
    */
    
    public bool canMove = true;
    public bool canAimShoot = true;
    public bool canInteract = true;
    private int currentAmmo;
    private float verticalVelocity = 0f;
    int velocityHash;
    int directionParamHash;
    private PlayerOnAimStayEvent AimStayEventObject;

    private InteractTrigger currentTrigger;
    private List<InteractTrigger> touchedTriggers;
    private int triggerIndex = -1;
    private bool isInScene;
    private int currentSceneEmote;

    private void OnEnable()
    {
        //onAimEnter += AimingAnimate;
        //onAimExit += AimingDeanimate;
        EventBus.Subscribe<PlayerOnAimEnterEvent>(AimingAnimate);
        EventBus.Subscribe<PlayerOnAimExitEvent>(AimingDeanimate);
        //onAimEnter += AimEnterMessage;
        //DialogueManager.onDialogueStart += StartDialogueState;
        //DialogueManager.onDialogueFinish += EndDialogueState;
        EventBus.Subscribe<DialogueEvents.DialogueOnStart>(StartDialogueState);
        EventBus.Subscribe<DialogueEvents.DialogueOnFinish>(EndDialogueState);
        
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerOnAimEnterEvent>(AimingAnimate);
        EventBus.Unsubscribe<PlayerOnAimExitEvent>(AimingDeanimate);
        //onAimEnter -= AimEnterMessage;
        //DialogueManager.onDialogueStart -= StartDialogueState;
        //DialogueManager.onDialogueFinish -= EndDialogueState;
        EventBus.Unsubscribe<DialogueEvents.DialogueOnStart>(StartDialogueState);
        EventBus.Unsubscribe<DialogueEvents.DialogueOnFinish>(EndDialogueState);
    }
    void Awake()
    {
        player = gameObject;
        isInScene = false;
        LoggerInstance = new Logger("PlayerControl");
        LoggerInstance.Enable();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        isInScene = false;
        currentSceneEmote = 0;
        velocityHash = Animator.StringToHash("velocity");
        directionParamHash = Animator.StringToHash("direction");
        touchedTriggers = new List<InteractTrigger>();
        AimStayEventObject = new PlayerOnAimStayEvent();
    }
    
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        currentAmmo = maxAmmo;
    }
    bool CanMove() =>
        playerState != PlayerState.NoControl &&
        playerState != PlayerState.Interacting;

    bool CanAim() =>
        playerState != PlayerState.NoControl &&
        playerState != PlayerState.NoGun &&
        playerState != PlayerState.Interacting;

    bool CanInteract() =>
        (playerState == PlayerState.Idle || playerState == PlayerState.NoGun) && playerState != PlayerState.Interacting;

    bool IsWalkingState() =>
        (playerState == PlayerState.Equipping || playerState == PlayerState.Aiming || playerState == PlayerState.Shooting || playerState == PlayerState.Unequipping);
    void Update()
    {
        
        if (CanMove()) HandleMovement();
        if (CanAim()) HandleAimShoot();
        if (CanInteract()) HandleInteraction();
        if (playerState == PlayerState.Interacting) UpdateInteractionState();
        HandleTriggerBrowse();
        //if (ShootingState!= ShootState.Idle) DebugLog(ShootingState.ToString());
    }
    void StartDialogueState(DialogueEvents.DialogueOnStart evt)
    {
        ChangeState(PlayerState.NoControl);
        LoggerInstance.Log("switched to no control");
        //stateTimer = -1f;
    }
    void EndDialogueState(DialogueEvents.DialogueOnFinish evt)
    {
        ChangeState(PlayerState.Idle, 1f);
    }
    

    // MAIN UPDATE FUNCTIONS

    void HandleMovement()
    {
        bool isWalking = Input.GetKey(WalkButton);
        
        float horizontal = Input.GetAxis("Horizontal");   // A-D
        float vertical = Input.GetAxis("Vertical");       // W-S
        float mouseX = Input.GetAxis("Mouse X");
        
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection.Normalize();

        if (controller.isGrounded)
        {
            verticalVelocity = 0f; // reset when touching ground
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        if (playerState == PlayerState.Aiming || playerState == PlayerState.Shooting)
        {
            transform.Rotate(Vector3.up * mouseX * aimSpeed);
        }
        else if (moveDirection.sqrMagnitude > 0.01f)
        {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed
                );
        }
        float AnimatorDirectionParameter = 0f;
        if (IsWalkingState() || isWalking)
        {
            moveDirection *= aimMoveSpeed;
        }
        else
        {
            moveDirection *= moveSpeed;
        }
        AnimatorDirectionParameter = Vector3.Dot(transform.forward.normalized, controller.velocity.normalized);

        moveDirection.y = verticalVelocity;
        animator.SetFloat(velocityHash, controller.velocity.magnitude);
        animator.SetFloat(directionParamHash, AnimatorDirectionParameter);
        controller.Move(moveDirection * Time.deltaTime);
    }
    void HandleAimShoot()
    {
        stateTimer -= Time.deltaTime;
        bool timerFinished = UpdateStateTimer();

        switch (playerState)
        {
            case PlayerState.Idle:
                if (Input.GetMouseButton(1)) // right click held
                {
                    ChangeState(PlayerState.Equipping, equipTime);
                    EventBus.Raise(new PlayerOnAimEnterEvent());
                    //onAimEnter?.Invoke();
                }
                break;

            case PlayerState.Equipping:
                if (!Input.GetMouseButton(1))
                {
                    // cancelled before finishing equip
                    ChangeState(PlayerState.Idle);
                    EventBus.Raise(new PlayerOnAimExitEvent());
                    break;
                }
                //onAimStay?.Invoke();
                EventBus.Raise(AimStayEventObject);
                if (timerFinished)
                {
                    ChangeState(PlayerState.Aiming);
                }
                break;

            case PlayerState.Aiming:
                EventBus.Raise(AimStayEventObject);
                if (!Input.GetMouseButton(1))
                {
                    ChangeState(PlayerState.Unequipping, unequipTime);
                    EventBus.Raise(new PlayerOnAimExitEvent());

                    break;
                }

                if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
                {
                    //EventBus.Raise(new PlayerOnShootEvent());
                    Shoot(new PlayerOnShootEvent());
                    ChangeState(PlayerState.Shooting, recoilDelay);
                }

                break;

            case PlayerState.Shooting:
                if (timerFinished)
                {
                    if (Input.GetMouseButton(1))
                        ChangeState(PlayerState.Aiming);
                    else
                    {
                        ChangeState(PlayerState.Unequipping, unequipTime);
                        EventBus.Raise(new PlayerOnAimExitEvent());
                    }
                }
                break;

            case PlayerState.Unequipping:
                if (timerFinished)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
        }
    }
    void HandleInteraction()
    {
        if (Input.GetKeyDown(InteractButton) && currentTrigger != null) //&& onInteractKeyPress != null [old condition]
        {
            
            animator.SetTrigger("interactmid"); // might be changed in the future
            ChangeState(PlayerState.Interacting, currentTrigger.interactionTime); //change state to interaction first
            currentTrigger.DoTrigger(); //sometimes there are player state changes after the interaction so it has to happen after the interaction
            LoggerInstance.Log("Currently Interacting...");
        }

    }
    void UpdateInteractionState()
    {
        if (UpdateStateTimer())
        {
            ChangeState(PlayerState.Idle);
            LoggerInstance.Log("Interaction finished");
        }
    }
    void HandleTriggerBrowse()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (triggerIndex == -1) return;
        if (scroll > 0f)
        {
            triggerIndex = (triggerIndex + 1) % touchedTriggers.Count;
            UpdateCurrentTrigger();
        }
        else if (scroll < 0f)
        {
            triggerIndex = (triggerIndex - 1 + touchedTriggers.Count) % touchedTriggers.Count;
            UpdateCurrentTrigger();
        }
    }

    // HELPER FUNCTIONS

        //HELPER: Animation
    void AimingAnimate(PlayerOnAimEnterEvent evt)
    {
        animator.SetBool("isAiming", true);
    }
    void AimingDeanimate(PlayerOnAimExitEvent evt)
    {
        animator.SetBool("isAiming", false);

    }
    public void ForcePlayAnimation()
    {

    }
    
        //HELPER: Gun
    void Shoot(PlayerOnShootEvent @event)
    {
        currentAmmo--;
        EventBus.Raise(new PlayerOnShootEvent());
        LoggerInstance.Log("Shot fired! Bullet left: " + currentAmmo);

        // TODO: spawn bullet, play sound, recoil animation, etc.
    }
    
    void ShootMessage()
    {
        LoggerInstance.Log("Shoot event");
    }

        //HELPER: PlayerState
    void ChangeState(PlayerState newState, float timer = 0f)
    {
        playerState = newState;
        stateTimer = timer;
    }
    bool UpdateStateTimer()
    {
        if (stateTimer > 0f)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f)
                return true; // just finished
        }
        return stateTimer <= 0f;
    }

        //HELPER: Trigger Behaviors
    public void RetrieveTrigger(InteractTrigger targetTrigger)
    {
        InteractionType interactionType = targetTrigger.GetInteractionType();
    }
    public bool RegisterTrigger(InteractTrigger targetTrigger)
    {
        if (touchedTriggers.Count <= 0)
        {
            touchedTriggers.Add(targetTrigger);
            triggerIndex += 1;
            UpdateCurrentTrigger();

            LoggerInstance.Log("Registered this trigger: ");
            return true;
        } else
        {
            touchedTriggers.Add(targetTrigger);
            return false;
        }
    }
    public void DeregisterTrigger(InteractTrigger targetTrigger)
    {
        if (touchedTriggers.Contains(targetTrigger))
        {
            int removedIndex = touchedTriggers.IndexOf(targetTrigger);
            touchedTriggers.Remove(targetTrigger);
            if (removedIndex <= triggerIndex)
                triggerIndex = Mathf.Clamp(triggerIndex - 1, 0, touchedTriggers.Count - 1);

            if (touchedTriggers.Count == 0)
            {
                
                triggerIndex = -1;
            }
            
            UpdateCurrentTrigger();
        }
        
    }
    private void UpdateCurrentTrigger()
    {
        if (triggerIndex >= 0 && triggerIndex < touchedTriggers.Count)
        {
            if (currentTrigger != null) SetGlow(false, currentTrigger.TriggerObjectRoot);
            currentTrigger = touchedTriggers[triggerIndex];
            LoggerInstance.Log("Current trigger set to: " + currentTrigger.gameObject.name);
            SetGlow(true, currentTrigger.TriggerObjectRoot);
        }
        else
        {
            if (currentTrigger != null) SetGlow(false, currentTrigger.TriggerObjectRoot);
            currentTrigger = null;
            LoggerInstance.Log("Current trigger set to: null " );

        }
    }
    public void SetGlow(bool enabled, GameObject root)
    {
        MeshRenderer[] activeTriggerRenderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (var rend in activeTriggerRenderers)
        {
            foreach (var mat in rend.materials) // handle multi-material meshes
            {
                if (enabled)
                {
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", glowColor * glowIntensity);
                }
                else
                {
                    mat.DisableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", Color.black);
                }
            }
        }
    }
    void DebugLog(string msg)
    {
        Debug.Log(msg);
    }
}
