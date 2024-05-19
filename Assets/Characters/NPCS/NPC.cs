using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject playerObject; 
    public float interactionDistance = 3f;
    public GameObject Interaction; 
    public GameObject DialogueBox; 
    public DialogueManager dialogueManager;
    public Animator animator;

    public bool dialogueActive = false;

    public AudioSource NPCsfx;



    void Start()
    {
        Interaction.SetActive(false);
        DialogueBox.SetActive(false);
        animator.SetBool("isTalking", false);
    }

    void Update()
    {
        if (playerObject != null)
        {
            float distance = Vector3.Distance(transform.position, playerObject.transform.position);

            if (distance <= interactionDistance)
            {
                if (!DialogueBox.activeSelf) 
                {
                    Interaction.SetActive(true);
                    dialogueActive = false;
                }

                if (Input.GetKeyDown(KeyCode.E) && !dialogueActive)
                {
                    DialogueBox.SetActive(true);
                    Interaction.SetActive(false);
                    InteractWithNPC();
                    dialogueActive = true;
                    NPCsfx.Play(); //audio clip
                }

                if (Input.GetKeyDown(KeyCode.E) && dialogueActive)
                {
                    if (dialogueManager.sentences.Count == 0)
                    {   
                        DialogueBox.SetActive(false);
                        animator.SetBool("isTalking", false);
                       
                    }
                   
                    NPCsfx.Play(); //audio clip
                    dialogueManager.DisplayNextSentence();
                }
            }
            else
            {
                Interaction.SetActive(false);
                DialogueBox.SetActive(false);
                animator.SetBool("isTalking", false);

                dialogueActive = false;
            }
        }
    }

    void InteractWithNPC()
    {
        animator.SetBool("isTalking", true);    
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();   
    }
    
}
