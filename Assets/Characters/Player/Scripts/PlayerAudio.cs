using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource movementAudio; // Reference to the audio source for movement sounds.
    public AudioSource skillAudio;    // Reference to the audio source for skill sounds.
    public GameObject victoryBGM;     // Reference to the victory background music.
    public GameObject normalBGM;      // Reference to the normal background music.
    public GameObject normalVictory;  // Reference to the victory UI.
    public GameObject normalDefeat;   // Reference to the defeat UI.
    public AudioClip walk;            // Sound clip for walking.
    public AudioClip attack;          // Sound clip for attacking.
    public AudioClip chest;           // Sound clip for claiming a chest.
    public AudioClip redSkill;        // Sound clip for using a red skill.
    public AudioClip blueSkill;       // Sound clip for using a blue skill.
    public AudioClip greenSkill;      // Sound clip for using a green skill.
    public bool isMoving;             // Flag to indicate if the player is moving.
    public bool claimed;              // Flag to indicate if the player has claimed a chest.

    void Update()
    {
        float health = GetComponent<PlayerController>().Health;
        if (health <= 0 || victoryBGM != null && victoryBGM.activeSelf || 
        normalVictory != null && normalVictory.activeSelf || normalDefeat.activeSelf)
        {
            movementAudio.Stop();
            skillAudio.Stop();
        }
    }

    public void Moving()
    {
        movementAudio.clip = walk;
        movementAudio.loop = true;
        movementAudio.volume = 1;
        movementAudio.pitch = 1.54f;
        if (!isMoving)
        {
            movementAudio.loop = true;
            isMoving = true;
            movementAudio.Play();
        }
    }

    public void StopMoving()
    {
        if (isMoving)
        {
            movementAudio.volume = 1;
            movementAudio.loop = true;
            movementAudio.pitch = 1.54f;
            movementAudio.Stop();
            isMoving = false;
        }
    }

    public void Attack()
    {
        if (claimed)
        {
            movementAudio.Stop();
            ClaimChest();
            claimed = false;
        }
        else {
            movementAudio.pitch = 1.54f;
            movementAudio.volume = 0.15f;
            movementAudio.clip = attack;
            movementAudio.loop = false;
            movementAudio.Play();
        }
        
    }

    public void ClaimChest()
    {
        movementAudio.volume = 0.5f;
        movementAudio.clip = chest;
        movementAudio.loop = false;
        movementAudio.Play();
    }

    public void RedSkill()
    {
        skillAudio.pitch = 1.54f;
        skillAudio.clip = redSkill;
        skillAudio.loop = false;
        skillAudio.Play();
    }

    public void BlueSkill()
    {
        skillAudio.pitch = 1.54f;
        skillAudio.clip = blueSkill;
        skillAudio.loop = false;
        skillAudio.Play();
    }

    public void GreenSkill()
    {
        skillAudio.clip = greenSkill;
        skillAudio.loop = false;
        skillAudio.pitch = 0.19f;
        skillAudio.Play();
    }

    public void PlayVictoryBGM()
    {
        normalBGM.SetActive(false);
        victoryBGM.SetActive(true);
    }

    public void NormalVictoryBGM()
    {
        normalBGM.SetActive(false);
        normalVictory.SetActive(true);
    }

    public void NormalDefeatBGM()
    {
        normalBGM.SetActive(false);
        normalDefeat.SetActive(true);
    }

}
