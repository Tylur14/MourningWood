using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Made by Tyler J. Sims - FEB 10 2020
/// Made for Mourning Wood
/// New Base Class for Treemon (May be renamed from TreemonMotor to this in the future)
/// </summary>

    [RequireComponent(typeof(NavMeshAgent))]
public class TreemonMotor : Entity
{
    public enum TreemonStates
    {
        IDLE, MOVING, ATTACKING
    }

    [Header("Treemon Settings")]
    public float lookRadius = 10.0f; // How far does it need to check for the player
    public bool canLoseFocus = false; // If true, it can go back to idle if leaves its range
    public Vector2 attentionSpanScope;  // Used for how long the AI should focus on any one task 
                                        // |-(exception being the ATTACKING state, in which it will persist till the player moves out of range)
    
    [Header("Treemon Attack Settings")]
    public float damageOut = 0.075f;    // How much damage this AI can output to the player (ex. 0.07f = 7% of player health)
    public float attackRate = 0.55f;
    private float attackTimer;
    public bool canAttack;


    [Header("Treemon Information")]
    public TreemonStates currentState;
    public bool focused; // Sets to true once it first sees the player and will now only seek out the player during it's 'MOVING' state.
    public bool followingPlayer;
    public float attentionSpan;
    public float distanceFromPlayer;
    

    private Quaternion targetLookDirection; // used for IDLE looking around
    public float rotateSpeed;

    private NavMeshAgent agent;
    [HideInInspector]
    public Transform target; // *Currently* always the player, could change if 'minons' or something along those lines are added.

    new void Start()
    {
        base.Start();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        attentionSpan = Random.Range(attentionSpanScope.x, attentionSpanScope.y);
        currentState = TreemonStates.IDLE;

        attackTimer = attackRate;
    }

    void Update()
    {
        distanceFromPlayer = Vector3.Distance(target.position, transform.position);
        if (!FindObjectOfType<GameController>().gameOver)
        {
            switch (currentState)
            {
                case TreemonStates.IDLE:
                    State_IDLE();
                    break;
                case TreemonStates.MOVING:
                    State_MOVING();
                    break;
                case TreemonStates.ATTACKING:
                    State_ATTACKING();
                    break;
            }

            // If the player is within the AI's look raidus check and see if the AI can see the player (LOS)
            if (distanceFromPlayer <= lookRadius && !focused)
            {
                focused = CanSeePlayer(); // If so, set the alert flag to true
            }
            // ~ Currently unused but for possible AI types that can lose track of the player
            else if (distanceFromPlayer > lookRadius && canLoseFocus) 
                focused = false;

            // If the AI is close enought to the player, set flag to enable attacking the player
            if (distanceFromPlayer <= agent.stoppingDistance * 1.15f)
                canAttack = true;
            // Else set flag to false
            else if (distanceFromPlayer > agent.stoppingDistance * 1.15f)
                canAttack = false;

            // If the AI is aware of the player and is close enough, attack the player
            if (canAttack && focused)
            {
                currentState = TreemonStates.ATTACKING;
                if (GetComponent<CAnimationController>() != null)
                    GetComponent<CAnimationController>().ChangeAnimation(2);
            }
                

            // Runs down the attack timer... duh
            if (attackTimer > 0)
                attackTimer -= Time.deltaTime;

            GetFacingDirection(agent.destination);
        }
    }
    #region Treemon States
    // MAKING NO MOVEMENT
    void State_IDLE()
    {
        if (!focused)
        {
            // Do Nothing
            agent.isStopped = true;
        }
        else if (focused)
        {
            // Run down attention span
            agent.isStopped = true;

            if (attentionSpan > 0)
                attentionSpan -= Time.deltaTime;
            else if (attentionSpan <= 0)
            {
                PickState();
            }
        }
    }

    // MOVING TOWARDS CURRENT TARGET
    void State_MOVING()
    {
        // MOVE TO TARGET

        if (followingPlayer)
            NavToTarget();

        agent.isStopped = false;
        if (attentionSpan > 0)
            attentionSpan -= Time.deltaTime;
        else if (attentionSpan <= 0)
        {
            PickState();
        }
    }

    
    // NOT MOVING, FACING TARGET, ATTACKING
    void State_ATTACKING()
    {
        if (canAttack)
        {
            agent.isStopped = true;
            //agent.SetDestination(transform.position);
            PickLookAtTarget();
            if (attackTimer <= 0 && CanSeePlayer()) // Make sure the AI can attack AND see the player
            {
                attackTimer = attackRate;
                target.GetComponent<Player>().TakeDamage(damageOut);
                print(transform.name + " is attacking! " + attackTimer);
            }
            else
                print("Pick State from ATTACKING");
        }
        else PickState();

    }
    #endregion

