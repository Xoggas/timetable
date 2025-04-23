namespace Timetable.Api.Shared;

public interface IModule
{
    void Register(WebApplicationBuilder builder);
}

public static class ModuleExtensions
{
    public static void AddModule<T>(this WebApplicationBuilder builder) where T : IModule, new()
    {
        var moduleInstance = new T();
        moduleInstance.Register(builder);
    }
}