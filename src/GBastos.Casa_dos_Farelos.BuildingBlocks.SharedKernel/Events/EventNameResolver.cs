namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Events;

public static class EventNameResolver
{
    public static string GetName(Type type)
        => type.Name.Replace("Event", "");
}