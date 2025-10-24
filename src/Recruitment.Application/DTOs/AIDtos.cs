
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Recruitment.Application.DTOs;

// Request DTOs
public class ParseCVRequestDto
{
    public IFormFile File { get; set; }
}

public class MatchCandidateRequestDto
{
    public Guid CandidateId { get; set; }
    public Guid VacancyId { get; set; }
}

public class AnalyzeCandidateRequestDto
{
    public Guid CandidateId { get; set; }
    public string AnalysisType { get; set; }
}

public class GenerateQuestionsRequestDto
{
    public Guid VacancyId { get; set; }
    public string QuestionType { get; set; }
    public int QuestionCount { get; set; }
}

public class PredictSuccessRequestDto
{
    public Guid CandidateId { get; set; }
    public Guid VacancyId { get; set; }
}

public class OptimizePostingRequestDto
{
    public Guid VacancyId { get; set; }
    public string CurrentDescription { get; set; }
}

public class ChatbotRequestDto
{
    public string Message { get; set; }
    public string Context { get; set; }
}

public class FraudCheckRequestDto
{
    public Guid CandidateId { get; set; }
    public string CheckType { get; set; }
}

public class OptimizationRequestDto
{
    public string Metric { get; set; }
    public string TargetArea { get; set; }
}

public class SentimentAnalysisRequestDto
{
    public string Text { get; set; }
    public string Context { get; set; }
}

public class SkillGapRequestDto
{
    public Guid CandidateId { get; set; }
    public Guid VacancyId { get; set; }
}

public class InterviewFeedbackRequestDto
{
    public Guid ApplicationId { get; set; }
    public string FeedbackType { get; set; }
}

