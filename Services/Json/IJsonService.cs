namespace AngParser.Service.JSON
{
    public interface IJsonService
    {
        T JsonConvertDeserializeObject<T>(string content);
        T JsonConvertDeserializeObjectWithNull<T>(string json);
    }
}
