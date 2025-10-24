using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Recruitment.Application.Interfaces;
using Recruitment.Domain.Entities;

namespace Recruitment.Infrastructure.Services;

public class VideoInterviewService : Hub, IVideoInterviewService
{
    private static readonly ConcurrentDictionary<string, InterviewRoom> _interviewRooms = new();
    private static readonly ConcurrentDictionary<string, List<InterviewRecording>> _recordings = new();
    private readonly ILogger<VideoInterviewService> _logger;

    public VideoInterviewService(ILogger<VideoInterviewService> logger)
    {
        _logger = logger;
    }

    public async Task<string> CreateInterviewRoomAsync(Guid applicationId, Guid interviewerId, Guid candidateId)
    {
        var roomId = Guid.NewGuid().ToString();

        var interviewRoom = new InterviewRoom
        {
            RoomId = roomId,
            ApplicationId = applicationId,
            InterviewerId = interviewerId,
            CandidateId = candidateId,
            CreatedAt = DateTime.UtcNow,
            Status = InterviewStatus.Scheduled,
            Participants = new List<Participant>
            {
                new Participant { UserId = interviewerId, Role = "interviewer", JoinedAt = DateTime.UtcNow },
                new Participant { UserId = candidateId, Role = "candidate", JoinedAt = null }
            }
        };

        _interviewRooms[roomId] = interviewRoom;

        // Notify participants about room creation
        await Clients.Users(interviewerId.ToString(), candidateId.ToString())
            .SendAsync("InterviewRoomCreated", new
            {
                roomId,
                applicationId,
                scheduledAt = DateTime.UtcNow.AddMinutes(5) // Auto-start in 5 minutes
            });

        _logger.LogInformation($"Interview room created: {roomId}");

        return roomId;
    }

    public async Task<bool> JoinInterviewRoomAsync(string roomId, Guid userId)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
        {
            await Clients.Caller.SendAsync("Error", "Interview room not found");
            return false;
        }

        var participant = room.Participants.FirstOrDefault(p => p.UserId == userId);
        if (participant == null)
        {
            await Clients.Caller.SendAsync("Error", "You are not authorized to join this interview");
            return false;
        }

        participant.JoinedAt = DateTime.UtcNow;
        participant.IsOnline = true;

        // Add user to SignalR group for this room
        await Groups.AddToGroupAsync(Context.ConnectionId, $"interview_{roomId}");

        // Notify other participants
        await Clients.Group($"interview_{roomId}").SendAsync("ParticipantJoined",
            new { userId, joinedAt = participant.JoinedAt });

        // Send current room state to the joining participant
        await Clients.Caller.SendAsync("RoomState", new
        {
            roomId = room.RoomId,
            status = room.Status,
            participants = room.Participants,
            startedAt = room.StartedAt,
            settings = room.Settings
        });

        _logger.LogInformation($"User {userId} joined interview room: {roomId}");

