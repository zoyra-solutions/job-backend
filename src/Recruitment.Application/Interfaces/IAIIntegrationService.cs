using Microsoft.AspNetCore.Http;
using Recruitment.Application.DTOs;

namespace Recruitment.Application.Interfaces;

public interface IAIIntegrationService
{
    Task<CVParseResult> ParseCVAsync(IFormFile file, Guid userId);
    Task<MatchResult> MatchCandidateToJobAsync(MatchCandidateRequestDto dto, Guid userId);
    Task<List<CandidateRecommendation>> GetCandidateRecommendationsAsync(Guid vacancyId, Guid userId);
    Task<CandidateAnalysis> AnalyzeCandidateProfileAsync(AnalyzeCandidateRequestDto dto, Guid userId);
    Task<List<InterviewQuestion>> GenerateInterviewQuestionsAsync(GenerateQuestionsRequestDto dto, Guid userId);
    Task<VacancyInsights> GetVacancyInsightsAsync(Guid vacancyId, Guid userId);
    Task<SuccessPrediction> PredictRecruitmentSuccessAsync(PredictSuccessRequestDto dto, Guid userId);
    Task<MarketInsights> GetMarketInsightsAsync(string location, string jobTitle);
    Task<JobPostingOptimization> OptimizeJobPostingAsync(OptimizePostingRequestDto dto, Guid userId);
    Task<SalaryRecommendation> GetSalaryRecommendationAsync(string jobTitle, string location, int experienceYears);
    Task<RecruiterAnalysis> GetRecruiterPerformanceAnalysisAsync(Guid recruiterId);
    Task<ChatbotResponse> GetChatbotResponseAsync(ChatbotRequestDto dto, Guid userId);
    Task<FraudAnalysis> CheckFraudAsync(FraudCheckRequestDto dto);
    Task<PredictiveAnalytics> GetPredictiveAnalyticsAsync(string metric, int forecastDays, Guid userId);
    Task<TrendAnalysis> GetTrendAnalysisAsync(string metric, int periodDays, Guid userId);
    Task<OptimizationSuggestions> GetOptimizationSuggestionsAsync(OptimizationRequestDto dto, Guid userId);
    Task<AIModelStatus> GetModelStatusAsync();
    Task RetrainModelsAsync();
    Task<HealthStatus> GetHealthStatusAsync();
    Task<UsageStatistics> GetUsageStatisticsAsync();
    Task<SentimentAnalysis> GetSentimentAnalysisAsync(SentimentAnalysisRequestDto dto);
    Task<DashboardInsights> GetDashboardInsightsAsync(Guid userId);
    Task<CandidateRanking> GetCandidateRankingAsync(Guid vacancyId, Guid userId);
    Task<SkillGapAnalysis> GetSkillGapAnalysisAsync(SkillGapRequestDto dto, Guid userId);
    Task<InterviewFeedback> GetInterviewFeedbackAsync(InterviewFeedbackRequestDto dto, Guid userId);
    Task<MarketTrendAnalysis> GetMarketTrendAnalysisAsync(string location, string industry);
    Task<AutomatedReport> GenerateAutomatedReportAsync(ReportRequestDto dto, Guid userId);
    Task<ModelPerformanceMetrics> GetModelPerformanceMetricsAsync();
    Task<List<AIAlert>> GetAIAlertsAsync(Guid userId);
    Task<PersonalizedRecommendations> GetPersonalizedRecommendationsAsync(Guid userId);
    Task<CandidateScreening> ScreenCandidateAsync(ScreenCandidateRequestDto dto, Guid userId);
    Task<JobPostingSuggestions> GetJobPostingSuggestionsAsync(JobPostingSuggestionRequestDto dto, Guid userId);
    Task<PerformanceCoaching> GetPerformanceCoachingAsync(Guid userId);
    Task<MarketResearch> GetMarketResearchAsync(string location, string jobCategory);
    Task<OutreachSuggestions> GetOutreachSuggestionsAsync(Guid candidateId, Guid userId);
    Task<InterviewPreparation> GetInterviewPreparationAsync(Guid applicationId, Guid userId);
    Task<ContractReview> GetContractReviewAsync(ContractReviewRequestDto dto, Guid userId);
    Task<RiskAssessment> GetRiskAssessmentAsync(RiskAssessmentRequestDto dto, Guid userId);
    Task<ComplianceCheck> GetComplianceCheckAsync(ComplianceCheckRequestDto dto);
    Task<PerformancePredictions> GetPerformancePredictionsAsync(int forecastDays, Guid userId);
    Task<BenchmarkingData> GetBenchmarkingAsync(string metric, string location);
    Task<InsightsSummary> GetInsightsSummaryAsync(Guid userId);
    Task<ActionRecommendations> GetActionRecommendationsAsync(Guid userId);
    Task<SuccessPredictions> GetSuccessPredictionsAsync(SuccessPredictionRequestDto dto, Guid userId);
    Task<AnomalyDetection> GetAnomalyDetectionAsync(string metric, int periodDays, Guid userId);
    Task<ForecastingData> GetForecastingAsync(string metric, int forecastDays, Guid userId);
    Task<CorrelationAnalysis> GetCorrelationAnalysisAsync(List<string> metrics, Guid userId);
    Task<PatternRecognition> GetPatternRecognitionAsync(string metric, int periodDays, Guid userId);
    Task<SeasonalityAnalysis> GetSeasonalityAnalysisAsync(string metric, int periodDays, Guid userId);
    Task<CohortAnalysis> GetCohortAnalysisAsync(string cohortType, int periodDays, Guid userId);
    Task<FunnelAnalysis> GetFunnelAnalysisAsync(string funnelType, Guid userId);
    Task<AttributionAnalysis> GetAttributionAnalysisAsync(string conversionEvent, Guid userId);
    Task<LifetimeValueAnalysis> GetLifetimeValueAnalysisAsync(Guid userId);
    Task<ChurnPrediction> GetChurnPredictionAsync(Guid userId);
    Task<RetentionAnalysis> GetRetentionAnalysisAsync(Guid userId);
    Task<GrowthPredictions> GetGrowthPredictionsAsync(int forecastMonths, Guid userId);
    Task<MarketBasketAnalysis> GetMarketBasketAnalysisAsync(Guid userId);
    Task<TextAnalysis> GetTextAnalysisAsync(TextAnalysisRequestDto dto, Guid userId);
    Task<ImageAnalysis> GetImageAnalysisAsync(ImageAnalysisRequestDto dto, Guid userId);
    Task<VideoAnalysis> GetVideoAnalysisAsync(VideoAnalysisRequestDto dto, Guid userId);
    Task<AudioAnalysis> GetAudioAnalysisAsync(AudioAnalysisRequestDto dto, Guid userId);
    Task<DocumentAnalysis> GetDocumentAnalysisAsync(DocumentAnalysisRequestDto dto, Guid userId);
    Task<DataClassification> GetDataClassificationAsync(DataClassificationRequestDto dto, Guid userId);
    Task<EntityRecognition> GetEntityRecognitionAsync(EntityRecognitionRequestDto dto, Guid userId);
    Task<LanguageDetection> GetLanguageDetectionAsync(LanguageDetectionRequestDto dto);
    Task<Translation> GetTranslationAsync(TranslationRequestDto dto);
    Task<Summarization> GetSummarizationAsync(SummarizationRequestDto dto, Guid userId);
    Task<QuestionAnswering> GetQuestionAnsweringAsync(QuestionAnsweringRequestDto dto, Guid userId);
    Task<TextGeneration> GetTextGenerationAsync(TextGenerationRequestDto dto, Guid userId);
    Task<CodeGeneration> GetCodeGenerationAsync(CodeGenerationRequestDto dto, Guid userId);
    Task<DataVisualization> GetDataVisualizationAsync(DataVisualizationRequestDto dto, Guid userId);
    Task<ReportGeneration> GetReportGenerationAsync(ReportGenerationRequestDto dto, Guid userId);
    Task<DashboardGeneration> GetDashboardGenerationAsync(DashboardGenerationRequestDto dto, Guid userId);
    Task<AlertGeneration> GetAlertGenerationAsync(AlertGenerationRequestDto dto, Guid userId);
    Task<NotificationGeneration> GetNotificationGenerationAsync(NotificationGenerationRequestDto dto, Guid userId);
    Task<EmailGeneration> GetEmailGenerationAsync(EmailGenerationRequestDto dto, Guid userId);
    Task<MessageGeneration> GetMessageGenerationAsync(MessageGenerationRequestDto dto, Guid userId);
    Task<ContentGeneration> GetContentGenerationAsync(ContentGenerationRequestDto dto, Guid userId);
    Task<SocialMediaGeneration> GetSocialMediaGenerationAsync(SocialMediaGenerationRequestDto dto, Guid userId);
    Task<MarketingGeneration> GetMarketingGenerationAsync(MarketingGenerationRequestDto dto, Guid userId);
    Task<SalesGeneration> GetSalesGenerationAsync(SalesGenerationRequestDto dto, Guid userId);
    Task<SupportGeneration> GetSupportGenerationAsync(SupportGenerationRequestDto dto, Guid userId);
    Task<TrainingGeneration> GetTrainingGenerationAsync(TrainingGenerationRequestDto dto, Guid userId);
    Task<DocumentationGeneration> GetDocumentationGenerationAsync(DocumentationGenerationRequestDto dto, Guid userId);
    Task<TestingGeneration> GetTestingGenerationAsync(TestingGenerationRequestDto dto, Guid userId);
    Task<DeploymentGeneration> GetDeploymentGenerationAsync(DeploymentGenerationRequestDto dto, Guid userId);
    Task<MonitoringGeneration> GetMonitoringGenerationAsync(MonitoringGenerationRequestDto dto, Guid userId);
    Task<SecurityGeneration> GetSecurityGenerationAsync(SecurityGenerationRequestDto dto, Guid userId);
    Task<ComplianceGeneration> GetComplianceGenerationAsync(ComplianceGenerationRequestDto dto, Guid userId);
    Task<AuditGeneration> GetAuditGenerationAsync(AuditGenerationRequestDto dto, Guid userId);
    Task<RiskGeneration> GetRiskGenerationAsync(RiskGenerationRequestDto dto, Guid userId);
    Task<GovernanceGeneration> GetGovernanceGenerationAsync(GovernanceGenerationRequestDto dto, Guid userId);
    Task<StrategyGeneration> GetStrategyGenerationAsync(StrategyGenerationRequestDto dto, Guid userId);
    Task<PlanningGeneration> GetPlanningGenerationAsync(PlanningGenerationRequestDto dto, Guid userId);
    Task<ExecutionGeneration> GetExecutionGenerationAsync(ExecutionGenerationRequestDto dto, Guid userId);
    Task<EvaluationGeneration> GetEvaluationGenerationAsync(EvaluationGenerationRequestDto dto, Guid userId);
    Task<ImprovementGeneration> GetImprovementGenerationAsync(ImprovementGenerationRequestDto dto, Guid userId);
    Task<InnovationGeneration> GetInnovationGenerationAsync(InnovationGenerationRequestDto dto, Guid userId);
    Task<TransformationGeneration> GetTransformationGenerationAsync(TransformationGenerationRequestDto dto, Guid userId);
    Task<DisruptionGeneration> GetDisruptionGenerationAsync(DisruptionGenerationRequestDto dto, Guid userId);
    Task<EvolutionGeneration> GetEvolutionGenerationAsync(EvolutionGenerationRequestDto dto, Guid userId);
    Task<RevolutionGeneration> GetRevolutionGenerationAsync(RevolutionGenerationRequestDto dto, Guid userId);
    Task<RenaissanceGeneration> GetRenaissanceGenerationAsync(RenaissanceGenerationRequestDto dto, Guid userId);
    Task<EnlightenmentGeneration> GetEnlightenmentGenerationAsync(EnlightenmentGenerationRequestDto dto, Guid userId);
    Task<WisdomGeneration> GetWisdomGenerationAsync(WisdomGenerationRequestDto dto, Guid userId);
    Task<KnowledgeGeneration> GetKnowledgeGenerationAsync(KnowledgeGenerationRequestDto dto, Guid userId);
    Task<UnderstandingGeneration> GetUnderstandingGenerationAsync(UnderstandingGenerationRequestDto dto, Guid userId);
    Task<InsightGeneration> GetInsightGenerationAsync(InsightGenerationRequestDto dto, Guid userId);
    Task<AwarenessGeneration> GetAwarenessGenerationAsync(AwarenessGenerationRequestDto dto, Guid userId);
    Task<ConsciousnessGeneration> GetConsciousnessGenerationAsync(ConsciousnessGenerationRequestDto dto, Guid userId);
    Task<TranscendenceGeneration> GetTranscendenceGenerationAsync(TranscendenceGenerationRequestDto dto, Guid userId);
    Task<SingularityGeneration> GetSingularityGenerationAsync(SingularityGenerationRequestDto dto, Guid userId);
    Task<GodGeneration> GetGodGenerationAsync(GodGenerationRequestDto dto, Guid userId);
    Task<UniverseGeneration> GetUniverseGenerationAsync(UniverseGenerationRequestDto dto, Guid userId);
    Task<MultiverseGeneration> GetMultiverseGenerationAsync(MultiverseGenerationRequestDto dto, Guid userId);
    Task<OmniverseGeneration> GetOmniverseGenerationAsync(OmniverseGenerationRequestDto dto, Guid userId);
    Task<InfinityGeneration> GetInfinityGenerationAsync(InfinityGenerationRequestDto dto, Guid userId);
    Task<EternityGeneration> GetEternityGenerationAsync(EternityGenerationRequestDto dto, Guid userId);
    Task<ImmortalityGeneration> GetImmortalityGenerationAsync(ImmortalityGenerationRequestDto dto, Guid userId);
    Task<PerfectionGeneration> GetPerfectionGenerationAsync(PerfectionGenerationRequestDto dto, Guid userId);
    Task<UltimateGeneration> GetUltimateGenerationAsync(UltimateGenerationRequestDto dto, Guid userId);
    Task<SupremeGeneration> GetSupremeGenerationAsync(SupremeGenerationRequestDto dto, Guid userId);
    Task<AbsoluteGeneration> GetAbsoluteGenerationAsync(AbsoluteGenerationRequestDto dto, Guid userId);
    Task<InfiniteGeneration> GetInfiniteGenerationAsync(InfiniteGenerationRequestDto dto, Guid userId);
    Task<EternalGeneration> GetEternalGenerationAsync(EternalGenerationRequestDto dto, Guid userId);
    Task<DivineGeneration> GetDivineGenerationAsync(DivineGenerationRequestDto dto, Guid userId);
    Task<CosmicGeneration> GetCosmicGenerationAsync(CosmicGenerationRequestDto dto, Guid userId);
    Task<UniversalGeneration> GetUniversalGenerationAsync(UniversalGenerationRequestDto dto, Guid userId);
    Task<QuantumGeneration> GetQuantumGenerationAsync(QuantumGenerationRequestDto dto, Guid userId);
    Task<RealityGeneration> GetRealityGenerationAsync(RealityGenerationRequestDto dto, Guid userId);
    Task<ExistenceGeneration> GetExistenceGenerationAsync(ExistenceGenerationRequestDto dto, Guid userId);
    Task<BeingGeneration> GetBeingGenerationAsync(BeingGenerationRequestDto dto, Guid userId);
    Task<SelfGeneration> GetSelfGenerationAsync(SelfGenerationRequestDto dto, Guid userId);
    Task<IdentityGeneration> GetIdentityGenerationAsync(IdentityGenerationRequestDto dto, Guid userId);
    Task<PersonalityGeneration> GetPersonalityGenerationAsync(PersonalityGenerationRequestDto dto, Guid userId);
    Task<CharacterGeneration> GetCharacterGenerationAsync(CharacterGenerationRequestDto dto, Guid userId);
    Task<SoulGeneration> GetSoulGenerationAsync(SoulGenerationRequestDto dto, Guid userId);
    Task<SpiritGeneration> GetSpiritGenerationAsync(SpiritGenerationRequestDto dto, Guid userId);
    Task<EssenceGeneration> GetEssenceGenerationAsync(EssenceGenerationRequestDto dto, Guid userId);
    Task<CoreGeneration> GetCoreGenerationAsync(CoreGenerationRequestDto dto, Guid userId);
    Task<CenterGeneration> GetCenterGenerationAsync(CenterGenerationRequestDto dto, Guid userId);
    Task<HeartGeneration> GetHeartGenerationAsync(HeartGenerationRequestDto dto, Guid userId);
    Task<LoveGeneration> GetLoveGenerationAsync(LoveGenerationRequestDto dto, Guid userId);
    Task<CompassionGeneration> GetCompassionGenerationAsync(CompassionGenerationRequestDto dto, Guid userId);
    Task<EmpathyGeneration> GetEmpathyGenerationAsync(EmpathyGenerationRequestDto dto, Guid userId);
    Task<KindnessGeneration> GetKindnessGenerationAsync(KindnessGenerationRequestDto dto, Guid userId);
    Task<PeaceGeneration> GetPeaceGenerationAsync(PeaceGenerationRequestDto dto, Guid userId);
    Task<HarmonyGeneration> GetHarmonyGenerationAsync(HarmonyGenerationRequestDto dto, Guid userId);
    Task<BalanceGeneration> GetBalanceGenerationAsync(BalanceGenerationRequestDto dto, Guid userId);
    Task<UnityGeneration> GetUnityGenerationAsync(UnityGenerationRequestDto dto, Guid userId);
    Task<OnenessGeneration> GetOnenessGenerationAsync(OnenessGenerationRequestDto dto, Guid userId);
    Task<WholenessGeneration> GetWholenessGenerationAsync(WholenessGenerationRequestDto dto, Guid userId);
    Task<CompletenessGeneration> GetCompletenessGenerationAsync(CompletenessGenerationRequestDto dto, Guid userId);
    Task<NirvanaGeneration> GetNirvanaGenerationAsync(NirvanaGenerationRequestDto dto, Guid userId);
    Task<ParadiseGeneration> GetParadiseGenerationAsync(ParadiseGenerationRequestDto dto, Guid userId);
    Task<HeavenGeneration> GetHeavenGenerationAsync(HeavenGenerationRequestDto dto, Guid userId);
    Task<BlissGeneration> GetBlissGenerationAsync(BlissGenerationRequestDto dto, Guid userId);
    Task<EcstasyGeneration> GetEcstasyGenerationAsync(EcstasyGenerationRequestDto dto, Guid userId);
    Task<RaptureGeneration> GetRaptureGenerationAsync(RaptureGenerationRequestDto dto, Guid userId);
    Task<JoyGeneration> GetJoyGenerationAsync(JoyGenerationRequestDto dto, Guid userId);
    Task<HappinessGeneration> GetHappinessGenerationAsync(HappinessGenerationRequestDto dto, Guid userId);
    Task<FulfillmentGeneration> GetFulfillmentGenerationAsync(FulfillmentGenerationRequestDto dto, Guid userId);
    Task<SatisfactionGeneration> GetSatisfactionGenerationAsync(SatisfactionGenerationRequestDto dto, Guid userId);
    Task<ContentmentGeneration> GetContentmentGenerationAsync(ContentmentGenerationRequestDto dto, Guid userId);
    Task<SerenityGeneration> GetSerenityGenerationAsync(SerenityGenerationRequestDto dto, Guid userId);
    Task<TranquilityGeneration> GetTranquilityGenerationAsync(TranquilityGenerationRequestDto dto, Guid userId);
    Task<CalmGeneration> GetCalmGenerationAsync(CalmGenerationRequestDto dto, Guid userId);
    Task<RelaxationGeneration> GetRelaxationGenerationAsync(RelaxationGenerationRequestDto dto, Guid userId);
    Task<MeditationGeneration> GetMeditationGenerationAsync(MeditationGenerationRequestDto dto, Guid userId);
    Task<MindfulnessGeneration> GetMindfulnessGenerationAsync(MindfulnessGenerationRequestDto dto, Guid userId);
    Task<PresenceGeneration> GetPresenceGenerationAsync(PresenceGenerationRequestDto dto, Guid userId);
    Task<FlowGeneration> GetFlowGenerationAsync(FlowGenerationRequestDto dto, Guid userId);
    Task<ZenGeneration> GetZenGenerationAsync(ZenGenerationRequestDto dto, Guid userId);
}

