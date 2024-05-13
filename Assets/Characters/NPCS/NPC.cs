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
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    DialogueBox.SetActive(true);
                    Interaction.SetActive(false);
                    InteractWithNPC();   
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (dialogueManager.sentences.Count == 0)
                    {   
                        DialogueBox.SetActive(false);
                        animator.SetBool("isTalking", false);
                    }
                   
                    dialogueManager.DisplayNextSentence();
                }
            }
            else
            {
                Interaction.SetActive(false);
                DialogueBox.SetActive(false);
                animator.SetBool("isTalking", false);
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
