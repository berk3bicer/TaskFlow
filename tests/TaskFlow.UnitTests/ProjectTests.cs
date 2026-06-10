using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.UnitTests;

public class ProjectTests
{
    // KURAL: Proje adı boşsa hata fırlamalı
    [Fact]
    public void Constructor_WhenNameIsEmpty_ThrowsDomainException()
    {
        var ownerId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new Project("", "açıklama", ownerId)
        );
    }

    // KURAL: Proje adı 100 karakteri geçerse hata fırlamalı
    [Fact]
    public void Constructor_WhenNameTooLong_ThrowsDomainException()
    {
        var longName = new string('a', 101);
        var ownerId = Guid.NewGuid();

        Assert.Throws<DomainException>(
            () => new Project(longName, "açıklama", ownerId)
        );
    }

    // KURAL: Geçerli girdiyle proje oluşmalı ve değerler doğru atanmalı
    [Fact]
    public void Constructor_WhenInputIsValid_CreatesProject()
    {
        var ownerId = Guid.NewGuid();

        var project = new Project("Projem", "açıklama", ownerId);

        Assert.Equal("Projem", project.Name);
        Assert.Equal(ownerId, project.OwnerId);
    }

    // KURAL: Update geçerli yeni adla ismi değiştirmeli
    [Fact]
    public void Update_WhenNameIsValid_ChangesName()
    {
        var project = new Project("Eski ad", "açıklama", Guid.NewGuid());

        project.Update("Yeni ad", "yeni açıklama");

        Assert.Equal("Yeni ad", project.Name);
    }

    // KURAL: Update boş adla çağrılırsa hata fırlamalı
    [Fact]
    public void Update_WhenNameIsEmpty_ThrowsDomainException()
    {
        var project = new Project("Eski ad", "açıklama", Guid.NewGuid());

        Assert.Throws<DomainException>(
            () => project.Update("", "yeni açıklama")
        );
    }
}