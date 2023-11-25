using System.Net.Http.Headers;
using HttpIntegrationTemplate;

var token = "zmmQIWFhqCRzM3PNJq3bNeFq1r8Cut3sys2qlbjXAzk=";
if (string.IsNullOrEmpty(token))
{
    throw new Exception("Укажите токен");
}

var candidateId = "655a4167d6073213f4e9b8ce";

if (string.IsNullOrEmpty(candidateId) || candidateId == token)
{
    throw new Exception("Укажите идентификатор кандидата");
}

var client = new HttpClient
{
    DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
    BaseAddress = new Uri("https://api-feature-configurator.dev.skillaz.ru/")
    
};

var candidateService = new CandidateService(client);

Console.WriteLine("Начинаем проверку кандидата");

candidateService.CheckCandidate(candidateId);

Console.WriteLine("Проверка окончена");
