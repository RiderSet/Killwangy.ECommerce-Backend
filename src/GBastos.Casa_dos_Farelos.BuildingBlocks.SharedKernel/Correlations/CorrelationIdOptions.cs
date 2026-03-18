namespace GBastos.Casa_dos_Farelos.BuildingBlocks.SharedKernel.Correlations;

public sealed class CorrelationIdOptions
{
    public string RequestHeader { get; set; } = "X-Correlation-ID";
    public bool IncludeInResponse { get; set; } = true;
}