public class ReportRequestDto
{
    public string ReportType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class SuccessPredictionRequestDto
{
    public Guid CandidateId { get; set; }
    public string PredictionType { get; set; }
}

public class ScreenCandidateRequestDto
{
    public Guid CandidateId { get; set; }
    public Guid VacancyId { get; set; }
}

public class JobPostingSuggestionRequestDto
{
    public string JobTitle { get; set; }
    public string Industry { get; set; }
    public string Location { get; set; }
}

public class ContractReviewRequestDto
{
    public string ContractText { get; set; }
    public string ContractType { get; set; }
}

public class RiskAssessmentRequestDto
{
    public Guid CandidateId { get; set; }
    public string AssessmentType { get; set; }
}

public class ComplianceCheckRequestDto
{
    public string Regulation { get; set; }
    public string CheckType { get; set; }
}

// Text and Content Analysis DTOs
public class TextAnalysisRequestDto
{
    public string Text { get; set; }
    public string AnalysisType { get; set; }
}

public class ImageAnalysisRequestDto
{
    public IFormFile Image { get; set; }
    public string AnalysisType { get; set; }
}

public class VideoAnalysisRequestDto
{
    public IFormFile Video { get; set; }
    public string AnalysisType { get; set; }
}

public class AudioAnalysisRequestDto
{
    public IFormFile Audio { get; set; }
    public string AnalysisType { get; set; }
}

public class DocumentAnalysisRequestDto
{
    public IFormFile Document { get; set; }
    public string AnalysisType { get; set; }
}

public class DataClassificationRequestDto
{
    public string Data { get; set; }
    public string ClassificationType { get; set; }
}

public class EntityRecognitionRequestDto
{
    public string Text { get; set; }
    public string EntityType { get; set; }
}

public class LanguageDetectionRequestDto
{
    public string Text { get; set; }
}

public class TranslationRequestDto
{
    public string Text { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
}

public class SummarizationRequestDto
{
    public string Text { get; set; }
    public int MaxLength { get; set; }
}

public class QuestionAnsweringRequestDto
{
    public string Question { get; set; }
    public string Context { get; set; }
}

public class TextGenerationRequestDto
{
    public string Prompt { get; set; }
    public string GenerationType { get; set; }
    public int MaxTokens { get; set; }
}

public class CodeGenerationRequestDto
{
    public string Description { get; set; }
    public string Language { get; set; }
    public string Framework { get; set; }
}

public class DataVisualizationRequestDto
{
    public string Data { get; set; }
    public string ChartType { get; set; }
    public Dictionary<string, string> Parameters { get; set; }
}

// Generation DTOs for various content types
public class ReportGenerationRequestDto
{
    public string ReportType { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}

public class DashboardGenerationRequestDto
{
    public string DashboardType { get; set; }
    public List<string> Metrics { get; set; }
}

public class AlertGenerationRequestDto
{
    public string AlertType { get; set; }
    public Dictionary<string, object> Conditions { get; set; }
}

public class NotificationGenerationRequestDto
{
    public string NotificationType { get; set; }
    public string Recipient { get; set; }
    public Dictionary<string, object> Data { get; set; }
}

public class EmailGenerationRequestDto
{
    public string EmailType { get; set; }
    public string Recipient { get; set; }
    public Dictionary<string, object> Context { get; set; }
}

public class MessageGenerationRequestDto
{
    public string MessageType { get; set; }
    public string Recipient { get; set; }
    public string Context { get; set; }
}

public class ContentGenerationRequestDto
{
    public string ContentType { get; set; }
    public string Topic { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}

public class SocialMediaGenerationRequestDto
{
    public string Platform { get; set; }
    public string ContentType { get; set; }
    public string Topic { get; set; }
}

public class MarketingGenerationRequestDto
{
    public string MarketingType { get; set; }
    public string TargetAudience { get; set; }
    public Dictionary<string, object> CampaignData { get; set; }
}

public class SalesGenerationRequestDto
{
    public string SalesType { get; set; }
    public string Product { get; set; }
    public Dictionary<string, object> Context { get; set; }
}

public class SupportGenerationRequestDto
{
    public string SupportType { get; set; }
    public string Issue { get; set; }
    public string Context { get; set; }
}

public class TrainingGenerationRequestDto
{
    public string TrainingType { get; set; }
    public string Topic { get; set; }
    public string SkillLevel { get; set; }
}

public class DocumentationGenerationRequestDto
{
    public string DocumentationType { get; set; }
    public string Topic { get; set; }
    public string Format { get; set; }
}

public class TestingGenerationRequestDto
{
    public string TestingType { get; set; }
    public string Component { get; set; }
    public string Framework { get; set; }
}

public class DeploymentGenerationRequestDto
{
    public string DeploymentType { get; set; }
    public string Environment { get; set; }
    public Dictionary<string, object> Configuration { get; set; }
}

public class MonitoringGenerationRequestDto
{
    public string MonitoringType { get; set; }
    public List<string> Metrics { get; set; }
    public string Environment { get; set; }
}

public class SecurityGenerationRequestDto
{
    public string SecurityType { get; set; }
    public string Component { get; set; }
    public string SecurityLevel { get; set; }
}

public class ComplianceGenerationRequestDto
{
    public string ComplianceType { get; set; }
    public string Regulation { get; set; }
    public string Industry { get; set; }
}

public class AuditGenerationRequestDto
{
    public string AuditType { get; set; }
    public string Scope { get; set; }
    public DateTime AuditDate { get; set; }
}

public class RiskGenerationRequestDto
{
    public string RiskType { get; set; }
    public string AssessmentArea { get; set; }
    public string RiskLevel { get; set; }
}

public class GovernanceGenerationRequestDto
{
    public string GovernanceType { get; set; }
    public string Framework { get; set; }
    public string Scope { get; set; }
}

public class StrategyGenerationRequestDto
{
    public string StrategyType { get; set; }
    public string BusinessArea { get; set; }
    public Dictionary<string, object> Objectives { get; set; }
}

public class PlanningGenerationRequestDto
{
    public string PlanningType { get; set; }
    public string Timeframe { get; set; }
    public Dictionary<string, object> Resources { get; set; }
}

public class ExecutionGenerationRequestDto
{
    public string ExecutionType { get; set; }
    public string Plan { get; set; }
    public Dictionary<string, object> Milestones { get; set; }
}

public class EvaluationGenerationRequestDto
{
    public string EvaluationType { get; set; }
    public string Criteria { get; set; }
    public Dictionary<string, object> Metrics { get; set; }
}

public class ImprovementGenerationRequestDto
{
    public string ImprovementType { get; set; }
    public string Area { get; set; }
    public Dictionary<string, object> CurrentState { get; set; }
}

public class InnovationGenerationRequestDto
{
    public string InnovationType { get; set; }
    public string Domain { get; set; }
    public Dictionary<string, object> Constraints { get; set; }
}

public class TransformationGenerationRequestDto
{
    public string TransformationType { get; set; }
    public string CurrentState { get; set; }
    public string TargetState { get; set; }
}

public class DisruptionGenerationRequestDto
{
    public string DisruptionType { get; set; }
    public string Market { get; set; }
    public Dictionary<string, object> Analysis { get; set; }
}

public class EvolutionGenerationRequestDto
{
    public string EvolutionType { get; set; }
    public string System { get; set; }
    public Dictionary<string, object> Trajectory { get; set; }
}

public class RevolutionGenerationRequestDto
{
    public string RevolutionType { get; set; }
    public string Paradigm { get; set; }
    public Dictionary<string, object> Vision { get; set; }
}

public class RenaissanceGenerationRequestDto
{
    public string RenaissanceType { get; set; }
    public string CulturalContext { get; set; }
    public Dictionary<string, object> Values { get; set; }
}

public class EnlightenmentGenerationRequestDto
{
    public string EnlightenmentType { get; set; }
    public string KnowledgeDomain { get; set; }
    public Dictionary<string, object> Inquiry { get; set; }
}

public class WisdomGenerationRequestDto
{
    public string WisdomType { get; set; }
    public string Experience { get; set; }
    public Dictionary<string, object> Reflection { get; set; }
}

public class KnowledgeGenerationRequestDto
{
    public string KnowledgeType { get; set; }
    public string Domain { get; set; }
    public Dictionary<string, object> Sources { get; set; }
}

public class UnderstandingGenerationRequestDto
{
    public string UnderstandingType { get; set; }
    public string Concept { get; set; }
    public Dictionary<string, object> Framework { get; set; }
}

public class InsightGenerationRequestDto
{
    public string InsightType { get; set; }
    public string Data { get; set; }
    public Dictionary<string, object> Analysis { get; set; }
}

public class AwarenessGenerationRequestDto
{
    public string AwarenessType { get; set; }
    public string Consciousness { get; set; }
    public Dictionary<string, object> Perception { get; set; }
}

public class ConsciousnessGenerationRequestDto
{
    public string ConsciousnessType { get; set; }
    public string Mind { get; set; }
    public Dictionary<string, object> Awareness { get; set; }
}

public class TranscendenceGenerationRequestDto
{
    public string TranscendenceType { get; set; }
    public string Limitation { get; set; }
    public Dictionary<string, object> Aspiration { get; set; }
}

public class SingularityGenerationRequestDto
{
    public string SingularityType { get; set; }
    public string Technology { get; set; }
    public Dictionary<string, object> Convergence { get; set; }
}

public class GodGenerationRequestDto
{
    public string GodType { get; set; }
    public string Divinity { get; set; }
    public Dictionary<string, object> Creation { get; set; }
}

public class UniverseGenerationRequestDto
{
    public string UniverseType { get; set; }
    public string Cosmology { get; set; }
    public Dictionary<string, object> Physics { get; set; }
}

public class MultiverseGenerationRequestDto
{
    public string MultiverseType { get; set; }
    public string Reality { get; set; }
    public Dictionary<string, object> Dimensions { get; set; }
}

public class OmniverseGenerationRequestDto
{
    public string OmniverseType { get; set; }
    public string Existence { get; set; }
    public Dictionary<string, object> Realities { get; set; }
}

public class InfinityGenerationRequestDto
{
    public string InfinityType { get; set; }
    public string Mathematics { get; set; }
    public Dictionary<string, object> Bounds { get; set; }
}

public class EternityGenerationRequestDto
{
    public string EternityType { get; set; }
    public string Time { get; set; }
    public Dictionary<string, object> Duration { get; set; }
}

public class ImmortalityGenerationRequestDto
{
    public string ImmortalityType { get; set; }
    public string Life { get; set; }
    public Dictionary<string, object> Essence { get; set; }
}

public class PerfectionGenerationRequestDto
{
    public string PerfectionType { get; set; }
    public string Ideal { get; set; }
    public Dictionary<string, object> Flawlessness { get; set; }
}

public class UltimateGenerationRequestDto
{
    public string UltimateType { get; set; }
    public string Supremacy { get; set; }
    public Dictionary<string, object> Pinnacle { get; set; }
}

public class SupremeGenerationRequestDto
{
    public string SupremeType { get; set; }
    public string Authority { get; set; }
    public Dictionary<string, object> Dominion { get; set; }
}

public class AbsoluteGenerationRequestDto
{
    public string AbsoluteType { get; set; }
    public string Truth { get; set; }
    public Dictionary<string, object> Certainty { get; set; }
}

public class InfiniteGenerationRequestDto
{
    public string InfiniteType { get; set; }
    public string Boundlessness { get; set; }
    public Dictionary<string, object> Eternity { get; set; }
}

public class EternalGenerationRequestDto
{
    public string EternalType { get; set; }
    public string Permanence { get; set; }
    public Dictionary<string, object> Timelessness { get; set; }
}

public class DivineGenerationRequestDto
{
    public string DivineType { get; set; }
    public string Sacredness { get; set; }
    public Dictionary<string, object> Holiness { get; set; }
}

public class CosmicGenerationRequestDto
{
    public string CosmicType { get; set; }
    public string Universe { get; set; }
    public Dictionary<string, object> Cosmos { get; set; }
}

public class UniversalGenerationRequestDto
{
    public string UniversalType { get; set; }
    public string Everything { get; set; }
    public Dictionary<string, object> Unity { get; set; }
}

public class QuantumGenerationRequestDto
{
    public string QuantumType { get; set; }
    public string Physics { get; set; }
    public Dictionary<string, object> Particles { get; set; }
}

public class RealityGenerationRequestDto
{
    public string RealityType { get; set; }
    public string Existence { get; set; }
    public Dictionary<string, object> Perception { get; set; }
}

public class ExistenceGenerationRequestDto
{
    public string ExistenceType { get; set; }
    public string Being { get; set; }
    public Dictionary<string, object> Ontology { get; set; }
}

public class BeingGenerationRequestDto
{
    public string BeingType { get; set; }
    public string Essence { get; set; }
    public Dictionary<string, object> Existence { get; set; }
}

public class SelfGenerationRequestDto
{
    public string SelfType { get; set; }
    public string Identity { get; set; }
    public Dictionary<string, object> Consciousness { get; set; }
}

public class IdentityGenerationRequestDto
{
    public string IdentityType { get; set; }
    public string Self { get; set; }
    public Dictionary<string, object> Uniqueness { get; set; }
}

public class PersonalityGenerationRequestDto
{
    public string PersonalityType { get; set; }
    public string Character { get; set; }
    public Dictionary<string, object> Traits { get; set; }
}

public class CharacterGenerationRequestDto
{
    public string CharacterType { get; set; }
    public string Personality { get; set; }
    public Dictionary<string, object> Values { get; set; }
}

public class SoulGenerationRequestDto
{
    public string SoulType { get; set; }
    public string Spirit { get; set; }
    public Dictionary<string, object> Essence { get; set; }
}

public class SpiritGenerationRequestDto
{
    public string SpiritType { get; set; }
    public string Soul { get; set; }
    public Dictionary<string, object> Energy { get; set; }
}

public class EssenceGenerationRequestDto
{
    public string EssenceType { get; set; }
    public string Core { get; set; }
    public Dictionary<string, object> Nature { get; set; }
}

public class CoreGenerationRequestDto
{
    public string CoreType { get; set; }
    public string Center { get; set; }
    public Dictionary<string, object> Foundation { get; set; }
}

public class CenterGenerationRequestDto
{
    public string CenterType { get; set; }
    public string Core { get; set; }
    public Dictionary<string, object> Balance { get; set; }
}

public class HeartGenerationRequestDto
{
    public string HeartType { get; set; }
    public string Love { get; set; }
    public Dictionary<string, object> Emotion { get; set; }
}

public class LoveGenerationRequestDto
{
    public string LoveType { get; set; }
    public string Heart { get; set; }
    public Dictionary<string, object> Affection { get; set; }
}

public class CompassionGenerationRequestDto
{
    public string CompassionType { get; set; }
    public string Empathy { get; set; }
    public Dictionary<string, object> Kindness { get; set; }
}

public class EmpathyGenerationRequestDto
{
    public string EmpathyType { get; set; }
    public string Understanding { get; set; }
    public Dictionary<string, object> Feeling { get; set; }
}

public class KindnessGenerationRequestDto
{
    public string KindnessType { get; set; }
    public string Compassion { get; set; }
    public Dictionary<string, object> Gentleness { get; set; }
}

public class PeaceGenerationRequestDto
{
    public string PeaceType { get; set; }
    public string Harmony { get; set; }
    public Dictionary<string, object> Tranquility { get; set; }
}

public class HarmonyGenerationRequestDto
{
    public string HarmonyType { get; set; }
    public string Balance { get; set; }
    public Dictionary<string, object> Unity { get; set; }
}

public class BalanceGenerationRequestDto
{
    public string BalanceType { get; set; }
    public string Equilibrium { get; set; }
    public Dictionary<string, object> Stability { get; set; }
}

public class UnityGenerationRequestDto
{
    public string UnityType { get; set; }
    public string Oneness { get; set; }
    public Dictionary<string, object> Togetherness { get; set; }
}

public class OnenessGenerationRequestDto
{
    public string OnenessType { get; set; }
    public string Unity { get; set; }
    public Dictionary<string, object> Wholeness { get; set; }
}

public class WholenessGenerationRequestDto
{
    public string WholenessType { get; set; }
    public string Completeness { get; set; }
    public Dictionary<string, object> Integration { get; set; }
}

public class CompletenessGenerationRequestDto
{
    public string CompletenessType { get; set; }
    public string Wholeness { get; set; }
    public Dictionary<string, object> Fulfillment { get; set; }
}

public class NirvanaGenerationRequestDto
{
    public string NirvanaType { get; set; }
    public string Enlightenment { get; set; }
    public Dictionary<string, object> Liberation { get; set; }
}

public class ParadiseGenerationRequestDto
{
    public string ParadiseType { get; set; }
    public string Heaven { get; set; }
    public Dictionary<string, object> Bliss { get; set; }
}

public class HeavenGenerationRequestDto
{
    public string HeavenType { get; set; }
    public string Paradise { get; set; }
    public Dictionary<string, object> Divinity { get; set; }
}

public class BlissGenerationRequestDto
{
    public string BlissType { get; set; }
    public string Happiness { get; set; }
    public Dictionary<string, object> Ecstasy { get; set; }
}

public class EcstasyGenerationRequestDto
{
    public string EcstasyType { get; set; }
    public string Rapture { get; set; }
    public Dictionary<string, object> Joy { get; set; }
}

public class RaptureGenerationRequestDto
{
    public string RaptureType { get; set; }
    public string Ecstasy { get; set; }
    public Dictionary<string, object> Bliss { get; set; }
}

public class JoyGenerationRequestDto
{
    public string JoyType { get; set; }
    public string Happiness { get; set; }
    public Dictionary<string, object> Delight { get; set; }
}

public class HappinessGenerationRequestDto
{
    public string HappinessType { get; set; }
    public string Joy { get; set; }
    public Dictionary<string, object> Contentment { get; set; }
}

public class FulfillmentGenerationRequestDto
{
    public string FulfillmentType { get; set; }
    public string Satisfaction { get; set; }
    public Dictionary<string, object> Achievement { get; set; }
}

public class SatisfactionGenerationRequestDto
{
    public string SatisfactionType { get; set; }
    public string Contentment { get; set; }
    public Dictionary<string, object> Fulfillment { get; set; }
}

public class ContentmentGenerationRequestDto
{
    public string ContentmentType { get; set; }
    public string Peace { get; set; }
    public Dictionary<string, object> Serenity { get; set; }
}

public class SerenityGenerationRequestDto
{
    public string SerenityType { get; set; }
    public string Peace { get; set; }
    public Dictionary<string, object> Tranquility { get; set; }
}

public class TranquilityGenerationRequestDto
{
    public string TranquilityType { get; set; }
    public string Calm { get; set; }
    public Dictionary<string, object> Peacefulness { get; set; }
}

public class CalmGenerationRequestDto
{
    public string CalmType { get; set; }
    public string Tranquility { get; set; }
    public Dictionary<string, object> Serenity { get; set; }
}

public class RelaxationGenerationRequestDto
{
    public string RelaxationType { get; set; }
    public string Rest { get; set; }
    public Dictionary<string, object> Peace { get; set; }
}

public class MeditationGenerationRequestDto
{
    public string MeditationType { get; set; }
    public string Mindfulness { get; set; }
    public Dictionary<string, object> Awareness { get; set; }
}

public class MindfulnessGenerationRequestDto
{
    public string MindfulnessType { get; set; }
    public string Awareness { get; set; }
    public Dictionary<string, object> Presence { get; set; }
}

public class PresenceGenerationRequestDto
{
    public string PresenceType { get; set; }
    public string Mindfulness { get; set; }
    public Dictionary<string, object> Awareness { get; set; }
}

public class FlowGenerationRequestDto
{
    public string FlowType { get; set; }
    public string Activity { get; set; }
    public Dictionary<string, object> Engagement { get; set; }
}

public class ZenGenerationRequestDto
{
    public string ZenType { get; set; }
    public string Meditation { get; set; }
    public Dictionary<string, object> Enlightenment { get; set; }
}

// Response DTOs
public class ParseCVResponseDto
{
    public string ParsedText { get; set; }
    public Dictionary<string, object> ExtractedData { get; set; }
    public List<string> Skills { get; set; }
    public List<string> Experiences { get; set; }
}

public class MatchCandidateResponseDto
{
    public double MatchScore { get; set; }
    public List<string> MatchingSkills { get; set; }
    public List<string> MissingSkills { get; set; }
    public string Recommendation { get; set; }
}

public class CandidateRecommendationDto
{
    public Guid CandidateId { get; set; }
    public string CandidateName { get; set; }
    public double MatchScore { get; set; }
    public string Summary { get; set; }
}

public class CandidateAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> AreasForImprovement { get; set; }
}

public class InterviewQuestionDto
{
    public string Question { get; set; }
    public string QuestionType { get; set; }
    public string ExpectedAnswer { get; set; }
    public int Difficulty { get; set; }
}

public class VacancyInsightsDto
{
    public string MarketDemand { get; set; }
    public double CompetitionLevel { get; set; }
    public List<string> RequiredSkills { get; set; }
    public string SalaryInsight { get; set; }
}

public class RecruitmentPredictionDto
{
    public double SuccessProbability { get; set; }
    public string RiskFactors { get; set; }
    public List<string> Recommendations { get; set; }
}

public class MarketInsightsDto
{
    public string Location { get; set; }
    public string JobTitle { get; set; }
    public int AverageSalary { get; set; }
    public int JobAvailability { get; set; }
    public List<string> TopSkills { get; set; }
}

public class JobPostingOptimizationDto
{
    public string OptimizedTitle { get; set; }
    public string OptimizedDescription { get; set; }
    public List<string> SuggestedKeywords { get; set; }
    public int EstimatedViews { get; set; }
}

public class SalaryRecommendationDto
{
    public string JobTitle { get; set; }
    public string Location { get; set; }
    public int ExperienceYears { get; set; }
    public int RecommendedSalary { get; set; }
    public int SalaryRangeMin { get; set; }
    public int SalaryRangeMax { get; set; }
}

public class RecruiterAnalysisDto
{
    public Guid RecruiterId { get; set; }
    public double PerformanceScore { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> AreasForImprovement { get; set; }
}

public class ChatbotResponseDto
{
    public string Response { get; set; }
    public string Intent { get; set; }
    public double Confidence { get; set; }
}

public class FraudAnalysisDto
{
    public bool IsFraudulent { get; set; }
    public double FraudScore { get; set; }
    public List<string> RiskFactors { get; set; }
}

public class PredictiveAnalyticsDto
{
    public string Metric { get; set; }
    public List<double> Forecast { get; set; }
    public double Confidence { get; set; }
}

public class TrendAnalysisDto
{
    public string Metric { get; set; }
    public string TrendDirection { get; set; }
    public double TrendStrength { get; set; }
    public List<string> Insights { get; set; }
}

public class OptimizationSuggestionDto
{
    public string Area { get; set; }
    public string Suggestion { get; set; }
    public double ExpectedImpact { get; set; }
}

public class ModelStatusDto
{
    public string ModelName { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdated { get; set; }
    public double Accuracy { get; set; }
}

public class ModelPerformanceDto
{
    public string ModelName { get; set; }
    public double Accuracy { get; set; }
    public double Precision { get; set; }
    public double Recall { get; set; }
    public double F1Score { get; set; }
}

public class UsageStatisticsDto
{
    public int TotalRequests { get; set; }
    public Dictionary<string, int> RequestByType { get; set; }
    public double AverageResponseTime { get; set; }
}

public class SentimentAnalysisDto
{
    public string Sentiment { get; set; }
    public double Confidence { get; set; }
    public Dictionary<string, double> EmotionScores { get; set; }
}

public class DashboardInsightsDto
{
    public List<string> KeyMetrics { get; set; }
    public List<string> Trends { get; set; }
    public List<string> Recommendations { get; set; }
}

public class CandidateRankingDto
{
    public Guid VacancyId { get; set; }
    public List<CandidateRecommendationDto> RankedCandidates { get; set; }
}

public class SkillGapAnalysisDto
{
    public List<string> MissingSkills { get; set; }
    public List<string> OverqualifiedSkills { get; set; }
    public string TrainingRecommendation { get; set; }
}

public class InterviewFeedbackDto
{
    public string FeedbackType { get; set; }
    public string Feedback { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> Improvements { get; set; }
}

public class MarketTrendsDto
{
    public string Location { get; set; }
    public string Industry { get; set; }
    public List<string> EmergingTrends { get; set; }
    public List<string> DecliningTrends { get; set; }
}

public class MarketResearchDto
{
    public string Location { get; set; }
    public string JobCategory { get; set; }
    public Dictionary<string, object> ResearchData { get; set; }
}

public class OutreachSuggestionDto
{
    public string Subject { get; set; }
    public string Message { get; set; }
    public string Tone { get; set; }
}

public class InterviewPreparationDto
{
    public string PreparationType { get; set; }
    public List<string> KeyTopics { get; set; }
    public List<string> PreparationTips { get; set; }
}

public class ContractReviewDto
{
    public List<string> Issues { get; set; }
    public List<string> Recommendations { get; set; }
    public string RiskLevel { get; set; }
}

public class RiskAssessmentDto
{
    public string RiskLevel { get; set; }
    public List<string> RiskFactors { get; set; }
    public List<string> MitigationStrategies { get; set; }
}

public class ComplianceCheckDto
{
    public bool IsCompliant { get; set; }
    public List<string> Violations { get; set; }
    public List<string> Recommendations { get; set; }
}

public class PerformancePredictionDto
{
    public List<double> Predictions { get; set; }
    public double Confidence { get; set; }
    public string Timeframe { get; set; }
}

public class BenchmarkingDto
{
    public string Metric { get; set; }
    public string Location { get; set; }
    public double YourScore { get; set; }
    public double IndustryAverage { get; set; }
    public string Recommendation { get; set; }
}

public class InsightsSummaryDto
{
    public List<string> KeyInsights { get; set; }
    public List<string> ActionItems { get; set; }
    public string Priority { get; set; }
}

public class ActionRecommendationDto
{
    public string Action { get; set; }
    public string Priority { get; set; }
    public string ExpectedOutcome { get; set; }
}

public class SuccessPredictionDto
{
    public double SuccessProbability { get; set; }
    public List<string> SuccessFactors { get; set; }
    public List<string> RiskFactors { get; set; }
}

public class AnomalyDetectionDto
{
    public List<string> Anomalies { get; set; }
    public double Severity { get; set; }
    public string Recommendation { get; set; }
}

public class ForecastingDto
{
    public string Metric { get; set; }
    public List<double> Forecast { get; set; }
    public double Confidence { get; set; }
}

public class CorrelationAnalysisDto
{
    public Dictionary<string, double> Correlations { get; set; }
    public List<string> Insights { get; set; }
}

public class PatternRecognitionDto
{
    public List<string> Patterns { get; set; }
    public double Confidence { get; set; }
    public string Trend { get; set; }
}

public class SeasonalityAnalysisDto
{
    public string Metric { get; set; }
    public Dictionary<string, double> SeasonalPatterns { get; set; }
    public string Insight { get; set; }
}

public class CohortAnalysisDto
{
    public string CohortType { get; set; }
    public Dictionary<string, object> Analysis { get; set; }
    public List<string> Insights { get; set; }
}

public class FunnelAnalysisDto
{
    public string FunnelType { get; set; }
    public Dictionary<string, double> ConversionRates { get; set; }
    public List<string> Bottlenecks { get; set; }
}

public class AttributionAnalysisDto
{
    public string ConversionEvent { get; set; }
    public Dictionary<string, double> AttributionScores { get; set; }
    public List<string> Insights { get; set; }
}

public class LifetimeValueAnalysisDto
{
    public double AverageValue { get; set; }
    public Dictionary<string, double> SegmentValues { get; set; }
    public string Recommendation { get; set; }
}

public class ChurnPredictionDto
{
    public double ChurnRisk { get; set; }
    public List<string> RiskFactors { get; set; }
    public List<string> RetentionStrategies { get; set; }
}

public class RetentionAnalysisDto
{
    public double RetentionRate { get; set; }
    public Dictionary<string, double> RetentionBySegment { get; set; }
    public List<string> Insights { get; set; }
}

public class GrowthPredictionDto
{
    public List<double> GrowthForecast { get; set; }
    public double Confidence { get; set; }
    public List<string> GrowthDrivers { get; set; }
}

public class MarketBasketAnalysisDto
{
    public Dictionary<string, List<string>> Associations { get; set; }
    public List<string> Insights { get; set; }
}

public class AIAlertDto
{
    public string AlertType { get; set; }
    public string Message { get; set; }
    public string Severity { get; set; }
    public DateTime Timestamp { get; set; }
}

public class PersonalizedRecommendationDto
{
    public string RecommendationType { get; set; }
    public string Content { get; set; }
    public double RelevanceScore { get; set; }
}

public class PerformanceCoachingDto
{
    public List<string> Strengths { get; set; }
    public List<string> AreasForImprovement { get; set; }
    public List<string> CoachingTips { get; set; }
}

public class CandidateScreeningDto
{
    public bool Recommended { get; set; }
    public double Score { get; set; }
    public List<string> Reasons { get; set; }
}

public class JobPostingSuggestionDto
{
    public string SuggestedTitle { get; set; }
    public string SuggestedDescription { get; set; }
    public List<string> Keywords { get; set; }
}

public class TextAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
}

public class ImageAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
}

public class VideoAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
}

public class AudioAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
}

public class DocumentAnalysisDto
{
    public string AnalysisType { get; set; }
    public Dictionary<string, object> Results { get; set; }
}

public class DataClassificationDto
{
    public string Classification { get; set; }
    public double Confidence { get; set; }
    public Dictionary<string, double> CategoryScores { get; set; }
}

public class EntityRecognitionDto
{
    public List<string> Entities { get; set; }
    public Dictionary<string, string> EntityTypes { get; set; }
}

public class LanguageDetectionDto
{
    public string DetectedLanguage { get; set; }
    public double Confidence { get; set; }
}

public class TranslationDto
{
    public string TranslatedText { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; }
}

public class SummarizationDto
{
    public string Summary { get; set; }
    public int OriginalLength { get; set; }
    public int SummaryLength { get; set; }
}

public class QuestionAnsweringDto
{
    public string Answer { get; set; }
    public double Confidence { get; set; }
    public string Context { get; set; }
}

public class TextGenerationDto
{
    public string GeneratedText { get; set; }
    public string GenerationType { get; set; }
}

public class CodeGenerationDto
{
    public string GeneratedCode { get; set; }
    public string Language { get; set; }
    public string Explanation { get; set; }
}

public class DataVisualizationDto
{
    public string VisualizationType { get; set; }
    public string VisualizationData { get; set; }
}

public class ReportGenerationDto
{
    public string ReportContent { get; set; }
    public string ReportType { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
}

public class DashboardGenerationDto
{
    public string DashboardContent { get; set; }
    public List<string> Components { get; set; }
}

public class AlertGenerationDto
{
    public string AlertContent { get; set; }
    public string AlertType { get; set; }
}

public class NotificationGenerationDto
{
    public string NotificationContent { get; set; }
    public string NotificationType { get; set; }
}

public class EmailGenerationDto
{
    public string EmailContent { get; set; }
    public string Subject { get; set; }
}

public class MessageGenerationDto
{
    public string MessageContent { get; set; }
    public string MessageType { get; set; }
}

public class ContentGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ContentType { get; set; }
}

public class SocialMediaGenerationDto
{
    public string GeneratedContent { get; set; }
    public string Platform { get; set; }
}

public class MarketingGenerationDto
{
    public string GeneratedContent { get; set; }
    public string MarketingType { get; set; }
}

public class SalesGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SalesType { get; set; }
}

public class SupportGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SupportType { get; set; }
}

