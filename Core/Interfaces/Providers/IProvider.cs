namespace Core.Interfaces.Providers
{
    public interface IProvider<out T>
    {
        T Get();

        T Get(params object[] parameters);
    }
}