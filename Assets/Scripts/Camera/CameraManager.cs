using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PWS.Input;
using UnityEngine;

namespace PWS.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        public ThirdPersonCamera Camera;


        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
           
        }
    }
}