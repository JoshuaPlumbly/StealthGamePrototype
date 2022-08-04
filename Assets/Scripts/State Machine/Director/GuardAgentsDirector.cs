using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class GuardAgentsDirector : MonoBehaviour
{
    // State Machine
    public StateMachine<GuardAgentsDirector> StateMachine { get; set; }

    #region Singlton Deisgn Patten
    static GuardAgentsDirector _instance;
    public static GuardAgentsDirector Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GuardAgentsDirector>();
            }

            return _instance;
        }
    }
    #endregion

    #region Encapulated
    public List<GameObject> GuardList
    {
        get
        {
            return guardList;
        }

        set
        {
            guardList = value;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public float SpawnTime
    {
        get
        {
            return spawnTime;
        }

        set
        {
            spawnTime = value;
        }
    }

    public int SpawnAmmount
    {
        get
        {
            return spawnAmmount;
        }

        set
        {
            spawnAmmount = value;
        }
    }

    public float ReplamentTime
    {
        get
        {
            return replamentTime;
        }

        set
        {
            replamentTime = value;
        }
    }

    public List<Transform> SpawnPoints
    {
        get
        {
            return spawnPoints;
        }

        set
        {
            spawnPoints = value;
        }
    }

    public GameObject GuardPrefab
    {
        get
        {
            return guardPrefab;
        }

        set
        {
            guardPrefab = value;
        }
    }

    public int GuardAlertAmount
    {
        get
        {
            return guardAlertAmount;
        }

        set
        {
            guardAlertAmount = value;
        }
    }

    public Light MainLight
    {
        get
        {
            return mainLight;
        }

        set
        {
            mainLight = value;
        }
    }

    public float NomalLightIntenity
    {
        get
        {
            return nomalLightIntenity;
        }

        set
        {
            nomalLightIntenity = value;
        }
    }

    public Color NomalLightColour
    {
        get
        {
            return nomalLightColour;
        }

        set
        {
            nomalLightColour = value;
        }
    }

    public float AlarmLightIntenity
    {
        get
        {
            return alarmLightIntenity;
        }

        set
        {
            alarmLightIntenity = value;
        }
    }

    public Color AlarmLightColour
    {
        get
        {
            return alarmLightColour;
        }

        set
        {
            alarmLightColour = value;
        }
    }
    #endregion

    [Header("Lightting")]
    [SerializeField]
    private Light mainLight;
    [SerializeField] private float nomalLightIntenity = 1.0f;
    [SerializeField] private Color nomalLightColour = Color.white;
    [SerializeField] private float alarmLightIntenity = 2.2f;
    [SerializeField] private Color alarmLightColour = Color.red;

    [Header("Lists and prefabs")]
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<GameObject> guardList;
    [SerializeField] private GameObject player;

    [Header("Guard Spawn")]
    [SerializeField] private int maxGuardAmount;
    [SerializeField] private float spawnTime;
    [SerializeField] private int spawnAmmount;
    [SerializeField] private float replamentTime;
    [SerializeField] private int guardAlertAmount;

    [Header("Patrol points")]
    [SerializeField] GameObject wantedLocPrefab;

    private Transform wantedLocation;
    public Neutral Neutral { get; private set; }
    public Alert Alert { get; private set; }

    public static event System.Action BackupRequested;

    private void Start()
    {
        // Cheate a new state machine.
        Alert = new Alert();
        Neutral = new Neutral();
        StateMachine = new StateMachine<GuardAgentsDirector>(this);

        // Create pool
        PoolManager.Instance.CreatePool(guardPrefab, 1);
        PoolManager.Instance.CreatePool(wantedLocPrefab, 1);

        // Get Spawn points.
        GameObject[] spawnArray = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject spawnObject in spawnArray)
        {
            spawnPoints.Add(spawnObject.transform);
        }

        // Get player.
        Player = GameObject.FindGameObjectWithTag("Player");

        // Start neutrol state.
        StateMachine.ChanageState(Neutral);
    }

    public static void BackupRequest() => BackupRequested?.Invoke();

    public void Update()
    {
        // Update state machine.
        StateMachine.Update();
    }
}
