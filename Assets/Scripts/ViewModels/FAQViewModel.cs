using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FAQViewModel : ViewModel
{
    [Header("General")]
    public Transform content;
    public Animator animator;

    private void OnEnable()
    {
        content.localPosition = new Vector3(content.localPosition.x, 0f);
    }

    public override void Initialize(params object[] list)
    {
        SetHeaderAction();
    }

    public override void SetHeaderAction()
    {
      
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            //ScreenManager.instance.BackToPreviousView();
            //ScreenManager.instance.GetView(ViewID.ProfileViewModel).SetHeaderAction();
            StartCoroutine(CloseRoutine());
        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetBackSprite();
        
    }

    IEnumerator CloseRoutine()
    {
        animator.SetTrigger("Activate");

        yield return new WaitForSeconds(0.35f);

        ScreenManager.instance.BackToPreviousView();
        ScreenManager.instance.GetView(ViewID.ProfileViewModel).SetHeaderAction();
    }
}
