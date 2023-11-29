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

    public float droneRespawnTime = 3f;

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
        StartCoroutine(SpawnDrones());
    }


    void Update()
    {
        if (!allowPowerUpChoice)
            return;

        if (Input.GetMouseButton(0))
            ExecuteChosenPowerUp(leftDrone, leftDronePowerUp, leftDronePowerUpUI);
        
        if (Input.GetMouseButton(1))
            ExecuteChosenPowerUp(rightDrone, rightDronePowerUp, leftDronePowerUpUI);
    }


    public void AddPowerUps()
    {
        availablePowerUps = new List<PowerUp>();

        availablePowerUps.Add(new PowerUp("FireRateBoost"));
        availablePowerUps.Add(new PowerUp("MissilesPierce"));
        availablePowerUps.Add(new PowerUp("ExplodeEnemies"));
        availablePowerUps.Add(new PowerUp("BlackHoleOnDeath"));

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "PowerUp")
                powerUpsUIList.Add(child.gameObject);
            // child.gameObject.SetActive(false);
        }
    }


IEnumerator SpawnDrones()
{
    Debug.Log("spawned");
    ResetDroneAnimationTriggers();

    PowerUp currentPowerUpLeft, currentPowerUpRight;
    GetRandomPowerUp(out currentPowerUpLeft, out currentPowerUpRight);

    leftDronePowerUpUI = GetUIOfPowerUp(currentPowerUpLeft);
    SetupUI(leftDronePowerUpUI, leftDrone);  // Pass left drone as the parent

    rightDronePowerUpUI = GetUIOfPowerUp(currentPowerUpRight);
    SetupUI(rightDronePowerUpUI, rightDrone);  // Pass right drone as the parent

    leftDroneAnim.SetTrigger("engage");
    rightDroneAnim.SetTrigger("engage");
    float dronePlayerEngageLength = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;

    yield return WaitForSomething(dronePlayerEngageLength);

    // Player can finally now choose a power-up.
    allowPowerUpChoice = true;
}

        // IEnumerator SpawnDrones()
        // {
        //     Debug.Log("spawned");
        //     ResetDroneAnimationTriggers();

        //     PowerUp currentPowerUpLeft, currentPowerUpRight;
        //     GetRandomPowerUp(out currentPowerUpLeft, out currentPowerUpRight);

        //     // Debug.Log(currentPowerUpLeft.Name);
        //     // Debug.Log(currentPowerUpRight.Name);

        //     leftDronePowerUpUI = powerUpsUIList[0];
        //     yield break;
        //     GetUIOfPowerUp(currentPowerUpLeft, leftDronePowerUpUI);
        //     GetUIOfPowerUp(currentPowerUpRight, rightDronePowerUpUI);

        //     SetupUI(leftDronePowerUpUI);
        //     SetupUI(rightDronePowerUpUI);


        //     // GetUIOfPowerUp(currentPowerUpLeft, currentPowerUpRight);
        //     //     SetupUI(leftDronePowerUpUI);
        // //     SetupUI(rightDronePowerUpUI);

        //     leftDroneAnim.SetTrigger("engage");
        //     rightDroneAnim.SetTrigger("engage");
        //     float dronePlayerEngageLength = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;

        //     yield return WaitForSomething(dronePlayerEngageLength);

        //     // Player can finally now choose a power-up.
        //     allowPowerUpChoice = true;
        // }
void GetRandomPowerUp(out PowerUp currentPowerUpLeft, out PowerUp currentPowerUpRight)
{
    int randomIndex = Random.Range(0, availablePowerUps.Count);
    currentPowerUpLeft = availablePowerUps[randomIndex];
    availablePowerUps.Remove(currentPowerUpLeft);

    randomIndex = Random.Range(0, availablePowerUps.Count);
    currentPowerUpRight = availablePowerUps[randomIndex];
    availablePowerUps.Add(currentPowerUpLeft);  // Note: Add back the removed power-up
}        

        // void GetRandomPowerUp(out PowerUp currentPowerUpLeft, out PowerUp currentPowerUpRight)
        // {
        //     Debug.Log("aaa");
        //                 Debug.Log(Random.Range(0, availablePowerUps.Count));
        //                 Debug.Log("bbb");
        //     int randomIndex = Random.Range(0, availablePowerUps.Count);
        //     Debug.Log("ccc");
            
        //         currentPowerUpLeft = availablePowerUps[randomIndex];
        //         Debug.Log("ddd");

        //     availablePowerUps.Remove(currentPowerUpLeft);
        //         Debug.Log("eee");


        //                 // Debug.Log(Random.Range(0, availablePowerUps.Count + 1));
                        
        //     randomIndex = Random.Range(0, availablePowerUps.Count);
        //         Debug.Log("fff");

    
        //         currentPowerUpRight = availablePowerUps[randomIndex];
        //         Debug.Log("ggg");

        //     availablePowerUps.Add(currentPowerUpLeft);
        //     Debug.Log("hhh");
        // }
