using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public Checkpoint[] _checkpoints;
    public Cinemachine.CinemachineFreeLook vCam;
    public float respawnDelayTime = 0.1f;

    [Header("Collectables")]
    public int _goldAcorns = 0;
    public int _normalAcorns = 0;

    public CapitalArea[] areaArray = new CapitalArea[4]; 


    // Start is called before the first frame update
    void Start()
    {
        InitializeAreaArray();
        SceneManager.sceneLoaded += onSceneLoaded;
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);

       

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
       
    }

    void onSceneLoaded(Scene s, LoadSceneMode l)
    {
        
        _checkpoints = FindObjectsOfType<Checkpoint>();

        Debug.Log(s.name);
    }

    void InitializeAreaArray()
    {
        for (int i = 0; i < areaArray.Length; i++)
        {
            areaArray[i] = new CapitalArea();
            switch (i)
            {
                case 0:
                    areaArray[i]._name = "Stronghold";
                    break;
                case 1:
                    areaArray[i]._name = "Dock";
                    break;
                case 2:
                    areaArray[i]._name = "Island";
                    break;
                case 3:
                    areaArray[i]._name = "Courtyard";
                    break;
            }

        }
    }

    public void CollectionListener(Collectable c)
    {
        Type type = c.GetTypeEnum();
        switch (type)
        {
            case Type.Normal:
                _normalAcorns++;
                areaArray[(int)c.GetAreaEnum()]._normal++;
                
                UIManager.instance.UpdateText("Nut_Display_Text", _normalAcorns);
                UIManager.instance.UpdateText(areaArray[(int)c.GetAreaEnum()]._name + " Nut Value", areaArray[(int)c.GetAreaEnum()]._normal);
                break;

            case Type.Gold:
                _goldAcorns++;
                areaArray[(int)c.GetAreaEnum()]._gold++;
                UIManager.instance.UpdateText("Gold_Nut_Display_Text", _goldAcorns);
                UIManager.instance.UpdateText(areaArray[(int)c.GetAreaEnum()]._name + " Gold Acorn Value", areaArray[(int)c.GetAreaEnum()]._gold);
                break;

            case Type.Time:
                break;

            default:
                Debug.LogError("Incorrect Type");
                break;
        }
    }

    public void EnableClocks(bool b)
    {
        Clock[] _clocks = FindObjectsOfType<Clock>();

        foreach (Clock c in _clocks)
        {
            c.EnableInteractions(b);
        }
    }


    Checkpoint GetActiveCheckpoint()
    {
        foreach (Checkpoint cp in _checkpoints)
        {
            if (cp._active == true)
            {
                return cp;
            }
        }
        return null;
    }

    public void Respawn(playermovement player)
    {
        Checkpoint activeCheckpoint = GetActiveCheckpoint();

        if (activeCheckpoint == null)
        {
            _checkpoints[0].SetActiveCheckpoint(true);
            activeCheckpoint = _checkpoints[0];
        }

        StartCoroutine(DelayPlayerMovementonRespawn(respawnDelayTime, player, activeCheckpoint));
    }

    IEnumerator DelayPlayerMovementonRespawn(float delayTime, playermovement player, Checkpoint activeCheckpoint)
    {
        UIManager.instance.StartFade();

        yield return new WaitForSeconds(delayTime);
        
        player.gameObject.transform.position = activeCheckpoint.gameObject.transform.position + Vector3.up * 10;
        player.GetComponent<CharacterController>().enabled = true;
    }

    public void SetActiveCheckpoint(Checkpoint newActiveCheckpoint)
    {
        foreach (Checkpoint cp in _checkpoints)
        {
            if (cp != newActiveCheckpoint)
                cp.SetActiveCheckpoint(false);
        }

        newActiveCheckpoint.SetActiveCheckpoint(true);
    }

    public void LookAtTargetTemp(Transform target)
    {
        StartCoroutine(LookAtTargetTemporarily(target));
    }

    IEnumerator LookAtTargetTemporarily(Transform target)
    {
        vCam.LookAt = target;
        yield return new WaitForSecondsRealtime(2f);
        vCam.LookAt = FindObjectOfType<playermovement>().gameObject.transform;
    }

}

[System.Serializable]
public struct CapitalArea
{
    public int _normal;
    public int _gold;
    public string _name;
}