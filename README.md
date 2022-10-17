## Practice Requirement
- Fork this repository
- Remove duplicate code in ArticleController and UserController
- Make all test cases pass

#### Environment Requirement
- .Net Core 3.1
- Visual Studio

####
Reference:
* https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0

#### Tips:
Custom TestServer:
```
            Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => { return string.Empty; });
                });
            }).CreateClient();
```

#### Steps
1. Fix the implementation codes to make the failed unit tests passed 
2. Fix all failed test
3. Refactor: Extract method to reduce duplicate codes