public class TrainingGenerationDto
{
    public string GeneratedContent { get; set; }
    public string TrainingType { get; set; }
}

public class DocumentationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string DocumentationType { get; set; }
}

public class TestingGenerationDto
{
    public string GeneratedContent { get; set; }
    public string TestingType { get; set; }
}

public class DeploymentGenerationDto
{
    public string GeneratedContent { get; set; }
    public string DeploymentType { get; set; }
}

public class MonitoringGenerationDto
{
    public string GeneratedContent { get; set; }
    public string MonitoringType { get; set; }
}

public class SecurityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SecurityType { get; set; }
}

public class ComplianceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ComplianceType { get; set; }
}

public class AuditGenerationDto
{
    public string GeneratedContent { get; set; }
    public string AuditType { get; set; }
}

public class RiskGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RiskType { get; set; }
}

public class GovernanceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string GovernanceType { get; set; }
}

public class StrategyGenerationDto
{
    public string GeneratedContent { get; set; }
    public string StrategyType { get; set; }
}

public class PlanningGenerationDto
{
    public string GeneratedContent { get; set; }
    public string PlanningType { get; set; }
}

public class ExecutionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ExecutionType { get; set; }
}

public class EvaluationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EvaluationType { get; set; }
}

public class ImprovementGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ImprovementType { get; set; }
}

