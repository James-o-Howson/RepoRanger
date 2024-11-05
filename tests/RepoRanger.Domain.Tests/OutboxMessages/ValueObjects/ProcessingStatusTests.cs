using RepoRanger.Domain.OutboxMessages.ValueObjects.Enums;

namespace RepoRanger.Domain.Tests.OutboxMessages.ValueObjects;

internal sealed class ProcessingStatusTests
{
    [TestCase(1, ExpectedResult = ProcessingStatus.Pending)]
    [TestCase(2, ExpectedResult = ProcessingStatus.Processing)]
    [TestCase(3, ExpectedResult = ProcessingStatus.Completed)]
    [TestCase(4, ExpectedResult = ProcessingStatus.DeadLettered)]
    [TestCase(5, ExpectedResult = ProcessingStatus.RetryPending)]
    public ProcessingStatus ProcessingStatusEnum_CastFromInt_IsValid(int actual)
    {
        return (ProcessingStatus) actual;
    }
    
    [TestCase("Pending", ExpectedResult = ProcessingStatus.Pending)]
    [TestCase("Processing", ExpectedResult = ProcessingStatus.Processing)]
    [TestCase("Completed", ExpectedResult = ProcessingStatus.Completed)]
    [TestCase("DeadLettered", ExpectedResult = ProcessingStatus.DeadLettered)]
    [TestCase("RetryPending", ExpectedResult = ProcessingStatus.RetryPending)]
    public ProcessingStatus ProcessingStatusEnum_ParsedFromString_IsValid(string actual)
    {
        return (ProcessingStatus) Enum.Parse(typeof(ProcessingStatus), actual);
    }
}