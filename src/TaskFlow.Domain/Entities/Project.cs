using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }

    public User? Owner { get; private set; }

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks.AsReadOnly();

    private Project() { }

    public Project(string name, string? description, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Proje adı boş olamaz");

        if (name.Length > 100)
            throw new DomainException("Proje adı 100 karakteri geçemez");

        if (ownerId == Guid.Empty)
            throw new DomainException("Geçersiz sahip ID");

        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Proje adı boş olamaz");

        if (name.Length > 100)
            throw new DomainException("Proje adı 100 karakteri geçemez");

        Name = name;
        Description = description;
        SetUpdatedAt();
    }

    public void AddTask(TaskItem task)
    {
        if (task == null)
            throw new DomainException("Task null olamaz");

        _tasks.Add(task);
        SetUpdatedAt();
    }
}