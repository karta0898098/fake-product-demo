using System.Collections.Generic;
using Newtonsoft.Json;


public class RequestLogin
{
    [JsonProperty("account")]
    public string Account { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
}

public class ResponseLogin
{
    public class UserInfo
    {
        [JsonProperty("money")]
        public long Money { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("userInfo")]
    public UserInfo User { get; set; }
}

public class RequestRegister
{
    [JsonProperty("account")]
    public string Account { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class ResponseRegister
{
    public class UserInfo
    {
        [JsonProperty("money")]
        public long Money { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("userInfo")]
    public UserInfo User { get; set; }
}

public class ResponseProducts
{
    [JsonProperty("products")]
    public List<ResponseProduct> Products { get; set; }
}

public class ResponseProduct
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("price")]
    public long Price { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}

public partial class ResponseBuyProducts
{
    [JsonProperty("create_at")]
    public string CreateAt { get; set; }

    [JsonProperty("record_id")]
    public long RecordId { get; set; }

    [JsonProperty("product")]
    public BuyProduct Product { get; set; }

    [JsonProperty("user")]
    public Buyer User { get; set; }

    public class BuyProduct
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }
    }

    public class Buyer
    {
        [JsonProperty("money")]
        public long Money { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

public class ResponeError
{
    [JsonProperty("error")]
    public string Error { get; set; }
}