    #region Helper Functions
    // ALERT OTHER NEARBY ENEMIES
    public void YellForHelp()
    {
        if (FindObjectOfType<TreemonMotor>() != null)
        {
            TreemonMotor[] monsters = FindObjectsOfType<TreemonMotor>();

            // Cycle through each Treemon in scene and if they are close enough, alert them
            // This circumvents the need for that specific treemon to have to see the player before being alerted
            foreach (TreemonMotor monster in monsters)
                if (Vector3.Distance(monster.transform.position, transform.position) < lookRadius)
                    monster.focused = true;

        }
    }

    // CAN THE TREEMON SEE THE PLAYER?
    bool CanSeePlayer()
    {
        RaycastHit hit;
        var heading = target.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance; // This is now the normalized direction.
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.transform.gameObject == target.gameObject)
                return true;
            else return false;
        }
        else return false;
    }

    // PICKS CURRENT STATE BASED ON RANDOM FACTORS AND PLAYER DISTANCE
    void PickState()
    {
        // Determine what the AI will do next
        // ! The closer the player is to the Treemon, the higher chance it should have to charge to the player instead of wander randomly

        // How long is the AI going to spend doing this action? If they are moving they should get to the destination FIRST!
        attentionSpan = Random.Range(attentionSpanScope.x, attentionSpanScope.y);

        // Find how far the player is but keep it between 0 and 40
        int j = (int)Mathf.Clamp(distanceFromPlayer, 0, 40);
        int i = Random.Range(0, 100);

        i -= j; // Remove the j value from i, the further away the less chance they should have to move towards the player
        i = Mathf.Clamp(i,0, 100); // But make sure i stays above 0; could be resolved with if(i<0) tho?

        // ?! - This needs to be heavily tested because I have no bloody idea if this produces the intended result!
        // Move the AI towards the player
        if(i > 8)
        {
            attentionSpan = attentionSpan * 2;
            currentState = TreemonStates.MOVING;
            followingPlayer = true;
            if (GetComponent<CAnimationController>() != null)
                GetComponent<CAnimationController>().ChangeAnimation(1);
            PickDestination(false);
        }
        // Move the AI in a random direction
        else if (i <= 8 && i > 3)
        {
            currentState = TreemonStates.MOVING;
            followingPlayer = false;
            attentionSpan = attentionSpan / 2;
            if (GetComponent<CAnimationController>() != null)
                GetComponent<CAnimationController>().ChangeAnimation(1);
            PickDestination(true);
        }
        // Halt the AI
        else
        {
            currentState = TreemonStates.IDLE;
            attentionSpan = attentionSpan / 2;
            if (GetComponent<CAnimationController>() != null)
                GetComponent<CAnimationController>().ChangeAnimation(0);
        }

    }

    // DEPENDING ON BOOL; PICKS RANDOM AGENT DESTINATION OR TRIES TO GET CLOSER TO PLAYER (not emotionally...)
    void PickDestination(bool random)
    {
        if (random)
        {
            Vector2 newLoc;
            newLoc.x = Random.Range(-lookRadius / 2, lookRadius / 2);
            newLoc.y = Random.Range(-lookRadius / 2, lookRadius / 2);
            var i = new Vector3(this.transform.position.x + newLoc.x, 0.0f, this.transform.position.z + newLoc.y);

            NavToTarget(i);
        }
        else if (!random)
            NavToTarget();
    }

    // Silly bit of code tbh, but just makes sure the enemy AI is facing the player when attacking
    void PickLookAtTarget()
    {
        if (focused)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.localRotation, lookRotation, Time.deltaTime * 15f);
        }

    }
    #endregion

    void NavToTarget()
    {
        agent.SetDestination(target.position);
    }
    void NavToTarget(Vector3 destination)
    {
        if (UnityEngine.AI.NavMesh.CalculatePath(transform.position, destination, agent.areaMask, agent.path))
            agent.SetDestination(destination);
    }

    void GetFacingDirection(Vector3 tar)
    {
        var dir = (tar - transform.position).normalized;

        dir.x = GetMixRoundedFloat(dir.x);
        dir.z = GetMixRoundedFloat(dir.z) *-1;

        if (GetComponent<CAnimationController>() != null)
            GetComponent<CAnimationController>().SetAnimDirection(dir);
    }

    int GetMixRoundedFloat(float i)
    {
        if (i > -0.45f && i < 0.45f)
            return 0;
        else if (i >= 0.46f)
            return 1;
        else if (i <= -0.46f)
            return -1;
        else return 0;
    }

    // IN-EDITOR VISUAL AID FOR TREEMON "VIEW DISTANCE"
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
