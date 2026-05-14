using TaskFlow.Domain.Common;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; private set; } = null!;

    public Guid TaskItemId { get; private set; }
    public TaskItem? TaskItem { get; private set; }

    public Guid AuthorId { get; private set; }
    public User? Author { get; private set; }

    private Comment() { }

    public Comment(string content, Guid taskItemId, Guid authorId)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Yorum içeriği boş olamaz");

        if (content.Length > 1000)
            throw new DomainException("Yorum 1000 karakteri geçemez");

        if (taskItemId == Guid.Empty)
            throw new DomainException("Geçersiz task ID");

        if (authorId == Guid.Empty)
            throw new DomainException("Geçersiz yazar ID");

        Content = content;
        TaskItemId = taskItemId;
        AuthorId = authorId;
    }

    public void Edit(string newContent)
    {
        if (string.IsNullOrWhiteSpace(newContent))
            throw new DomainException("Yorum içeriği boş olamaz");

        if (newContent.Length > 1000)
            throw new DomainException("Yorum 1000 karakteri geçemez");

        Content = newContent;
        SetUpdatedAt();
    }
}