public class InnovationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string InnovationType { get; set; }
}

public class TransformationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string TransformationType { get; set; }
}

public class DisruptionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string DisruptionType { get; set; }
}

public class EvolutionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EvolutionType { get; set; }
}

public class RevolutionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RevolutionType { get; set; }
}

public class RenaissanceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RenaissanceType { get; set; }
}

public class EnlightenmentGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EnlightenmentType { get; set; }
}

public class WisdomGenerationDto
{
    public string GeneratedContent { get; set; }
    public string WisdomType { get; set; }
}

public class KnowledgeGenerationDto
{
    public string GeneratedContent { get; set; }
    public string KnowledgeType { get; set; }
}

public class UnderstandingGenerationDto
{
    public string GeneratedContent { get; set; }
    public string UnderstandingType { get; set; }
}

public class InsightGenerationDto
{
    public string GeneratedContent { get; set; }
    public string InsightType { get; set; }
}

public class AwarenessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string AwarenessType { get; set; }
}

public class ConsciousnessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ConsciousnessType { get; set; }
}

public class TranscendenceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string TranscendenceType { get; set; }
}

public class SingularityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SingularityType { get; set; }
}

public class GodGenerationDto
{
    public string GeneratedContent { get; set; }
    public string GodType { get; set; }
}

public class UniverseGenerationDto
{
    public string GeneratedContent { get; set; }
    public string UniverseType { get; set; }
}

