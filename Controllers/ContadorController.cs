using Microsoft.AspNetCore.Mvc;
using APIContagem.Models;
using Microsoft.ApplicationInsights;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class ContadorController : ControllerBase
{
    private static readonly Contador _CONTADOR = new Contador();
    private readonly IConfiguration _configuration;
    private TelemetryClient _telemetry;

    public ContadorController(IConfiguration configuration, TelemetryClient telemetry)
    {
        _configuration = configuration;
        _telemetry = telemetry;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        _telemetry.TrackTrace("GET requisitado");

        int valorAtualContador;

        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;

        }

        return new()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = _configuration["MensagemVariavel"]
        };
    }}
