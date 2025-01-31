using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Define the states of the player.
public enum PlayerState
{
    Idle,
    Move,
    Attack,
    Cast,
    Dodge
}

public class PlayerController : Animals
{
    private float dodgeDistance = 5f; 
    public TMP_Text BlueSpiritText;
    public TMP_Text RedSpiritText;
    public TMP_Text GreenSpiritText;
    public int BlueSpirit;
    public int RedSpirit;
    public int GreenSpirit;
    [SerializeField]
    private CameraController CameraController; // Reference to the camera controller.
    public Slider powerSlider;

    private float sprintSpeedMultiplier = 2f; // Speed multiplier when sprinting.
    private float speed = 0.5f;

    private float rotationSmoothness = 10f; // Smoothness factor for rotation adjustments.

    private float currentSpeed = 0f; // Current movement speed.
    private float speedVelocity = 0f; // SmoothDamp's velocity for speed adjustments.
    private float speedSmoothTime = 0.5f; // Time to smoothly transition between speeds.
    private float h;
    private float v;
    private bool sprinting;
    private bool dodge;
    private int AttackIndex = 0;
    public Animator Animator;
    public WeaponCollider[] weaponCollider;
    private BossController bossController;

    public GameObject HealParticle;
    public GameObject KnockDownParticle;
    public GameObject MissleParticle;
    public GameObject ShootingParticle;
    public Transform MissleStartTransform;
    private bool isCasting = false;
    public bool redCasted;
    public bool blueCasted;
    public bool greenCasted;
    private PlayerAudio playerAudio;

    private PlayerState playerState; // The current state of the player.
    private Transform SkillTransform;


    // Property to get and set player's state.
    public PlayerState PlayerState
    {
        get => playerState;
        private set
        {
            playerState = value;
            switch (playerState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.Move:
                    break;
                case PlayerState.Attack:
                    break;
                case PlayerState.Cast:
                    break;
                case PlayerState.Dodge: 
                    break;
            }
        }
    }

    void Start()
    {
        base.Start();
        SkillTransform = this.transform;
        playerAudio = GetComponent<PlayerAudio>();
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        //showSpiritNum.SetText("0");
    }

    private void LateUpdate()
    {
        if (Health<=0)
        {
            Animator.SetTrigger("Dead");
            canMove = false;
        }
        else if (Health > 5000)
        {
            Health = 5000;
        }
        //Debug.Log(Health);
        StateOnUpdate(); // Check and update based on the current state.
        AdjustRotationToGround(); // Adjust player rotation based on ground's angle.
        
        // teleport back to ground if player is under terrain
        if (!Physics.Raycast(transform.position + new Vector3(0,1,0), Vector3.down, 100))
        {
            transform.position = new Vector3(transform.position.x,transform.position.y+1,transform.position.z); 
            
        }
    }

