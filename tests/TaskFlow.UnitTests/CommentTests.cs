using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class CommentTests
{
    // KURAL 1: İçerik boşsa DomainException fırlamalı
    [Fact]
    public void Constructor_WhenContentIsEmpty_ThrowsDomainException()
    {
        var emptyContent = "";
        var taskItemId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new Comment(emptyContent, taskItemId, authorId)
        );
    }

    // KURAL 2: İçerik 1000 karakterden uzunsa DomainException fırlamalı
    [Fact]
    public void Constructor_WhenContentTooLong_ThrowsDomainException()
    {
        var longContent = new string('a', 1001);
        var taskItemId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new Comment(longContent, taskItemId, authorId)
        );
    }

    // KURAL 3: İçerik geçerliyse Comment başarıyla oluşmalı
    [Fact]
    public void Constructor_WhenInputIsValid_CreatesComment()
    {
        var content = "Geçerli bir yorum";
        var taskItemId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var comment = new Comment(content, taskItemId, authorId);

        Assert.Equal(content, comment.Content);
    }
}