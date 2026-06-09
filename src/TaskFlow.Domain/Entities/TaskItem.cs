using TaskFlow.Domain.Common;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public DateTime? DueDate { get; private set; }

    public Guid ProjectId { get; private set; }
    public Project? Project { get; private set; }

    public Guid? AssignedToId { get; private set; }
    public User? AssignedTo { get; private set; }

    private readonly List<Comment> _comments = new();
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    private TaskItem() { }

    public TaskItem(string title, string? description, Guid projectId, DateTime? dueDate = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task başlığı boş olamaz");

        if (title.Length > 200)
            throw new DomainException("Task başlığı 200 karakteri geçemez");

        if (projectId == Guid.Empty)
            throw new DomainException("Geçersiz proje ID");

        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow.Date)
            throw new DomainException("Bitiş tarihi geçmişte olamaz");

        Title = title;
        Description = description;
        ProjectId = projectId;
        DueDate = dueDate;
        Status = TaskItemStatus.Todo;
    }

    public void UpdateStatus(TaskItemStatus newStatus)
    {
        Status = newStatus;
        SetUpdatedAt();
    }

    public void UpdateDetails(string title, string? description, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Task başlığı boş olamaz");

        if (title.Length > 200)
            throw new DomainException("Task başlığı 200 karakteri geçemez");

        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow.Date)
            throw new DomainException("Bitiş tarihi geçmişte olamaz");

        Title = title;
        Description = description;
        DueDate = dueDate;

        SetUpdatedAt();
    }

    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("Geçersiz kullanıcı ID");

        AssignedToId = userId;
        SetUpdatedAt();
    }

    public void Unassign()
    {
        AssignedToId = null;
        AssignedTo = null;
        SetUpdatedAt();
    }

    public void AddComment(Comment comment)
    {
        if (comment == null)
            throw new DomainException("Yorum null olamaz");

        _comments.Add(comment);
        SetUpdatedAt();
    }
}