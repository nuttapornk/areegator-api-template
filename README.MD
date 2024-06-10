# Template
```
platform-domain-component
areegator-subbroker-processapi
areegator-subbroker-companyapi
areegator-subbroker-agentapi
```

# Install template

```
dotnet new install Insplatform.Architecture.MicroserviceApi.Template::_TEMPLATEVERSION_
 --nuget-source githubtidlor 
```

# Test create from sample

```ssh
dotnet new ins-api-sln -p areegator -d subbroker -o companyapi
```
- ✔ .NET 8
- ✔ Clean Architect
- ✔ WeatherForecast Example Api
- ✔ Database EF Core with LINQ
- ✔ Redis
- ✔ Health,Alive
- ✔ Logging ELK with Confluent Kafka
- Pagging,Sort,Search
- 