GameObject GetUIOfPowerUp(PowerUp powerUp)
{
    switch (powerUp.Name)
    {
        case "FireRateBoost":
            return playerMissiles.fireRateMultiplier == 0 ? powerUpsUIList[0] : powerUpsUIList[1];
        case "MissilesPierce":
            return powerUpsUIList[2];
        case "ExplodeEnemies":
            return playerMissiles.explosionChains == 0 ? powerUpsUIList[3] : powerUpsUIList[4];
        case "BlackHoleOnDeath":
            return powerUpsUIList[5];
        default:
            return null;
    }
}        

        // void GetUIOfPowerUp(PowerUp leftPowerUp, PowerUp rightPowerUp)
        // {
        //     // GetUIOfPowerUp(leftPowerUp, leftDronePowerUpUI);
        //     // GetUIOfPowerUp(rightPowerUp, rightDronePowerUpUI);

        //     SetupUI(leftDronePowerUpUI);
        //     SetupUI(rightDronePowerUpUI);
        // }

    void ResetDroneAnimationTriggers()
    {
        leftDroneAnim.ResetTrigger("engage");
        leftDroneAnim.ResetTrigger("disengage");
        rightDroneAnim.ResetTrigger("engage");
        rightDroneAnim.ResetTrigger("disengage");
    }




    // Really don't wanna do it this way but I need the UI working, and fast.
    void GetUIOfPowerUp(PowerUp powerUp, GameObject powerUpUI)
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

void SetupUI(GameObject powerUpUI, GameObject drone)
{
    powerUpUI.SetActive(true);
    powerUpUI.transform.parent = drone.transform;  // Parent to the specified drone
    powerUpUI.transform.localPosition = new Vector3(0, 3f, -1.5f);
}


    // void SetupUI(GameObject powerUpUI)
    // {
    //     powerUpUI.SetActive(true);
    //     powerUpUI.transform.parent = gameObject.transform.GetChild(0);
    //     powerUpUI.transform.localPosition = new Vector3(0, 3f, -1.5f);        
    // }


    void ExecuteChosenPowerUp(GameObject drone, PowerUp powerUp, GameObject powerUpUI)
    {
        allowPowerUpChoice = false;

        MissilePiercingWasSelected(powerUp);
        MissileFireRateWasSelected(powerUp);
        EnemyExplosionWasSelected(powerUp);
        BlackHoleWasSelected(powerUp);

        powerUpUI.transform.parent = drone.transform;

        // Animator animatorOfPowerUp = drone.GetComponentInChildren<Animator>().play
        // animatorOfPowerUp.SetTrigger("PowerUpMovesToPlayer");
        // float lengthUntilMovedToPlayer = animatorOfPowerUp.GetCurrentAnimatorStateInfo(0).length;

        // powerUp flashes, plays an animation(?), comes into player.
        
        // Animator powerUpAnim = powerUp.gameObject.GetComponent<Animator>();

        // GetCurrentAnimatorStateInfo
        // StartCoroutine(WaitForSomething(powerUp.Animator))

        StartCoroutine(WaitForSomething(2f));
        powerUpUI.transform.parent = gameObject.transform;
        powerUpUI.SetActive(false);

        leftDroneAnim.SetTrigger("disengage");
        rightDroneAnim.SetTrigger("disengage");

        Invoke("SpawnDrones", droneRespawnTime);
    }

    IEnumerator WaitForSomething(float length)
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