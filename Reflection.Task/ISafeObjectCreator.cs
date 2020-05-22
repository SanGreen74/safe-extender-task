namespace Reflection.Task
{
    public interface ISafeObjectCreator
    {
        T CreateSafe<T>()
            where T : class;
    }
}