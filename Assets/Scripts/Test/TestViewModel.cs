using UnityEngine;
using UnityEngine.UI;

public class TestViewModel : ViewModel
{
    public Image testImage;

    // Start is called before the first frame update
    void Start()
    {
        Initialize<TestViewModel, TestPresenter, TestInteractor>();   
    }

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        CallPresenter("blue");
    }

    public override void CallPresenter(params object[] list)
    {
        presenter.CallInteractor(list);
    }

    public override void DisplayOnResult(params object[] list)
    {
        string color = (string)list[0];
        if (color == "blue") {
            testImage.color = Color.blue;
        }

        if (color == "green")
        {
            testImage.color = Color.green;
        }

        if (color == "red")
        {
            testImage.color = Color.red;
        }
    }

    public void BlueButtonOnClick() {
        CallPresenter("blue");
    }

    public void GreenButtonOnClick()
    {
        CallPresenter("green");
    }

    public void RedButtonOnClick()
    {
        CallPresenter("red");
    }

}
