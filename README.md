StorageSpike.sln

This service

- Will be used to test ideas relating to dispatch optimisation of storage

## How to Contribute

Create a pull request targeting main and request a review with project developers

## Runbooks
[Runbook](RUNBOOK.md), [deployments](DEPLOYMENT.md) and [monitoring](MONITORING.md)

### Urls

Integration: todo add url to services here

Staging: todo add url to services here

Prod: todo add url to services here

### Docs

todo link to swagger and any documentation outside of repo

## Local Development

### Build and test

The service can be exercised end-to-end using the included Postman collection. (TODO always add a postman collection)

#### Build

todo

## Deployment

todo

## Health Checks

This would be an example

- /diagnostics/healthcheck
- /ping

as an idea

```json
{
  "ApplicationId": "42970",
  "ServiceName": "StorageSpike",
  "Version": "1.0.0",
  "Slice": "blue",~~~~
  "IsHealthy": true,
  "Checks": []
}
```

For now the health check requirements are not known need to be implemented

## Exceptions

Example not yet defined

```json
{
  "errors": [
    {
      "code": "10454.10",
      "detail": "InvalidProperty: The property field is required.",
      "meta": {
      	"severity": "correctable",
	"service": "StorageSpike"
      }
    }
  ]
}
```

## Monitoring

should include links to all telemetry (i.e. NR and logstash) not sure what these are yet

## Dependencies

## Wiki


