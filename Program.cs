using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

await app.RunAsync();


[McpServerToolType]
public class CepTools
{
    [McpServerTool, Description("Consulta informações sobre o CEP fornecido")]
    public async Task<ViaCepResultado> ConsultarCEP([Description("O CEP que será consultado")] string cep)
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Erro ao consultar o CEP.{response.StatusCode}");
        }

        var resultado = await response.Content.ReadFromJsonAsync<ViaCepResultado>();

        return resultado ?? throw new Exception("Falha ao converter resposta.");
    }
};

public class ViaCepResultado
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Gia { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
    public string Siafi { get; set; } = string.Empty;
};