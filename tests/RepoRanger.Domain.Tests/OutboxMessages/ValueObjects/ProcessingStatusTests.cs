using RepoRanger.Domain.OutboxMessages.ValueObjects;

namespace RepoRanger.Domain.Tests.OutboxMessages.ValueObjects;

internal sealed class ProcessingStatusTests
{
    [TestCase(1, ExpectedResult = ProcessingStatus.Unprocessed)]
    [TestCase(2, ExpectedResult = ProcessingStatus.Succeeded)]
    [TestCase(3, ExpectedResult = ProcessingStatus.Failed)]
    public ProcessingStatus ProcessingStatusEnum_CastFromInt_IsValid(int actual)
    {
        return (ProcessingStatus) actual;
    }
    
    [TestCase("Unprocessed", ExpectedResult = ProcessingStatus.Unprocessed)]
    [TestCase("Succeeded", ExpectedResult = ProcessingStatus.Succeeded)]
    [TestCase("Failed", ExpectedResult = ProcessingStatus.Failed)]
    public ProcessingStatus ProcessingStatusEnum_ParsedFromString_IsValid(string actual)
    {
        return (ProcessingStatus) Enum.Parse(typeof(ProcessingStatus), actual);
    }
}