using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WSpellCooldown : MonoBehaviour
{
    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private TMP_Text textCooldown;

    //variables for cooldownTimer
    private bool isCooldown = false;
    private float cooldownTime = 10.0f;
    private float cooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Abilities should be available at the start of game, therefore hide the cooldown image
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UseSpell();
        }

        if (isCooldown)
        {
            ApplyCooldown();
        }
    }

    void ApplyCooldown()
    {
        //subtrack time since last called
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0.0f)
        {
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    public void UseSpell()
    {
        if (isCooldown)
        {
            // if the spell is on cooldown, it cannot be used
            //TODO: add a sound effect so this is more clear to the player!
        }

        else
        {
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
        }
    }

    void CooldownImageOff()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

}