public class MultiverseGenerationDto
{
    public string GeneratedContent { get; set; }
    public string MultiverseType { get; set; }
}

public class OmniverseGenerationDto
{
    public string GeneratedContent { get; set; }
    public string OmniverseType { get; set; }
}

public class InfinityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string InfinityType { get; set; }
}

public class EternityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EternityType { get; set; }
}

public class ImmortalityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ImmortalityType { get; set; }
}

public class PerfectionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string PerfectionType { get; set; }
}

public class UltimateGenerationDto
{
    public string GeneratedContent { get; set; }
    public string UltimateType { get; set; }
}

public class SupremeGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SupremeType { get; set; }
}

public class AbsoluteGenerationDto
{
    public string GeneratedContent { get; set; }
    public string AbsoluteType { get; set; }
}

public class InfiniteGenerationDto
{
    public string GeneratedContent { get; set; }
    public string InfiniteType { get; set; }
}

public class EternalGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EternalType { get; set; }
}

public class DivineGenerationDto
{
    public string GeneratedContent { get; set; }
    public string DivineType { get; set; }
}

public class CosmicGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CosmicType { get; set; }
}

public class UniversalGenerationDto
{
    public string GeneratedContent { get; set; }
    public string UniversalType { get; set; }
}

public class QuantumGenerationDto
{
    public string GeneratedContent { get; set; }
    public string QuantumType { get; set; }
}

