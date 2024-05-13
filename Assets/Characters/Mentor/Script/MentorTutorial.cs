using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorTutorial : MonoBehaviour
{
    public GameObject DialogueBox; 
    public DialogueManager dialogueManager;
    public Animator animator;


    void Start()
    {
        DialogueBox.SetActive(true);
        animator.SetBool("isTalking", true);
        InteractWithMordon();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (dialogueManager.sentences.Count == 0)
            {   
                DialogueBox.SetActive(false);
            }       
            dialogueManager.DisplayNextSentence();
        }
    }

    void InteractWithMordon()
    {
        animator.SetBool("isTalking", true);
        DialogueTrigger dialogueTrigger = GetComponent
        <DialogueTrigger>();
        dialogueTrigger.TriggerDialogue();   
    }
}