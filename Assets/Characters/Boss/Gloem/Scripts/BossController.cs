using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static BossController;


public enum SkillDirection
{
    Front,
    Back,
    Any
}

public enum SkillDistance
{
    Close,
    Medium,
    Far
}

public class BossController : Animals
{
    public Transform playerTransform;  
    private NavMeshAgent navMeshAgent;
    public Animator bossAnimator;
    public GameObject Rock;
    public float Speed;

    public WeaponCollider[] Colliders;
    private float distance;
    private float direction;
    private bool canMove = false;

    private WeaponCollider currentCollider;
    private string currentTrigger;

    private List<BossSkill> bossSkills = new List<BossSkill>();

    private Dictionary<string, SkillCooldown> skillCooldowns = new Dictionary<string, SkillCooldown>();


    private bool isDead = false;



    private bool isExecutingAction = false;
    void Start()
    { 
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = 0;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeSkills();
        InitializeCooldowns();

    }
    private void InitializeSkills()
    {
        bossSkills.Add(new BossSkill("att_back", SkillDistance.Close, 10f, SkillDirection.Back));
        bossSkills.Add(new BossSkill("att_dash", SkillDistance.Medium, 20f, SkillDirection.Front));
        bossSkills.Add(new BossSkill("att_1", SkillDistance.Close, 20f, SkillDirection.Front));
        bossSkills.Add(new BossSkill("att_2", SkillDistance.Close, 20f, SkillDirection.Front));
        bossSkills.Add(new BossSkill("att_ground1", SkillDistance.Close, 30f, SkillDirection.Any));
        bossSkills.Add(new BossSkill("att_ground2", SkillDistance.Close, 30f, SkillDirection.Front));
        bossSkills.Add(new BossSkill("att_lfoot", SkillDistance.Close, 15f, SkillDirection.Front));
        bossSkills.Add(new BossSkill("att_rfoot", SkillDistance.Close, 15f, SkillDirection.Front));
    }
    private void InitializeCooldowns()
    {
        foreach (var skill in bossSkills)
        {
            skillCooldowns.Add(skill.skillName, new SkillCooldown(skill.skillName, skill.cooldown));
        }
    }


    void Update()
    {
        if (Health <= 0 && isDead == false)
        {
            isDead = true;
            navMeshAgent.speed = 0;
            setDead();
            Destroy(gameObject, 15f);
            return;
        }

        if (isDead == false)
        {
            Decision();
        }

    }

    private bool IsDirectionValid(float playerDirectionAngle, SkillDirection direction)
    {
        switch (direction)
        {
            case SkillDirection.Front:
                return playerDirectionAngle < 90f; 
            case SkillDirection.Back:
                return playerDirectionAngle > 90f; 
            case SkillDirection.Any:
                return true; 
            default:
                return false; 
        }
    }

    private bool IsDistanceValid(float playerDistance, SkillDistance requiredDistance)
    {
        switch (requiredDistance)
        {
            case SkillDistance.Close:
                return playerDistance <= 3.5f;  // Assuming 3.5f is the threshold for Close distance.
            case SkillDistance.Medium:
                return playerDistance > 3.5f && playerDistance <= 15f;  // Assuming between 3.5f and 5f is Medium distance.
            case SkillDistance.Far:
                return playerDistance > 15f && playerDistance <= 35f;  // Assuming anything more than 5f is Far.
            default:
                return false;
        }
    }
    private BossSkill selectedSkill = null;

    private void Decision()
    {
        if (isExecutingAction) return;
        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);
        float playerDirectionAngle = Vector3.Angle(transform.forward, playerTransform.position - transform.position);


        foreach (var skill in bossSkills)
        {
            if (IsDistanceValid(playerDistance, skill.triggerDistance) && skillCooldowns[skill.skillName].IsAvailable)
            {
                if (IsDirectionValid(playerDirectionAngle, skill.triggerDirection))
                {
                    selectedSkill = skill;
                    break;
                }
            }
        }

