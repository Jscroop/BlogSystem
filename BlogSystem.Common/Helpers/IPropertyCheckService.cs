namespace BlogSystem.Common.Helpers
{
    public interface IPropertyCheckService
    {
        bool TypeHasProperties<T>(string fields);
    }
}
