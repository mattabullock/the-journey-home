using UnityEngine;
using System.Collections;

public class CameraSystem : SystemBase {
    
    bool stateChanged = false;
    
    // Use this for initialization
    protected override void Start () {
        base.Start ();
        down = true;
        currentHitPoints = 0;
    }
    
    
    
    // Update is called once per frame
    protected override void Update () {
        base.Update ();
        if (!down && stateChanged) {
            SpawnManager.mapEnemy = true;
            SpawnManager.maskChanged = true;
            stateChanged = false;
        } else if(down && stateChanged) {
            SpawnManager.mapEnemy = false;
            SpawnManager.maskChanged = true;
            stateChanged = false;
        }
    }
    
    protected override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        base.OnPhotonSerializeView (stream, info);
        if (stream.isWriting) {
            
        } else if (stream.isReading) {
            
        }
    }
    
    [RPC]
    protected override void repair (float amt) {
        if (currentHitPoints >= hitPoints) {
            return; 
        } else if (currentHitPoints + amt > hitPoints) {
            currentHitPoints = hitPoints;
        } else if (currentHitPoints + amt > threshold && down) {
            down = false;
            stateChanged = true;
            currentHitPoints += amt;
            belowThresh = false;
            //          StartCoroutine (trigger(flickerOn));
        } else {
            currentHitPoints += amt;
        }
        
        currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
    }
    
    [RPC]
    protected override void TakeDamage(float amt) {
        
        if (currentHitPoints <= 0) {
            return;
        } else if (currentHitPoints - amt <= 0) {
            currentHitPoints = 0;
            if(!belowThresh) {
                stateChanged = true;
                down = true;
                //              StartCoroutine (trigger(flickerOff));
                belowThresh = true;
            }
        } else {
            if(!belowThresh) {
                //              StartCoroutine (trigger(flickerOn));
            }
            currentHitPoints -= amt;
        }   
        
        currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
    }
    
    protected IEnumerator trigger(float times) {
        bool curr = false;
        float i = 0f;
        float start = times / 100f;
        while (i < times) {
            yield return new WaitForSeconds (start - (i * .01f));
            down = curr;
            curr = !curr;
            i++;
        }
    }
    
}
