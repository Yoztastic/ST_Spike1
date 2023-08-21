using Host.Contracts.Serialisation;
using Refit;
using Sdk;
using StorageSpike.Host.Contracts.Requests;
using StorageSpike.Host.Contracts.Responses;

var httpMessageHandler = new ConversationStarter { InnerHandler = new HandlerInspector { InnerHandler = new HttpClientHandler() } };

var client = new HttpClient(httpMessageHandler) { BaseAddress = new Uri("https://localhost:42970")};

client.DefaultRequestHeaders.Add("User-Agent","Sdk.Consumer");

var serializer = new SystemTextJsonContentSerializer(Serialisation.Options);
var refitSettings = new RefitSettings { ContentSerializer = serializer };

var storageApi = RestService.For<IStorageApi>(client,refitSettings);

var storageResponse = await "Call storage".Do<StorageConstraintsResponse>(()=> storageApi.GetStorageConstraints("someString", new StorageConstraintsRequest()));