        return true;
    }

    public async Task LeaveInterviewRoomAsync(string roomId, Guid userId)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
            return;

        var participant = room.Participants.FirstOrDefault(p => p.UserId == userId);
        if (participant != null)
        {
            participant.IsOnline = false;
            participant.LeftAt = DateTime.UtcNow;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"interview_{roomId}");

        await Clients.Group($"interview_{roomId}").SendAsync("ParticipantLeft", new { userId });

        _logger.LogInformation($"User {userId} left interview room: {roomId}");
    }

    public async Task StartInterviewAsync(string roomId, Guid startedBy)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
            return;

        room.Status = InterviewStatus.InProgress;
        room.StartedAt = DateTime.UtcNow;
        room.StartedBy = startedBy;

        // Initialize recording
        var recording = new InterviewRecording
        {
            RecordingId = Guid.NewGuid().ToString(),
            RoomId = roomId,
            StartedAt = DateTime.UtcNow,
            StartedBy = startedBy,
            Status = RecordingStatus.Recording
        };

        if (!_recordings.ContainsKey(roomId))
            _recordings[roomId] = new List<InterviewRecording>();

        _recordings[roomId].Add(recording);

        await Clients.Group($"interview_{roomId}").SendAsync("InterviewStarted",
            new { startedAt = room.StartedAt, recordingId = recording.RecordingId });

        _logger.LogInformation($"Interview started in room: {roomId}");
    }

    public async Task EndInterviewAsync(string roomId, Guid endedBy, InterviewResult result)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
            return;

        room.Status = InterviewStatus.Completed;
        room.EndedAt = DateTime.UtcNow;
        room.EndedBy = endedBy;
        room.Result = result;

        // Stop all recordings
        if (_recordings.ContainsKey(roomId))
        {
            foreach (var recording in _recordings[roomId])
            {
                recording.Status = RecordingStatus.Completed;
                recording.EndedAt = DateTime.UtcNow;
            }
        }

        await Clients.Group($"interview_{roomId}").SendAsync("InterviewEnded",
            new { endedAt = room.EndedAt, result, duration = room.Duration });

        // Clean up after a delay
        _ = Task.Delay(TimeSpan.FromMinutes(30)).ContinueWith(_ =>
        {
            _interviewRooms.TryRemove(roomId, out _);
            _recordings.TryRemove(roomId, out _);
        });

        _logger.LogInformation($"Interview ended in room: {roomId}");
    }

    // WebRTC signaling methods
    public async Task SendWebRTCOffer(string roomId, Guid targetUserId, object offer)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("WebRTCOffer",
            new { fromUserId = Context.UserIdentifier, targetUserId, offer });
    }

    public async Task SendWebRTCAnswer(string roomId, Guid targetUserId, object answer)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("WebRTCAnswer",
            new { fromUserId = Context.UserIdentifier, targetUserId, answer });
    }

    public async Task SendICEcandidate(string roomId, Guid targetUserId, object candidate)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("ICECandidate",
            new { fromUserId = Context.UserIdentifier, targetUserId, candidate });
    }

    // Screen sharing methods
    public async Task StartScreenShare(string roomId, Guid userId, string streamId)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("ScreenShareStarted",
            new { userId, streamId, startedAt = DateTime.UtcNow });
    }

    public async Task StopScreenShare(string roomId, Guid userId)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("ScreenShareStopped",
            new { userId, stoppedAt = DateTime.UtcNow });
    }

    // Recording control methods
    public async Task PauseRecordingAsync(string roomId, Guid userId)
    {
        if (_recordings.ContainsKey(roomId))
        {
            foreach (var recording in _recordings[roomId])
            {
                recording.Status = RecordingStatus.Paused;
                recording.PausedAt = DateTime.UtcNow;
            }
        }

        await Clients.Group($"interview_{roomId}").SendAsync("RecordingPaused",
            new { pausedBy = userId, pausedAt = DateTime.UtcNow });
    }

    public async Task ResumeRecordingAsync(string roomId, Guid userId)
    {
        if (_recordings.ContainsKey(roomId))
        {
            foreach (var recording in _recordings[roomId])
            {
                recording.Status = RecordingStatus.Recording;
                recording.ResumedAt = DateTime.UtcNow;
            }
        }

        await Clients.Group($"interview_{roomId}").SendAsync("RecordingResumed",
            new { resumedBy = userId, resumedAt = DateTime.UtcNow });
    }

    public async Task<List<InterviewRecording>> GetInterviewRecordingsAsync(string roomId)
    {
        if (_recordings.ContainsKey(roomId))
            return _recordings[roomId];

        return new List<InterviewRecording>();
    }

    // Chat during interview
    public async Task SendInterviewMessageAsync(string roomId, Guid userId, string message, string messageType = "text")
    {
        var interviewMessage = new InterviewMessage
        {
            MessageId = Guid.NewGuid().ToString(),
            RoomId = roomId,
            UserId = userId,
            Message = message,
            MessageType = messageType,
            Timestamp = DateTime.UtcNow
        };

        await Clients.Group($"interview_{roomId}").SendAsync("InterviewMessage",
            interviewMessage);

        // Store message for recording purposes
        await StoreInterviewMessageAsync(interviewMessage);
    }

    // Interview notes and ratings
    public async Task UpdateInterviewNotesAsync(string roomId, Guid userId, string notes)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
            return;

        var participant = room.Participants.FirstOrDefault(p => p.UserId == userId);
        if (participant != null)
        {
            participant.Notes = notes;
            participant.NotesUpdatedAt = DateTime.UtcNow;
        }

        await Clients.Group($"interview_{roomId}").SendAsync("NotesUpdated",
            new { userId, notes, updatedAt = DateTime.UtcNow });
    }

    public async Task UpdateParticipantRatingAsync(string roomId, Guid raterId, Guid targetUserId, int rating, string feedback)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("ParticipantRatingUpdated",
            new { raterId, targetUserId, rating, feedback, timestamp = DateTime.UtcNow });
    }

    // Interview evaluation
    public async Task SubmitInterviewEvaluationAsync(string roomId, Guid evaluatorId, InterviewEvaluation evaluation)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("InterviewEvaluationSubmitted",
            new { evaluatorId, evaluation, submittedAt = DateTime.UtcNow });

        // Store evaluation for later analysis
        await StoreInterviewEvaluationAsync(roomId, evaluatorId, evaluation);
    }

    // Breakout rooms for panel interviews
    public async Task CreateBreakoutRoomAsync(string mainRoomId, string breakoutRoomId, List<Guid> participantIds)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"breakout_{breakoutRoomId}");

        await Clients.Group($"interview_{mainRoomId}").SendAsync("BreakoutRoomCreated",
            new { breakoutRoomId, participantIds, createdAt = DateTime.UtcNow });
    }

    public async Task MoveToBreakoutRoomAsync(string breakoutRoomId, Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"breakout_{breakoutRoomId}");
        await Clients.Caller.SendAsync("MovedToBreakoutRoom", breakoutRoomId);
    }

    public async Task ReturnFromBreakoutRoomAsync(string mainRoomId, string breakoutRoomId, Guid userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"breakout_{breakoutRoomId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"interview_{mainRoomId}");

        await Clients.Group($"interview_{mainRoomId}").SendAsync("ParticipantReturned",
            new { userId, fromBreakoutRoom = breakoutRoomId, returnedAt = DateTime.UtcNow });
    }

    // Interview templates and questions
    public async Task LoadInterviewTemplateAsync(string roomId, string templateId)
    {
        // In real implementation, load from database
        var template = new InterviewTemplate
        {
            TemplateId = templateId,
            Title = "Technical Interview Template",
            Sections = new List<InterviewSection>
            {
                new InterviewSection
                {
                    Title = "Technical Skills",
                    Questions = new List<string>
                    {
                        "Tell me about your experience with React",
                        "How do you handle state management in large applications?",
                        "Describe your experience with REST APIs"
                    }
                },
                new InterviewSection
                {
                    Title = "Problem Solving",
                    Questions = new List<string>
                    {
                        "Walk me through how you would debug a performance issue",
                        "How do you approach learning new technologies?"
                    }
                }
            }
        };

        await Clients.Group($"interview_{roomId}").SendAsync("InterviewTemplateLoaded", template);
    }

    public async Task SubmitInterviewQuestionAsync(string roomId, Guid interviewerId, string question)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("InterviewQuestionAsked",
            new { interviewerId, question, askedAt = DateTime.UtcNow });
    }

    // Real-time feedback and reactions
    public async Task SendReactionAsync(string roomId, Guid userId, string reactionType)
    {
        await Clients.Group($"interview_{roomId}").SendAsync("ReactionSent",
            new { userId, reactionType, timestamp = DateTime.UtcNow });
    }

    public async Task RequestTechnicalHelpAsync(string roomId, Guid userId, string issue)
    {
        // Notify admins about technical issues
        await Clients.Group("role_admin").SendAsync("TechnicalHelpRequested",
            new { roomId, userId, issue, requestedAt = DateTime.UtcNow });
    }

    // Interview analytics
    public async Task GetInterviewAnalyticsAsync(string roomId)
    {
        if (!_interviewRooms.TryGetValue(roomId, out var room))
            return;

        var analytics = new InterviewAnalytics
        {
            RoomId = roomId,
            Duration = room.Duration,
            ParticipantCount = room.Participants.Count,
            MessagesCount = await GetMessageCountAsync(roomId),
            RecordingDuration = await GetRecordingDurationAsync(roomId),
            TechnicalIssues = await GetTechnicalIssuesAsync(roomId),
            AverageRating = await GetAverageRatingAsync(roomId)
        };

        await Clients.Caller.SendAsync("InterviewAnalytics", analytics);
    }

    // Private helper methods
    private async Task StoreInterviewMessageAsync(InterviewMessage message)
    {
        // In real implementation, save to database
        _logger.LogInformation($"Interview message stored: {message.MessageId}");
    }

    private async Task StoreInterviewEvaluationAsync(string roomId, Guid evaluatorId, InterviewEvaluation evaluation)
    {
        // In real implementation, save to database
        _logger.LogInformation($"Interview evaluation stored for room: {roomId}");
    }

    private async Task<int> GetMessageCountAsync(string roomId)
    {
        // In real implementation, query database
        return 0;
    }

    private async Task<TimeSpan> GetRecordingDurationAsync(string roomId)
    {
        if (_recordings.ContainsKey(roomId))
        {
            return _recordings[roomId]
                .Where(r => r.Status == RecordingStatus.Completed)
                .Sum(r => (r.EndedAt - r.StartedAt).Value.TotalMinutes)
                .Minutes();
        }

        return TimeSpan.Zero;
    }

    private async Task<List<string>> GetTechnicalIssuesAsync(string roomId)
    {
        // In real implementation, query database
        return new List<string>();
    }

    private async Task<double> GetAverageRatingAsync(string roomId)
    {
        // In real implementation, query database
        return 4.2;
    }
}

