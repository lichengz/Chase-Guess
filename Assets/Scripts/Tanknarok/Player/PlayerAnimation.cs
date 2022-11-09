using Fusion;
using Fusion.Editor;
using UnityEngine;

namespace FusionExamples.Tanknarok
{
    [ScriptHelp(BackColor = EditorHeaderBackColor.Green)]
    [RequireComponent(typeof(NetworkMecanimAnimator))]
    public class PlayerAnimation : NetworkBehaviour
    {
        private NetworkMecanimAnimator networkAnimator;
        private Animator localAnimator;

        public override void Spawned()
        {
            base.Spawned();
            networkAnimator = GetComponent<NetworkMecanimAnimator>();
            localAnimator = GetComponentInChildren<Animator>();
            // PlayerMovement.OnAnim += Handle_OnAnim;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            base.Despawned(runner, hasState);
            // PlayerMovement.OnAnim -= Handle_OnAnim;
        }

        public void Run()
        {
            // if (Object.HasInputAuthority)
            // {
            //     localAnimator.SetTrigger("run");
            // }
            // else
            {
                networkAnimator.SetTrigger("run", true);
            }
        }

        public void Stop()
        {
            // if (Object.HasInputAuthority)
            // {
            //     localAnimator.SetTrigger("stop");
            // }
            // else
            {
                networkAnimator.SetTrigger("stop", true);
            }
        }
    }
}