using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    public enum GuardState
    {
        Patrol,
        Suspicious,
        Shoot
    }

    public GuardState currentState;

    public Transform player;

    public float rotationSpeed = 10f;
    public float investigateStopDistance = 0.5f;

    public Animator animator;

    // Shooting
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private float fireTimer;

    private string currentAnimation;

    private Vector3 lastKnownPosition;

    private NavMeshAgent agent;

    public Transform[] patrolPoints;

    private int currentPatrolIndex;

    // Search
    public float searchDuration = 3f;
    public float searchRotationSpeed = 120f;

    private float searchTimer;
    private float searchRotation;
    private bool searching;

    // Patrol wait
    public float patrolWaitTime = 2f;

    private float patrolTimer;
    private bool waitingAtPatrolPoint;

    public PlayerDeath playerDeath;

    public Transform playerAimPoint;

    public GameObject fireParticlePrefab;

    public BulletPool bulletPool;

    public GuardAIIcon aiIcon;

    public AudioSource gunAudioSource;
    public AudioClip gunshotClip;

    [UnityEngine.Range(0f, 1f)]
    public float gunshotVolume = 0.8f;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        playerDeath = player.GetComponent<PlayerDeath>();

        currentState = GuardState.Patrol;

        Debug.Log("Guard State: PATROL");
    }


    private void Update()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                Patrol();
                break;

            case GuardState.Suspicious:
                Investigate();
                break;

            case GuardState.Shoot:
                Shoot();
                break;
        }

        UpdateAnimation();
    }


    
    // ANIMATION

    private void UpdateAnimation()
    {
        switch (currentState)
        {
            case GuardState.Patrol:

                if (agent.velocity.sqrMagnitude > 0.01f)
                    PlayAnimation("Walk");
                else
                    PlayAnimation("Idle");

                break;


            case GuardState.Suspicious:

                if (searching)
                    PlayAnimation("Idle");
                else
                    PlayAnimation("Walk");

                break;


            case GuardState.Shoot:

                PlayAnimation("Shoot");

                break;
        }
    }


    private void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
            return;

        currentAnimation = animationName;

        animator.CrossFade(
            animationName,
            0.15f,
            0,
            0f
        );
    }


    
    // PATROL

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        if (waitingAtPatrolPoint)
        {
            agent.isStopped = true;

            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0f;

                waitingAtPatrolPoint = false;

                currentPatrolIndex++;

                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = 0;
                }

                agent.isStopped = false;
            }

            return;
        }

        agent.isStopped = false;
        agent.updateRotation = true;

        agent.SetDestination(
            patrolPoints[currentPatrolIndex].position
        );

        if (!agent.pathPending &&
            agent.remainingDistance <= 0.5f)
        {
            waitingAtPatrolPoint = true;
            patrolTimer = 0f;

            Debug.Log(
                "Reached Patrol Point: " +
                currentPatrolIndex
            );
        }
    }


    // INVESTIGATE

    private void Investigate()
    {
        if (!searching)
        {
            agent.isStopped = false;
            agent.updateRotation = true;

            agent.SetDestination(lastKnownPosition);

            if (!agent.pathPending &&
                agent.remainingDistance <= investigateStopDistance)
            {
                searching = true;

                agent.isStopped = true;
                agent.updateRotation = false;

                searchTimer = 0f;

                searchRotation =
                    transform.eulerAngles.y;
            }

            return;
        }


        

        searchTimer += Time.deltaTime;

        searchRotation +=
            searchRotationSpeed *
            Time.deltaTime;

        transform.rotation =
            Quaternion.Euler(
                0f,
                searchRotation,
                0f
            );


        if (searchTimer >= searchDuration)
        {
            searching = false;

            searchTimer = 0f;

            agent.updateRotation = true;
            agent.isStopped = false;

            agent.ResetPath();

            currentState = GuardState.Patrol;

            if (aiIcon != null)
                aiIcon.HideIcons();

            Debug.Log("Search complete → PATROL");
        }
    }


    
    // SHOOT
    

    private void Shoot()
    {

        if(playerDeath != null && playerDeath.IsDead)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
        }

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;

            Debug.Log("SHOOTING NOW!");

            FireBullet();
        }
    }


    private void FireBullet()
    {
        if (bulletPool == null || firePoint == null || player == null)
            return;

        Vector3 targetPosition =
            player.position + Vector3.up * 1f;

        Vector3 direction =
            targetPosition - firePoint.position;

        direction.Normalize();

        Quaternion bulletRotation =
            Quaternion.LookRotation(direction);

        // BULLET
        GameObject bullet = bulletPool.GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = bulletRotation;
            bullet.SetActive(true);

            //GUNSHOT AUDIO

            if(gunAudioSource != null && gunshotClip != null)
            {
                gunAudioSource.PlayOneShot(gunshotClip, gunshotVolume);
            }
        }

        // MUZZLE FLASH
        if (fireParticlePrefab != null)
        {
            GameObject flash = Instantiate(
                fireParticlePrefab,
                firePoint.position,
                firePoint.rotation
            );

            flash.SetActive(true);

            ParticleSystem particle =
                flash.GetComponent<ParticleSystem>();

            if (particle != null)
            {
                particle.Play();
            }

            Destroy(flash, 1f);
        }

        Debug.Log("BULLET + MUZZLE FLASH FIRED");
    }


    // PLAYER DETECTED

    public void PlayerDetected()
    {
        lastKnownPosition = player.position;

        searching = false;
        searchTimer = 0f;

        waitingAtPatrolPoint = false;
        patrolTimer = 0f;

        agent.isStopped = true;
        agent.updateRotation = false;

        if (currentState != GuardState.Shoot)
        {
            currentState = GuardState.Shoot;

            // Shoot immediately
            fireTimer = fireRate;

            if(aiIcon != null)
                aiIcon.ShowShooting();

            Debug.Log("State → SHOOT");
        }
    }


    // PLAYER LOST

    public void PlayerLost()
    {
        lastKnownPosition = player.position;


        if (currentState == GuardState.Shoot)
        {
            currentState = GuardState.Suspicious;

            searching = false;
            searchTimer = 0f;

            fireTimer = 0f;

            agent.isStopped = false;
            agent.updateRotation = true;

            if(aiIcon != null)
                aiIcon.ShowSuspicious();

            Debug.Log("State → SUSPICIOUS");
        }
    }
}