// Domain models for video interviews
public class InterviewRoom
{
    public string RoomId { get; set; } = string.Empty;
    public Guid ApplicationId { get; set; }
    public Guid InterviewerId { get; set; }
    public Guid CandidateId { get; set; }
    public DateTime CreatedAt { get; set; }
    public InterviewStatus Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public Guid? StartedBy { get; set; }
    public Guid? EndedBy { get; set; }
    public InterviewResult? Result { get; set; }
    public List<Participant> Participants { get; set; } = new();
    public InterviewSettings Settings { get; set; } = new();

    public TimeSpan Duration => (EndedAt ?? DateTime.UtcNow) - (StartedAt ?? CreatedAt);
}

public class Participant
{
    public Guid UserId { get; set; }
    public string Role { get; set; } = string.Empty; // interviewer, candidate, observer
    public DateTime? JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool IsOnline { get; set; }
    public string? Notes { get; set; }
    public DateTime? NotesUpdatedAt { get; set; }
    public int? Rating { get; set; }
    public string? Feedback { get; set; }
}

public class InterviewSettings
{
    public bool AllowRecording { get; set; } = true;
    public bool AllowScreenShare { get; set; } = true;
    public bool AllowChat { get; set; } = true;
    public bool RequireConsent { get; set; } = true;
    public int MaxParticipants { get; set; } = 4;
    public TimeSpan? MaxDuration { get; set; }
}

