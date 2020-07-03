using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFading : MonoBehaviour
{
    public Animator animator;
    private int TargetLevelBuildIndex;

    void Start()
    {
        // Set animator to follow the global FadeInOnLoad's instruction
        animator.enabled = GlobalVars.FadeInOnLoad;
        // Reset FadeInOnLoad to true 
        GlobalVars.FadeInOnLoad = true;
    }

    public void FadeToLevel(int targetLevelindex)
    {
        TargetLevelBuildIndex = targetLevelindex;

        animator.enabled = true;
        animator.SetTrigger("FadeOut");        
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(TargetLevelBuildIndex);
    }
}
