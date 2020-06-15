using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFading : MonoBehaviour
{
    public Animator animator;
    private int TargetLevelNumber;

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    FadeToLevel(2);
        //}

    }

    public void FadeToLevel(int targetLevel)
    {
        TargetLevelNumber = targetLevel;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(GlobalVars.FirstLevelBuildIndex - 1 + TargetLevelNumber);
    }
}