public class InterviewRecording
{
    public string RecordingId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public Guid StartedBy { get; set; }
    public RecordingStatus Status { get; set; }
    public string? FilePath { get; set; }
    public long FileSize { get; set; }
    public string? StorageUrl { get; set; }
    public DateTime? PausedAt { get; set; }
    public DateTime? ResumedAt { get; set; }
    public List<string> Participants { get; set; } = new();
}

public class InterviewMessage
{
    public string MessageId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageType { get; set; } = "text"; // text, system, emoji
    public DateTime Timestamp { get; set; }
}

public class InterviewEvaluation
{
    public int TechnicalSkills { get; set; }
    public int CommunicationSkills { get; set; }
    public int ProblemSolving { get; set; }
    public int CulturalFit { get; set; }
    public int OverallRating { get; set; }
    public string? Strengths { get; set; }
    public string? AreasForImprovement { get; set; }
    public string? AdditionalNotes { get; set; }
    public bool RecommendHiring { get; set; }
}

public class InterviewTemplate
{
    public string TemplateId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<InterviewSection> Sections { get; set; } = new();
}

public class InterviewSection
{
    public string Title { get; set; } = string.Empty;
    public List<string> Questions { get; set; } = new();
    public int EstimatedMinutes { get; set; }
}

public class InterviewAnalytics
{
    public string RoomId { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int ParticipantCount { get; set; }
    public int MessagesCount { get; set; }
    public double RecordingDuration { get; set; }
    public List<string> TechnicalIssues { get; set; } = new();
    public double AverageRating { get; set; }
}

public enum InterviewStatus
{
    Scheduled,
    InProgress,
    Paused,
    Completed,
    Cancelled,
    TechnicalIssues
}

public enum RecordingStatus
{
    Recording,
    Paused,
    Completed,
    Failed,
    Processing
}

public class InterviewResult
{
    public string OverallAssessment { get; set; } = string.Empty;
    public bool RecommendedForHire { get; set; }
    public int OverallScore { get; set; }
    public List<string> KeyStrengths { get; set; } = new();
    public List<string> AreasForImprovement { get; set; } = new();
    public string? NextSteps { get; set; }
    public DateTime EvaluatedAt { get; set; }
    public Guid EvaluatedBy { get; set; }
}

// Video processing service for interview recordings
public class VideoProcessingService : IVideoProcessingService
{
    private readonly ILogger<VideoProcessingService> _logger;

