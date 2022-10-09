using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [SerializeField] Orbuculum[] orbs;
    int answerOrb;
    public bool isClear;
    
    void Awake()
    {
        answerOrb = Mathf.RoundToInt(Random.Range(0, 3));
        orbs[answerOrb].isAnswer = true;
    }
}
