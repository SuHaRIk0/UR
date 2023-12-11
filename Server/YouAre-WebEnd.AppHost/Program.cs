var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.YouAre_API>("YouAre.api");

builder.Build().Run();
