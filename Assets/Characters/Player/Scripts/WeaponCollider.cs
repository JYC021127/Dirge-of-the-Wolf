using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    // Start is called before the first frame update

    public Animals self;
    public List<string> tags;

    private List<GameObject> targets = new List<GameObject>();

    public void OnDisable()
    {
        targets.Clear();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(targets);
        if (targets.Contains(other.gameObject)) return;
        targets.Add(other.gameObject);
        foreach (string tag in tags) {
            if (other.gameObject.tag == tag)
            {
                // claimed a chest
                if (tag == "Chest")
                {
                    player.GetComponent<PlayerAudio>().claimed = true;
                    player.GetComponent<PlayerAudio>().Attack();
                    // Health chest - health will not exceed max health
                    if (other.gameObject.name.StartsWith("Health")) 
                    {
                        Animals playerScript = player.GetComponent<Animals>();
                        float maxPlayerHealth = playerScript.GetMaxHealth();
                        float addHealth = other.gameObject.GetComponent<HealthChest>().addHealth;
                        if (playerScript.Health + addHealth >= maxPlayerHealth)
                        {
                            playerScript.Health = maxPlayerHealth;
                        }
                        else {
                            playerScript.Health += addHealth;
                        }
                    }
                    // Invincible chest - player becomes invincible for 10 seconds
                    if (other.gameObject.name.StartsWith("Invincible"))
                    {
                        player.GetComponent<Animals>().SetPlayerInvinsible(true);
                    }
                    // Attack chest - increase player damage by 10% for 20 seconds, this inlcudes wolf spirits in boss fight
                    if (other.gameObject.name.StartsWith("Attack"))
                    {
                        player.GetComponent<Animals>().SetPlayerAttackBuff(true);
                    }
                    // Defence chest - decrease enemy damage by 10%, this includes boss damage in boss fight
                    if (other.gameObject.name.StartsWith("Defence"))
                    {
                        player.GetComponent<Animals>().SetPlayerDefenceBuff(true);
                    }
                    other.gameObject.name = "Claimed";
                }

                Animals enemy = other.gameObject.GetComponent<Animals>();
                if (enemy!=null)
                {
                    enemy.TakeDamage(self.Damage);  // self is player if enemy is "Enemy"
                }
                
                else
                {
                    Debug.LogWarning("Animals component not found on the collided object.");
                }
                break;
            }
        }
    }

}
