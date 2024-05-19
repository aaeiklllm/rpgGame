using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public ThirdPersonController player;

    public bool daggerTested = false;
    public bool healTested = false;
    public bool lightningTested = false;

    public AudioSource sfxManager;

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
                if (currentDialogueIndex == 1 && !player.Grounded) //jump
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 2 && player.isAttacking) //attack
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 3 && player.isBlocking) //block
                {
                    CloseDialogue(); 
                    return;
                }

                else if (currentDialogueIndex == 4) //skills
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        // called UseCurrentAbility

                        if (player.currentAbilityIndex == 0)
                        {
                            // casted dagger
                            daggerTested = true;
                        }
                        else if (player.currentAbilityIndex == 1 && player.currentMana > 0)
                        {
                            healTested = true;
                        }
                        else if (player.currentAbilityIndex == 2 && player.currentMana > 0)
                        {
                            lightningTested = true;
                        }

                        Debug.Log("here ===================");
                        
                    }

                    if (player.currentMana == 0) player.currentMana = player.maxMana;

                    if (daggerTested && healTested && lightningTested)
                    {
                        CloseDialogue();
                        return;
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    dialogueManager.DisplayNextSentence();
                    sfxManager.Play();

                }
            }
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialogueManager.sentences.Count == 0 && currentDialogueIndex == 6)
                {
                    SceneManager.LoadScene("3Mordon", LoadSceneMode.Single);
                }
                else if (dialogueManager.sentences.Count == 0)
                {
                    CloseDialogue();
                    return;
                }

                dialogueManager.DisplayNextSentence();
                sfxManager.Play();
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

        sfxManager.Play();

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