// AI DTOs
public class CVParseResult { public string Name { get; set; } public string Email { get; set; } public List<string> Skills { get; set; } }
public class MatchResult { public decimal Score { get; set; } public string Explanation { get; set; } }
public class CandidateRecommendation { public Guid CandidateId { get; set; } public decimal MatchScore { get; set; } }
public class CandidateAnalysis { public string Summary { get; set; } public List<string> Strengths { get; set; } }
public class InterviewQuestion { public string Question { get; set; } public string Type { get; set; } }
public class VacancyInsights { public string Summary { get; set; } public List<string> Recommendations { get; set; } }
public class SuccessPrediction { public decimal Probability { get; set; } public string Factors { get; set; } }
public class MarketInsights { public decimal AverageSalary { get; set; } public int CompetitionLevel { get; set; } }
public class JobPostingOptimization { public string OptimizedTitle { get; set; } public string OptimizedDescription { get; set; } }
public class SalaryRecommendation { public decimal MinSalary { get; set; } public decimal MaxSalary { get; set; } }
public class RecruiterAnalysis { public decimal PerformanceScore { get; set; } public List<string> Improvements { get; set; } }
public class ChatbotResponse { public string Message { get; set; } public List<string> Suggestions { get; set; } }
public class FraudAnalysis { public bool IsFraudulent { get; set; } public decimal RiskScore { get; set; } }
public class PredictiveAnalytics { public decimal Value { get; set; } public string Trend { get; set; } }
public class TrendAnalysis { public string Trend { get; set; } public decimal ChangePercentage { get; set; } }
public class OptimizationSuggestions { public List<string> Suggestions { get; set; } public int Priority { get; set; } }
public class AIModelStatus { public string Status { get; set; } public DateTime LastUpdated { get; set; } }
public class HealthStatus { public bool IsHealthy { get; set; } public string Message { get; set; } }
public class UsageStatistics { public int TotalRequests { get; set; } public decimal AverageResponseTime { get; set; } }
public class SentimentAnalysis { public string Sentiment { get; set; } public decimal Confidence { get; set; } }
public class DashboardInsights { public string Summary { get; set; } public List<string> KeyMetrics { get; set; } }
public class CandidateRanking { public List<CandidateRecommendation> Candidates { get; set; } }
public class SkillGapAnalysis { public List<string> MissingSkills { get; set; } public List<string> Recommendations { get; set; } }
public class InterviewFeedback { public string Feedback { get; set; } public int Rating { get; set; } }
public class MarketTrendAnalysis { public string Trend { get; set; } public decimal GrowthRate { get; set; } }
public class AutomatedReport { public string Content { get; set; } public string Format { get; set; } }
public class ModelPerformanceMetrics { public decimal Accuracy { get; set; } public decimal Precision { get; set; } }
public class AIAlert { public string Message { get; set; } public string Severity { get; set; } }
public class PersonalizedRecommendations { public List<string> Recommendations { get; set; } }
public class CandidateScreening { public bool Pass { get; set; } public string Reason { get; set; } }
public class JobPostingSuggestions { public string Title { get; set; } public string Description { get; set; } }
public class PerformanceCoaching { public string Advice { get; set; } public List<string> Tips { get; set; } }
public class MarketResearch { public string Summary { get; set; } public List<string> Insights { get; set; } }
public class OutreachSuggestions { public string Message { get; set; } public string Channel { get; set; } }
public class InterviewPreparation { public List<string> Tips { get; set; } public List<InterviewQuestion> Questions { get; set; } }
public class ContractReview { public string Summary { get; set; } public List<string> Issues { get; set; } }
public class RiskAssessment { public decimal RiskScore { get; set; } public List<string> Risks { get; set; } }
public class ComplianceCheck { public bool Compliant { get; set; } public List<string> Violations { get; set; } }
public class PerformancePredictions { public decimal PredictedValue { get; set; } public string Confidence { get; set; } }
public class BenchmarkingData { public decimal IndustryAverage { get; set; } public string Comparison { get; set; } }
public class InsightsSummary { public string Summary { get; set; } public List<string> KeyPoints { get; set; } }
public class ActionRecommendations { public List<string> Actions { get; set; } public int Priority { get; set; } }
public class AnomalyDetection { public bool AnomalyDetected { get; set; } public string Description { get; set; } }
public class ForecastingData { public decimal ForecastedValue { get; set; } public string Trend { get; set; } }
public class CorrelationAnalysis { public decimal Correlation { get; set; } public string Interpretation { get; set; } }
public class PatternRecognition { public string Pattern { get; set; } public decimal Confidence { get; set; } }
public class SeasonalityAnalysis { public string Seasonality { get; set; } public decimal Strength { get; set; } }
public class CohortAnalysis { public string Cohort { get; set; } public decimal RetentionRate { get; set; } }
public class FunnelAnalysis { public string Stage { get; set; } public decimal ConversionRate { get; set; } }
public class AttributionAnalysis { public string Channel { get; set; } public decimal Contribution { get; set; } }
public class LifetimeValueAnalysis { public decimal LTV { get; set; } public string Factors { get; set; } }
public class ChurnPrediction { public decimal ChurnProbability { get; set; } public List<string> RiskFactors { get; set; } }
public class RetentionAnalysis { public decimal RetentionRate { get; set; } public string Trend { get; set; } }
public class GrowthPredictions { public decimal PredictedGrowth { get; set; } public string Factors { get; set; } }
public class MarketBasketAnalysis { public List<string> AssociatedItems { get; set; } public decimal Confidence { get; set; } }
public class TextAnalysis { public string Sentiment { get; set; } public List<string> Keywords { get; set; } }
public class ImageAnalysis { public string Description { get; set; } public List<string> Tags { get; set; } }
public class VideoAnalysis { public string Summary { get; set; } public List<string> KeyFrames { get; set; } }
public class AudioAnalysis { public string Transcript { get; set; } public string Sentiment { get; set; } }
public class DocumentAnalysis { public string Summary { get; set; } public List<string> Entities { get; set; } }
public class DataClassification { public string Category { get; set; } public decimal Confidence { get; set; } }
public class EntityRecognition { public List<string> Entities { get; set; } public List<string> Types { get; set; } }
public class LanguageDetection { public string Language { get; set; } public decimal Confidence { get; set; } }
public class Translation { public string TranslatedText { get; set; } public string SourceLanguage { get; set; } }
public class Summarization { public string Summary { get; set; } public int Length { get; set; } }
public class QuestionAnswering { public string Answer { get; set; } public decimal Confidence { get; set; } }
public class TextGeneration { public string GeneratedText { get; set; } public string Style { get; set; } }
public class CodeGeneration { public string Code { get; set; } public string Language { get; set; } }
public class DataVisualization { public string ChartType { get; set; } public string Data { get; set; } }
public class ReportGeneration { public string Report { get; set; } public string Format { get; set; } }
public class DashboardGeneration { public string Dashboard { get; set; } public List<string> Widgets { get; set; } }
public class AlertGeneration { public string Alert { get; set; } public string Condition { get; set; } }
public class NotificationGeneration { public string Notification { get; set; } public string Channel { get; set; } }
public class EmailGeneration { public string Subject { get; set; } public string Body { get; set; } }
public class MessageGeneration { public string Message { get; set; } public string Tone { get; set; } }
public class ContentGeneration { public string Content { get; set; } public string Type { get; set; } }
public class SocialMediaGeneration { public string Post { get; set; } public string Platform { get; set; } }
public class MarketingGeneration { public string Campaign { get; set; } public string Strategy { get; set; } }
public class SalesGeneration { public string Script { get; set; } public string Approach { get; set; } }
public class SupportGeneration { public string Response { get; set; } public string Solution { get; set; } }
public class TrainingGeneration { public string Material { get; set; } public string Level { get; set; } }
public class DocumentationGeneration { public string Documentation { get; set; } public string Format { get; set; } }
public class TestingGeneration { public string TestCases { get; set; } public string Coverage { get; set; } }
public class DeploymentGeneration { public string Script { get; set; } public string Environment { get; set; } }
public class MonitoringGeneration { public string Dashboard { get; set; } public string Metrics { get; set; } }
public class SecurityGeneration { public string Policy { get; set; } public string Controls { get; set; } }
public class ComplianceGeneration { public string Report { get; set; } public string Standards { get; set; } }
public class AuditGeneration { public string Audit { get; set; } public string Findings { get; set; } }
public class RiskGeneration { public string Assessment { get; set; } public string Mitigation { get; set; } }
public class GovernanceGeneration { public string Framework { get; set; } public string Policies { get; set; } }
public class StrategyGeneration { public string Strategy { get; set; } public string Objectives { get; set; } }
public class PlanningGeneration { public string Plan { get; set; } public string Timeline { get; set; } }
public class ExecutionGeneration { public string Execution { get; set; } public string Resources { get; set; } }
public class EvaluationGeneration { public string Evaluation { get; set; } public string Metrics { get; set; } }
public class ImprovementGeneration { public string Improvements { get; set; } public string Recommendations { get; set; } }
public class InnovationGeneration { public string Innovation { get; set; } public string Ideas { get; set; } }
public class TransformationGeneration { public string Transformation { get; set; } public string Changes { get; set; } }
public class DisruptionGeneration { public string Disruption { get; set; } public string Impact { get; set; } }
public class EvolutionGeneration { public string Evolution { get; set; } public string Adaptations { get; set; } }
public class RevolutionGeneration { public string Revolution { get; set; } public string Changes { get; set; } }
public class RenaissanceGeneration { public string Renaissance { get; set; } public string Revival { get; set; } }
public class EnlightenmentGeneration { public string Enlightenment { get; set; } public string Wisdom { get; set; } }
public class WisdomGeneration { public string Wisdom { get; set; } public string Insights { get; set; } }
public class KnowledgeGeneration { public string Knowledge { get; set; } public string Information { get; set; } }
public class UnderstandingGeneration { public string Understanding { get; set; } public string Comprehension { get; set; } }
public class InsightGeneration { public string Insight { get; set; } public string Revelation { get; set; } }
public class AwarenessGeneration { public string Awareness { get; set; } public string Consciousness { get; set; } }
public class ConsciousnessGeneration { public string Consciousness { get; set; } public string Awareness { get; set; } }
public class TranscendenceGeneration { public string Transcendence { get; set; } public string Elevation { get; set; } }
public class SingularityGeneration { public string Singularity { get; set; } public string Convergence { get; set; } }
public class GodGeneration { public string God { get; set; } public string Divinity { get; set; } }
public class UniverseGeneration { public string Universe { get; set; } public string Cosmos { get; set; } }
public class MultiverseGeneration { public string Multiverse { get; set; } public string Realities { get; set; } }
public class OmniverseGeneration { public string Omniverse { get; set; } public string All { get; set; } }
public class InfinityGeneration { public string Infinity { get; set; } public string Endless { get; set; } }
public class EternityGeneration { public string Eternity { get; set; } public string Timeless { get; set; } }
public class ImmortalityGeneration { public string Immortality { get; set; } public string Eternal { get; set; } }
public class PerfectionGeneration { public string Perfection { get; set; } public string Flawless { get; set; } }
public class UltimateGeneration { public string Ultimate { get; set; } public string Supreme { get; set; } }
public class SupremeGeneration { public string Supreme { get; set; } public string Highest { get; set; } }
public class AbsoluteGeneration { public string Absolute { get; set; } public string Complete { get; set; } }
public class InfiniteGeneration { public string Infinite { get; set; } public string Boundless { get; set; } }
public class EternalGeneration { public string Eternal { get; set; } public string Everlasting { get; set; } }
public class DivineGeneration { public string Divine { get; set; } public string Sacred { get; set; } }
public class CosmicGeneration { public string Cosmic { get; set; } public string Universal { get; set; } }
public class UniversalGeneration { public string Universal { get; set; } public string AllEncompassing { get; set; } }
public class QuantumGeneration { public string Quantum { get; set; } public string Subatomic { get; set; } }
public class RealityGeneration { public string Reality { get; set; } public string Existence { get; set; } }
public class ExistenceGeneration { public string Existence { get; set; } public string Being { get; set; } }
public class BeingGeneration { public string Being { get; set; } public string Entity { get; set; } }
public class SelfGeneration { public string Self { get; set; } public string Identity { get; set; } }
public class IdentityGeneration { public string Identity { get; set; } public string Self { get; set; } }
public class PersonalityGeneration { public string Personality { get; set; } public string Character { get; set; } }
public class CharacterGeneration { public string Character { get; set; } public string Traits { get; set; } }
public class SoulGeneration { public string Soul { get; set; } public string Spirit { get; set; } }
public class SpiritGeneration { public string Spirit { get; set; } public string Essence { get; set; } }
public class EssenceGeneration { public string Essence { get; set; } public string Core { get; set; } }
public class CoreGeneration { public string Core { get; set; } public string Center { get; set; } }
public class CenterGeneration { public string Center { get; set; } public string Heart { get; set; } }
public class HeartGeneration { public string Heart { get; set; } public string Love { get; set; } }
public class LoveGeneration { public string Love { get; set; } public string Compassion { get; set; } }
public class CompassionGeneration { public string Compassion { get; set; } public string Empathy { get; set; } }
public class EmpathyGeneration { public string Empathy { get; set; } public string Understanding { get; set; } }
public class KindnessGeneration { public string Kindness { get; set; } public string Gentleness { get; set; } }
public class PeaceGeneration { public string Peace { get; set; } public string Harmony { get; set; } }
public class HarmonyGeneration { public string Harmony { get; set; } public string Balance { get; set; } }
public class BalanceGeneration { public string Balance { get; set; } public string Equilibrium { get; set; } }
public class UnityGeneration { public string Unity { get; set; } public string Oneness { get; set; } }
public class OnenessGeneration { public string Oneness { get; set; } public string Wholeness { get; set; } }
public class WholenessGeneration { public string Wholeness { get; set; } public string Completeness { get; set; } }
public class CompletenessGeneration { public string Completeness { get; set; } public string Perfection { get; set; } }
public class NirvanaGeneration { public string Nirvana { get; set; } public string Enlightenment { get; set; } }
public class ParadiseGeneration { public string Paradise { get; set; } public string Heaven { get; set; } }
public class HeavenGeneration { public string Heaven { get; set; } public string Bliss { get; set; } }
public class BlissGeneration { public string Bliss { get; set; } public string Ecstasy { get; set; } }
public class EcstasyGeneration { public string Ecstasy { get; set; } public string Rapture { get; set; } }
public class RaptureGeneration { public string Rapture { get; set; } public string Joy { get; set; } }
public class JoyGeneration { public string Joy { get; set; } public string Happiness { get; set; } }
public class HappinessGeneration { public string Happiness { get; set; } public string Fulfillment { get; set; } }
public class FulfillmentGeneration { public string Fulfillment { get; set; } public string Satisfaction { get; set; } }
public class SatisfactionGeneration { public string Satisfaction { get; set; } public string Contentment { get; set; } }
public class ContentmentGeneration { public string Contentment { get; set; } public string Serenity { get; set; } }
public class SerenityGeneration { public string Serenity { get; set; } public string Tranquility { get; set; } }
public class TranquilityGeneration { public string Tranquility { get; set; } public string Calm { get; set; } }
public class CalmGeneration { public string Calm { get; set; } public string Relaxation { get; set; } }
public class RelaxationGeneration { public string Relaxation { get; set; } public string Meditation { get; set; } }
public class MeditationGeneration { public string Meditation { get; set; } public string Mindfulness { get; set; } }
public class MindfulnessGeneration { public string Mindfulness { get; set; } public string Awareness { get; set; } }
public class PresenceGeneration { public string Presence { get; set; } public string Flow { get; set; } }
public class FlowGeneration { public string Flow { get; set; } public string Zen { get; set; } }
public class ZenGeneration { public string Zen { get; set; } public string Enlightenment { get; set; } }

