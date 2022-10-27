using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace FusionExamples.Tanknarok
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] GameObject VCDriver;
        [SerializeField] Animator VCAnimator;

        Transform player;
        private bool inited = false;
        public static CameraManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = GetComponent<CameraManager>();
            }
        }

        public void Init(Transform player)
        {
            this.player = player;
            inited = true;
        }

        void Update()
        {
            if (!inited) return;
            VCAnimator.SetFloat("X", player.position.x);
            VCAnimator.SetFloat("Y", player.position.z);
        }
    }
}