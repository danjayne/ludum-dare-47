using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayDeathSound(animator.gameObject);

        Destroy(animator.gameObject, stateInfo.length);
    }

    private static void PlayDeathSound(GameObject destroyThis)
    {
        switch (Utils.PrefabName(destroyThis))
        {
            case "Cauldron":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.SmashCauldron);
                break;
            case "Frankensteins-monster":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.FrankensteinHurt);
                break;
            case "Ghost":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.GhostHurt);
                break;
            case "Spider":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.SpiderHurt);
                break;
            case "Witch":
                AudioManager.Instance.PlaySoundEffect(SoundEffectEnum.WitchHurt);
                break;
        }
    }
}