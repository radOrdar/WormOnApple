﻿using System;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelProgression", menuName = "StaticData/LevelProgression", order = 0)]
    public class LevelProgressionData : ScriptableObject
    {
        [SerializeField] private ProgressionUnit[] progressions;

        public ProgressionUnit GetProgression(int level)
        {
            // return progressions[Mathf.Clamp(level, 0, progressions.Length - 1)];
            ProgressionUnit progressionUnit = progressions[0];
            progressionUnit.speed += 2 * Mathf.Log10(level + 1);
            progressionUnit.pickupNum += (int)(20 * Mathf.Sqrt(level + 1));
            progressionUnit.posionNum += (int) (5 * Mathf.Log10(level + 1));
            Debug.Log($"NUMBER OF POISON {progressionUnit.posionNum}");
            Debug.Log($"SNAKE SPEED {progressionUnit.speed}");
            return progressionUnit;
        }
    }

    [Serializable]
    public struct ProgressionUnit
    {
        public float speed;
        public int pickupNum, posionNum;
    }
}