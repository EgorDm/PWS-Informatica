using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PWS.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private Transform[] _spawnPoints;

        private void Start()
        {
            foreach (var networkCharacter in GameManager.Players)
            {
                networkCharacter.Setup();
            }
        }

    }
}
