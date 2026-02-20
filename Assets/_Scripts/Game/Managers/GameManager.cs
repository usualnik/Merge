using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnScoreChanged;
    public event Action OnCoinsChanged;
    public event Action OnHintsChanged;
    public event Action<RecepieDataSO> OnNewRecepieFound;

    [SerializeField] private int _score;
    [SerializeField] private int _coins;
    [SerializeField] private int _hints;

    private HashSet<int> _recepieFoundIndexes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null, false);
        }
        else
            Destroy(gameObject);

        _recepieFoundIndexes = new HashSet<int>();
    }

    private void Start()
    {
        MixingManager.Instance.OnRecepieFound += MixingManager_OnRecepieFound;
    }
    private void OnDestroy()
    {
        MixingManager.Instance.OnRecepieFound -= MixingManager_OnRecepieFound;

    }

    private void MixingManager_OnRecepieFound(RecepieDataSO obj)
    {
        HandleRecepieFound(obj);
    }

    private void HandleRecepieFound(RecepieDataSO recepieFoundData)
    {
        if (recepieFoundData == null || _recepieFoundIndexes.Contains(recepieFoundData.RecepieID)) return;
     
        _score++;
        OnScoreChanged?.Invoke();

        _recepieFoundIndexes.Add(recepieFoundData.RecepieID);
        OnNewRecepieFound?.Invoke(recepieFoundData);
    }
    

    public int Score => _score;
    public int Coins => _coins;
    public int Hints => _hints;
    public HashSet<int> RecepieFoundIndexes => _recepieFoundIndexes;

}
