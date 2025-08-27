# Demo code with .NET 8 C#


## Create producer project
* ASP.NET Core Web API
* [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka) 

```
$dotnet new webapi -n UserApiProducer
$cd UserApiProducer
$dotnet add package Confluent.Kafka
$dotnet add package Microsoft.EntityFrameworkCore.InMemory

$dotnet restore
$dotnet run
```

## Create consumer project
* Console App
* [Confluent.Kafka](https://www.nuget.org/packages/Confluent.Kafka) 

```
$dotnet new console -n UserConsumer
$cd UserConsumer
$dotnet add package Confluent.Kafka

$dotnet restore
$dotnet run
```