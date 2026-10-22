using Prometheus;
using TaskFlow.Application.Interfaces.Metrics;

namespace TaskFlow.Infrastructure.Metrics;

public class PrometheusTaskFlowMetrics : ITaskFlowMetrics
{
    private static readonly Counter TasksCreated =
        Prometheus.Metrics.CreateCounter(
            "taskflow_tasks_created_total",
            "Total number of successfully created tasks.");

    private static readonly Counter TasksCompleted =
        Prometheus. Metrics.CreateCounter(
            "taskflow_tasks_completed_total",
            "Total number of tasks transitioned to Done.");

    private static readonly Counter ProjectsCreated =
        Prometheus. Metrics.CreateCounter(
            "taskflow_projects_created_total",
            "Total number of successfully created projects.");

    private static readonly Counter CommentsCreated =
        Prometheus.Metrics.CreateCounter(
            "taskflow_comments_created_total",
            "Total number of successfully created comments.");
    
    private static readonly Counter UsersCreated =
        Prometheus.Metrics.CreateCounter(
            "taskflow_users_created_total",
            "Total number of successfully created users.");

    public void IncrementTasksCreated() =>
        TasksCreated.Inc();

    public void IncrementTasksCompleted() =>
        TasksCompleted.Inc();

    public void IncrementProjectsCreated() =>
        ProjectsCreated.Inc();

    public void IncrementCommentsCreated() =>
        CommentsCreated.Inc();
    
    public void IncrementUsersCreated() =>
        UsersCreated.Inc();
}