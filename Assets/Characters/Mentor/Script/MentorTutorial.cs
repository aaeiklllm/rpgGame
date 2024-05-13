using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorTutorial : MonoBehaviour
{
    public GameObject DialogueBox;
    public DialogueManager dialogueManager;
    public Animator animator;
    private bool isPaused = false;

    public int dialogueTriggersLength;
    private int currentDialogueIndex = 0;
    private bool waitingForInput = false;
    private bool shouldCloseDialogueBox = true;

    void Start()
    {
        InteractWithNextDialogueTrigger();
    }

    void Update()
    {
        if (waitingForInput)
        {
            if (dialogueManager.sentences.Count == 0)
            {
                if (currentDialogueIndex == 1 && Input.GetKey(KeyCode.Space)) //jump
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 2 && Input.GetKey(KeyCode.Mouse0)) //attack
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 3 && Input.GetKey(KeyCode.Mouse1)) //block
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 4 && Input.GetKey(KeyCode.F)) //skills
                {
                    CloseDialogue(); 
                    return;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    dialogueManager.DisplayNextSentence();
                }
            }
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (dialogueManager.sentences.Count == 0)
                {
                    CloseDialogue();
                    return;
                }

                dialogueManager.DisplayNextSentence();
            }
        }
    }

    void InteractWithNextDialogueTrigger()
    {
        DialogueBox.SetActive(true);
        animator.SetBool("isTalking", true);

        DialogueTrigger[] dialogueTriggers = gameObject.GetComponents<DialogueTrigger>();
        dialogueTriggersLength = dialogueTriggers.Length;

        if (currentDialogueIndex < dialogueTriggersLength)
        {
            dialogueTriggers[currentDialogueIndex].TriggerDialogue();
            Debug.Log(currentDialogueIndex);
        }

        waitingForInput = (currentDialogueIndex >= 1 && currentDialogueIndex <= 4);
    }

    void CloseDialogue()
    {
        DialogueBox.SetActive(false);
        if (currentDialogueIndex < dialogueTriggersLength - 1)
        {
            StartCoroutine(WaitAndTriggerNextDialogue(3f));
        }
        waitingForInput = false;
    }

    IEnumerator WaitAndTriggerNextDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentDialogueIndex++;
        InteractWithNextDialogueTrigger();
    }
}
