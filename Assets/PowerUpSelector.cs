using System.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PowerUpSelector : MonoBehaviour
{
    GameManager gameManager;
    FireMissile playerMissiles;

    public GameObject leftDrone;
    public GameObject rightDrone;

    Animator leftDroneAnim;
    Animator rightDroneAnim;

    GameObject leftDronePowerUpUI;
    GameObject rightDronePowerUpUI;
    public List<GameObject> powerUpsUIList;

    List<PowerUp> availablePowerUps;

    PowerUp leftDronePowerUp;
    PowerUp rightDronePowerUp;    

    public string Name { get; set; }

    bool allowPowerUpChoice;


    void Start()
    {
        gameManager = GameManager.Instance;
        playerMissiles = FireMissile.Instance;

        leftDroneAnim = leftDrone.GetComponent<Animator>();
        rightDroneAnim = rightDrone.GetComponent<Animator>();

        AddPowerUps();
    }


    void Update()
    {
        if (!allowPowerUpChoice)
            return;

        if (Input.GetMouseButton(0))
            ExecuteChosenPowerUp(leftDronePowerUp);
        
        if (Input.GetMouseButton(1))
            ExecuteChosenPowerUp(rightDronePowerUp);
    }


    public void AddPowerUps()
    {
        availablePowerUps = new List<PowerUp>();

        availablePowerUps.Add(new PowerUp("FireRateBoost"));
        availablePowerUps.Add(new PowerUp("MissilesPierce"));
        availablePowerUps.Add(new PowerUp("ExplodeEnemies"));
        availablePowerUps.Add(new PowerUp("BlackHoleOnDeath"));

        foreach (GameObject child in gameObject.transform)
        {
            powerUpsUIList.Add(child);
            child.SetActive(false);
        }

        StartCoroutine(DronesFindRandomPowerUp());
    }


    IEnumerator DronesFindRandomPowerUp()
    {
        ResetDroneAnimationTriggers();

        PowerUp currentPowerUpLeft, currentPowerUpRight;
        DeterminePowerUp(out currentPowerUpLeft, out currentPowerUpRight);        
        DeterminePowerUpUIs(currentPowerUpLeft, currentPowerUpRight);

        leftDroneAnim.SetTrigger("engage");
        rightDroneAnim.SetTrigger("engage");
        float animLengths = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;

        yield return WaitForDronesToReachPlayer(animLengths);

        // Player can finally now choose a power-up.
        allowPowerUpChoice = true;
    }

    private void DeterminePowerUp(out PowerUp currentPowerUpLeft, out PowerUp currentPowerUpRight)
    {
        int randomIndexLeft = Random.Range(0, availablePowerUps.Count + 1);
        currentPowerUpLeft = availablePowerUps[randomIndexLeft];

        // Get second (right side) power-up. Prevent from picking what left-side picked.

        // List<PowerUp> remainingPowerUps = availablePowerUps;
        availablePowerUps.Remove(currentPowerUpLeft);
        int randomIndexRight = Random.Range(0, availablePowerUps.Count + 1);
        availablePowerUps.Add(currentPowerUpLeft);

        currentPowerUpRight = availablePowerUps[randomIndexRight];
    }

    private void DeterminePowerUpUIs(PowerUp currentPowerUpLeft, PowerUp currentPowerUpRight)
    {
        GetUI(currentPowerUpLeft, leftDronePowerUpUI);
        GetUI(currentPowerUpRight, rightDronePowerUpUI);

        SetupUI(leftDronePowerUpUI);
        SetupUI(rightDronePowerUpUI);
    }

    private void ResetDroneAnimationTriggers()
    {
        leftDroneAnim.ResetTrigger("engage");
        leftDroneAnim.ResetTrigger("disengage");
        rightDroneAnim.ResetTrigger("engage");
        rightDroneAnim.ResetTrigger("disengage");
    }




    // Really don't wanna do it this way but I need the UI working, and fast.
    void GetUI(PowerUp powerUp, GameObject powerUpUI)
    {
        switch (powerUp.Name)
        {
            case "FireRateBoost":
            {
                if (playerMissiles.fireRateMultiplier == 0)
                    powerUpUI = powerUpsUIList[0];
                else if (playerMissiles.fireRateMultiplier == 2)
                    powerUpUI = powerUpsUIList[1];
                break;
            }

            case "MissilesPierce":
            {
                powerUpUI = powerUpsUIList[2];
                break;
            }

            case "ExplodeEnemies":
            {
                if (playerMissiles.explosionChains == 0)
                    powerUpUI = powerUpsUIList[3];
                else if (playerMissiles.explosionChains == 1)
                    powerUpUI = powerUpsUIList[4];
                break;
            }

            case "BlackHoleOnDeath":
            {
                powerUpUI = powerUpsUIList[5];
                break;
            }
                       

            default:
                break;
        }
    }


    void SetupUI(GameObject powerUpUI)
    {        
        powerUpUI.SetActive(true);
        powerUpUI.transform.localPosition = new Vector3(0, 3f, -1.5f);        
    }


    void ExecuteChosenPowerUp(PowerUp powerUp)
    {
        allowPowerUpChoice = false;

        MissilePiercingWasSelected(powerUp);
        MissileFireRateWasSelected(powerUp);
        EnemyExplosionWasSelected(powerUp);
        BlackHoleWasSelected(powerUp);

        // powerUp flashes, plays an animation, comes into player.

        leftDroneAnim.SetTrigger("disengage");
        rightDroneAnim.SetTrigger("disengage");        
    }

    IEnumerator WaitForDronesToReachPlayer(float length)
    {
        yield return new WaitForSeconds(length);        
    }


    # region Determine power-up actions.
    void MissilePiercingWasSelected(PowerUp selectedPowerUp)
    {
        if (selectedPowerUp.Name == "MissilesPierce")
        {
            playerMissiles.NewMissilesNowPierce();
            availablePowerUps.Remove(selectedPowerUp);
        }
    }


    void MissileFireRateWasSelected(PowerUp selectedPowerUp)
    {
        if (selectedPowerUp.Name == "FireRateBoost")
        {
            if (playerMissiles.fireRateMultiplier == 1)
                playerMissiles.fireRateMultiplier = 2;

            if (playerMissiles.fireRateMultiplier == 2)
            {
                availablePowerUps.Remove(selectedPowerUp);
                playerMissiles.fireRateMultiplier = 3;
            }
        }
    }


    void EnemyExplosionWasSelected(PowerUp selectedPowerUp)
    {
        if (selectedPowerUp.Name == "ExplodeEnemies")
        {
            if (playerMissiles.explosionChains == 0)
                playerMissiles.explosionChains++;

            if (playerMissiles.explosionChains == 1)
            {
                availablePowerUps.Remove(selectedPowerUp);
                playerMissiles.explosionChains++;
            }
        }
    }


    void BlackHoleWasSelected(PowerUp selectedPowerUp)
    {
        if (selectedPowerUp.Name == "BlackHoleOnDeath")
        {
            PlayerController.Instance.canBecomeBlackHole = true;
            availablePowerUps.Remove(selectedPowerUp);
        }
    }

    #endregion
}


    public class PowerUp
    {
        public string Name { get; set; }


        public PowerUp(string name) =>
            Name = name;
    }