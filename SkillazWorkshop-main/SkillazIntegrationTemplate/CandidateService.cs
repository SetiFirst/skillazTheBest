using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpIntegrationTemplate;

public class CandidateService
{
    private readonly HttpClient _httpClient;

    public CandidateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public void CheckCandidate(string id)
    {
        var candidate = GetCandidateInfo(id);
        var vacancy = GetVacancyInfo(candidate.VacancyId);

        if (CalculateMatching(candidate, vacancy))
        {
            AddCommentToCandidate(id, "Подходит");
        }
        else
        {
            AddCommentToCandidate(id, "Не подходит");
        }
    }

    private bool CalculateMatching(CandidateInfo candidateInfo, VacancyInfo vacancyInfo)
    {
        var CandidatSkills = candidateInfo.CommonCVInfo.Skills;
        var VacancySkills = vacancyInfo.Data.RequiredSkills;

        var result = CandidatSkills.Intersect(VacancySkills);

        if (result.Count() * 100 / VacancySkills.Count() < 70)
        {
            return false;
        }
        if (candidateInfo.CommonCVInfo.WorkExperiences != null)
        {
            var workExperience = candidateInfo.CommonCVInfo.WorkExperiences.Sum(x => (double)((x.EndDate ?? DateTime.UtcNow) - x.StartDate).TotalDays / 30);
        
            if (workExperience <
                vacancyInfo.Data.WorkExperience - 6)
            {
                return false;
            } 
        }
        else
        {
            return false;
        }

        if (!candidateInfo.CommonCVInfo.Citizenship.Contains(vacancyInfo.Data.Citizenship))
        {
            return false;
        }

        if (vacancyInfo.Data.NeedDriverLicence)
        {
            if(candidateInfo.CommonCVInfo.DrivingExperience.DrivingLicense == "Undefined")
            {
                return false;
            }
        }

        return true;
    }

    public CandidateInfo GetCandidateInfo(string id)
    {
        var message = new HttpRequestMessage(HttpMethod.Post, "/open-api/objects/candidates/filtered")
        {
            Content = JsonContent.Create(new CandidateInfoRequest
            {
                Ids = new[] { id }
            }, options: new() { PropertyNamingPolicy = null })
        };

        var response = _httpClient.Send(message);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Ошибка запроса");
        }

        var deserializedResponse = JsonSerializer.Deserialize<CandidateInfoResponse>(response.Content.ReadAsStream());

        return deserializedResponse?.Items.FirstOrDefault() ?? throw new Exception("Не найден кандидат");
    }

    private VacancyInfo GetVacancyInfo(string vacancyId)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, $"/open-api/objects/vacancies/{vacancyId}")
        {
        };
        var response = _httpClient.Send(message);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Ошибка запроса");
        }
        var deserializedResponse = JsonSerializer.Deserialize<VacancyInfo>(response.Content.ReadAsStream());

        return deserializedResponse ?? throw new Exception("Не найден кандидат");

    }

    private void AddCommentToCandidate(string id, string text)
    {
        //todo: используйте _httpClient для отправки комментария 
    }
}




public class CandidateInfoRequest
{
    public string[] Ids { get; set; }
    public CandidateCommonCVInfo CommonCVInfo { get; set; }
}

public class CandidateInfoResponse
{
    public CandidateInfo[] Items { get; set; }
}

public class CandidateInfo
{
    public string VacancyId { get; set; }
    public List<CandidateNote> Notes { get; set; }
    public string Id { get; set; }
    public string FirstName { get; set; }

    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public string ContactPhoneNumber { get; set; }
    public string ContactEmail { get; set; }
    public CandidateCommonCVInfo CommonCVInfo { get; set; }


}


public class CandidateCommonCVInfo
{
    public string[] Citizenship { get; set; }

    public string[] Skills { get; set; }
    public CandidateWorkExperience[] WorkExperiences { get; set; }
    public string City { get; set; }

    public string Country { get; set; }
    public CandidateDrivingExperience DrivingExperience { get; set; }
    public List<CandidateNote> Notes { get; set; }



}

public class CandidateDrivingExperience
{
    public bool HasPersonalCar { get; set; }
    public string DrivingLicense { get; set; }
}
public class CandidateWorkExperience
{
    public string CompanyName { get; set; }
    public string Position { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string Industries { get; set; }
    public string City { get; set; }
    public string EploymentType { get; set; }

    public int TotalMonths { get; set; }
}

public class CandidateNote
{
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}






public class VacancyInfo
{
    public string Id { set; get; } 
    public string Name { get; set; }
    public bool IsActive { get; set; }  
    public string FunnelId { get; set; }
    public VacancyData Data { get; set; }

}

public class VacancyData
{
    public string Name { get; set; }

    [JsonPropertyName("ExtraData.RequiredSkills")]
    public string[] RequiredSkills {  get; set; }

    [JsonPropertyName("ExtraData.WorkExperience")]
    public int WorkExperience {  get; set; }


    [JsonPropertyName("ExtraData.Citizenship")]
    public string Citizenship {  get; set; }


    [JsonPropertyName("ExtraData.NeedDriverLicence")]
    public bool NeedDriverLicence {  get; set; }
    

    public string FunnelId { get; set; }

}

public class VacancyInfoRequest
{
    public VacancyData VacancyData { get; set; }
}

public class VacancyInfoResponse
{
    public VacancyInfo[] Items { get; set; }
}
