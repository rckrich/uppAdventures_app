using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewModel : MonoBehaviour
{
    [SerializeField]
    private ViewID viewID = ViewID.None;
    protected Presenter presenter;
    public System.Action backAction;
    public virtual void Initialize(params object[] list) { }
    public virtual void Initialize<TViewModel, TPresenter>(params object[] list) where TViewModel : ViewModel where TPresenter : Presenter
    {
        if (presenter == null)
        {
            presenter = gameObject.AddComponent(typeof(TPresenter)) as TPresenter;
            presenter.Initialize<TViewModel>(this);
        }
    }
    public virtual void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list) where TViewModel : ViewModel where TPresenter : Presenter where TInteractor : Interactor {
        if (presenter == null)
        {
            presenter = gameObject.AddComponent(typeof(TPresenter)) as TPresenter;
            presenter.Initialize<TViewModel, TPresenter, TInteractor>(this);
        }
    }
    public virtual void CallPresenter(params object[] list) {
        presenter.CallInteractor(list);
    }
    public virtual void DisplayOnResult(params object[] list) { }
    public virtual void DisplayOnFailedResult(params object[] list) { }
    public virtual void DisplayOnNetworkError(params object[] list) { }
    public virtual void DisplayOnServerError(params object[] list) { }
    public virtual void SetActive(bool value) { this.gameObject.SetActive(value); }
    public virtual ViewID GetViewID() { return viewID; }
    public virtual void SetHeaderAction() { }
    protected virtual bool CheckDeviceNetworkReachability() {
        return (Application.internetReachability != NetworkReachability.NotReachable);
    }
}