    public VideoProcessingService(ILogger<VideoProcessingService> logger)
    {
        _logger = logger;
    }

    public async Task<string> ProcessInterviewRecordingAsync(string recordingId, string filePath)
    {
        try {
            // In real implementation, this would:
            // 1. Process the raw video file
            // 2. Extract audio for transcription
            // 3. Generate thumbnails
            // 4. Create compressed versions
            // 5. Upload to storage
            // 6. Generate shareable links

            _logger.LogInformation($"Processing interview recording: {recordingId}");

            // Simulate processing time
            await Task.Delay(5000);

            var processedUrl = $"https://storage.yourplatform.com/interviews/{recordingId}/processed.mp4";

            _logger.LogInformation($"Interview recording processed: {processedUrl}");

            return processedUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to process interview recording: {recordingId}");
            throw;
        }
    }

    public async Task<string> GenerateInterviewTranscriptAsync(string recordingId, string audioFilePath)
    {
        try {
            // In real implementation, this would:
            // 1. Extract audio from video
            // 2. Use speech-to-text service (Google Speech-to-Text, AWS Transcribe, etc.)
            // 3. Generate timestamped transcript
            // 4. Identify speakers
            // 5. Extract key points

            _logger.LogInformation($"Generating transcript for recording: {recordingId}");

            // Mock transcript
            var transcript = new InterviewTranscript
            {
                RecordingId = recordingId,
                Language = "vi",
                Duration = TimeSpan.FromMinutes(45),
                Speakers = new List<TranscriptSpeaker>
                {
                    new TranscriptSpeaker { SpeakerId = "interviewer", Name = "John Smith" },
                    new TranscriptSpeaker { SpeakerId = "candidate", Name = "Nguyen Van A" }
                },
                Segments = new List<TranscriptSegment>
                {
                    new TranscriptSegment
                    {
                        StartTime = TimeSpan.FromSeconds(0),
                        EndTime = TimeSpan.FromSeconds(30),
                        SpeakerId = "interviewer",
                        Text = "Tell me about your experience with React development.",
                        Confidence = 0.95
                    },
                    new TranscriptSegment
                    {
                        StartTime = TimeSpan.FromSeconds(30),
                        EndTime = TimeSpan.FromSeconds(60),
                        SpeakerId = "candidate",
                        Text = "I've been working with React for 3 years, primarily on large-scale applications.",
                        Confidence = 0.92
                    }
                }
            };

            return JsonConvert.SerializeObject(transcript);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to generate transcript: {recordingId}");
            throw;
        }
    }

    public async Task<List<string>> ExtractKeyPointsAsync(string transcript)
    {
        // In real implementation, use NLP to extract key points
        return new List<string>
        {
            "Strong React experience (3+ years)",
            "Experience with large-scale applications",
            "Good problem-solving skills demonstrated"
        };
    }

    public async Task<InterviewSummary> GenerateInterviewSummaryAsync(string roomId)
    {
        // In real implementation, use AI to generate comprehensive summary
        return new InterviewSummary
        {
            RoomId = roomId,
            OverallScore = 8.5,
            KeyStrengths = new List<string> { "Technical expertise", "Communication skills" },
            AreasForImprovement = new List<string> { "Leadership experience" },
            Recommendation = "Strong hire",
            DetailedAnalysis = "Candidate demonstrates excellent technical skills and cultural fit..."
        };
    }
}

public class InterviewTranscript
{
    public string RecordingId { get; set; } = string.Empty;
    public string Language { get; set; } = "vi";
    public TimeSpan Duration { get; set; }
    public List<TranscriptSpeaker> Speakers { get; set; } = new();
    public List<TranscriptSegment> Segments { get; set; } = new();
}

public class TranscriptSpeaker
{
    public string SpeakerId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class TranscriptSegment
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string SpeakerId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public double Confidence { get; set; }
}

public class InterviewSummary
{
    public string RoomId { get; set; } = string.Empty;
    public double OverallScore { get; set; }
    public List<string> KeyStrengths { get; set; } = new();
    public List<string> AreasForImprovement { get; set; } = new();
    public string Recommendation { get; set; } = string.Empty;
    public string DetailedAnalysis { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}