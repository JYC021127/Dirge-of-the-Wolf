using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour

{
    //pages
    public GameObject mainpage;
    public GameObject setting;
    public GameObject instruction;
    public GameObject story1;
    public GameObject story2;
    public GameObject story3;
    public GameObject story4;


    public GameObject GRASS;
    public GameObject grass;
    public GameObject selectgrass;

    public GameObject SNOW;
    public GameObject snowfield;
    public GameObject snowunlock;
    public GameObject selectsnow;
    //public GameObject swamp;

    public GameObject DESERT;
    public GameObject desert;
    public GameObject desertunlock;
    public GameObject selectdesert;


    public GameObject controls;
    public GameObject chests;
    public GameObject health;
    public GameObject skills;
    public GameObject attack;
    public GameObject defence;
    public GameObject invincible;



    public Slider Slider;
    public AudioSource BGM;
    public GameObject statusManager;

    public GameObject pausePage;
    public GameObject progressLostPage;
    public GameObject allSelectionButtons;
    public AudioSource movementSound;
    public AudioSource skillSound;
    public GameObject defeatBGM;
    public GameObject victoryBGM;
    private bool movementPlaying;
    private bool skillPlaying;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            statusManager = GameObject.Find("StatusManager");
            // if Desert is active, then Desert is unlocked
            if (statusManager.transform.Find("Desert").gameObject.activeSelf)
            {
                desert.SetActive(false);
                desertunlock.SetActive(true);
            }
            // if Snow is active, then Snow is unlocked
            if (statusManager.transform.Find("Snow").gameObject.activeSelf)
            {
                snowfield.SetActive(false);
                snowunlock.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "StartScene")
        {
            // pause game
            if (Input.GetKey(KeyCode.Escape) && !victoryBGM.activeSelf && !defeatBGM.activeSelf)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.GetComponent<PlayerController>().enabled = false;
                }
                movementSound.Stop();
                skillSound.Stop();
                
                // stop all time related animation,effects
                Time.timeScale = 0;
                Cursor.visible = true; 
                Cursor.lockState = CursorLockMode.Confined;
                pausePage.SetActive(true);
            }
        }
        else {
            BGM.volume = Slider.value;
        }
    }

    public void pauseToGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
        pausePage.SetActive(false);
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void pauseToInstructions()
    {
        instruction.SetActive(true);
    }

    public void backpausefrominstru()
    {
        // close chests and open controls
        chestsToControls();
        skillsToControls();

        //close setting page
        instruction.SetActive(false);
    }

    public void pauseToProgressLost()
    {
        allSelectionButtons.SetActive(false);
        progressLostPage.SetActive(true);
    }

    public void progressLostToPause()
    {
        progressLostPage.SetActive(false);
        allSelectionButtons.SetActive(true);
    }



    //setting button
    public void settingbutton()
    {
        //close main page
        mainpage.SetActive(false);
        //open setting page
        setting.SetActive(true);

    }
    //go back to mainpage (from setting page)
    public void backmainpage()
    {
        //open main page
        mainpage.SetActive(true);

        //close setting page
        setting.SetActive(false);
    }




    //instructionbutton
    public void instructionbutton()
    {
        //close main page
        mainpage.SetActive(false);
        //open instruction page
        instruction.SetActive(true);

    }

    //go back to mainpage (from instruction page)
    public void backmainpagefrominstru()
    {
        //open main page
        mainpage.SetActive(true);

        // close chests and open controls
        chestsToControls();
        skillsToControls();

        //close setting page
        instruction.SetActive(false);
    }


    public void start()
    {
        //open main page
        mainpage.SetActive(true);

        //close selectgrass
        selectgrass.SetActive(false);
    }


    //go back to mainpage (from grassmap page)
    public void backmainfromgrass()
    {
        //open main page
        mainpage.SetActive(true);

        if (selectgrass.activeSelf)
        {
            selectgrass.SetActive(false);
            grass.SetActive(true);
        }
        //close grass
        GRASS.SetActive(false);
    }


    //go back to mainpage (from snowfield page)
    public void backmainfromsnow()
    {
        //open main page
        mainpage.SetActive(true);

        if (selectsnow.activeSelf)
        {
            selectsnow.SetActive(false);
            snowunlock.SetActive(true);
        }
        //close snowfield map page
        SNOW.SetActive(false);
    }

    //go back to mainpage (from desert page)
    public void backmainfromdesert()
    {
        //open main page
        mainpage.SetActive(true);
        if (selectdesert.activeSelf)
        {
            selectdesert.SetActive(false);
            desertunlock.SetActive(true);
        }
        //close desert
        DESERT.SetActive(false);
    }


    public void grasstoselectgrass()
    {
        grass.SetActive(false);
        selectgrass.SetActive(true);
    }

    public void deserttoselectdesert()
    {
        desert.SetActive(false);
        selectdesert.SetActive(true);
    }

    public void snowtoselectsnow()
    {
        snowfield.SetActive(false);
        selectsnow.SetActive(true);
    }


    public void grasstodesert()
    {
        if (selectgrass.activeSelf)
        {
            selectgrass.SetActive(false);
            grass.SetActive(true);
        }
        GRASS.SetActive(false);
        DESERT.SetActive(true);
    }

    public void deserttograss()
    {
        if (selectdesert.activeSelf)
        {
            selectdesert.SetActive(false);
            desertunlock.SetActive(true);
        }
        DESERT.SetActive(false);
        GRASS.SetActive(true);
    }

    public void deserttosnow()
    {
        if (selectdesert.activeSelf)
        {
            selectdesert.SetActive(false);
            desertunlock.SetActive(true);
        }
        DESERT.SetActive(false);
        SNOW.SetActive(true);
    }

    public void snowtodesert()
    {
        if (selectsnow.activeSelf)
        {
            selectsnow.SetActive(false);
            snowunlock.SetActive(true);
        }
        SNOW.SetActive(false);
        DESERT.SetActive(true);
    }




    //startgamebutton
    public void startgamebutton()
    {
        //close main page
        mainpage.SetActive(false);
        //open game page
        story1.SetActive(true);
    }

    //1to2
    public void story1tostory2()
    {
        //open story1 page
        story2.SetActive(true);

        //close story2 page
        story1.SetActive(false);
    }

    //1tomain
    public void story1backmain()
    {
        //open main page
        mainpage.SetActive(true);

        //close story1 page
        story1.SetActive(false);
    }


    //2to3
    public void story2tostory3()
    {
        //open story3 page
        story3.SetActive(true);

        //close story2 page
        story2.SetActive(false);
    }
    //2tomain
    public void story2backmain()
    {
        //open main page
        mainpage.SetActive(true);

        //close story2 page
        story2.SetActive(false);
    }

    //2to1
    public void story2tostory1()
    {
        //open story1 page
        story1.SetActive(true);

        //close story2 page
        story2.SetActive(false);
    }


    //3to4
    public void story3tostory4()
    {
        //open story4 page
        story4.SetActive(true);

        //close story3 page
        story3.SetActive(false);
    }
    //3tomain
    public void story3backmain()
    {
        //open main page
        mainpage.SetActive(true);

        //close story2 page
        story3.SetActive(false);
    }
    //3to2
    public void story3tostory2()
    {
        //open story2 page
        story2.SetActive(true);

        //close story3 page
        story3.SetActive(false);
    }
    //4to3
    public void story4tostory3()
    {
        //open story3 page
        story3.SetActive(true);

        //close story4 page
        story4.SetActive(false);
    }
    //4tomain
    public void story4backmain()
    {
        //open main page
        mainpage.SetActive(true);

        //close story2 page
        story4.SetActive(false);
    }
    //4tomap
    public void story4tomap()
    {
        //open grass map page
        GRASS.SetActive(true);

        //close story4 page
        story4.SetActive(false);
    }

    public void controlsToSkills()
    {
        // close controls
        controls.SetActive(false);
        skills.SetActive(true);
    }

    public void skillsToControls()
    {
        skills.SetActive(false);
        controls.SetActive(true);
    }

    public void chestsToSkills()
    {
        // close chest
        chests.SetActive(false);
        // open skills
        skills.SetActive(true);

        // open health
        health.SetActive(true);

        // close other chests
        attack.SetActive(false);
        defence.SetActive(false);
        invincible.SetActive(false);
    }

    public void skillsToChests()
    {
        // close skills
        skills.SetActive(false);
        // open chests
        chests.SetActive(true);
        health.SetActive(true);
    }
    
    public void controlsToChests()
    {
        // close controls
        controls.SetActive(false);
        // open chests
        chests.SetActive(true);
        health.SetActive(true);
    }

    public void chestsToControls()
    {
        // close chest
        chests.SetActive(false);
        // open controls
        controls.SetActive(true);

        // open health
        health.SetActive(true);

        // close other chests
        attack.SetActive(false);
        defence.SetActive(false);
        invincible.SetActive(false);
    }

    public void healthToAttack()
    {
        health.SetActive(false);
        attack.SetActive(true);
    }

    public void attackToHealth()
    {
        health.SetActive(true);
        attack.SetActive(false);
    }

    public void attackToDefence()
    {
        attack.SetActive(false);
        defence.SetActive(true);
    }

    public void defenceToAttack()
    {
        attack.SetActive(true);
        defence.SetActive(false);
    }

    public void defenceToInvincible()
    {
        defence.SetActive(false);
        invincible.SetActive(true);
    }

    public void invincibleToDefence()
    {
        defence.SetActive(true);
        invincible.SetActive(false);
    }
}