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
            ExecuteChosenPowerUp(rightDrone, rightDronePowerUp, rightDronePowerUpUI);
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
    yield return new WaitForSeconds(droneRespawnTime);
    Debug.Log("spawned");
    ResetDroneAnimationTriggers();

    PowerUp currentPowerUpLeft, currentPowerUpRight;
    GetRandomPowerUp(out currentPowerUpLeft, out currentPowerUpRight);

    leftDronePowerUp = currentPowerUpLeft;
    rightDronePowerUp = currentPowerUpRight;

            Debug.Log("spawning... " +leftDronePowerUp );
            Debug.Log("spawning... " +rightDronePowerUp );
            Debug.Log("spawning... " +leftDronePowerUp.Name );
            Debug.Log("spawning... " +rightDronePowerUp.Name );

        leftDronePowerUpUI = GetUIOfPowerUp(leftDronePowerUp);
        rightDronePowerUpUI = GetUIOfPowerUp(rightDronePowerUp);
    // leftDronePowerUpUI = GetUIOfPowerUp(currentPowerUpLeft);
    // rightDronePowerUpUI = GetUIOfPowerUp(currentPowerUpRight);        

    SetupUI(leftDronePowerUpUI, leftDrone);
    SetupUI(rightDronePowerUpUI, rightDrone);

    leftDroneAnim.SetTrigger("engage");
    rightDroneAnim.SetTrigger("engage");


    float dronePlayerEngageLength = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;
    yield return WaitForSomething(dronePlayerEngageLength);

    // Player can finally now choose a power-up.
    allowPowerUpChoice = true;
}

void GetRandomPowerUp(out PowerUp currentPowerUpLeft, out PowerUp currentPowerUpRight)
{
    int randomIndex = Random.Range(0, availablePowerUps.Count);
    currentPowerUpLeft = availablePowerUps[randomIndex];
    availablePowerUps.Remove(currentPowerUpLeft);

    randomIndex = Random.Range(0, availablePowerUps.Count);
    currentPowerUpRight = availablePowerUps[randomIndex];
    availablePowerUps.Add(currentPowerUpLeft);  // Note: Add back the removed power-up
}        

    // Not how I wanted to do it but I need this done and fast.
    GameObject GetUIOfPowerUp(PowerUp powerUp)
    {
        Debug.Log("GetUIOfPowerUp called");
        foreach (GameObject powerUpUI in powerUpsUIList)
        {            
            switch (powerUp.Name)
            {
                case "FireRateBoost":
                    var currentFireRate = playerMissiles.fireRateMultiplier;
                    Debug.Log("fire rate called");
                    
                    if (currentFireRate == 1 && powerUpUI.name == "FireRateBoost UI (2x)")
                        return powerUpUI;
                    else if (currentFireRate == 2 && powerUpUI.name == "FireRateBoost UI (3x)")
                        return powerUpUI;
                    break;

                case "ExplodeEnemies":
                    Debug.Log("explode enemies  called");
                    var explodeChains = playerMissiles.explosionChains;

                    if (explodeChains == 0 && powerUpUI.name == "ExplodeEnemies UI")
                        return powerUpUI;
                    if (explodeChains == 1 && powerUpUI.name == "ExplodeEnemies (More) UI")                    
                        return powerUpUI;
                    break;

                case "MissilesPierce":
                    if (powerUpUI.name == "MissilesPierce UI")
                        return powerUpUI;
                    break;


                case "BlackHoleOnDeath":
                    if (powerUpUI.name == "BlackHole UI")
                        return powerUpUI;
                    break;
            }
        }

        return null;
    }        

    void ResetDroneAnimationTriggers()
    {
        leftDroneAnim.ResetTrigger("engage");
        leftDroneAnim.ResetTrigger("disengage");
        rightDroneAnim.ResetTrigger("engage");
        rightDroneAnim.ResetTrigger("disengage");
    }



void SetupUI(GameObject powerUpUI, GameObject drone)
{
    Debug.Log(powerUpUI);
    Debug.Log(powerUpUI.name);

    Debug.Log($"power up UI = {powerUpUI.name}");
    powerUpUI.SetActive(true);
    powerUpUI.transform.SetParent(drone.transform);
    // powerUpUI.transform.parent = drone.transform;  // Parent to the specified drone
    powerUpUI.transform.localPosition = new Vector3(0, 3f, -1.5f);
}



    void ExecuteChosenPowerUp(GameObject drone, PowerUp powerUp, GameObject powerUpUI)
    {
        allowPowerUpChoice = false;

        MissilePiercingWasSelected(powerUp);
        MissileFireRateWasSelected(powerUp);
        EnemyExplosionWasSelected(powerUp);
        BlackHoleWasSelected(powerUp);

        StartCoroutine(powerUpUI.GetComponent<DroppedPowerUpMovesToPlayer>().Move(drone.transform.position, this.gameObject));

        // powerUpUI.GetComponent<DroppedPowerUpMovesToPlayer>().StartCoroutine("Move", drone.transform.localPosition, this.gameObject);
        // powerUpUI.SetActive(false);

        
        leftDroneAnim.SetTrigger("disengage");
        rightDroneAnim.SetTrigger("disengage");
        float droneDisengageLength = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(ReturnPowerUpUIsToPowerUpUIList(droneDisengageLength));        

        StartCoroutine(SpawnDrones());
        
    }

    IEnumerator WaitForSomething(float length)
    {
        Debug.Log($"waiting {length} seconds");
        yield return new WaitForSeconds(length);        
    }

    IEnumerator ReturnPowerUpUIsToPowerUpUIList(float droneDisengageLength)
    {
        yield return StartCoroutine(WaitForSomething(droneDisengageLength - 0.1f));
        leftDronePowerUpUI.transform.SetParent(gameObject.transform);
        rightDronePowerUpUI.transform.SetParent(gameObject.transform);
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
            playerMissiles.explosionChains++;
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