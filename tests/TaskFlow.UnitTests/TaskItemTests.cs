using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class TaskItemTests
{
    // KURAL: Başlık boşsa hata fırlamalı
    [Fact]
    public void Constructor_WhenTitleIsEmpty_ThrowsDomainException()
    {
        var projectId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new TaskItem("", "açıklama", projectId)
        );
    }

    // KURAL: Başlık 200 karakteri geçerse hata fırlamalı
    [Fact]
    public void Constructor_WhenTitleTooLong_ThrowsDomainException()
    {
        var longTitle = new string('a', 201);
        var projectId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new TaskItem(longTitle, "açıklama", projectId)
        );
    }

    // KURAL: Bitiş tarihi geçmişte olursa hata fırlamalı
    [Fact]
    public void Constructor_WhenDueDateInPast_ThrowsDomainException()
    {
        var projectId = Guid.NewGuid();
        var pastDate = DateTime.UtcNow.AddDays(-1);

        Assert.Throws<DomainException>(
            () => new TaskItem("Başlık", "açıklama", projectId, pastDate)
        );
    }

    // KURAL: Geçerli girdiyle task oluşmalı ve başlangıç durumu Todo olmalı
    [Fact]
    public void Constructor_WhenInputIsValid_CreatesTaskWithTodoStatus()
    {
        var projectId = Guid.NewGuid();

        var task = new TaskItem("Başlık", "açıklama", projectId);

        Assert.Equal("Başlık", task.Title);
        Assert.Equal(TaskItemStatus.Todo, task.Status);
    }

    // KURAL: UpdateStatus durumu değiştirmeli
    [Fact]
    public void UpdateStatus_ChangesStatus()
    {
        var task = new TaskItem("Başlık", "açıklama", Guid.NewGuid());

        task.UpdateStatus(TaskItemStatus.InProgress);

        Assert.Equal(TaskItemStatus.InProgress, task.Status);
    }

    // KURAL: AssignTo geçerli kullanıcıyı atamalı
    [Fact]
    public void AssignTo_WhenUserIdIsValid_SetsAssignedTo()
    {
        var task = new TaskItem("Başlık", "açıklama", Guid.NewGuid());
        var userId = Guid.NewGuid();

        task.AssignTo(userId);

        Assert.Equal(userId, task.AssignedToId);
    }

    // KURAL: AssignTo boş kullanıcı ID ile hata fırlatmalı
    [Fact]
    public void AssignTo_WhenUserIdIsEmpty_ThrowsDomainException()
    {
        var task = new TaskItem("Başlık", "açıklama", Guid.NewGuid());

        Assert.Throws<DomainException>(
            () => task.AssignTo(Guid.Empty)
        );
    }

    // KURAL: Unassign atamayı kaldırmalı (null yapmalı)
    [Fact]
    public void Unassign_RemovesAssignment()
    {
        var task = new TaskItem("Başlık", "açıklama", Guid.NewGuid());
        task.AssignTo(Guid.NewGuid());

        task.Unassign();

        Assert.Null(task.AssignedToId);
    }
}