    // Check the current state and execute related function.
    private void StateOnUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        KeyPressDetect(h, v);
        switch (playerState)
        {
            case PlayerState.Idle:
                Move(0,0);
                break;
            case PlayerState.Move:
                Move(h,v); // Player movement logic.
                break;
            case PlayerState.Attack:
                Attack(); // Player's attack logic.
                break;
            case PlayerState.Cast:
                break;
            case PlayerState.Dodge:
                Dodge(h,v);
                break;
        }
    }

    float runTransition = 0;

    // Update idle animation state.

    
    private float redCooldown = 20f;
    private float greenCooldown = 20f;
    private float lastRedTime = -1f; 
    private float lastGreenTime = -1f; 

    private void KeyPressDetect(float h, float v)
    {
        if (GameObject.FindGameObjectWithTag("Canvas").GetComponent<CountDown>().remainingTime > 0)
        {
            if (Input.GetMouseButton(1))
            { 
                Vector3 cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0; 
                cameraForward.Normalize(); 

                transform.rotation = Quaternion.LookRotation(cameraForward);
            }
            if (Input.GetMouseButtonDown(0))
            {
                playerState = PlayerState.Attack;
                Attack();
            }
            else if (Input.GetKey(KeyCode.E) && GreenSpirit > 0 && Health<5000 && !isCasting)
            {
                float timeSinceLastCast = Time.time - lastGreenTime;
                if (timeSinceLastCast >= greenCooldown)
                {
                    Cast(0);
                }
            }
            else if (Input.GetKey(KeyCode.R) && RedSpirit > 0 && !isCasting)
            {
                float timeSinceLastCast = Time.time - lastRedTime;
                if (timeSinceLastCast >= redCooldown)
                {
                    Cast(1);
                }
            }
            else if (Input.GetKey(KeyCode.F) && BlueSpirit > 0 && !isCasting)
            {
                Cast(2);
            }

            else {
                redCasted = false;
                blueCasted = false;
                greenCasted = false;
            }
        }

        if ((h != 0 || v != 0))
        {
            if (Input.GetKey(KeyCode.Space) && canDodge && powerSlider.value >= 500)
            {
                playerState = PlayerState.Dodge;
            }
            else
            {
                playerState = PlayerState.Move;
                Move(h,v);
                dodge = false;
            }
            
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) && canDodge && powerSlider.value >= 500)
            {
                playerState= PlayerState.Dodge;
            }
            else
            {
                playerState = PlayerState.Idle;
                h = 0;
                v = 0;
                dodge = false;
            }
            
        }
        
        // if (Input.GetButtonDown("Dig"))
        // {
        //     Animator.SetTrigger("Dig");
        //     playerState = PlayerState.Dig;
        //     Dig();
        // }

    }

    private GameObject currentParticle;

    private void Cast(int n)
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null && Health > 0) 
        {
            isCasting = true;
            switch (n)
            {
                case 0:
                    SkillTransform = this.transform;
                    playerState = PlayerState.Cast;
                    currentParticle = HealParticle;
                    Health += 500;
                    GreenSpirit -= 1;
                    greenCasted = true;
                    lastGreenTime = Time.time;
                    playerAudio.GreenSkill();
                    break;
                case 1:
                    playerState = PlayerState.Cast;
                    bossController = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
                    SkillTransform = bossController.BloodPosition;
                    RedSpirit -= 1;
                    currentParticle = KnockDownParticle;
                    redCasted = true;
                    lastRedTime = Time.time;
                    playerAudio.RedSkill();
                    break;
                case 2:
                    playerState = PlayerState.Cast;
                    SkillTransform = MissleStartTransform;
                    StartCoroutine(FireMissiles());
                    currentParticle = ShootingParticle;
                    break;
            }
            Animator.SetTrigger("Hawl");
            UpdateSpiritNum();
        }
    }

     private IEnumerator FireMissiles()
    {
        if (BlueSpirit>=15)
        {
            blueCasted = true;
            for (int i = 0; i < 15; i++)
            {
                playerAudio.BlueSkill();
                BlueSpirit -= 1;
                Instantiate(MissleParticle, MissleStartTransform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f); 
            }
        }
        else
        {

            for (int i = 0; i < BlueSpirit; i++)
            {
                BlueSpirit -= 1;
                playerAudio.BlueSkill();
                blueCasted = true;
                Instantiate(MissleParticle, MissleStartTransform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
        }

    }

    private bool canDodge = true;
    private bool isDodging = false;
    private float lastDodgeTime = -1f; 
    private float dodgeCooldown = 4f;
    private void Dodge(float h, float v)
    {
        float timeSinceLastDodge = Time.time - lastDodgeTime;

        if (timeSinceLastDodge >= dodgeCooldown && !isDodging && powerSlider.value >= 500)
        {
            dodge = true;
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Camera.main.transform.right;

            Vector3 inputDirection = v * cameraForward + h * cameraRight;
            // using raycast to get the direction that lays on the ground (helps with slope)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f)) 
            {
                // projection given the direction that wolf is going to move, and the normal of terrain
                inputDirection = Vector3.ProjectOnPlane(inputDirection, hit.normal);
            }
            Vector3 dodgeDirection = (inputDirection.magnitude > 0.1f) ? inputDirection : transform.forward;

            StartCoroutine(DodgeMovement(dodgeDirection));
            lastDodgeTime = Time.time;
        }
        else {
            dodge = false;
        }
    }

    private IEnumerator DodgeMovement(Vector3 dodgeDirection)
    {
        Debug.Log("??????????????????????");
        Damage *= 1.5f;
        weaponCollider[1].gameObject.SetActive(true);
        Transform trail = transform.Find("Trail");
        trail.gameObject.SetActive(true);
        isDodging = true;
        Animator.SetBool("isDodging", true);
        float dodgeSpeed = dodgeDistance / 0.2f; 
        float dodgeTime = 0;

        Vector3 originalPosition = transform.position; 
        Vector3 targetPosition = transform.position + dodgeDirection * dodgeDistance; 

        while (dodgeTime < 0.2f) 
        {
            dodgeTime += Time.deltaTime;
            transform.position = Vector3.Lerp(originalPosition, targetPosition, dodgeTime / 0.2f); 
            yield return null;
        }

        Damage /= 1.5f;
        weaponCollider[1].gameObject.SetActive(false);

        transform.position = targetPosition; 
        isDodging = false;
        Animator.SetBool("isDodging", false);

        yield return new WaitForSeconds(0.2f);

        trail.gameObject.SetActive(false);

    }



    private bool canMove = true;

    // Movement function for player.
    private void Move(float h, float v)
    {
        AttackIndex = 0;
        if (!canMove) return;
        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        float targetSpeed = speed * inputDirection.magnitude;

        Transform dust = transform.Find("Dust");

        if (Input.GetKey(KeyCode.LeftShift) && powerSlider.value > 100)
        {
            //dust.GetComponent<ParticleSystem>().Play();
            targetSpeed *= sprintSpeedMultiplier; // Increase speed if sprinting.
            sprinting = true;
            GetComponent<PlayerAudio>().movementAudio.pitch = 2.24f;
        }
        else {
            dust.GetComponent<ParticleSystem>().Stop();
            sprinting = false;
            GetComponent<PlayerAudio>().movementAudio.pitch = 1.54f;
        }

        // Smoothly transition speed.
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, speedSmoothTime);
        if (h == 0 && v == 0)
        {
            GetComponent<PlayerAudio>().StopMoving();
            dust.GetComponent<ParticleSystem>().Stop();
            sprinting = false;
            targetSpeed = 0f;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, speedSmoothTime);
            if (Mathf.Abs(currentSpeed) < 0.1f)
            {
                currentSpeed = 0f;
                playerState = PlayerState.Idle;
            }

        }
        Animator.SetFloat("Move", currentSpeed*2f); // Update animator's Move parameter.

        // Rotate based on camera's orientation.
        Quaternion rot = Quaternion.Euler(0, CameraController.Yaw, 0);
        velocity = rot * inputDirection * currentSpeed;

        if (velocity.sqrMagnitude > sqrMaxSpeed)
        {
            velocity = velocity.normalized * maxSpeed; // Limit speed to max speed.
        }

        // Update player's position.
        transform.position += velocity * Time.deltaTime* currentSpeed;

        // Rotate the player based on movement direction.
        if (h != 0 || v != 0)
        {
            Quaternion targetDirQuaternion = Quaternion.LookRotation(velocity);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 10f);
            GetComponent<PlayerAudio>().Moving();
            if (Input.GetKey(KeyCode.LeftShift) && powerSlider.value > 100)
            {
                dust.GetComponent<ParticleSystem>().Play();
                sprinting = true;
                GetComponent<PlayerAudio>().movementAudio.pitch = 2.24f;
            }
            else
            {
                dust.GetComponent<ParticleSystem>().Stop();
                sprinting = false;
                GetComponent<PlayerAudio>().movementAudio.pitch = 1.54f;
            }
        }
    }

    // Adjust the player's rotation based on the ground's normal.
    private void AdjustRotationToGround()
    {
        Ray ray = new Ray(transform.position, -transform.up); // Raycast downwards.
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            Vector3 groundNormal = hit.normal; // Get ground's normal.
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

            // Smoothly rotate the player to align with ground's normal.
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
        }
    }



    private bool canAttack = true;
    private void Attack()
    {
        if (canAttack)
        {
            Animator.SetTrigger("Attack");
            canMove = false;
        }
        else
        {
            return;
        }
 

    }






    private void EndAttack()
    {
        weaponCollider[0].gameObject.SetActive(false);
        canDodge = true;
        canAttack = true;
        Animator.ResetTrigger("Attack");
        Animator.SetBool("CanMove", true);
        canMove = true;
        //AttackIndex += 1;
        if(AttackIndex > 2)
        {
            AttackIndex = 0;
        }
        playerState = PlayerState.Idle;
    }
    private void StartAttack()
    {

        GetComponent<PlayerAudio>().Attack();
        GetComponent<PlayerAudio>().isMoving = false;
        Animator.SetBool("CanMove", false);
        canDodge = false;
        canAttack = false;
        canMove = false;
        weaponCollider[0].gameObject.SetActive(true);
        Animator.ResetTrigger("Attack");
    }

    private void NextAttack()
    { 
        weaponCollider[0].gameObject.SetActive(false);
        canAttack = true;
    }


    private void StartHawl()
    {
        currentParticle = Instantiate(currentParticle,SkillTransform.position,Quaternion.identity, SkillTransform);
        canMove= false;
        canDodge= false;
        canAttack= false;
        Animator.SetBool("CanMove", false);

        Destroy(currentParticle, 2f);
    }

    private void EndHawl()
    {
        Animator.ResetTrigger("Hawl");
        canAttack = true;
        canDodge = true;
        canMove = true;
        isCasting = false;
        Animator.SetBool("CanMove",true);
    }

    // creating a new wolf spirit


    public void setMove(bool value)
    {
        canMove = value;
    }

    public void AddSpiritNum(string color)
    {
        switch (color)
        {
            case "red":
                RedSpirit += 1;
                break;
            case "blue":
                BlueSpirit += 1;
                break;
            case "green":
                GreenSpirit += 1;
                break;
        }
        UpdateSpiritNum();
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
        // uncommont this if bug fixed for player getting stuck when taking 
        // damage by multiple enemies
        if (Health > 0)
        {
            Animator.SetTrigger("takedmg");
        }
        if (Health <= 0 && this.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        GameObject getHit = Instantiate(particle, BloodPosition.position, Quaternion.identity, this.transform);

        Destroy(getHit, 2f);
    }

    public bool IsSprinting()
    {
        return sprinting;
    }
    public bool Dodged()
    {
        return dodge;
    }

    private void UpdateSpiritNum()
    {
        RedSpiritText.SetText(RedSpirit.ToString());
        GreenSpiritText.SetText(GreenSpirit.ToString());
        BlueSpiritText.SetText(BlueSpirit.ToString());
    }
}