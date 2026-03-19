using Microsoft.AspNetCore.Http;
using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Application.Common.Errors;

public static class AppErrors
{
    public static ErrorMessage AuthenticationRequired() => new()
    {
        Title = "Kimlik doğrulama gerekli",
        Detail = "Oturum geçersiz veya eksik. Lütfen yeniden giriş yapın.",
        Instance = "auth/required",
        Status = StatusCodes.Status401Unauthorized
    };

    public static ErrorMessage InvalidLogin() => new()
    {
        Title = "Geçersiz giriş",
        Detail = "E-posta veya şifre hatalı.",
        Instance = "auth/login",
        Status = StatusCodes.Status401Unauthorized
    };

    public static ErrorMessage UserNotFound() => new()
    {
        Title = "Kullanıcı bulunamadı",
        Detail = "Kullanıcı bulunamadı.",
        Instance = "auth/refresh",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage UserAlreadyExists() => new()
    {
        Title = "Kayıt hatası",
        Detail = "Bu e-posta adresi ile kayıtlı bir kullanıcı zaten mevcut.",
        Instance = "auth/register",
        Status = StatusCodes.Status409Conflict
    };

    public static ErrorMessage SurveyNotAssigned() => new()
    {
        Title = "Anket atanmamış",
        Detail = "Bu anket size atanmamış.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status403Forbidden
    };

    public static ErrorMessage SurveyAlreadySubmitted() => new()
    {
        Title = "Anket zaten dolduruldu",
        Detail = "Bu anketi daha önce doldurdunuz.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status409Conflict
    };

    public static ErrorMessage SurveyNotFound() => new()
    {
        Title = "Anket bulunamadı",
        Detail = "Anket bulunamadı.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage SurveyNotActive() => new()
    {
        Title = "Anket aktif değil",
        Detail = "Bu anket şu an aktif değil.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status400BadRequest
    };

    public static ErrorMessage SurveyDateRangeInvalid() => new()
    {
        Title = "Geçersiz tarih aralığı",
        Detail = "Anket geçerli tarih aralığında değil.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status400BadRequest
    };

    public static ErrorMessage InvalidAnswers() => new()
    {
        Title = "Geçersiz cevaplar",
        Detail = "Tüm sorulara tam olarak bir kez cevap vermelisiniz.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status400BadRequest
    };

    public static ErrorMessage AnswerTemplateNotFound() => new()
    {
        Title = "Cevap şablonu bulunamadı",
        Detail = "İstenen cevap şablonu bulunamadı.",
        Instance = "answer-templates",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage QuestionNotFound() => new()
    {
        Title = "Soru bulunamadı",
        Detail = "İstenen soru bulunamadı.",
        Instance = "questions",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage SurveyNotFoundForAdmin() => new()
    {
        Title = "Anket bulunamadı",
        Detail = "İstenen anket bulunamadı.",
        Instance = "surveys",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage SurveyReportNotFound() => new()
    {
        Title = "Anket bulunamadı",
        Detail = "Rapor için anket bulunamadı.",
        Instance = "survey-reports",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage RouteIdMismatch() => new()
    {
        Title = "Geçersiz istek",
        Detail = "URL'deki kimlik ile gövdedeki kimlik eşleşmiyor.",
        Instance = "api/route-id",
        Status = StatusCodes.Status400BadRequest
    };

    public static ErrorMessage SurveyCannotBeFilledNow() => new()
    {
        Title = "Anket görüntülenemiyor",
        Detail = "Anket bulunamadı, aktif değil veya geçerli tarih aralığında değil.",
        Instance = "survey-filling/view",
        Status = StatusCodes.Status404NotFound
    };

    public static ErrorMessage ValidationFailed(string detail) => new()
    {
        Title = "Doğrulama hatası",
        Detail = detail,
        Instance = "validation",
        Status = StatusCodes.Status400BadRequest
    };

    public static ErrorMessage InvalidAnswerOption() => new()
    {
        Title = "Geçersiz cevap",
        Detail = "Seçilen şık, sorunun cevap şablonuna ait değil.",
        Instance = "survey-filling/submit",
        Status = StatusCodes.Status400BadRequest
    };
}
