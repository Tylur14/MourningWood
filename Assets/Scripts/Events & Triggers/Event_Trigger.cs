using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Trigger : MonoBehaviour
{
    public enum TriggerState { Interaction, OTE }

    [Header("Trigger Settings")]
    public TriggerState triggerState;           // Is it an interaction (with button press) or does it trigger when player enters collider
    public bool requiresKey = false;            // Does this trigger require the player to have a specific key?
    [SerializeField] string displayMessage;
    [Range(0,2)]
    public int assocdKeyID;                     // 0 = gold key, 1 = silver key, 2 = bone key 
    public bool replayable = true;              // Possibly going to be removed, but added in case I want events that can be triggered multiple times
    public Event_Mover[] moverParentEvents;     // Events to activate

    [Header("Sequence Settings")]
    public bool isSequential = false;           // Does this play in an ordered sequence on at a time (true)? or do all the events play at the same time (false)? 
    public int sequenceIndex;               // ~? Needs investigation and work
    public float timeBetweenAnimations;         // Time between each sequence event

    [Header("Trigger Information")]
    public bool ableToActivate = false;         // Lets the system know when it's okie to fire off event
    public bool activated = false;              // Is event activated?

    private AudioSource aSrc;

    void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
        // Get refs
        if (GetComponent<AudioSource>() != null)
            aSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (triggerState)
        {
            case TriggerState.Interaction:
                if(!activated)
                    CheckInteraction();
                break;
            case TriggerState.OTE:
                if(!activated)
                    CheckTrigger();
                break;
        }
    }

    #region Trigger Activation Check
    public void CheckInteraction()
    {
        if (ableToActivate && Input.GetButtonDown("Interact") && CheckKey())
            if (!isSequential)
                Activate();
            else if (isSequential)
                SequenceActivation();
    }

    public void CheckTrigger()
    {
        if (ableToActivate && CheckKey())
            if (!isSequential)
                Activate();
            else if (isSequential)
                SequenceActivation();
    }
    #endregion

    #region Event Activators
    public void Activate()
    {
        activated = true;
        if (aSrc != null)
            aSrc.Play();
        if (moverParentEvents != null)
            foreach (Event_Mover mover in moverParentEvents)
                mover.PlayEvent();
        if(replayable)
            Destroy(this);
    }

    public void SequenceActivation()
    {
        activated = true;
        if(aSrc!=null)
            aSrc.Play();
        if (sequenceIndex < moverParentEvents.Length)
        {
            if (sequenceIndex < moverParentEvents.Length)
                moverParentEvents[sequenceIndex].PlayEvent();
            sequenceIndex++;
            Invoke("SequenceActivation", timeBetweenAnimations);
        }
        else Destroy(this);
        
    }
    #endregion

    public bool CheckKey()
    {
        print("Checking!");
        if (requiresKey)
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>().hasKeys[assocdKeyID] == true)
                return true;
            else { ableToActivate = false; GameObject.FindGameObjectWithTag("Display_GuiMessage").GetComponent<Display_GUIMessage>().DisplayWarning(displayMessage); return false; }
        }
        else return true;
    }

    #region Player Range Check
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !activated) // Check if player is in range of trigger
        {
            ableToActivate = true;
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")              // Player is no longer in range of trigger
            ableToActivate = false;
    }
    #endregion
}
