using System.Linq.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// COMPLETE mess. Refactor/fix later.
public class PowerUpSelector : MonoBehaviour
{
    
    GameManager gameManager;
    FireMissile playerMissiles;

    [SerializeField] GameObject leftDrone;
    [SerializeField] GameObject rightDrone;
    [SerializeField] Vector3 powerUpAttachPoint;

    public float baseDroneRespawnTime = 3f;

    public List<GameObject> powerUpsUIList;
    GameObject leftDronePowerUpUI;
    GameObject rightDronePowerUpUI;

    Animator leftDroneAnim;
    Animator rightDroneAnim;

    List<PowerUp> availablePowerUps;
    PowerUp leftDronePowerUp;
    PowerUp rightDronePowerUp;    

    public string Name { get; set; }

    bool allowPowerUpChoice;

    public int totalPowerUpsAcquired;


    void Start()
    {
        gameManager = GameManager.Instance;
        playerMissiles = FireMissile.Instance;
    

        leftDroneAnim = leftDrone.GetComponent<Animator>();
        rightDroneAnim = rightDrone.GetComponent<Animator>();

        AddPowerUps();
        // StartCoroutine(SpawnDrones());
    }


    void Update()
    {
        // Debug.Log(allowPowerUpChoice);
        if (!allowPowerUpChoice)
            return;

        if (Input.GetMouseButton(0))
            StartCoroutine(ExecuteChosenPowerUp(leftDrone, leftDronePowerUp, leftDronePowerUpUI));
        
        if (Input.GetMouseButton(1))
            StartCoroutine(ExecuteChosenPowerUp(rightDrone, rightDronePowerUp, rightDronePowerUpUI));
    }


    public void AddPowerUps()
    {
        availablePowerUps = new List<PowerUp>();

        availablePowerUps.Add(new PowerUp("FireRateBoost"));
        // availablePowerUps.Add(new PowerUp("FireRateSuperBoost"));
        availablePowerUps.Add(new PowerUp("MissilesPierce"));

        availablePowerUps.Add(new PowerUp("ExplodeEnemies"));
        // availablePowerUps.Add(new PowerUp("ChainExplodeEnemies"));

        availablePowerUps.Add(new PowerUp("BlackHoleOnDeath"));

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.tag == "PowerUp")
                powerUpsUIList.Add(child.gameObject);
            // child.gameObject.SetActive(false);
        }
    }