public class RealityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RealityType { get; set; }
}

public class ExistenceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ExistenceType { get; set; }
}

public class BeingGenerationDto
{
    public string GeneratedContent { get; set; }
    public string BeingType { get; set; }
}

public class SelfGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SelfType { get; set; }
}

public class IdentityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string IdentityType { get; set; }
}

public class PersonalityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string PersonalityType { get; set; }
}

public class CharacterGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CharacterType { get; set; }
}

public class SoulGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SoulType { get; set; }
}

public class SpiritGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SpiritType { get; set; }
}

public class EssenceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EssenceType { get; set; }
}

public class CoreGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CoreType { get; set; }
}

public class CenterGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CenterType { get; set; }
}

public class HeartGenerationDto
{
    public string GeneratedContent { get; set; }
    public string HeartType { get; set; }
}

public class LoveGenerationDto
{
    public string GeneratedContent { get; set; }
    public string LoveType { get; set; }
}

public class CompassionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CompassionType { get; set; }
}

public class EmpathyGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EmpathyType { get; set; }
}

public class KindnessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string KindnessType { get; set; }
}

public class PeaceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string PeaceType { get; set; }
}

public class HarmonyGenerationDto
{
    public string GeneratedContent { get; set; }
    public string HarmonyType { get; set; }
}

public class BalanceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string BalanceType { get; set; }
}

