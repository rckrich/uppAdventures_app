using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Presenter : MonoBehaviour
{
    protected ViewModel viewModel;
    protected Interactor interactor;

    public virtual void Initialize<TViewModel>(ViewModel viewModel) where TViewModel : ViewModel
    {
        if (this.viewModel == null)
            this.viewModel = (TViewModel)viewModel;
    }
    public virtual void Initialize<TViewModel, TPresenter, TInteractor>(ViewModel viewModel) where TViewModel : ViewModel where TPresenter : Presenter where TInteractor : Interactor{
        if (this.viewModel == null)
            this.viewModel = (TViewModel)viewModel;

        if (interactor == null)
        {
            interactor = gameObject.AddComponent(typeof(TInteractor)) as TInteractor;
            interactor.Initialize<TPresenter>(this);
        }
    }
    public virtual void CallInteractor(params object[] list) { }
    public virtual void OnResult(params object[] list) {
        viewModel.DisplayOnResult(list);
    }
    public virtual void OnFailedResult(params object[] list) { }
    public virtual void OnNetworkError(params object[] list) { }
    public virtual void OnServerError(params object[] list) { }
    protected void AddEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.AddListener<T>(listener);
    }

    protected void InvokeEvent<T>(T e) where T : AppEvent
    {
        EventManager.instance.InvokeEvent<T>(e);
    }

    protected void RemoveEventListener<T>(EventManager.EventDelegate<T> listener) where T : AppEvent
    {
        EventManager.instance.RemoveListener<T>(listener);
    }
}
