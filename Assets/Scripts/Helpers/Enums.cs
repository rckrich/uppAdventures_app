public enum ViewID
{
    None,
    TestViewModel,
    LogInViewModel,
    PopUpViewModel,
    ProfileViewModel,
    ChangeAvatarViewModel,
    FAQViewModel,
    EventFeedViewModel,
    EventDetailViewModel,
    StoreViewModel,
    StoreItemDetailViewModel,
    ScanPopUpViewModel,
    BuyPopUpViewModel,
    SuccessfulBuyPopUpViewModel,
    UnsuccessfulBuyPopUpViewModel,
    MenuViewModel,
    HeaderViewModel,
    AssitedEventFeedViewModel,
    LoadingViewModel,
    AssistPopUpViewModel,
    SuccessfulAssistPopUpViewModel,
    UnsuccessfulAssistPopUpViewModel,
    UPCardViewModel
}

public enum HTTPMethods
{
    Post,
    Get,
    Put,
    Delete
}

public enum PopUpViewModelTypes
{
    Central,
    Downwards
}

public enum LogInMethods
{
    PostLogIn,
    GetUserData
}

public enum ChangeAvatarMethods { 
    GetUserAvatars,
    PostChangeAvatar
}

public enum StoreItemsViews
{
    GetProducts,
    GetPromos,
    GetAvatars
}

public enum StoreGetMethods
{
    GetStoreItems,
    GetUserAvatars
}

public enum AssistMethods
{
    GetEvent,
    PostAssist,
    GetUserData
}

public enum BuyMethods
{
    GetStoreItem,
    PostBuy,
    GetUserData
}


public enum PointsTypes {
    Sports,
    Cultural,
    Acedemic,
    StudentIssues,
    UPMovement
}