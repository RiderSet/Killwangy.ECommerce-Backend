namespace GBastos.Casa_dos_Farelos.Observability.Correlations;

public sealed class CorrelationIdOptions
{
    public string RequestHeader { get; set; } = "X-Correlation-ID";
    public bool IncludeInResponse { get; set; } = true;
}