namespace TaskFlow.Application.Interfaces.Metrics;

public interface ITaskFlowMetrics
{
    void IncrementTasksCreated();
    void IncrementTasksCompleted();
    void IncrementProjectsCreated();
    void IncrementCommentsCreated();
    void IncrementUsersCreated();
}