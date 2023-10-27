using System;
using System.Collections.Generic;

public class User {
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public int? coins { get; set; }
    public object avatar_id { get; set; }
    public DateTime email_verified_at { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public Avatar avatar { get; set; }
    public List<StudentPoint> student_points { get; set; }
}

public class StudentPoint
{
    public int id { get; set; }
    public int category_id { get; set; }
    public int user_id { get; set; }
    public int amount { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public Category category { get; set; }
}

public class Category
{
    public int id { get; set; }
    public string name { get; set; }
    public int campus_id { get; set; }
    public DateTime created_at { get; set; }
    public object updated_at { get; set; }
}

public class Campus
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime created_at { get; set; }
    public object updated_at { get; set; }
}

public class EventEntity
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public int coins { get; set; }
    public string department { get; set; }
    public string description { get; set; }
    public string place { get; set; }
    public int campus_id { get; set; }
    public int category_id { get; set; }
    public int user_id { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string qr_code { get; set; }
    public Campus campus { get; set; }
    public Category category { get; set; }
}

public class StoreItem
{
    public int id { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string stock_type { get; set; }
    public int? stock { get; set; }
    public int price { get; set; }
    public string description { get; set; }
    public string qr_code { get; set; }
    public int campus_id { get; set; }
    public int media_id { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public Media media { get; set; }
}

public class Producto : StoreItem
{

}

public class Promocion : StoreItem
{
    
}

public class Avatar : StoreItem
{
   
}

public class Media
{
    public int id { get; set; }
    public string type { get; set; }
    public string url { get; set; }
    public object thumbnail { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public string absolute_url { get; set; }
}

public class LogInEntity {
    public string access_token { get; set; }
    public string token_type { get; set; }
    public string expires_at { get; set; }
    public User user { get; set; }
}

public class GetUserEntity
{
    public User user;
}

public class LogOutEntity
{

}

public class GetEvents {
    public List<EventEntity> page;
    public int next_page;
}

public class GetAvialableStoreItems
{
    public List<Producto> productos { get; set; }
    public List<Promocion> promociones { get; set; }
    public List<Avatar> avatares { get; set; }
}

public class GetEvent
{
    public EventEntity @event { get; set; }
}

public class GetStoreItems
{
    public List<Producto> productos { get; set; }
    public List<Promocion> promociones { get; set; }
    public List<Avatar> avatares { get; set; }
}

public class GetStoreItem
{
    public StoreItem item { get; set; }
}

public class GetAvatars
{
    public List<Avatar> avatars { get; set; }
}

public class PostChangeAvatar {
    public Avatar avatar;
}

public class ResponseEntity {

}

public class BuyEntity
{
    public StoreItem storeItem;
}

public class ErrorEntity {
    public string message { get; set; }
}