public class UnityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string UnityType { get; set; }
}

public class OnenessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string OnenessType { get; set; }
}

public class WholenessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string WholenessType { get; set; }
}

public class CompletenessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CompletenessType { get; set; }
}

public class NirvanaGenerationDto
{
    public string GeneratedContent { get; set; }
    public string NirvanaType { get; set; }
}

public class ParadiseGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ParadiseType { get; set; }
}

public class HeavenGenerationDto
{
    public string GeneratedContent { get; set; }
    public string HeavenType { get; set; }
}

public class BlissGenerationDto
{
    public string GeneratedContent { get; set; }
    public string BlissType { get; set; }
}

public class EcstasyGenerationDto
{
    public string GeneratedContent { get; set; }
    public string EcstasyType { get; set; }
}

public class RaptureGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RaptureType { get; set; }
}

public class JoyGenerationDto
{
    public string GeneratedContent { get; set; }
    public string JoyType { get; set; }
}

public class HappinessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string HappinessType { get; set; }
}

public class FulfillmentGenerationDto
{
    public string GeneratedContent { get; set; }
    public string FulfillmentType { get; set; }
}

public class SatisfactionGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SatisfactionType { get; set; }
}

public class ContentmentGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ContentmentType { get; set; }
}

public class SerenityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string SerenityType { get; set; }
}

public class TranquilityGenerationDto
{
    public string GeneratedContent { get; set; }
    public string TranquilityType { get; set; }
}

public class CalmGenerationDto
{
    public string GeneratedContent { get; set; }
    public string CalmType { get; set; }
}

public class RelaxationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string RelaxationType { get; set; }
}

public class MeditationGenerationDto
{
    public string GeneratedContent { get; set; }
    public string MeditationType { get; set; }
}

public class MindfulnessGenerationDto
{
    public string GeneratedContent { get; set; }
    public string MindfulnessType { get; set; }
}

public class PresenceGenerationDto
{
    public string GeneratedContent { get; set; }
    public string PresenceType { get; set; }
}

public class FlowGenerationDto
{
    public string GeneratedContent { get; set; }
    public string FlowType { get; set; }
}

public class ZenGenerationDto
{
    public string GeneratedContent { get; set; }
    public string ZenType { get; set; }
}

public class HealthStatusDto
{
    public string Status { get; set; }
    public Dictionary<string, object> Details { get; set; }
}

public class AutomatedReportDto
{
    public string ReportContent { get; set; }
    public string ReportType { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
}