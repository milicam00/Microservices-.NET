namespace BuildingBlocks.Infrastructure.IInternalCommandsMapper
{
    public interface IInternalCommandsMapper
    {
        string GetName(Type type);
        Type GetType(string name);
    }
}