public IEnumerator SpawnDrones()
{
    ResetDroneAnimationTriggers();
    // Debug.Log("drone spawned");
    PowerUp currentPowerUpLeft, currentPowerUpRight;
    GetRandomPowerUp(out currentPowerUpLeft, out currentPowerUpRight);
    // Debug.Log($"SpawnDrones. left powerUp = {currentPowerUpLeft.Name}. right powerUp = {currentPowerUpRight.Name}");

    leftDronePowerUp = currentPowerUpLeft;
    rightDronePowerUp = currentPowerUpRight;

            // Debug.Log("spawning... " +leftDronePowerUp );
            // Debug.Log("spawning... " +rightDronePowerUp );
            // Debug.Log("spawning... " +leftDronePowerUp.Name );
            // Debug.Log("spawning... " +rightDronePowerUp.Name );

        leftDronePowerUpUI = GetUIOfPowerUp(leftDronePowerUp);
        rightDronePowerUpUI = GetUIOfPowerUp(rightDronePowerUp);

    yield return SetupUI(leftDronePowerUpUI, leftDrone);
    yield return SetupUI(rightDronePowerUpUI, rightDrone);

    // Debug.Log("drone engaged");
    // Debug.Break();
    leftDroneAnim.SetTrigger("engage");
    rightDroneAnim.SetTrigger("engage");

    float droneToPlayerEngageLengths = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;

    yield return new WaitForSeconds(droneToPlayerEngageLengths);

    // Player can finally now choose a power-up.    
    allowPowerUpChoice = true;
}

    void GetRandomPowerUp(out PowerUp currentPowerUpLeft, out PowerUp currentPowerUpRight)
    {
                // Debug.Log("available powerups = " + Random.Range(0, availablePowerUps.Count));
        // Debug.Break();
        int randomIndex = Random.Range(0, availablePowerUps.Count);        
        currentPowerUpLeft = availablePowerUps[randomIndex];

        // Remove, then add it back, so that right powerUp can't select what left selected
        availablePowerUps.Remove(currentPowerUpLeft);

        randomIndex = Random.Range(0, availablePowerUps.Count);
        currentPowerUpRight = availablePowerUps[randomIndex];

        availablePowerUps.Add(currentPowerUpLeft);
    }        

    // Not how I wanted to do it but I need this done and fast.
    GameObject GetUIOfPowerUp(PowerUp powerUp)
    {
        // Debug.Log("GetUIOfPowerUp called");
        foreach (GameObject powerUpUI in powerUpsUIList)
        {            
            switch (powerUp.Name)
            {
                case "FireRateBoost":
                    var currentFireRate = playerMissiles.fireRateMultiplier;

                    if (currentFireRate == 1 && powerUpUI.name == "FireRateBoost UI (2x)")
                        return powerUpUI;
                    break;

                case "FireRateSuperBoost":
                    var currentFireRate_super = playerMissiles.fireRateMultiplier;

                    if (currentFireRate_super == 2 && powerUpUI.name == "FireRateBoost UI (3x)")
                        return powerUpUI;
                    break;            


                case "ExplodeEnemies":
                    var explodeChains = playerMissiles.explosionChains;

                    if (explodeChains == 0 && powerUpUI.name == "ExplodeEnemies UI")
                        return powerUpUI;
                    break;
                    
                case "ChainExplodeEnemies":
                    var explodeChains_super = playerMissiles.explosionChains;

                    if (explodeChains_super == 1 && powerUpUI.name == "ExplodeEnemies (More) UI")
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
        // return;
        // Debug.Log("drone triggers all reset");
        // return;
        leftDroneAnim.ResetTrigger("engage");
        leftDroneAnim.ResetTrigger("disengage");
        rightDroneAnim.ResetTrigger("engage");
        rightDroneAnim.ResetTrigger("disengage");
    }



    IEnumerator SetupUI(GameObject powerUpUI, GameObject drone)
    {
        // Debug.Log(powerUpUI);
        // Debug.Log(powerUpUI.name);

        // Debug.Log($"power up UI = {powerUpUI.name}");
        powerUpUI.SetActive(true);
        powerUpUI.transform.SetParent(drone.transform);

        powerUpUI.transform.localPosition = new Vector3(powerUpAttachPoint.x, powerUpAttachPoint.y, powerUpAttachPoint.z);            
        yield break;
    }



    IEnumerator ExecuteChosenPowerUp(GameObject drone, PowerUp powerUp, GameObject powerUpUI)
    {
        // Debug.Log("executed power up");
        allowPowerUpChoice = false;
        totalPowerUpsAcquired++;

        MissilePiercingWasSelected(powerUp);
        MissileFireRateWasSelected(powerUp);
        EnemyExplosionWasSelected(powerUp);
        BlackHoleWasSelected(powerUp);

        // var test = transform.TransformPoint(powerUpAttachPoint);
        Vector3 detachPointWithOffset = new Vector3(drone.transform.position.x, drone.transform.position.y - 0.6f, drone.transform.position.z);        
        yield return StartCoroutine(powerUpUI.GetComponent<DroppedPowerUpMovesToPlayer>().Move(detachPointWithOffset, this.gameObject));
        GetComponent<AudioSource>().Play();

        // powerUpUI.GetComponent<DroppedPowerUpMovesToPlayer>().StartCoroutine("Move", drone.transform.localPosition, this.gameObject);
        // powerUpUI.SetActive(false);
        
        Debug.Log("drone disengaged");
        leftDroneAnim.SetTrigger("disengage");
        rightDroneAnim.SetTrigger("disengage");
        float droneDisengageLength = leftDroneAnim.GetCurrentAnimatorStateInfo(0).length;

        yield return StartCoroutine(ReturnPowerUpUIsToPowerUpUIList(droneDisengageLength));

        if (totalPowerUpsAcquired < 5)
        {
            // ResetDroneAnimationTriggers();
            yield return new WaitForSeconds(baseDroneRespawnTime);
            StartCoroutine(SpawnDrones());
        }
        else
            StopAllCoroutines();

        yield break;
    }

    IEnumerator WaitForSomething(float length)
    {
        // Debug.Log($"waiting {length} seconds");
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
        if (selectedPowerUp.Name == "FireRateBoost" || selectedPowerUp.Name == "FireRateSuperBoost")
        {
            playerMissiles.fireRateMultiplier++;
            availablePowerUps.Remove(selectedPowerUp);

            if (selectedPowerUp.Name == "FireRateBoost")
            {
                GameObject fireEvenFaster = GameObject.Find("FireRateBoost UI (3x)");
                fireEvenFaster.transform.SetParent(gameObject.transform);

                powerUpsUIList.Add(fireEvenFaster);
                availablePowerUps.Add(new PowerUp("FireRateSuperBoost"));
            }
        }               
    }


    void EnemyExplosionWasSelected(PowerUp selectedPowerUp)
    {
        if (selectedPowerUp.Name == "ExplodeEnemies" || selectedPowerUp.Name == "ChainExplodeEnemies")
        {
            playerMissiles.explosionChains++;
            availablePowerUps.Remove(selectedPowerUp);

            if (selectedPowerUp.Name == "ExplodeEnemies")
            {
                GameObject explodeMore = GameObject.Find("ExplodeEnemies (More) UI");
                explodeMore.transform.SetParent(gameObject.transform);

                powerUpsUIList.Add(explodeMore);
                availablePowerUps.Add(new PowerUp("ChainExplodeEnemies"));
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