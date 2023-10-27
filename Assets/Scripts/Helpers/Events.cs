public class EventButtonOnClickEvent : AppEvent
{
    private EventEntity eventEntity;
    public EventButtonOnClickEvent(EventEntity _eventEntity) {
        eventEntity = _eventEntity;
    }
    public EventEntity GetEventEntity() { return eventEntity; }
}
public class AssitedEventButtonOnClickEvent : AppEvent
{
    private EventEntity eventEntity;
    public AssitedEventButtonOnClickEvent(EventEntity _eventEntity)
    {
        eventEntity = _eventEntity;
    }
    public EventEntity GetEventEntity() { return eventEntity; }
}
public class ChangeAvatarOnClickEvent : AppEvent
{
    private Avatar avatar;
    private UnityEngine.Sprite sprite;
    public ChangeAvatarOnClickEvent(Avatar _avatar, UnityEngine.Sprite _sprite)
    {
        avatar = _avatar;
        sprite = _sprite;
    }
    public Avatar GetAvatar()
    {
        return avatar;
    }
    public UnityEngine.Sprite GetSprite()
    {
        return sprite;
    }
}