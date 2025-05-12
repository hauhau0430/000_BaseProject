public abstract class Singleton<T>  where T : class, new()
{
    private static readonly T instance = new();
    public static T Instance => instance;

    protected Singleton() { }
}
