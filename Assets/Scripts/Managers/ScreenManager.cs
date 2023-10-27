using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Manager {

    private const int MIN_BACK_STACK_COUNT = 1;

    private Stack<ViewModel> backViewStack;

    [Header("Views Array")]
    [SerializeField]
    private ViewModel[] views = null;

    [Header("Current View")]
    [SerializeField]
    private ViewModel currentView = null;

    [Header("Header View")]
    [SerializeField]
    private ViewModel headerView = null;

    private static ScreenManager _instance;

    public static ScreenManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ScreenManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        backViewStack = new Stack<ViewModel>();
    }

    public void BackToPreviousView()
    {
        if (backViewStack.Count >= MIN_BACK_STACK_COUNT) {
            currentView.SetActive(false);
            currentView = backViewStack.Peek();
            backViewStack.Pop();
        }
        
    }

    public void ChangeView(ViewID viewID, bool isSubView = false)
    {
        SelectedChangeOfViews(viewID, currentView, views, backViewStack, isSubView);   
    }

    public void CloseView(ViewID viewID, bool isUpper, bool isSubView = false)
    {
        if (isSubView)
            backViewStack.Push(currentView);

        foreach (ViewModel viewInstance in views)
        {
            if (((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).GetViewID() == viewID)
            {
                //Close View;
                ((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).SetActive(false);
            }
        }
    }

    public ViewModel GetView(ViewID viewID)
    {
        foreach (ViewModel viewInstance in views)
        {
            if (((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).GetViewID() == viewID)
            {
                return viewInstance;
            }
        }
        
        return null;
    }

    public ViewModel GetCurrentView() {
        return currentView;
    }

    public ViewModel GetHeaderView() {
        return headerView;
    }

    public bool HasSubViews() {
        if (backViewStack.Count <= 0)
        {
            return false;
        }
        else {
            return true;
        }
    }

    private void SelectedChangeOfViews(ViewID viewID, ViewModel currentView, ViewModel[] views, Stack<ViewModel> viewStack, bool isSubView = false) {

        if (isSubView)
        {
            if (currentView != null) {
                if (viewID != ((ViewModel)(currentView.GetComponent(typeof(ViewModel)))).GetViewID())
                    viewStack.Push(currentView);
            }
        }
        else
        {
            ClearViews();
            viewStack.Clear();
        }


        foreach (ViewModel viewInstance in views)
        {
            if (((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).GetViewID() == viewID)
            {
                this.currentView = viewInstance;

                //Open current View
                ((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).SetActive(true);
            }
        }
    }

    private void ClearViews()
    {
        foreach (ViewModel viewInstance in views)
        {
            //Close upper viewInstance
            ((ViewModel)(viewInstance.GetComponent(typeof(ViewModel)))).SetActive(false);
        }
    }

}