        if (selectedSkill != null)
        {
            Cast(selectedSkill.skillName);
        }
        else
        {
            Chase();
        }
    }

    private void Chase()
    {
        navMeshAgent.enabled = true;
        canMove = true;
        navMeshAgent.speed = Speed;
        bossAnimator.SetFloat("Move",1f);
        navMeshAgent.SetDestination(playerTransform.position);
    }

    private void Cast(string skillName)
    {
        //Debug.Log("Executing skill: " + skillName);
        bossAnimator.SetFloat("Move", 0f);
        navMeshAgent.speed = 0;
        isExecutingAction = true;
        switch (skillName)
        {
            case "att_back":
                currentCollider = Colliders[0];
                currentTrigger = "Attackback";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_dash":

                currentCollider = Colliders[0];
                currentTrigger = "Attackdash";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_1":
                navMeshAgent.speed = 0;
                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[0];
                currentTrigger = "Attack1";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_2":
                navMeshAgent.speed = 0;
                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[0];
                currentTrigger = "Attack2";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_ground1":
                navMeshAgent.speed = 0;
                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[4];
                currentTrigger = "AttackGround1";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_ground2":
                navMeshAgent.speed = 0;

                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[4];
                currentTrigger = "AttackGround2";
                bossAnimator.SetTrigger(currentTrigger);
                break;

            case "att_lfoot":
                navMeshAgent.speed = 0;
                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[2];
                currentTrigger = "lfoot";
                bossAnimator.SetTrigger(currentTrigger);
                break;
            case "att_rfoot":
                navMeshAgent.speed = 0;
                bossAnimator.SetFloat("Move", 0f);
                currentCollider = Colliders[3];
                currentTrigger = "rfoot";
                bossAnimator.SetTrigger(currentTrigger);
                break;
        }
        skillCooldowns[skillName].Use();
    }



    public class SkillCooldown
    {
        public string SkillName { get; private set; }
        public float Cooldown { get; private set; }
        private float lastUseTime = -Mathf.Infinity;

        public SkillCooldown(string skillName, float cooldown)
        {
            this.SkillName = skillName;
            this.Cooldown = cooldown;
        }

        public bool IsAvailable => Time.time >= lastUseTime + Cooldown;

        public void Use()
        {
            lastUseTime = Time.time;
        }
    }

    public class BossSkill
    {
        public string skillName;
        public SkillDistance triggerDistance;
        public float cooldown;
        public SkillDirection triggerDirection;
        public float lastUseTime = -Mathf.Infinity;

        public BossSkill(string skillName, SkillDistance triggerDistance, float cooldownTime, SkillDirection triggerDirection)
        {
            this.skillName = skillName;
            this.triggerDistance = triggerDistance;
            this.cooldown = cooldownTime;
            this.triggerDirection = triggerDirection;
        }
    }


    private void StartAttack()
    {
        transform.LookAt(playerTransform.position);
        //Debug.Log("111");
        isExecutingAction = true;
        bossAnimator.SetBool("CanMove", false);
    }

    private void StartDMG1()
    {

        currentCollider.gameObject.SetActive(true);
        bossAnimator.ResetTrigger(currentTrigger);
    }

    private void StartDMG2() 
    {
        currentCollider.gameObject.SetActive(false);    
        currentCollider = Colliders[1];
        currentCollider.gameObject.SetActive(true);
        bossAnimator.ResetTrigger(currentTrigger);
    }

    private void EndAttack()
    {
        //Debug.Log("222");
        selectedSkill = null;
        isExecutingAction = false;
        currentCollider.gameObject.SetActive(false);
        bossAnimator.ResetTrigger(currentTrigger);
        bossAnimator.SetBool("CanMove", true);
        Rock.SetActive(false);
        navMeshAgent.speed = Speed;
    }


    private Vector3 lockedPlayerPosition;
    private GameObject currentRock; 

    private void StartLock()
    {
        lockedPlayerPosition = playerTransform.position; 
        currentRock = Instantiate(Rock, new Vector3(lockedPlayerPosition.x, lockedPlayerPosition.y - 5f, lockedPlayerPosition.z), Quaternion.identity);
    }

    private void SummitRock()
    {
        StartCoroutine(RiseRock());
    }

    private IEnumerator RiseRock()
    {
        float startY = currentRock.transform.position.y;
        float endY = lockedPlayerPosition.y;

        float journeyLength = endY - startY;
        float startTime = Time.time;

        float distanceCovered = 0;
        while (distanceCovered < journeyLength)
        {
            distanceCovered = (Time.time - startTime) * 40f;
            float fractionOfJourney = distanceCovered / journeyLength;

            currentRock.transform.position = new Vector3(lockedPlayerPosition.x, Mathf.Lerp(startY, endY, fractionOfJourney), lockedPlayerPosition.z);

            yield return null;
        }

        currentRock.transform.position = lockedPlayerPosition;
        Destroy(currentRock,15f);
    }

    public void setDead()
    {
        bossAnimator.SetTrigger("Dead");
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
        // uncommont this if bug fixed for player getting stuck when taking 
        // damage by multiple enemies
        bossAnimator.SetTrigger("takedmg");
        if (Health <= 0 && this.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        GameObject getHit = Instantiate(particle, BloodPosition.position, Quaternion.identity, this.transform);

        Destroy(getHit, 2f);
    }

    private void StartDown()
    {

        isExecutingAction = true;
        navMeshAgent.speed = 0;
        bossAnimator.SetFloat("Move", 0f);
    }

    private void EndRise()
    {

        isExecutingAction = false;
    }

    public void KnockDown()
    {
        Debug.Log("down down down");
        bossAnimator.SetTrigger("Down");
    }

    public Transform objectA;
    public Transform objectB;


}