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

        AddPowerUps();
    }


    void Update()
    {
        if (!allowPowerUpChoice)
            return;
            // set true elsewhere later.

        if (Input.GetMouseButton(0))
        {
            allowPowerUpChoice = false;
            ExecuteChosenPowerUp(leftDronePowerUp);
        }
        
        if (Input.GetMouseButton(1))
        {
            allowPowerUpChoice = false;            
            ExecuteChosenPowerUp(rightDronePowerUp);
        }        
    }


    public void AddPowerUps()
    {
        availablePowerUps = new List<PowerUp>();

        availablePowerUps.Add(new PowerUp("FireRateBoost"));
        availablePowerUps.Add(new PowerUp("MissilesPierce"));
        availablePowerUps.Add(new PowerUp("ExplodeEnemies"));
        availablePowerUps.Add(new PowerUp("BlackHoleOnDeath"));

        foreach (GameObject child in gameObject.transform)
            powerUpsUIList.Add(child);

        DronesFindRandomPowerUp();
    }


    void DronesFindRandomPowerUp()
    {
        // Get first (left side) power-up.

        int randomIndexLeft = Random.Range(0, availablePowerUps.Count + 1);
        PowerUp currentPowerUpLeft = availablePowerUps[randomIndexLeft];

        // Get second (right side) power-up. Prevent from picking what left-side picked.

            // List<PowerUp> remainingPowerUps = availablePowerUps;
        availablePowerUps.Remove(currentPowerUpLeft);
        int randomIndexRight = Random.Range(0, availablePowerUps.Count + 1);
        availablePowerUps.Add(currentPowerUpLeft);

        PowerUp currentPowerUpRight = availablePowerUps[randomIndexRight];

        GetUI(currentPowerUpLeft, leftDronePowerUpUI);
        GetUI(currentPowerUpRight, rightDronePowerUpUI);
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


    void ExecuteChosenPowerUp(PowerUp powerUp)
    {
        StartCoroutine(WaitBeforePowerUpToReachPlayer(powerUp));

        MissilePiercingWasSelected(powerUp);
        MissileFireRateWasSelected(powerUp);
        EnemyExplosionWasSelected(powerUp);
        BlackHoleWasSelected(powerUp);
    }

    IEnumerator WaitBeforePowerUpToReachPlayer(PowerUp powerUp)
    {
            // powerup animation
            yield return new WaitForSeconds(0.5f);
        yield break;
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