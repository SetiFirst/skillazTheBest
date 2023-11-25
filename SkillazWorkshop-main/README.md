# Задание
Необходимо доработать консольное приложение - реализовать методы получения кандидатов и вакансий по API, а также проверку соответствия кандидата и его вакансии.

В классе `CandidateService` необходимо реализовать все методы с пометкой `//todo:`
А сюда скидываем результаты: https://forms.gle/z4RNScRbb3Ydvi9V8
<br />
<h3>1️⃣ GetCandidateInfo - получение кандидата по API</h3>

POST `/open-api/objects/candidates/filtered`

<details>
  <summary>Поля кандидата</summary><br />

- `CommonCandidateInfo.Skills` - навыки, массив строк
- `CommonCandidateInfo.Citizenship` - гражданства кандидата, массив строк
- `CommonCandidateInfo.WorkExperience` - опыт работы, массив объектов
- `CommonCandidateInfo.DrivingExperiences` - опыт вождения, массив объектов

</details>

<details>
  <summary>Тело запроса</summary><br />

```json
{
  "Ids": [
    "string"
  ]
} 
```

</details>
<details>
  <summary>Тело ответа</summary><br />

```json
{
  "Items": [
    {
      "Id": "string",
      "FirstName": "string",
      "MiddleName": "string",
      "LastName": "string",
      "ContactPhoneNumber": "string",
      "ContactEmail": "string",
      "CommonCVInfo": {
        "BirthDate": "2023-11-02T05:19:53.652Z",
        "Skills": [
          "string"
        ],
        "Citizenship": [
          "string"
        ],
        "WorkExperience": [
          {
            "CompanyName": "string",
            "Position": "string",
            "StartDate": "2023-11-02T05:19:53.652Z",
            "EndDate": "2023-11-02T05:19:53.652Z",
            "TotalMonths": 0,
            "Description": "string",
            "Industries": "string",
            "City": "string",
            "EmploymentType": "Any"
          }
        ],
        "City": "string",
        "Country": "string",
        "DrivingExperiences": [
          {
            "HasPersonalCar": true,
            "DrivingLicense": "Undefined"
          }
        ]
      },
      "VacancyId": "string",
      "Notes": [
        {
          "Id": "string",
          "Text": "string",
          "CreatedById": "string",
          "CreatedByName": "string",
          "CreatedByClientId": "string",
          "UpdatedById": "string",
          "UpdatedByName": "string",
          "UpdatedByClientId": "string",
          "IsInternal": true,
          "CreatedAt": "2023-11-02T05:19:53.652Z",
          "UpdatedAt": "2023-11-02T05:19:53.652Z",
          "NotifyAt": "2023-11-02T05:19:53.652Z",
          "RefCandidateId": "string",
          "CandidateStatusId": "string"
        }
      ]
    }
  ],
  "NextPage": 0,
  "TotalPages": 0,
  "TotalItems": 0
}
```
</details>
<br />
<h3>2️⃣ GetVacancyInfo - получение вакансии по API</h3>

GET `/open-api/objects/vacancies/{vacancyId}`

<details>
  <summary>Поля вакансии</summary><br />

- `Name` - название вакансии
- `ExtraData.RequiredSkills` - требуемые навыки, массив строк
- `ExtraData.WorkExperience` - требуемый опыт работы, число
- `ExtraData.Citizenship` - требуемое гражданство, строка, одно из значений
    - `Rus`
    - `Kz`
    - `Any`
- `ExtraData.NeedDriverLicense` - требуется наличие водительских прав

</details>
<details>
  <summary>Тело ответа</summary><br />

```json
{
  "Id": "string",
  "Name": "string",
  "IsActive": true,
  "FunnelId": "string",
  "Data": {
    "Name": "string",
    "FunnelId": "string",
    "ExtraData.RequiredSkills": [
      "string"
    ],
    "ExtraData.WorkExperience": 0,
    "ExtraData.Citizenship": "string",
    "ExtraData.NeedDriverLicense": true
  }
}
```

</details>
<br />
<h3>3️⃣ CalculateMatching - алгоритм соответствия кандидата вакансии</h3>
<details>
  <summary>Критерии соответствия вакансии и кандидата</summary><br />

- **Навыки** - пересечение навыков от 70%
- **Требуемый опыт работы** - погрешность в 6 месяцев в меньшую сторону
- **Требуемое гражданство** - если не соответствует, то кандидат не подходит. Если "Any" - не проверяем 
- **Наличие водительских прав** - если в вакансии true, а в кандидате false - не подходит. В остальных случаях подходит по этому критерию

</details>
<br />
<h3>4️⃣ AddCommentToCandidate - добавление комментария кандидату по API</h3>

POST `/open-api/objects/candidates/{candidateId}/notes`
<details>
  <summary>Тело запроса</summary><br />

```json
{
  "Add": [
    {
      "Text": "string"
    }
  ]
}
```

</details>