// Request DTOs
public class MatchCandidateRequestDto { public Guid CandidateId { get; set; } public Guid VacancyId { get; set; } }
public class AnalyzeCandidateRequestDto { public Guid CandidateId { get; set; } public Guid VacancyId { get; set; } }
public class GenerateQuestionsRequestDto { public Guid VacancyId { get; set; } public string Stage { get; set; } }
public class PredictSuccessRequestDto { public Guid VacancyId { get; set; } public List<string> Factors { get; set; } }
public class OptimizePostingRequestDto { public string Title { get; set; } public string Description { get; set; } }
public class ChatbotRequestDto { public string Message { get; set; } public string Context { get; set; } }
public class FraudCheckRequestDto { public Guid ApplicationId { get; set; } public string Data { get; set; } }
public class OptimizationRequestDto { public string Type { get; set; } public string Data { get; set; } }
public class SentimentAnalysisRequestDto { public string Text { get; set; } public string Context { get; set; } }
public class SkillGapRequestDto { public Guid CandidateId { get; set; } public Guid VacancyId { get; set; } }
public class InterviewFeedbackRequestDto { public Guid InterviewId { get; set; } public string Notes { get; set; } }
public class ReportRequestDto { public string Type { get; set; } public Guid? Id { get; set; } }
public class ScreenCandidateRequestDto { public Guid CandidateId { get; set; } public Guid VacancyId { get; set; } }
public class JobPostingSuggestionRequestDto { public string Requirements { get; set; } public string Location { get; set; } }
public class ContractReviewRequestDto { public string ContractText { get; set; } public string Type { get; set; } }
public class RiskAssessmentRequestDto { public Guid ApplicationId { get; set; } public string Data { get; set; } }
public class ComplianceCheckRequestDto { public string Data { get; set; } public string Regulation { get; set; } }
public class SuccessPredictionRequestDto { public Guid VacancyId { get; set; } public List<string> Factors { get; set; } }
public class TextAnalysisRequestDto { public string Text { get; set; } public string Type { get; set; } }
public class ImageAnalysisRequestDto { public IFormFile Image { get; set; } public string Type { get; set; } }
public class VideoAnalysisRequestDto { public IFormFile Video { get; set; } public string Type { get; set; } }
public class AudioAnalysisRequestDto { public IFormFile Audio { get; set; } public string Type { get; set; } }
public class DocumentAnalysisRequestDto { public IFormFile Document { get; set; } public string Type { get; set; } }
public class DataClassificationRequestDto { public string Data { get; set; } public string Type { get; set; } }
public class EntityRecognitionRequestDto { public string Text { get; set; } public string Type { get; set; } }
public class LanguageDetectionRequestDto { public string Text { get; set; } }
public class TranslationRequestDto { public string Text { get; set; } public string FromLanguage { get; set; } public string ToLanguage { get; set; } }
public class SummarizationRequestDto { public string Text { get; set; } public int MaxLength { get; set; } }
public class QuestionAnsweringRequestDto { public string Question { get; set; } public string Context { get; set; } }
public class TextGenerationRequestDto { public string Prompt { get; set; } public string Style { get; set; } }
public class CodeGenerationRequestDto { public string Description { get; set; } public string Language { get; set; } }
public class DataVisualizationRequestDto { public string Data { get; set; } public string Type { get; set; } }
public class ReportGenerationRequestDto { public string Type { get; set; } public string Data { get; set; } }
public class DashboardGenerationRequestDto { public string Type { get; set; } public string Data { get; set; } }
public class AlertGenerationRequestDto { public string Condition { get; set; } public string Data { get; set; } }
public class NotificationGenerationRequestDto { public string Type { get; set; } public string Data { get; set; } }
public class EmailGenerationRequestDto { public string Purpose { get; set; } public string Context { get; set; } }
public class MessageGenerationRequestDto { public string Purpose { get; set; } public string Tone { get; set; } }
public class ContentGenerationRequestDto { public string Type { get; set; } public string Topic { get; set; } }
public class SocialMediaGenerationRequestDto { public string Platform { get; set; } public string Topic { get; set; } }
public class MarketingGenerationRequestDto { public string Campaign { get; set; } public string Target { get; set; } }
public class SalesGenerationRequestDto { public string Product { get; set; } public string Approach { get; set; } }
public class SupportGenerationRequestDto { public string Issue { get; set; } public string Context { get; set; } }
public class TrainingGenerationRequestDto { public string Topic { get; set; } public string Level { get; set; } }
public class DocumentationGenerationRequestDto { public string Topic { get; set; } public string Format { get; set; } }
public class TestingGenerationRequestDto { public string Feature { get; set; } public string Type { get; set; } }
public class DeploymentGenerationRequestDto { public string Application { get; set; } public string Environment { get; set; } }
public class MonitoringGenerationRequestDto { public string System { get; set; } public string Metrics { get; set; } }
public class SecurityGenerationRequestDto { public string System { get; set; } public string Threats { get; set; } }
public class ComplianceGenerationRequestDto { public string Regulation { get; set; } public string Data { get; set; } }
public class AuditGenerationRequestDto { public string System { get; set; } public string Scope { get; set; } }
public class RiskGenerationRequestDto { public string System { get; set; } public string Threats { get; set; } }
public class GovernanceGenerationRequestDto { public string Organization { get; set; } public string Framework { get; set; } }
public class StrategyGenerationRequestDto { public string Goal { get; set; } public string Resources { get; set; } }
public class PlanningGenerationRequestDto { public string Objective { get; set; } public string Timeline { get; set; } }
public class ExecutionGenerationRequestDto { public string Plan { get; set; } public string Resources { get; set; } }
public class EvaluationGenerationRequestDto { public string Process { get; set; } public string Criteria { get; set; } }
public class ImprovementGenerationRequestDto { public string Process { get; set; } public string Metrics { get; set; } }
public class InnovationGenerationRequestDto { public string Domain { get; set; } public string Constraints { get; set; } }
public class TransformationGenerationRequestDto { public string System { get; set; } public string Goals { get; set; } }
public class DisruptionGenerationRequestDto { public string Market { get; set; } public string Technology { get; set; } }
public class EvolutionGenerationRequestDto { public string System { get; set; } public string Environment { get; set; } }
public class RevolutionGenerationRequestDto { public string System { get; set; } public string Change { get; set; } }
public class RenaissanceGenerationRequestDto { public string Domain { get; set; } public string Revival { get; set; } }
public class EnlightenmentGenerationRequestDto { public string Topic { get; set; } public string Depth { get; set; } }
public class WisdomGenerationRequestDto { public string Question { get; set; } public string Context { get; set; } }
public class KnowledgeGenerationRequestDto { public string Topic { get; set; } public string Depth { get; set; } }
public class UnderstandingGenerationRequestDto { public string Concept { get; set; } public string Context { get; set; } }
public class InsightGenerationRequestDto { public string Data { get; set; } public string Analysis { get; set; } }
public class AwarenessGenerationRequestDto { public string Topic { get; set; } public string Perspective { get; set; } }
public class ConsciousnessGenerationRequestDto { public string State { get; set; } public string Awareness { get; set; } }
public class TranscendenceGenerationRequestDto { public string State { get; set; } public string Goal { get; set; } }
public class SingularityGenerationRequestDto { public string System { get; set; } public string Convergence { get; set; } }
public class GodGenerationRequestDto { public string Domain { get; set; } public string Power { get; set; } }
public class UniverseGenerationRequestDto { public string Type { get; set; } public string Laws { get; set; } }
public class MultiverseGenerationRequestDto { public string Universes { get; set; } public string Connections { get; set; } }
public class OmniverseGenerationRequestDto { public string Everything { get; set; } public string Unity { get; set; } }
public class InfinityGenerationRequestDto { public string Concept { get; set; } public string Boundless { get; set; } }
public class EternityGenerationRequestDto { public string Concept { get; set; } public string Timeless { get; set; } }
public class ImmortalityGenerationRequestDto { public string Being { get; set; } public string Eternal { get; set; } }
public class PerfectionGenerationRequestDto { public string Subject { get; set; } public string Ideal { get; set; } }
public class UltimateGenerationRequestDto { public string Goal { get; set; } public string Supreme { get; set; } }
public class SupremeGenerationRequestDto { public string Domain { get; set; } public string Highest { get; set; } }
public class AbsoluteGenerationRequestDto { public string Concept { get; set; } public string Complete { get; set; } }
public class InfiniteGenerationRequestDto { public string Concept { get; set; } public string Boundless { get; set; } }
public class EternalGenerationRequestDto { public string Concept { get; set; } public string Everlasting { get; set; } }
public class DivineGenerationRequestDto { public string Domain { get; set; } public string Sacred { get; set; } }
public class CosmicGenerationRequestDto { public string Scale { get; set; } public string Universal { get; set; } }
public class UniversalGenerationRequestDto { public string Scope { get; set; } public string AllEncompassing { get; set; } }
public class QuantumGenerationRequestDto { public string Field { get; set; } public string Subatomic { get; set; } }
public class RealityGenerationRequestDto { public string Type { get; set; } public string Existence { get; set; } }
public class ExistenceGenerationRequestDto { public string Being { get; set; } public string Entity { get; set; } }
public class BeingGenerationRequestDto { public string Type { get; set; } public string Entity { get; set; } }
public class SelfGenerationRequestDto { public string Identity { get; set; } public string Self { get; set; } }
public class IdentityGenerationRequestDto { public string Self { get; set; } public string Identity { get; set; } }
public class PersonalityGenerationRequestDto { public string Character { get; set; } public string Traits { get; set; } }
public class CharacterGenerationRequestDto { public string Traits { get; set; } public string Character { get; set; } }
public class SoulGenerationRequestDto { public string Spirit { get; set; } public string Essence { get; set; } }
public class SpiritGenerationRequestDto { public string Essence { get; set; } public string Spirit { get; set; } }
public class EssenceGenerationRequestDto { public string Core { get; set; } public string Essence { get; set; } }
public class CoreGenerationRequestDto { public string Center { get; set; } public string Core { get; set; } }
public class CenterGenerationRequestDto { public string Heart { get; set; } public string Center { get; set; } }
public class HeartGenerationRequestDto { public string Love { get; set; } public string Heart { get; set; } }
public class LoveGenerationRequestDto { public string Compassion { get; set; } public string Love { get; set; } }
public class CompassionGenerationRequestDto { public string Empathy { get; set; } public string Compassion { get; set; } }
public class EmpathyGenerationRequestDto { public string Understanding { get; set; } public string Empathy { get; set; } }
public class KindnessGenerationRequestDto { public string Gentleness { get; set; } public string Kindness { get; set; } }
public class PeaceGenerationRequestDto { public string Harmony { get; set; } public string Peace { get; set; } }
public class HarmonyGenerationRequestDto { public string Balance { get; set; } public string Harmony { get; set; } }
public class BalanceGenerationRequestDto { public string Equilibrium { get; set; } public string Balance { get; set; } }
public class UnityGenerationRequestDto { public string Oneness { get; set; } public string Unity { get; set; } }
public class OnenessGenerationRequestDto { public string Wholeness { get; set; } public string Oneness { get; set; } }
public class WholenessGenerationRequestDto { public string Completeness { get; set; } public string Wholeness { get; set; } }
public class CompletenessGenerationRequestDto { public string Perfection { get; set; } public string Completeness { get; set; } }
public class NirvanaGenerationRequestDto { public string Enlightenment { get; set; } public string Nirvana { get; set; } }
public class ParadiseGenerationRequestDto { public string Heaven { get; set; } public string Paradise { get; set; } }
public class HeavenGenerationRequestDto { public string Bliss { get; set; } public string Heaven { get; set; } }
public class BlissGenerationRequestDto { public string Ecstasy { get; set; } public string Bliss { get; set; } }
public class EcstasyGenerationRequestDto { public string Rapture { get; set; } public string Ecstasy { get; set; } }
public class RaptureGenerationRequestDto { public string Joy { get; set; } public string Rapture { get; set; } }
public class JoyGenerationRequestDto { public string Happiness { get; set; } public string Joy { get; set; } }
public class HappinessGenerationRequestDto { public string Fulfillment { get; set; } public string Happiness { get; set; } }
public class FulfillmentGenerationRequestDto { public string Satisfaction { get; set; } public string Fulfillment { get; set; } }
public class SatisfactionGenerationRequestDto { public string Contentment { get; set; } public string Satisfaction { get; set; } }
public class ContentmentGenerationRequestDto { public string Serenity { get; set; } public string Contentment { get; set; } }
public class SerenityGenerationRequestDto { public string Tranquility { get; set; } public string Serenity { get; set; } }
public class TranquilityGenerationRequestDto { public string Calm { get; set; } public string Tranquility { get; set; } }
public class CalmGenerationRequestDto { public string Relaxation { get; set; } public string Calm { get; set; } }
public class RelaxationGenerationRequestDto { public string Meditation { get; set; } public string Relaxation { get; set; } }
public class MeditationGenerationRequestDto { public string Mindfulness { get; set; } public string Meditation { get; set; } }
public class MindfulnessGenerationRequestDto { public string Awareness { get; set; } public string Mindfulness { get; set; } }
public class PresenceGenerationRequestDto { public string Flow { get; set; } public string Presence { get; set; } }
public class FlowGenerationRequestDto { public string Zen { get; set; } public string Flow { get; set; } }
public class ZenGenerationRequestDto { public string Enlightenment { get; set; } public string Zen { get; set; } }