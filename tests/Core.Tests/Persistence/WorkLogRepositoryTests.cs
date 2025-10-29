namespace Core.Tests.Persistence;
public class WorkLogRepositoryTests
{
    private static WorkLogRepository CreateRepository()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);
        return new WorkLogRepository(context);
    }

    [Fact]
    public async Task Deve_Adicionar_E_Buscar_Por_Status()
    {
        // Arrange
        var repo = CreateRepository();
        await repo.AddAsync(new WorkLog { Date = DateTime.UtcNow, Message = "Start", Status = "Running" });
        await repo.AddAsync(new WorkLog { Date = DateTime.UtcNow, Message = "Finish", Status = "Done" });

        // Act
        var results = await repo.GetByStatusAsync("Done");

        // Assert
        results.Should().HaveCount(1);
        results.First().Message.Should().Be("Finish");
    }

    [Fact]
    public async Task Deve_Atualizar_Mensagem_Com_Sucesso()
    {
        // Arrange
        var repo = CreateRepository();
        var log = await repo.AddAsync(new WorkLog { Date = DateTime.UtcNow, Message = "Initial", Status = "Pending" });

        // Act
        var updated = await repo.UpdateMessageAsync(log.Id, "Updated Message");

        // Assert
        updated.Should().NotBeNull();
        updated!.Message.Should().Be("Updated Message");
    }
}
